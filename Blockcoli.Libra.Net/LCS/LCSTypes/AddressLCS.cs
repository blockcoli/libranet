///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

namespace Blockcoli.Libra.Net.LCS
{
    public class AddressLCS
    {
        public byte[] ValueByte { get; set; }
        public string Value { get; set; }
        public uint Length { get; set; } = 32;

        public override string ToString()
        {
            return Value;
        }
    }
}
