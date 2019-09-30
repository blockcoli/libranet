///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;
namespace Blockcoli.Libra.Net.LCS
{
    public class TransactionArgumentLCS
    {
        public Types.TransactionArgument.Types.ArgType ArgType { get; set; }

        public ulong U64 { get; set; }
        public AddressLCS Address { get; set; }
        public byte[] ByteArray { get; set; }
        public string String { get; set; }

        public override string ToString()
        {
            if (ArgType == Types.TransactionArgument.Types.ArgType.Address)
                return "{" + $"{ArgType}: {Address}" + "}";
            else if (ArgType == Types.TransactionArgument.Types.ArgType.Bytearray)
                return "{" + $"{ArgType}: {ByteArray.ByteArrayToString()}" + "}";
            else if (ArgType == Types.TransactionArgument.Types.ArgType.String)
                return "{" + $"{ArgType}: {String}" + "}";
            else if (ArgType == Types.TransactionArgument.Types.ArgType.U64)
                return "{"+ $"{ArgType}: {U64}" + "}";


            return "TransactionArgumentLCS is null";
        }
    }
}
