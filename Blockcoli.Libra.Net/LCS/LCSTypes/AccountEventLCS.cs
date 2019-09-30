///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;

namespace Blockcoli.Libra.Net.LCS
{
    public class AccountEventLCS
    {
        public string Account { get; internal set; }
        public ulong Amount { get; internal set; }

        public override string ToString()
        {
            return "{ Account = " + Account + "," + Environment.NewLine +
                "Amount = " + Amount
                + "}";
        }
    }
}
