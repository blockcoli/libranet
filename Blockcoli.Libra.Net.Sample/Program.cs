using System;
using System.Threading.Tasks;
using System.Linq;
using Blockcoli.Libra.Net.Wallet;
using Blockcoli.Libra.Net.Client;

namespace Blockcoli.Libra.Net.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
			LibraWallet wallet = null;
			var client = new LibraClient(LibraNetwork.Testnet);

			while (true)
			{
				Console.WriteLine("=== Blockcoli.Libra.Net ===");
				Console.WriteLine("1) Create Wallet");
				Console.WriteLine("2) Recover Wallet");
				if (wallet != null) Console.WriteLine($"3) Create Account ({wallet.Accounts.Count})");
				else Console.WriteLine($"3) Create Account");
				Console.WriteLine("4) List Account");
				Console.WriteLine("5) Mint");
				Console.WriteLine("6) Query Balance");
				Console.WriteLine("7) Transfer");
				Console.Write("Menu = ");
				var menu = Console.ReadLine();
				switch (menu)
				{
					case "1":
						wallet = new LibraWallet();
						Console.WriteLine($"Mnemonic = {wallet.Mnemonic.ToString()}");
						break;
					case "2":
						Console.Write("Mnemonic = ");
						var mnemonic = Console.ReadLine();
						wallet = new LibraWallet(mnemonic);
						break;
					case "3":
						if (wallet == null)
						{
							wallet = new LibraWallet();
							Console.WriteLine($"Mnemonic = {wallet.Mnemonic.ToString()}");
						}
						var account = wallet.NewAccount();
						Console.WriteLine($"Address = {account.Address}");
						break;
					case "4":
						foreach (var item in wallet.Accounts)
						{
							Console.WriteLine(item.Value.Address);
						}
						break;
					case "5":
						Console.Write("Address = ");
						var address = Console.ReadLine();
						if (wallet.Accounts.ContainsKey(address))
						{
							Console.Write("Amount (LIB) = ");
							var amount = ulong.Parse(Console.ReadLine()) * 1000000;
							var sequenceNumber = await client.MintWithFaucetService(wallet.Accounts[address].Address, amount);
							Console.WriteLine($"Sequence Number = {sequenceNumber}");
						}
						else Console.WriteLine("Invalid address.");
						break;
					case "6":
						var accountStates = await client.QueryBalances(wallet.Accounts.Values.Select(a => a.Address).ToArray());
						accountStates.ForEach(accountState => {
							Console.WriteLine($"{(double)accountState.Balance / 1000000}");
						});
						break;
					case "7":
						Console.Write("Sender Address = ");
						var sender = Console.ReadLine();
						if (wallet.Accounts.ContainsKey(sender))
						{
							Console.Write("Recipient Address = ");
							var recipientAddress = Console.ReadLine();
							Console.Write("Amount (LIB) = ");
							var amount = ulong.Parse(Console.ReadLine()) * 1000000;
							Console.Write("Gas Unit Price (0) = ");
							var input = Console.ReadLine();
							var gasUnitPrice = string.IsNullOrEmpty(input) ? 0UL : ulong.Parse(input);
							Console.Write("Max Gas Amount (1000000) = ");
							input = Console.ReadLine();
							var maxGasAmount = string.IsNullOrEmpty(input) ? 1000000UL : ulong.Parse(input);
							var response = await client.TransferCoins(wallet.Accounts[sender], recipientAddress, amount, gasUnitPrice, maxGasAmount);
							Console.WriteLine(response.AcStatus.Message);
						}
						else Console.WriteLine("Invalid address.");
						break;
				}
			}
		}
    }
}
