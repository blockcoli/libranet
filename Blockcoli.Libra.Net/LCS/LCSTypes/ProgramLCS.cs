///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockcoli.Libra.Net.LCS
{
    public class ProgramLCS : PayloadLCS
    {
        public override string ToString()
        {
            string retStr = "{" +
                string.Format("CodeStringLength = {0},{1}", Code.Length,
                Environment.NewLine);
            retStr += string.Format("CodeString = {0},{1}", Code,
                Environment.NewLine);
            retStr += "Arguments = [";
            foreach (var item in TransactionArguments)
            {
                retStr += item;
                if (item != TransactionArguments.Last())
                    retStr += string.Format(",{0}", Environment.NewLine);
            }
            retStr += string.Format("],{0}", Environment.NewLine);
            retStr += "Modules = [";
            foreach (var item in Modules)
            {
                retStr += item.ByteArrayToString();
                if (item != Modules.Last())
                    retStr += string.Format(",{0}", Environment.NewLine);
            }
            retStr += "]";
            return retStr;
        }
    }
}
