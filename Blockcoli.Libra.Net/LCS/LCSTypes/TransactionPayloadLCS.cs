﻿///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;

namespace Blockcoli.Libra.Net.LCS
{
    public class TransactionPayloadLCS
    {
        public TransactionPayloadType PayloadType { get; set; }

        public ProgramLCS Program { get; set; }
        public WriteSetLCS WriteSet { get; set; }
        public ScriptLCS Script { get; set; }
        public ModuleLCS Module { get; set; }

        public override string ToString()
        {
            string retStr = "{" +
                string.Format("PayloadType = {0},{1}", PayloadType, Environment.NewLine);

            if (PayloadType == TransactionPayloadType.Program)
            {
                retStr += string.Format(" Program = {0},", Program);
            }
            else if (PayloadType == TransactionPayloadType.WriteSet)
            {
                retStr += string.Format(" WriteSet = {0},", WriteSet);
            }
            retStr += "}";
            return retStr;
        }

        public static TransactionPayloadLCS FromProgram(PayloadLCS payloadLCS)
        {
            var program = new ProgramLCS();
            program.Code = payloadLCS.Code;
            program.Modules = payloadLCS.Modules;
            program.TransactionArguments = payloadLCS.TransactionArguments;
            return new TransactionPayloadLCS
            {
                PayloadType = TransactionPayloadType.Program,
                Program = program                    
            };
        }

        public static TransactionPayloadLCS FromScript(PayloadLCS payloadLCS)
        {
            var script = new ScriptLCS();
            script.Code = payloadLCS.Code;
            script.Modules = payloadLCS.Modules;
            script.TransactionArguments = payloadLCS.TransactionArguments;
            return new TransactionPayloadLCS
            {
                PayloadType = TransactionPayloadType.Script,
                Script = script                    
            };
        }
    }
}