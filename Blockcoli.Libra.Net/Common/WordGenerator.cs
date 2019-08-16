///===================================================================================================
///
/// Source https://github.com/jarleli/Bip39Words
/// Owner  https://github.com/jarleli
///
///===================================================================================================

using Blockcoli.Libra.Net.Constant;

namespace Blockcoli.Libra.Net.Common
{
    public static class WordGenerator
    {
        public static CryptoRandom Random = new CryptoRandom(true);

        public static string GetRandomWord()
        {
            return MnemonicWords.Default[GetRandomNumber()];
        }

        public static string[] GetRandomWord(int length)
        {
            var words = new string[length];
            for (var i = 0; i < length; i++)
            {
                words[i] = MnemonicWords.Default[GetRandomNumber()];
            }
            return words;
        }
        public static string GetRandomWordString(int length)
        {
            var words = string.Empty;
            for (var i = 0; i < length; i++)
            {
                words += MnemonicWords.Default[GetRandomNumber()];
                words += " ";
            }

            return words.Trim();
        }


        private static int GetRandomNumber()
        {
            return Random.Next(0, 2048);
        }
    }
}