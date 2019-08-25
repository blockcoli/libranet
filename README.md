# Blockcoli.Libra.Net
Libra Wallet SDK for .NET

Build from .NET Standard 2.1 Preview in .NET Core 3.0 Preview 8

## Installation
To install with nuget run:

```
dotnet add package Blockcoli.Libra.Net --version 0.1.0
```

## Usage

Using two namespaces

```c#
using Blockcoli.Libra.Net.Wallet;
using Blockcoli.Libra.Net.Client;
```

### Creating an Account

In order to create a libra account, you would need to instantiate the `LibraWallet` like:

```c#
// Creating a new Wallet
var wallet = new LibraWallet();

// Generate a new Account
var account = wallet.NewAccount();

// you can see your address by:
Console.WriteLine(account.Address);
```

### Minting Amount
To mint you need to create a `LibraClient` and use it to mint

```c#
// Currently minting only works for testnet and uses the faucet service.
var client = new LibraClient(LibraNetwork.Testnet);

// Mint amout in Micro-Libra. (1 Libra = 1000000 Micro-Libra)
var sequenceNumber = await client.MintWithFaucetService(account.Address, 1000000);
```

### Checking an address balance

```c#
// You can use parameter array for check multiple address.
var accountStates = await client.QueryBalance(account.Address);
Console.WriteLine(accountStates[0].Balance);
```

Related projects
----------------

- Libra Core by perfectmak : https://github.com/perfectmak/libra-core
- Libra Core by kulapio : https://github.com/kulapio/libra-core
- Elliptic-curve cryptography by PeterWaher : https://github.com/PeterWaher/IoTGateway

License
----------------
MIT License Copyright (c) 2019 Blockcoli
