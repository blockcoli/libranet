using Google.Protobuf;
using Blockcoli.Libra.Net.Crypto;
using System.Linq;

namespace Blockcoli.Libra.Net.Wallet
{
    public class Account
    {
        public string Address { get; private set; }
        public Edwards25519 KeyPair { get; private set; }    
        public ByteString Signature
        {
            get
            {
                var bytes = Constant.HashSaltValues.RawTransactionHashSalt.ToBytes().Concat(Address.ToBytes()).ToArray();
                var hash = new SHA3_256().ComputeVariable(bytes);
                return KeyPair.Sign(hash).ToByteString();
            }
        }

        public ByteString PublicKey => KeyPair.PublicKey.ToByteString();

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
            this.KeyPair = keyPair;
            this.Address = new SHA3_256().ComputeVariable(this.KeyPair.PublicKey).ToHexString();
        }
    }
}