///===================================================================================================
///
/// Source https://github.com/1950Labs/2019_POC_Libra
///
///===================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blockcoli.Libra.Net.LCS
{
    public class LibraCanonicalDeserialization
    {
        public byte[] U64ToByte(ulong source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] AddressToByte(AddressLCS source)
        {
            var len = U32ToByte((uint)source.Length);
            var data = source.Value.HexStringToByteArray();
            return len.Concat(data).ToArray();
        }

        public byte[] U32ToByte(uint source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] StringToByte(string source)
        {
            var data = Encoding.UTF8.GetBytes(source);
            var len = U32ToByte((uint)data.Length);
            return len.Concat(data).ToArray();
        }

        public byte[] ByteArrayToByte(byte[] source)
        {
            var len = U32ToByte((uint)source.Length);
            var data = source;
            return len.Concat(data).ToArray();
        }

        public byte[] ListByteArrayToByte(List<byte[]> source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Count);
            retArr = retArr.Concat(len).ToList();
            foreach (var item in source)
            {
                var localLen = U32ToByte((uint)item.Length);
                retArr = retArr.Concat(localLen).ToList();
                retArr = retArr.Concat(item).ToList();
            }
            return retArr.ToArray();
        }

        public byte[] BoolToByte(bool source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] TransactionPayloadToByte(TransactionPayloadLCS source)
        {
            var result = U32ToByte((uint)source.PayloadType);
            switch (source.PayloadType)
            {
                case TransactionPayloadType.Program:
                    var pro = LCSCore.LCSDeserialization(source.Program);
                    result = result.Concat(pro).ToArray();
                    break;
                case TransactionPayloadType.WriteSet:
                    var writeSet = LCSCore.LCSDeserialization(source.WriteSet);
                    result = result.Concat(writeSet).ToArray();
                    break;
            }
            return result;
        }

        public byte[] ProgramToByte(ProgramLCS source)
        {
            var result = ByteArrayToByte(source.Code);
            var argLen = U32ToByte((uint)source.TransactionArguments.Count);
            result = result.Concat(argLen).ToArray();
            foreach (var arg in source.TransactionArguments)
            {
                var argData = LCSCore.LCSDeserialization(arg);
                result = result.Concat(argData).ToArray();
            }

            var module = LCSCore.LCSDeserialization(source.Modules);
            result = result.Concat(module).ToArray();

            return result;
        }

        public byte[] AccessPathToByte(AccessPathLCS source)
        {
            var result = LCSCore.LCSDeserialization(source.Address);
            var path = LCSCore.LCSDeserialization(source.Path);
            result = result.Concat(path).ToArray();
            return result;
        }

        public byte[] WriteOpToByte(WriteOpLCS source)
        {
            switch (source.WriteOpType)
            {
                case WriteOpType.Value:
                    var type = U32ToByte((uint)source.WriteOpType);
                    var data = LCSCore.LCSDeserialization(source.Value);
                    return type.Concat(data).ToArray();
                default:
                    return U32ToByte((uint)source.WriteOpType);
            }
        }

        public byte[] WriteSetToByte(WriteSetLCS source)
        {
            var result = U32ToByte((uint)source.WriteSet.Count);
            foreach (var writeSet in source.WriteSet)
            {
                var key = LCSCore.LCSDeserialization(writeSet.Key);
                result = result.Concat(key).ToArray();
                var ops = LCSCore.LCSDeserialization(writeSet.Value);
                result = result.Concat(ops).ToArray();
            }

            return result;
        }

        public byte[] RawTransactionToByte(RawTransactionLCS source)
        {
            var result = LCSCore.LCSDeserialization(source.Sender);
            var sequence = LCSCore.LCSDeserialization(source.SequenceNumber);
            result = result.Concat(sequence).ToArray();
            var payload = LCSCore.LCSDeserialization(source.TransactionPayload);
            result = result.Concat(payload).ToArray();
            var max = LCSCore.LCSDeserialization(source.MaxGasAmount);
            result = result.Concat(max).ToArray();
            var gas = LCSCore.LCSDeserialization(source.GasUnitPrice);
            result = result.Concat(gas).ToArray();
            var expire = LCSCore.LCSDeserialization(source.ExpirationTime);
            result = result.Concat(expire).ToArray();
            return result;
        }

        public byte[] ScriptToByte(ScriptLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] ModuleToByte(ModuleLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] TransactionArgumentToByte(TransactionArgumentLCS source)
        {
            var len = U32ToByte((uint)source.ArgType);
            byte[] data;
            switch (source.ArgType)
            {
                case Types.TransactionArgument.Types.ArgType.Address:               
                    data = LCSCore.LCSDeserialization(source.Address);  
                    return len.Concat(data).ToArray();               
                case Types.TransactionArgument.Types.ArgType.Bytearray:
                    data = ByteArrayToByte(source.ByteArray);
                    return len.Concat(data).ToArray();
                case Types.TransactionArgument.Types.ArgType.String:
                    data = StringToByte(source.String);
                    return len.Concat(data).ToArray();
                case Types.TransactionArgument.Types.ArgType.U64:                                        
                    data = BitConverter.GetBytes(source.U64);
                    return len.Concat(data).ToArray();
            }

            throw new InvalidOperationException();
        }

        public byte[] ListTransactionArgumentToByte(List<TransactionArgumentLCS> source)
        {
            throw new NotImplementedException();
        }
    }
}