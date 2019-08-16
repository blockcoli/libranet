using System;

namespace Blockcoli.Libra.Net.Wallet
{
    public class Seed
    {
        byte[] data;
        public byte[] ToBytes() => data;

        public static Seed FromMnemonic(Mnemonic mnemonic, string salt = "LIBRA")
        {
            var bytes = mnemonic.ToBytes().Pbkdf($"{Constant.KeyPrefixes.MnemonicSalt}{salt}", 2048, 32, HashAlgorithm.SHA3_256);
            return new Seed(bytes);
        }

        public static Seed FromMnemonic(string[] words, string salt = "LIBRA")
        {
            var mnemonic = new Mnemonic(words);
            return FromMnemonic(mnemonic, salt);
        }

        public Seed(byte[] data)
        {
            if (data.Length != 32)
            {
                throw new Exception("Seed data length must be 32 bits");
            }
            this.data = data;
        }
    }
}