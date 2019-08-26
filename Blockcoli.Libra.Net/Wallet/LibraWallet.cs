using System.Collections.Generic;

namespace Blockcoli.Libra.Net.Wallet
{
    public class LibraWallet
    {
        KeyFactory keyFactory;
        public Mnemonic Mnemonic { get; set; }
        ulong lastChild;
        public Dictionary<string, Account> Accounts { get; set; }

        public LibraWallet(Mnemonic mnemonic)
        {   
            Accounts = new Dictionary<string, Account>();  
            lastChild = 0;   
            this.Mnemonic = mnemonic;
            var seed = Seed.FromMnemonic(mnemonic);             
            keyFactory = new KeyFactory(seed);
        }

        public LibraWallet() : this(new Mnemonic())
        {   
            
        }

        public LibraWallet(string words) : this(new Mnemonic(words.Split(' ')))
        {
            
        }

        public LibraWallet(string[] words) : this(new Mnemonic(words))
        {
            
        }

        public Account NewAccount()
        {
            var account = new Account(keyFactory.GenerateKey(lastChild));
            lastChild++;
            Accounts.Add(account.Address, account);
            return account;
        }
    }
}