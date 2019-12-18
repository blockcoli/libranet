using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockcoli.Libra.Net.LCS
{
    public class PayloadLCS
    {
        public byte[] Code { get; set; }
        public List<TransactionArgumentLCS> TransactionArguments { get; set; }
        public List<byte[]> Modules { get; set; }
    }
}
