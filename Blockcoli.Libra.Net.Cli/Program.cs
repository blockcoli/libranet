using System;
using  Blockcoli.Libra.Net.Client;
using Blockcoli.Libra.Net.Wallet;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using AdmissionControl;

namespace Blockcoli.Libra.Net.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var p = new Program(LibraNetwork.Testnet);            
            await p.Run();
        }

        LibraNetwork network;
        string serverHost;
        LibraWallet wallet;
        LibraClient client;

        public Program(LibraNetwork network)
        {
            this.network = network;    
            this.wallet = new LibraWallet();    
            this.client = new LibraClient(network);    
            switch (network)
            {
                case LibraNetwork.Testnet:
                    this.serverHost = Constant.ServerHosts.TestnetAdmissionControl;
                    break;
            }

            Console.WriteLine("Blockcoli.Libra.Net.Cli command-line interface for Libra Wallet.");
        }

        public async Task Run()
        {
            Help();
            Console.WriteLine("\nPlease, input commands:\n");
            while (true)
            {
                Console.Write("libra% ");
                var input = Console.ReadLine();
                var cmd = input.Split(" ");
                if (string.IsNullOrEmpty(cmd[0])) continue;

                switch (cmd[0])
                {
                    case "a":
                    case "account":
                        await Account(cmd);
                        break;
                    case "q":
                    case "query":
                        await Query(cmd);
                        break;
                    case "t":
                    case "tb":
                    case "transferb":
                    case "transfer":
                        await Transfer(cmd);
                        break;
                    case "h":
                    case "help":
                        Help();
                        break;
                    case "q!":
                    case "quit":
                        return;
                    default:                        
                        Console.WriteLine($"Unknown command: \"{cmd[0]}\"");
                        break;
                }
            }
        }

        void Help()
        {
            Console.WriteLine($"Connected to validator at: {serverHost}");
            Console.WriteLine(
@"usage: <command> <args>

Use the following commands:

account | a
	Account operations
query | q
	Query operations
transfer | transferb | t | tb
	<sender_account_address>|<sender_account_ref_id> <receiver_account_address>|<receiver_account_ref_id> <number_of_coins> [gas_unit_price_in_micro_libras (default=0)] [max_gas_amount_in_micro_libras (default 100000)] Suffix 'b' is for blocking.
	Transfer coins (in libra) from account to another.
help | h
	Prints this help
quit | q!
	Exit this client
");
        }

        async Task Account(string[] cmd)
        {
            if (cmd.Length <= 1)
            {
                AccountHelp();
                return;
            }

            switch (cmd[1])
            {
                case "c":
                case "create":     
                    AccountCreate();               
                    break;
                case "la":
                case "list":
                    AccountList();
                    break;
                case "r":
                case "recover":
                    AccountRecover(cmd);
                    break;
                case "w":
                case "write":
                    AccountWrite(cmd);
                    break;
                case "m":
                case "mb":
                case "mintb":                
                case "mint":
                    await AccountMint(cmd);
                    break;
                default:
                    AccountHelp();
                    break;
            }
        }

        void AccountHelp()
        {
            Console.WriteLine(
@"usage: account <arg>

Use the following args for this command:

create | c
	Create an account. Returns reference ID to use in other operations
list | la
	Print all accounts that were created or loaded
recover | r <file_path>
	Recover Libra wallet from the file path
write | w <file_path>
	Save Libra wallet mnemonic recovery seed to disk
mint | mintb | m | mb <receiver_account_ref_id>|<receiver_account_address> <number_of_coins>
	Mint coins to the account. Suffix 'b' is for blocking
");
        }

        void AccountCreate()
        {                   
            try
            {     
                var account = wallet.NewAccount();
                Console.WriteLine(">> Creating/retrieving next account from wallet");
                Console.WriteLine($"Created/retrieved account #{wallet.Accounts.Count-1} address {account.Address}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void AccountList()
        {
            if (wallet.Accounts.Count <= 0)
            {
                Console.WriteLine("No user accounts");
                return;
            }

            wallet.Accounts.ForEachWithIndex((account, i) => 
            {
                Console.WriteLine($"User account index: {i}, address: {account.Value.Address}");
            });
        }

        void AccountRecover(string[] cmd)
        {
            Console.WriteLine(">> Recovering Wallet");
            if (cmd.Length <= 2) 
            {
                Console.WriteLine("[ERROR] Error recovering Libra wallet: Invalid number of arguments for recovering wallets");
                return;
            }
            var fileName = cmd[2];

            if (!File.Exists(fileName)) 
            {
                Console.WriteLine("[ERROR] Error recovering Libra wallet: LibraWalletGeneric: No such file or directory (os error 2)");
                return;
            }

            var reader = new StreamReader(fileName);
            var words = reader.ReadLine().Split(';');
            reader.Close();

            if (words.Length <= 1) 
            {
                Console.WriteLine("[ERROR] Error recovering Libra wallet: LibraWalletGeneric: Invalid entry");
                return;
            }

            Console.WriteLine($"Wallet recovered and the first {words[1]} child accounts were derived");
            wallet = new LibraWallet(words[0]);
            for (int i = 0; i < int.Parse(words[1]); i++)
            {
                var account = wallet.NewAccount();
                Console.WriteLine($"#{i} address {account.Address}");
            }
            
        }

        void AccountWrite(string[] cmd)
        {
            Console.WriteLine(">> Saving Libra wallet mnemonic recovery seed to disk");
            if (cmd.Length <= 2) 
            {
                Console.WriteLine("[ERROR] Error writing mnemonic recovery seed to file: Invalid number of arguments for writing recovery");
                return;
            }

            var writer = new StreamWriter(cmd[2]);
            writer.Write($"{wallet.Mnemonic.ToString()};{wallet.Accounts.Count}");
            writer.Close();
        }

        async Task AccountMint(string[] cmd)
        {
            Console.WriteLine(">> Minting coins");
            if (cmd.Length <= 3) 
            {
                Console.WriteLine("[ERROR] Error minting coins: Invalid number of arguments for mint");
                return;
            }

            string address;
            try
            {
                address = GetAddressOrIndex(cmd[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            
            ulong amount;
            if (!ulong.TryParse(cmd[3], out amount))
            {
                Console.WriteLine("[ERROR] Error minting coins: Invalid decimal: unknown character");
                return;
            }            
            
            await client.MintWithFaucetService(address, amount);
            Console.WriteLine("Mint request submitted");
        }

        private string GetAddressOrIndex(string addressOrIndex)
        {
            string address;
            int index;
            if (int.TryParse(addressOrIndex, out index))
            {
                if (wallet.Accounts.Count < index + 1)
                {
                    throw new Exception("[ERROR] Error minting coins: Unable to find account by account reference id: 0, to see all existing accounts, run: 'account list'");
                }
                address = wallet.Accounts.ElementAt(index).Value.Address;
            }
            else address = addressOrIndex;

            return address;
        }

        private async Task Query(string[] cmd)
        {
            if (cmd.Length <= 1)
            {
                QueryHelp();
                return;
            }

            switch (cmd[1])
            {
                case "b":
                case "balance":     
                    await QueryBalance(cmd);              
                    break;
                case "s":
                case "sequence":
                    Console.WriteLine("Not implemented method.");
                    break;
                case "as":
                case "account_state":
                    Console.WriteLine("Not implemented method.");
                    break;
                case "ts":
                case "txn_acc_seq":
                    Console.WriteLine("Not implemented method.");
                    break;
                case "tr":              
                case "txn_range":
                    Console.WriteLine("Not implemented method.");
                    break;
                case "ev":              
                case "event":
                    Console.WriteLine("Not implemented method.");
                    break;
                default:
                    QueryHelp();
                    break;
            }
        }

        void QueryHelp()
        {
            Console.WriteLine(
@"usage: query <arg>

Use the following args for this command:

balance | b <account_ref_id>|<account_address>
	Get the current balance of an account
sequence | s <account_ref_id>|<account_address> [reset_sequence_number=true|false]
	Get the current sequence number for an account, and reset current sequence number in CLI (optional, default is false)
account_state | as <account_ref_id>|<account_address>
	Get the latest state for an account
txn_acc_seq | ts <account_ref_id>|<account_address> <sequence_number> <fetch_events=true|false>
	Get the committed transaction by account and sequence number.  Optionally also fetch events emitted by this transaction.
txn_range | tr <start_version> <limit> <fetch_events=true|false>
	Get the committed transactions by version range. Optionally also fetch events emitted by these transactions.
event | ev <account_ref_id>|<account_address> <sent|received> <start_sequence_number> <ascending=true|false> <limit>
	Get events by account and event type (sent|received).
");
        }

        private async Task QueryBalance(string[] cmd)
        {
            string address;
            try
            {
                address = GetAddressOrIndex(cmd[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            try
            {
                var state = await client.QueryBalance(address);
                Console.WriteLine($"Sequence Number: {state.SequenceNumber}");
                Console.WriteLine($"Balance is: {(double)state.Balance/1000000:N6}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task Transfer(string[] cmd)
        {
            if (cmd.Length <= 1)
            {
                TransferHelp();
                return;
            }

            if (cmd.Length < 4) 
            {
                Console.WriteLine("Invalid number of arguments for transfer");
                TransferHelp();
                return;
            }

            var senderAddress = GetAddressOrIndex(cmd[1]);
            var receiverAddress = GetAddressOrIndex(cmd[2]);
            var amount = ulong.Parse(cmd[3]);
            var gasUnitPrice = 0UL;
            var maxGasAmount = 100000UL;
            if (senderAddress.Length == 5) gasUnitPrice = ulong.Parse(cmd[4]);
            if (senderAddress.Length == 6) maxGasAmount = ulong.Parse(cmd[5]);

            try
            {
                var response = await client.TransferCoins(wallet.Accounts[senderAddress], receiverAddress, amount, gasUnitPrice, maxGasAmount);
                Console.WriteLine($"AC Status: {response.AcStatus.Code}"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void TransferHelp()
        {
            Console.WriteLine(
@"transfer | transferb | t | tb
	<sender_account_address>|<sender_account_ref_id> <receiver_account_address>|<receiver_account_ref_id> <number_of_coins> [gas_unit_price_in_micro_libras (default=0)] [max_gas_amount_in_micro_libras (default 100000)] Suffix 'b' is for blocking.
");
        }

    }
}
