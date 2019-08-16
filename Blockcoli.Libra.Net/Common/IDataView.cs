namespace Blockcoli.Libra.Net.Common
{
    public interface IDataView
    {
        byte[] Buffer { get; set; }
        int ByteOffset { get; set; }
        int ByteLength { get; set; }

        byte GetUint8(int bytePosition);
        void SetUint8(int byteOffset, byte value);
        sbyte GetInt8(int bytePosition);
        void SetInt8(int byteOffset, sbyte value);

        ushort GetUint16(int bytePosition, bool? littleEndian = null);
        void SetUint16(int byteOffset, ushort value, bool? littleEndian = null);
        short GetInt16(int bytePosition, bool? littleEndian = null);
        void SetInt16(int byteOffset, short value, bool? littleEndian = null);

        uint GetUint32(int bytePosition, bool? littleEndian = null);
        void SetUint32(int byteOffset, uint value, bool? littleEndian = null);
        int GetInt32(int bytePosition, bool? littleEndian = null);
        void SetInt32(int byteOffset, int value, bool? littleEndian = null);

        ulong GetBigUint64(int bytePosition, bool? littleEndian = null);
        void SetBigUint64(int byteOffset, ulong value, bool? littleEndian = null);
        long GetBigInt64(int bytePosition, bool? littleEndian = null);
        void SetBigInt64(int byteOffset, long value, bool? littleEndian = null);

        float GetFloat32(int bytePosition, bool? littleEndian = null);
        void SetFloat32(int byteOffset, float value, bool? littleEndian = null);

        double GetFloat64(int bytePosition, bool? littleEndian = null);
        void SetFloat64(int byteOffset, double value, bool? littleEndian = null);
    }
}