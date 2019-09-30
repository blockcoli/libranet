///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;

namespace Blockcoli.Libra.Net.LCS
{
    public class WriteOpLCS
    {
        public WriteOpType WriteOpType { get; set; }

        public byte[] Value { get; set; }
        public override string ToString()
        {
            if (WriteOpType == LCS.WriteOpType.Value)
            {
                return Value.ByteArrayToString();
            }
            else
                return "Deletion";
        }
    }
}
