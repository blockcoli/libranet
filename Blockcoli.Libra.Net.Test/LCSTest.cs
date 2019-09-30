using Blockcoli.Libra.Net.LCS;
using Blockcoli.Libra.Net;
using Xunit;
using System.Collections.Generic;

namespace Blockcoli.Libra.Net.Test
{
    public class LCSTest
    {
        [Fact]
        public void AccountAddress()
        {
            var addressLCS = new AddressLCS()
            {
                Value = "ca820bf9305eb97d0d784f71b3955457fbf6911f5300ceaa5d7e8621529eae19"
            };
            var actual = LCSCore.LCSDeserialization(addressLCS).ByteArrayToString();
            var expected = "20000000CA820BF9305EB97D0D784F71B3955457FBF6911F5300CEAA5D7E8621529EAE19".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TransactionArgumentU64()
        {
            var u64Argument = new TransactionArgumentLCS()
            {
                U64 = 9213671392124193148,
                ArgType = Types.TransactionArgument.Types.ArgType.U64
            };

            var actual = LCSCore.LCSDeserialization(u64Argument).ByteArrayToString();
            var expected = "000000007CC9BDA45089DD7F".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TransactionArgumentAccountAddress()
        {            
            var transactionArgument = new TransactionArgumentLCS
            {
                ArgType = Types.TransactionArgument.Types.ArgType.Address,
                Address = new AddressLCS()
                {
                    Value = "2c25991785343b23ae073a50e5fd809a2cd867526b3c1db2b0bf5d1924c693ed"
                }
            };
            var actual = LCSCore.LCSDeserialization(transactionArgument).ByteArrayToString();
            var expected = "01000000200000002C25991785343B23AE073A50E5FD809A2CD867526B3C1DB2B0BF5D1924C693ED".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TransactionArgumentString()
        {
            var stringArgument = new TransactionArgumentLCS()
            {
                String = "Hello, World!",
                ArgType = Types.TransactionArgument.Types.ArgType.String
            };

            var actual = LCSCore.LCSDeserialization(stringArgument).ByteArrayToString();
            var expected = "020000000D00000048656C6C6F2C20576F726C6421".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TransactionArgumentByteAddress()
        {        
            var transactionArgument = new TransactionArgumentLCS
            {
                ArgType = Types.TransactionArgument.Types.ArgType.Bytearray,
                ByteArray = "cafed00d".FromHexToBytes()
            };

            var actual = LCSCore.LCSDeserialization(transactionArgument).ByteArrayToString();
            var expected = "0300000004000000CAFED00D".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Program()
        {            
            var program = new ProgramLCS();
            program.Code = "move".ToBytes();
            program.TransactionArguments = new List<TransactionArgumentLCS>();
            program.TransactionArguments.Add(new TransactionArgumentLCS 
            {  
                ArgType = Types.TransactionArgument.Types.ArgType.String,
                String = "CAFE D00D"
            });
            program.TransactionArguments.Add(new TransactionArgumentLCS 
            {  
                ArgType = Types.TransactionArgument.Types.ArgType.String,
                String = "cafe d00d"
            });
            program.Modules = new List<byte[]>();
            program.Modules.Add("CA".FromHexToBytes());
            program.Modules.Add("FED0".FromHexToBytes());
            program.Modules.Add("0D".FromHexToBytes());

            var actual = LCSCore.LCSDeserialization(program).ByteArrayToString();
            var expected = "040000006D6F766502000000020000000900000043414645204430304402000000090000006361666520643030640300000001000000CA02000000FED0010000000D".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccessPath()
        {            
            var accessPath = new AccessPathLCS 
            { 
                Address = new AddressLCS 
                { 
                    Value = "9a1ad09742d1ffc62e659e9a7797808b206f956f131d07509449c01ad8220ad4"                    
                },
                Path =  "01217da6c6b3e19f1825cfb2676daecce3bf3de03cf26647c78df00b371b25cc97".FromHexToBytes()         
            };
            var actual = LCSCore.LCSDeserialization(accessPath).ByteArrayToString();
            var expected = "200000009A1AD09742D1FFC62E659E9A7797808B206F956F131D07509449C01AD8220AD42100000001217DA6C6B3E19F1825CFB2676DAECCE3BF3DE03CF26647C78DF00B371B25CC97".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteOpDeletion()
        {                
            var writeOp = new WriteOpLCS
            {
                WriteOpType = WriteOpType.Deletion                
            };
            var actual = LCSCore.LCSDeserialization(writeOp).ByteArrayToString();
            var expected = "00000000".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteOpValue()
        {          
            var writeOp = new WriteOpLCS
            {
                WriteOpType = WriteOpType.Value,
                Value = "cafed00d".FromHexToBytes()
            };           
            var actual = LCSCore.LCSDeserialization(writeOp).ByteArrayToString();
            var expected = "0100000004000000CAFED00D".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteSet()
        {          
            var writeSet = new WriteSetLCS();
            writeSet.WriteSet = new Dictionary<AccessPathLCS, WriteOpLCS>();
            writeSet.WriteSet.Add(new AccessPathLCS 
            { 
                Address = new AddressLCS 
                { 
                    Value = "a71d76faa2d2d5c3224ec3d41deb293973564a791e55c6782ba76c2bf0495f9a"                    
                },
                Path =  "01217da6c6b3e19f1825cfb2676daecce3bf3de03cf26647c78df00b371b25cc97".FromHexToBytes()         
            }, new WriteOpLCS 
            { 
                WriteOpType = WriteOpType.Deletion
            });

            writeSet.WriteSet.Add(new AccessPathLCS 
            { 
                Address = new AddressLCS 
                { 
                    Value = "c4c63f80c74b11263e421ebf8486a4e398d0dbc09fa7d4f62ccdb309f3aea81f"                    
                },
                Path =  "01217da6c6b3e19f18".FromHexToBytes()         
            }, new WriteOpLCS 
            { 
                WriteOpType = WriteOpType.Value,
                Value = "cafed00d".FromHexToBytes()
            });            
            
            var actual = LCSCore.LCSDeserialization(writeSet).ByteArrayToString();
            var expected = "0200000020000000A71D76FAA2D2D5C3224EC3D41DEB293973564A791E55C6782BA76C2BF0495F9A2100000001217DA6C6B3E19F1825CFB2676DAECCE3BF3DE03CF26647C78DF00B371B25CC970000000020000000C4C63F80C74B11263E421EBF8486A4E398D0DBC09FA7D4F62CCDB309F3AEA81F0900000001217DA6C6B3E19F180100000004000000CAFED00D".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TransactionPayloadWithProgram()
        {          
            var program = new ProgramLCS();
            program.Code = "move".ToBytes();
            program.TransactionArguments = new List<TransactionArgumentLCS>();
            program.TransactionArguments.Add(new TransactionArgumentLCS 
            {  
                ArgType = Types.TransactionArgument.Types.ArgType.String,
                String = "CAFE D00D"
            });
            program.TransactionArguments.Add(new TransactionArgumentLCS 
            {  
                ArgType = Types.TransactionArgument.Types.ArgType.String,
                String = "cafe d00d"
            });
            program.Modules = new List<byte[]>();
            program.Modules.Add("CA".FromHexToBytes());
            program.Modules.Add("FED0".FromHexToBytes());
            program.Modules.Add("0D".FromHexToBytes());

            var transactionPayload = new TransactionPayloadLCS
            {
                PayloadType = TransactionPayloadType.Program,
                Program = program                    
            };
            var actual = LCSCore.LCSDeserialization(transactionPayload).ByteArrayToString();
            var expected = "00000000040000006D6F766502000000020000000900000043414645204430304402000000090000006361666520643030640300000001000000CA02000000FED0010000000D".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TransactionPayloadWithWriteSet()
        {          
            var writeSet = new WriteSetLCS();
            writeSet.WriteSet = new Dictionary<AccessPathLCS, WriteOpLCS>();
            writeSet.WriteSet.Add(new AccessPathLCS 
            { 
                Address = new AddressLCS 
                { 
                    Value = "a71d76faa2d2d5c3224ec3d41deb293973564a791e55c6782ba76c2bf0495f9a"                    
                },
                Path =  "01217da6c6b3e19f1825cfb2676daecce3bf3de03cf26647c78df00b371b25cc97".FromHexToBytes()         
            }, new WriteOpLCS 
            { 
                WriteOpType = WriteOpType.Deletion
            });

            writeSet.WriteSet.Add(new AccessPathLCS 
            { 
                Address = new AddressLCS 
                { 
                    Value = "c4c63f80c74b11263e421ebf8486a4e398d0dbc09fa7d4f62ccdb309f3aea81f"                    
                },
                Path =  "01217da6c6b3e19f18".FromHexToBytes()         
            }, new WriteOpLCS 
            { 
                WriteOpType = WriteOpType.Value,
                Value = "cafed00d".FromHexToBytes()
            });   

            var transactionPayload = new TransactionPayloadLCS
            {
                PayloadType = TransactionPayloadType.WriteSet,
                WriteSet = writeSet    
            };
            var actual = LCSCore.LCSDeserialization(transactionPayload).ByteArrayToString();
            var expected = "010000000200000020000000A71D76FAA2D2D5C3224EC3D41DEB293973564A791E55C6782BA76C2BF0495F9A2100000001217DA6C6B3E19F1825CFB2676DAECCE3BF3DE03CF26647C78DF00B371B25CC970000000020000000C4C63F80C74B11263E421EBF8486A4E398D0DBC09FA7D4F62CCDB309F3AEA81F0900000001217DA6C6B3E19F180100000004000000CAFED00D".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RawTransactionWithProgram()
        {          
            var program = new ProgramLCS();
            program.Code = "move".ToBytes();
            program.TransactionArguments = new List<TransactionArgumentLCS>();
            program.TransactionArguments.Add(new TransactionArgumentLCS 
            {  
                ArgType = Types.TransactionArgument.Types.ArgType.String,
                String = "CAFE D00D"
            });
            program.TransactionArguments.Add(new TransactionArgumentLCS 
            {  
                ArgType = Types.TransactionArgument.Types.ArgType.String,
                String = "cafe d00d"
            });
            program.Modules = new List<byte[]>();
            program.Modules.Add("CA".FromHexToBytes());
            program.Modules.Add("FED0".FromHexToBytes());
            program.Modules.Add("0D".FromHexToBytes());

            var transaction = new RawTransactionLCS
            {
                Sender = new AddressLCS
                {
                    Value = "3a24a61e05d129cace9e0efc8bc9e33831fec9a9be66f50fd352a2638a49b9ee"
                },
                SequenceNumber = 32,
                TransactionPayload = new TransactionPayloadLCS
                {
                    PayloadType = TransactionPayloadType.Program,
                    Program = program                    
                },
                MaxGasAmount = 10000UL,
                GasUnitPrice = 20000UL,
                ExpirationTime = 86400UL
            };   
            var actual = LCSCore.LCSDeserialization(transaction).ByteArrayToString();
            var expected = "200000003A24A61E05D129CACE9E0EFC8BC9E33831FEC9A9BE66F50FD352A2638A49B9EE200000000000000000000000040000006D6F766502000000020000000900000043414645204430304402000000090000006361666520643030640300000001000000CA02000000FED0010000000D1027000000000000204E0000000000008051010000000000".ToLower();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RawTransaction()
        {          
            var writeSet = new WriteSetLCS();
            writeSet.WriteSet = new Dictionary<AccessPathLCS, WriteOpLCS>();
            writeSet.WriteSet.Add(new AccessPathLCS 
            { 
                Address = new AddressLCS 
                { 
                    Value = "a71d76faa2d2d5c3224ec3d41deb293973564a791e55c6782ba76c2bf0495f9a"                    
                },
                Path =  "01217da6c6b3e19f1825cfb2676daecce3bf3de03cf26647c78df00b371b25cc97".FromHexToBytes()         
            }, new WriteOpLCS 
            { 
                WriteOpType = WriteOpType.Deletion
            });

            writeSet.WriteSet.Add(new AccessPathLCS 
            { 
                Address = new AddressLCS 
                { 
                    Value = "c4c63f80c74b11263e421ebf8486a4e398d0dbc09fa7d4f62ccdb309f3aea81f"                    
                },
                Path =  "01217da6c6b3e19f18".FromHexToBytes()         
            }, new WriteOpLCS 
            { 
                WriteOpType = WriteOpType.Value,
                Value = "cafed00d".FromHexToBytes()
            });   

            var transaction = new RawTransactionLCS
            {
                Sender = new AddressLCS
                {
                    Value = "c3398a599a6f3b9f30b635af29f2ba046d3a752c26e9d0647b9647d1f4c04ad4"
                },
                SequenceNumber = 32,
                TransactionPayload = new TransactionPayloadLCS
                {
                    PayloadType = TransactionPayloadType.WriteSet,
                    WriteSet = writeSet
                },
                MaxGasAmount = 0UL,
                GasUnitPrice = 0UL,
                ExpirationTime = 18446744073709551615UL
            };   
            var actual = LCSCore.LCSDeserialization(transaction).ByteArrayToString();
            var expected = "20000000C3398A599A6F3B9F30B635AF29F2BA046D3A752C26E9D0647B9647D1F4C04AD42000000000000000010000000200000020000000A71D76FAA2D2D5C3224EC3D41DEB293973564A791E55C6782BA76C2BF0495F9A2100000001217DA6C6B3E19F1825CFB2676DAECCE3BF3DE03CF26647C78DF00B371B25CC970000000020000000C4C63F80C74B11263E421EBF8486A4E398D0DBC09FA7D4F62CCDB309F3AEA81F0900000001217DA6C6B3E19F180100000004000000CAFED00D00000000000000000000000000000000FFFFFFFFFFFFFFFF".ToLower();
            Assert.Equal(expected, actual);
        }
    }
}