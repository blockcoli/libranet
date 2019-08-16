using Blockcoli.Libra.Net.Wallet;
using Xunit;
using System.Linq;

namespace Blockcoli.Libra.Net.Test
{
    public class WalletTest
    {
        [Fact]
        public void Mnemonic()
        {
            var words = new string[] { "total", "usual", "label", "sea", "similar", "attitude", "pilot", "charge", "school", "duck", "funny", "year", "vault", "hood", "second", "various", "next", "critic", "sport", "garden", "track", "opinion", "slight", "across" };
            var mnemonic = new Mnemonic(words);
            var actual = mnemonic.ToBytes();
            var expected = new byte[] { 116, 111, 116, 97, 108, 32, 117, 115, 117, 97, 108, 32, 108, 97, 98, 101, 108, 32, 115, 101, 97, 32, 115, 105, 109, 105, 108, 97, 114, 32, 97, 116, 116, 105, 116, 117, 100, 101, 32, 112, 105, 108, 111, 116, 32, 99, 104, 97, 114, 103, 101, 32, 115, 99, 104, 111, 111, 108, 32, 100, 117, 99, 107, 32, 102, 117, 110, 110, 121, 32, 121, 101, 97, 114, 32, 118, 97, 117, 108, 116, 32, 104, 111, 111, 100, 32, 115, 101, 99, 111, 110, 100, 32, 118, 97, 114, 105, 111, 117, 115, 32, 110, 101, 120, 116, 32, 99, 114, 105, 116, 105, 99, 32, 115, 112, 111, 114, 116, 32, 103, 97, 114, 100, 101, 110, 32, 116, 114, 97, 99, 107, 32, 111, 112, 105, 110, 105, 111, 110, 32, 115, 108, 105, 103, 104, 116, 32, 97, 99, 114, 111, 115, 115 };
            Assert.True(actual.SequenceEqual(expected));
        }

        [Fact]
        public void KeyFactory_GenerateKey()
        {
            var words = new string[] { "total", "usual", "label", "sea", "similar", "attitude", "pilot", "charge", "school", "duck", "funny", "year", "vault", "hood", "second", "various", "next", "critic", "sport", "garden", "track", "opinion", "slight", "across" };       
            var seed = Seed.FromMnemonic(words);            
            var keyFactory = new KeyFactory(seed);
            var eddsa = keyFactory.GenerateKey(255);
            var actual = eddsa.PublicKey;
            var expected = new byte[] { 97, 201, 21, 44, 110, 6, 3, 224, 125, 217, 81, 193, 88, 188, 86, 161, 221, 213, 133, 36, 32, 72, 125, 206, 145, 125, 132, 87, 81, 79, 130, 95 };
            Assert.True(actual.SequenceEqual(expected));
        }

    }
}