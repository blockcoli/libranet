///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;

namespace Blockcoli.Libra.Net.LCS
{
    public class AccessPathLCS
    {
        public AddressLCS Address { get; set; }
        public byte[] Path { get; set; }

        public override string ToString()
        {
            string retVal = "AccessPath {" + Environment.NewLine;
            retVal += "address = " + Address + "," + Environment.NewLine;
            retVal += "}";
            return retVal;
        }
    }
}
