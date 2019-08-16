using System;

namespace Blockcoli.Libra.Net.Common
{
    public class CursorBuffer
    {
        readonly IDataView dataView;
        readonly bool littleEndian;
        int bytePosition;

        public CursorBuffer(byte[] typedArray, bool littleEndian = true)
        {
            this.dataView = DataView.Create(ref typedArray);
            this.littleEndian = littleEndian;
            this.bytePosition = 0;
        }

        public CursorBuffer(byte[] typedArray, int offset, int length, bool littleEndian = true)
        {
            this.dataView = DataView.Create(ref typedArray, offset, length);
            this.littleEndian = littleEndian;
            this.bytePosition = 0;
        }

        public byte Read8()
        {
            var value = dataView.GetUint8(bytePosition);
            this.bytePosition += 1;
            return value;
        }

        public uint Read32()
        {
            var value = dataView.GetUint32(bytePosition, littleEndian);
            this.bytePosition += 4;
            return value;
        }

        public ulong Read64()
        {
            var value = dataView.GetBigUint64(bytePosition, littleEndian);
            this.bytePosition += 8;
            return value;
        }

        public byte[] ReadXBytes(int x)
        {
            var startPosition = bytePosition + dataView.ByteOffset;
            var value = new byte[x];
            Array.Copy(dataView.Buffer, startPosition, value, 0, x);
            bytePosition += x;
            return value;
        }

        public bool ReadBool()
        {
            var value = dataView.GetUint8(bytePosition);
            this.bytePosition += 1;
            if (value != 0 && value != 1)
            {
                throw new Exception($"bool must be 0 or 1, found ${value}");
            }

            return value != 0;
        }
    }
}