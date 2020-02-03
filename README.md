# Blockcoli.Libra.Net
Libra Wallet SDK for .NET

Blockcoli is an open source project which is the world 1st .NET SDK for creating Libra wallet. It is hosted on Github repository and also built as a Nuget package. 

What's New
- Update Proto file

## Installation
To install the nuget package from a command line, run:

```
dotnet add package Blockcoli.Libra.Net
```

Or from Visual Studio Package Manager Console
```
Install-Package Blockcoli.Libra.Net
```

## Usage

At the top of your class file, add `using` for two namespaces.

```c#
using Blockcoli.Libra.Net.Wallet;
using Blockcoli.Libra.Net.Client;
```

### Creating an account

In order to create a libra account, you would need to instantiate the `LibraWallet` as shown in the following code example:

```c#
// Create a new wallet
var wallet = new LibraWallet();

// Create a new account
var account = wallet.NewAccount();

// Mnemonic words (secret) for recovering wallet
var mnemonic = wallet.Mnemonic;
Console.WriteLine(mnemonic);

// Get your account address
Console.WriteLine(account.Address);
```

### Recovering an account

In order to create a libra account, you would need to instantiate the `LibraWallet` as shown in the following code example:

```c#
// Recovering a wallet with mnemonic words (secret)
var mnemonic = "elite kidney kangaroo enhance list mule paddle arrange popular frown ahead carry endorse thunder toy broken absorb level surprise arrow analyst segment wave fat";

// Create a wallet
var wallet = new LibraWallet(mnemonic);

// Recover an Account
var account = wallet.NewAccount();

// Get your account address
Console.WriteLine(account.Address);
```

### Minting an amount
To mint an amount, you would need to create a `LibraClient` and use it to mint

```c#
// Currently minting only works for testnet and uses the faucet service.
var client = new LibraClient(LibraNetwork.Testnet);

// Mint amout in Micro-Libra. (1 Libra = 1000000 Micro-Libra)
var sequenceNumber = await client.MintWithFaucetService(account.Address, 1000000);
```

### Checking an address balance

```c#
// You can use parameter array for checking multiple addresses.
var accountState = await client.QueryBalance(account.Address);
Console.WriteLine(accountState.Balance);
```

### Transferring Libra coins

```c#
// You can use parameter array for checking multiple addresses.
var sender = wallet.Accounts.ElementAt(0).Value;
var receiverAddress = "face4412ab3325cf6e26624cff089eb1bf8ec6da489f05aad72a81de5ff7b5d1";
var amount = 10000000UL;
var success = await client.TransferCoins(sender, receiverAddress, amount);
Console.WriteLine($"AC Status: {success}");
```

Related projects
----------------

- Libra Core by perfectmak : https://github.com/perfectmak/libra-core
- Libra Core by kulapio : https://github.com/kulapio/libra-core
- Elliptic-curve cryptography by PeterWaher : https://github.com/PeterWaher/IoTGateway
- 1950Labs / 2019_POC_Libra : https://github.com/1950Labs/2019_POC_Libra

License
----------------
MIT License Copyright (c) 2019 Blockcoli

### Official website
[blockcoli.com](https://blockcoli.com)
