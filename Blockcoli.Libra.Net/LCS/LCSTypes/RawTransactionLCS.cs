///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;

namespace Blockcoli.Libra.Net.LCS
{
    public class RawTransactionLCS
    {
        public ulong MaxGasAmount { get; set; }
        public ulong GasUnitPrice { get; set; }
        public ulong ExpirationTime { get; set; }
        public AddressLCS Sender { get; set; }
        public ulong SequenceNumber { get; set; }
        public TransactionPayloadLCS TransactionPayload { get; set; }

        public override string ToString()
        {
            string retStr = "{" + string.Format("sender: {0},{1}", Sender, Environment.NewLine);
            retStr += string.Format("sequence_number: {0},{1}", SequenceNumber, Environment.NewLine);
            retStr += string.Format("payload: {0},{1}", TransactionPayload, Environment.NewLine);
            retStr += string.Format("max_gas_amount: {0},{1}", MaxGasAmount, Environment.NewLine);
            retStr += string.Format("gas_unit_price: {0},{1}", GasUnitPrice, Environment.NewLine);
            retStr += string.Format("expiration_time: {0} seconds", ExpirationTime) + "}";
            return retStr;
        }
    }
}
