using System;
using Blockcoli.Libra.Net.Common;
using Xunit;
using System.Linq;

namespace Blockcoli.Libra.Net.Test
{
    public class DataViewTest
    {
        [Fact]
        public void SetGet_Uint8()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetUint8(1);
            var expected = 255;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Int8()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetInt8(1, 127);
            var actual = view.GetInt8(1);
            var expected = 127;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint8_Get_Int8()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetInt8(1);
            var expected = -1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint8_Get_Uint16()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetUint16(1);
            var expected = 65280;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint8_Get_Uint32()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetUint32(1);
            var expected = 4278190080U;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint8_Get_BigUint64()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetBigUint64(1);
            var expected = 18374686479671623680UL;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Uint16()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint16(1, 65535);
            var actual = view.GetUint16(1);
            var expected = 65535;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Int16()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetInt16(1, 32767);
            var actual = view.GetInt16(1);
            var expected = 32767;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint16_Get_Int16()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint16(1, 65535);
            var actual = view.GetInt16(1);
            var expected = -1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Uint32()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint32(1, 4294967295);
            var actual = view.GetUint32(1);
            var expected = 4294967295;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Int32()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetInt32(1, 2147483647);
            var actual = view.GetInt32(1);
            var expected = 2147483647;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint32_Get_Int32()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint32(1, 4294967295);
            var actual = view.GetInt32(1);
            var expected = -1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_BigUint64()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetBigUint64(1, 18446744073709551615);
            var actual = view.GetBigUint64(1);
            var expected = 18446744073709551615;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_BigInt64()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetBigInt64(1, 9223372036854775807);
            var actual = view.GetBigInt64(1);
            var expected = 9223372036854775807;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint64_Get_Int64()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetBigUint64(1, 18446744073709551615);
            var actual = view.GetBigInt64(1);
            var expected = -1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Float32()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetFloat32(1, (float)Math.PI);
            var actual = view.GetFloat32(1);
            var expected = 3.1415927410125732f;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Float64()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetFloat64(1, Math.PI);
            var actual = view.GetFloat64(1);
            var expected = 3.141592653589793;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ByteOffset_Uint8()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer, 2, 4);
            view.SetUint8(0, 255);
            var actual = view.Buffer;
            var expected = new byte[16] { 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Assert.True(actual.SequenceEqual(expected));
        }

        [Fact]
        public void Constructor()
        {
            var buffer = new byte[16];
            var view1 = DataView.Create(ref buffer);
            var view2 = DataView.Create(ref buffer, 12, 4);
            view1.SetInt8(12, 42);
            var actual = view2.GetInt8(0);
            var expected = 42;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void Buffer()
        {
            var buffer = new byte[123];
            var view = DataView.Create(ref buffer);
            var actual = view.Buffer.Length;
            var expected = 123;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ByteLength()
        {
            var buffer = new byte[16];
            var view1 = DataView.Create(ref buffer);
            var view2 = DataView.Create(ref buffer, 12, 4);
            var actual = view1.ByteLength + view2.ByteLength;
            var expected = 20;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ByteOffset()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer, 12, 4);
            var actual = view.ByteOffset;
            var expected = 12;
            Assert.Equal(actual, expected);
        }

        //==========================

        [Fact]
        public void Set_Uint8_Get_Uint16_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetUint16(1, true);
            var expected = 255;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint8_Get_Uint32_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetUint32(1, true);
            var expected = 255U;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint8_Get_BigUint64_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint8(1, 255);
            var actual = view.GetBigUint64(1, true);
            var expected = 255UL;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Uint16_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint16(1, 65535, true);
            var actual = view.GetUint16(1, true);
            var expected = 65535;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Int16_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetInt16(1, 32767, true);
            var actual = view.GetInt16(1, true);
            var expected = 32767;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint16_Get_Int16_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint16(1, 65535, true);
            var actual = view.GetInt16(1, true);
            var expected = -1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Uint32_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint32(1, 4294967295, true);
            var actual = view.GetUint32(1, true);
            var expected = 4294967295;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Int32_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetInt32(1, 2147483647, true);
            var actual = view.GetInt32(1, true);
            var expected = 2147483647;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint32_Get_Int32_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetUint32(1, 4294967295, true);
            var actual = view.GetInt32(1, true);
            var expected = -1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_BigUint64_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetBigUint64(1, 18446744073709551615, true);
            var actual = view.GetBigUint64(1, true);
            var expected = 18446744073709551615;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_BigInt64_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetBigInt64(1, 9223372036854775807, true);
            var actual = view.GetBigInt64(1, true);
            var expected = 9223372036854775807;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Uint64_Get_Int64_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetBigUint64(1, 18446744073709551615, true);
            var actual = view.GetBigInt64(1, true);
            var expected = -1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Float32_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetFloat32(1, (float)Math.PI, true);
            var actual = view.GetFloat32(1, true);
            var expected = 3.1415927410125732f;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetGet_Float64_LittleEndian()
        {
            var buffer = new byte[16];
            var view = DataView.Create(ref buffer);
            view.SetFloat64(1, Math.PI, true);
            var actual = view.GetFloat64(1, true);
            var expected = 3.141592653589793;
            Assert.Equal(expected, actual);
        }
    }
}