using System;
using System.Linq;
using Blockcoli.Libra.Net.Common;

namespace Blockcoli.Libra.Net.Wallet
{
    public class Mnemonic
    {
        string[] words;
        public string[] ToArray() => words;
        public override string ToString() => string.Join(" ", words);
        public byte[] ToBytes()
        {
            var wordsString = ToString();
            var uintArray = new byte[wordsString.Length];
            for (int idx = 0; idx < uintArray.Length; idx++)
            {
                uintArray[idx] = (byte)wordsString[idx];
            }
            return uintArray;            
        }

        public Mnemonic(params string[] words)
        {
            if (words.Length <= 0)
            {
                this.words = WordGenerator.GetRandomWord(24);  
                return;              
            }

            if (words.Length < 6 || words.Length % 6 != 0) throw new Exception("Mnemonic must have a word count divisible with 6");            

            foreach (var word in words) 
            {
                if (!Constant.MnemonicWords.Default.Contains(word)) throw new Exception("Mnemonic contains an unknown word");                
            }
            
            this.words = words;
        }
    }
}