using System;

namespace Blockcoli.Libra.Net.Common
{
    public class DataView : IDataView
    {
        public byte[] Buffer { get; set; }
        public int ByteOffset { get; set; }
        public int ByteLength { get; set; }

        public static IDataView Create(ref byte[] buffer, int offset, int length)
        {
            return new DataView(ref buffer, offset, length);
        }

        public static IDataView Create(ref byte[] buffer)
        {
            return new DataView(ref buffer, 0, buffer.Length);
        }

        private DataView(ref byte[] buffer, int offset, int length)
        {
            this.Buffer = buffer;
            this.ByteOffset = offset;
            this.ByteLength = length;
        }

        private DataView(ref byte[] buffer) : this(ref buffer, 0, buffer.Length)
        {
            
        }

        public byte GetUint8(int byteOffset)
        {
            return Buffer[this.ByteOffset + byteOffset];
        }

        public void SetUint8(int byteOffset, byte value)
        {
            Buffer[this.ByteOffset + byteOffset] = value;            
        }

        public sbyte GetInt8(int byteOffset)
        {
            return (sbyte)Buffer[this.ByteOffset + byteOffset];
        }

        public void SetInt8(int byteOffset, sbyte value)
        {
            Buffer[this.ByteOffset + byteOffset] = (byte)value; 
        }

        public ushort GetUint16(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 2;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);         
        }

        public void SetUint16(int byteOffset, ushort value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length);            
        }

        public short GetInt16(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 2;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);            
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0); 
        }

        public void SetInt16(int byteOffset, short value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length); 
        }

        public uint GetUint32(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 4;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0); 
        }

        public void SetUint32(int byteOffset, uint value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length); 
        }

        public int GetInt32(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 4;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0); 
        }

        public void SetInt32(int byteOffset, int value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length); 
        }

        public ulong GetBigUint64(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 8;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0); 
        }

        public void SetBigUint64(int byteOffset, ulong value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length); 
        }

        public long GetBigInt64(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 8;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0); 
        }

        public void SetBigInt64(int byteOffset, long value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length); 
        }

        public float GetFloat32(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 4;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0); 
        }

        public void SetFloat32(int byteOffset, float value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length); 
        }

        public double GetFloat64(int byteOffset, bool? littleEndian = null)
        {
            var byteCount = 8;
            var bytes = new byte[byteCount];
            Array.Copy(Buffer, byteOffset, bytes, 0, byteCount);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0); 
        }

        public void SetFloat64(int byteOffset, double value, bool? littleEndian = null)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!littleEndian.HasValue || !littleEndian.Value) Array.Reverse(bytes);
            Array.Copy(bytes, 0, Buffer, byteOffset, bytes.Length);
        }

    }
}