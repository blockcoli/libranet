using Google.Protobuf;
using Blockcoli.Libra.Net.Crypto;
using System.Linq;

namespace Blockcoli.Libra.Net.Wallet
{
    public class Account
    {
        public string Address { get; private set; }
        private Edwards25519 keyPair;        
        public ByteString Signature
        {
            get
            {
                var bytes = Constant.HashSaltValues.RawTransactionHashSalt.ToBytes().Concat(Address.ToBytes()).ToArray();
                var hash = new SHA3_256().ComputeVariable(bytes);
                return keyPair.Sign(hash).ToByteString();
            }
        }

        public ByteString PublicKey => keyPair.PublicKey.ToByteString();

        public static Account FromSecretKey(byte[] secretKey)
        {
            return new Account(new Edwards25519(secretKey));
        }

        public static Account FromSecretKey(string secretKeyHex)
        {
            return Account.FromSecretKey(secretKeyHex.ToBytes());
        }

        public Account(Edwards25519 keyPair)
        {
            this.keyPair = keyPair;
            this.Address = new SHA3_256().ComputeVariable(this.keyPair.PublicKey).ToHexString();
        }
    }
}