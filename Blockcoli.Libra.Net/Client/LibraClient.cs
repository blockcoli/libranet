using System.Threading.Tasks;
using Blockcoli.Libra.Net.Wallet;
using Types;
using static AdmissionControl.AdmissionControl;
using System.Linq;
using System.Net.Http;
using System.Net;
using System;
using Grpc.Core;
using AdmissionControl;
using System.Collections.Generic;
using Blockcoli.Libra.Net.Common;
using Blockcoli.Libra.Net.Crypto;
using Blockcoli.Libra.Net.LCS;

namespace Blockcoli.Libra.Net.Client
{
    public class LibraClient
    {
        public LibraNetwork Network { get; private set; } 
        public string Host { get; private set; } 
        public int Port { get; private set; }        
        public string FaucetServerHost { get; private set; }
        AdmissionControlClient acClient;
        public ChannelCredentials ChannelCredentials { get; private set; }

        public LibraClient(LibraNetwork network)
        {
            this.Network = network;

            switch (network)
            {
                case LibraNetwork.Testnet:     
                    Host = Constant.ServerHosts.TestnetAdmissionControl;
                    Port = 8000;
                    ChannelCredentials = ChannelCredentials.Insecure;
                    FaucetServerHost = Constant.ServerHosts.TestnetFaucet;            
                    break;
            }

            Channel channel = new Channel(Host, Port, ChannelCredentials);            
            acClient = new AdmissionControlClient(channel);
        }

        public async Task<ulong> MintWithFaucetService(string address, ulong amount)
        {
            var httpClient = new HttpClient();
            var url = $"http://{FaucetServerHost}?amount={amount}&address={address}";            
            var response = await httpClient.PostAsync(url, null);
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Failed to query faucet service. Code: {response.StatusCode}");
            var sequenceNumber = await response.Content.ReadAsStringAsync();
            return ulong.Parse(sequenceNumber) - 1;
        }

        public async Task<bool> TransferCoins(Account sender, string receiverAddress, ulong amount, ulong gasUnitPrice = 0, ulong maxGasAmount = 1000000)
        {
            try
            {
                var accountState = await QueryBalance(sender.Address);

                var program = new ProgramLCS();
                program.Code = Convert.FromBase64String(Constant.ProgamBase64Codes.PeerToPeerTxn);
                program.TransactionArguments = new List<TransactionArgumentLCS>();
                program.TransactionArguments.Add(new TransactionArgumentLCS 
                {  
                    ArgType = Types.TransactionArgument.Types.ArgType.Address,
                    Address = new AddressLCS { Value = receiverAddress }
                });
                program.TransactionArguments.Add(new TransactionArgumentLCS 
                {  
                    ArgType = Types.TransactionArgument.Types.ArgType.U64,
                    U64 = amount
                });
                program.Modules = new List<byte[]>();

                var transaction = new RawTransactionLCS
                {
                    Sender = new AddressLCS { Value = sender.Address },
                    SequenceNumber = accountState.SequenceNumber,
                    TransactionPayload = new TransactionPayloadLCS
                    {
                        PayloadType = TransactionPayloadType.Program,
                        Program = program                    
                    },
                    MaxGasAmount = maxGasAmount,
                    GasUnitPrice = gasUnitPrice,
                    ExpirationTime = (ulong)Math.Floor((decimal)DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000) + 100
                };   
                var transactionLCS = LCSCore.LCSDeserialization(transaction);
                
                var digestSHA3 = new SHA3_256();
                var saltDigest = digestSHA3.ComputeVariable(Constant.HashSaltValues.RawTransactionHashSalt.ToBytes());
                var saltDigestAndTransaction = saltDigest.Concat(transactionLCS).ToArray();
                var hash = digestSHA3.ComputeVariable(saltDigestAndTransaction);
                var senderSignature = sender.KeyPair.Sign(hash);                

                var publicKeyLen = BitConverter.GetBytes((uint)sender.PublicKey.Length);
                var signatureLen = BitConverter.GetBytes((uint)senderSignature.Length);
                var signedTxn = transactionLCS.Concat(publicKeyLen).ToArray();
                signedTxn = signedTxn.Concat(sender.PublicKey).ToArray();
                signedTxn = signedTxn.Concat(signatureLen).ToArray();
                signedTxn = signedTxn.Concat(senderSignature).ToArray();

                var request = new SubmitTransactionRequest
                {
                    SignedTxn = new SignedTransaction
                    {
                        SignedTxn = signedTxn.ToByteString()
                    }
                };                
                
                
                var response = await acClient.SubmitTransactionAsync(request);
                return response.AcStatus.Code == AdmissionControlStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Wallet.AccountState> QueryBalance(string address)
        {
            var request = new UpdateToLatestLedgerRequest();
            var accountStateRequest = new GetAccountStateRequest{ Address = address.ToByteString() };
                
            var requestItem = new RequestItem { GetAccountStateRequest = accountStateRequest };
            request.RequestedItems.Add(requestItem);

            var response = await acClient.UpdateToLatestLedgerAsync(request);   
        
            return DecodeAccountStateBlob(response.ResponseItems.SingleOrDefault().GetAccountStateResponse.AccountStateWithProof.Blob); 
        }

        public async Task<List<Wallet.AccountState>> QueryBalances(params string[] addresses)
        {
            var request = new UpdateToLatestLedgerRequest();
            foreach (var address in addresses)
            {
                var accountStateRequest = new GetAccountStateRequest{ Address = address.ToByteString() };
                
                var requestItem = new RequestItem { GetAccountStateRequest = accountStateRequest };
                request.RequestedItems.Add(requestItem);
            }

            var accountStates = new List<Wallet.AccountState>();
            var response = await acClient.UpdateToLatestLedgerAsync(request);                                    
            foreach (var item in response.ResponseItems)
            {
                var accountState = DecodeAccountStateBlob(item.GetAccountStateResponse.AccountStateWithProof.Blob);                            
                accountStates.Add(accountState);                                                                                  
            }

            return accountStates;
        }

        private static Wallet.AccountState DecodeAccountStateBlob(AccountStateBlob blob)
        {            
            var cursor = new CursorBuffer(blob.Blob.ToByteArray());
            var blobLen = cursor.Read32();

            var state = new Dictionary<string, byte[]>();
            for (int i = 0; i < blobLen; i++)
            {
                var keyLen = cursor.Read32();
                var keyBuffer = new byte[keyLen];
                for (int j = 0; j < keyLen; j++)
                {
                    keyBuffer[j] = cursor.Read8();
                }

                var valueLen = cursor.Read32();
                var valueBuffer = new byte[valueLen];
                for (int k = 0; k < valueLen; k++)
                {
                    valueBuffer[k] = cursor.Read8();
                }

                state[keyBuffer.ToHexString()] = valueBuffer;
            }

            return Wallet.AccountState.FromBytes(state[Constant.PathValues.AccountStatePath]);
        }
    }
}