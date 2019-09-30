///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;

namespace Blockcoli.Libra.Net.LCS
{
    public class ModuleLCS
    {
        public byte[] Code { get; internal set; }

        public override string ToString()
        {
            string retStr = "{" +
                string.Format("CodeStringLength = {0},{1}", Code.Length, Environment.NewLine);
            retStr += string.Format("CodeString = {0},{1}", Code, Environment.NewLine);
            retStr += "]";
            return retStr;
        }
    }
}
