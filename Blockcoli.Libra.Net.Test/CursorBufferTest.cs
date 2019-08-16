using Blockcoli.Libra.Net.Common;
using Xunit;
using System.Linq;

namespace Blockcoli.Libra.Net.Test
{
    public class CursorBufferTest
    {
        [Fact]
        public void Read8()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var cursor = new CursorBuffer(bytes);
            var actual = cursor.Read8();
            actual = cursor.Read8();
            actual = cursor.Read8();
            var expected = 3;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void Read32()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var cursor = new CursorBuffer(bytes, false);
            var actual = cursor.Read32();
            actual = cursor.Read32();
            actual = cursor.Read32();
            var expected = 151653132U;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void Read32_LittleEndian()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var cursor = new CursorBuffer(bytes, true);
            var actual = cursor.Read32();
            actual = cursor.Read32();
            actual = cursor.Read32();
            var expected = 202050057U;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void Read64()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var cursor = new CursorBuffer(bytes, false);
            var actual = cursor.Read64();
            actual = cursor.Read64();
            var expected = 651345242494996240UL;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void Read64_LittleEndian()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var cursor = new CursorBuffer(bytes, true);
            var actual = cursor.Read64();
            actual = cursor.Read64();
            var expected = 1157159078456920585UL;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ReadXBytes()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var cursor = new CursorBuffer(bytes, 1, bytes.Length-1);
            var actual = cursor.ReadXBytes(3);
            actual = cursor.ReadXBytes(3);
            actual = cursor.ReadXBytes(3);
            var expected = new byte[] { 8, 9, 10 };
            Assert.True(actual.SequenceEqual(expected));
        }

        [Fact]
        public void ReadBool1()
        {
            var bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var cursor = new CursorBuffer(bytes);
            var actual = cursor.ReadBool();
            var expected = false;
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ReadBool2()
        {
            var bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var cursor = new CursorBuffer(bytes);
            var actual = cursor.ReadBool();
            actual = cursor.ReadBool();
            var expected = true;
            Assert.Equal(actual, expected);
        }
    }
}