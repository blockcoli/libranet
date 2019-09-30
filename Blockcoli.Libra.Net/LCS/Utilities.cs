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
    public static class Utilities
    {
        // public static readonly string MainAdress =
        //     "000000000000000000000000000000000000000000000000000000000a550c18";
        //"0000000000000000000000000000000000000000000000000000000000000000";

        public static readonly byte[] PtPTrxBytecode = new byte[] { 76, 73, 66, 82, 65, 86, 77, 10, 1, 0, 7, 1, 74, 0, 0, 0, 4, 0, 0, 0, 3, 78, 0, 0, 0, 6, 0, 0, 0, 13, 84, 0, 0, 0, 6, 0, 0, 0, 14, 90, 0, 0, 0, 6, 0, 0, 0, 5, 96, 0, 0, 0, 41, 0, 0, 0, 4, 137, 0, 0, 0, 32, 0, 0, 0, 8, 169, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 1, 3, 0, 2, 0, 2, 4, 2, 0, 3, 2, 4, 2, 3, 0, 6, 60, 83, 69, 76, 70, 62, 12, 76, 105, 98, 114, 97, 65, 99, 99, 111, 117, 110, 116, 4, 109, 97, 105, 110, 15, 112, 97, 121, 95, 102, 114, 111, 109, 95, 115, 101, 110, 100, 101, 114, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 4, 0, 12, 0, 12, 1, 19, 1, 1, 2 };
        public static readonly byte[] MintTrxBytecode = new byte[] { 76, 73, 66, 82, 65, 86, 77, 10, 1, 0, 7, 1, 74, 0, 0, 0, 6, 0, 0, 0, 3, 80, 0, 0, 0, 6, 0, 0, 0, 13, 86, 0, 0, 0, 6, 0, 0, 0, 14, 92, 0, 0, 0, 6, 0, 0, 0, 5, 98, 0, 0, 0, 51, 0, 0, 0, 4, 149, 0, 0, 0, 32, 0, 0, 0, 8, 181, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 1, 4, 0, 2, 0, 2, 4, 2, 0, 3, 2, 4, 2, 3, 0, 6, 60, 83, 69, 76, 70, 62, 12, 76, 105, 98, 114, 97, 65, 99, 99, 111, 117, 110, 116, 9, 76, 105, 98, 114, 97, 67, 111, 105, 110, 4, 109, 97, 105, 110, 15, 109, 105, 110, 116, 95, 116, 111, 95, 97, 100, 100, 114, 101, 115, 115, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 4, 0, 12, 0, 12, 1, 19, 1, 1, 2 };

        public static bool IsPtPOrMint(byte[] code)
        {
            if (code.SequenceEqual(Utilities.PtPTrxBytecode) || code.SequenceEqual(Utilities.MintTrxBytecode)) return true;

            return false;
        }

        public static bool IsAddress(string adress)
        {
            if (string.IsNullOrEmpty(adress)) return false;

            var arry = adress.HexStringToByteArray();
            if (arry.Length != 32) return false;

            return true;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static byte[] HexStringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static DateTime UnixTimeStampToDateTime(this ulong unixTimeStamp)
        {
            try
            {
                // TODO
                //1562008648525
                var dtDateTime = DateTimeOffset.FromUnixTimeSeconds((long)unixTimeStamp)
                                       .DateTime.ToLocalTime();
                return dtDateTime;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string ByteArrayToString(this byte[] arr)
        {
            return BitConverter.ToString(arr).Replace("-", "").ToLower();
        }

        public static string ByteArrayToString(this byte arr)
        {
            return BitConverter.ToString(new byte[] { arr }).Replace("-", "").ToLower();
        }

        public static string UIntToByteArry(this uint arr)
        {
            return BitConverter.ToString(BitConverter.GetBytes(arr)).Replace("-", "").ToLower();
        }

        public static List<List<byte>> SplitToSublists(List<byte> source)
        {
            return source
                     .Select((x, i) => new { Index = i, Value = x })
                     .GroupBy(x => x.Index / 4)
                     .Select(x => x.Select(v => v.Value).ToList())
                     .ToList();
        }

        public static IEnumerable<byte> Read_u8(this IEnumerable<byte> source, ref int localCursor, int count)
        {
            var retArr = source.Skip(localCursor).Take(count).ToArray();
            localCursor += count;
            return retArr;
        }

        public static string ToShortAddress(this string address)
        {
            if (address == Constant.Addresses.AssociationAddress) return "0x0";
            else return address;
        }

        public static bool IsUnicModule(string item)
        {
            //                                ModuleHandles: [
            //"7d13ec86e79d42665915eeda815047e2b95762d9d2fa996410c502b917c7bff8.<SELF>",
            //"7d13ec86e79d42665915eeda815047e2b95762d9d2fa996410c502b917c7bff8.EarmarkedLibraCoin",
            //"0000000000000000000000000000000000000000000000000000000000000000.LibraCoin",
            //"0000000000000000000000000000000000000000000000000000000000000000.LibraAccount"
            //]

            if (item.Contains("<SELF>") ||
                item.Contains("0000000000000000000000000000000000000000000000000000000000000000.LibraCoin") ||
                item.Contains("0000000000000000000000000000000000000000000000000000000000000000.LibraAccount"))
            {
                return false;
            }

            return true;
        }
    }
}
