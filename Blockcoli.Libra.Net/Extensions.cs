using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Google.Protobuf;
using Blockcoli.Libra.Net.Crypto;
using System.Collections.Generic;

namespace Blockcoli.Libra.Net
{
    public static class Extensions
    {
        public static byte[] ToBytes(this uint number)
        {
            var bytes = BitConverter.GetBytes(number);
            Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] ToBytes(this ulong number)
        {
            var bytes = BitConverter.GetBytes(number);
            Array.Reverse(bytes);
            return bytes;
        }        

        public static byte[] ToBytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static ByteString ToByteString(this string text)
        {            
            return text.FromHexToBytes().ToByteString();
        }

        public static byte[] FromHexToBytes(this string hexString)
        {
            byte[] retval = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                retval[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return retval;
        }

        public static string ToTextString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-","").ToLower();
        }

        public static ByteString ToByteString(this byte[] bytes)
        {
            return ByteString.CopyFrom(bytes);
        }

        public static byte[] Reverse(this byte[] bytes)
        {
            Array.Reverse(bytes);
            return bytes;
        }

        public static void PrintHex(this byte[] data)
        {
            data.ToList().ForEach(x => Console.Write($"{x:X}, ")); Console.WriteLine();
        }

        public static void PrintDec(this byte[] data)
        {            
            data.ToList().ForEach(x => Console.Write($"{x:00}, ")); Console.WriteLine();
        }

        public static void Print(this string[] data)
        {            
            data.ToList().ForEach(x => Console.Write($"{x}, ")); Console.WriteLine();
        }

        public static T[] Slice<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }  

        public static byte[] Pbkdf(this byte[] password, byte[] salt, int iterations, int outputLen)
        {
            var hmacLength = 32;
            var outputBuffer = new byte[outputLen];
            var hmacOutput = new byte[hmacLength];
            var block = new byte[salt.Length + 4];
            var leftLength = Math.Ceiling((decimal)outputLen / hmacLength);
            var rightLength = outputLen - (leftLength - 1) * hmacLength;
            salt.CopyTo(block, 0);

            for (var i = 1u; i <= leftLength; i++)
            {
                var intBytes = i.ToBytes();
                Array.Copy(intBytes, 0, block, salt.Length, intBytes.Length);

                var hmac = block.Hmac(password);
                hmac.CopyTo(hmacOutput, 0);

                for (var j = 1; j < iterations; j++)
                {
                    hmac = hmac.Hmac(password);
                    for (var k = 0; k < hmacLength; k++)
                    {
                        hmacOutput[k] ^= hmac[k];
                    }
                }

                var destPos = (i - 1) * hmacLength;
                var len = i == leftLength ? (int)rightLength : hmacLength;
                Array.Copy(hmacOutput, 0, outputBuffer, destPos, len);
            }

            return outputBuffer;
        }

        public static byte[] Pbkdf(this byte[] password, string salt, int iterations, int outputLen)
        {
            return password.Pbkdf(salt.ToBytes(), iterations, outputLen);
        }

        public static byte[] Pbkdf(this string password, byte[] salt, int iterations, int outputLen)
        {
            return password.ToBytes().Pbkdf(salt, iterations, outputLen);
        }

        public static byte[] Pbkdf(this string password, string salt, int iterations, int outputLen)
        {
            return password.ToBytes().Pbkdf(salt.ToBytes(), iterations, outputLen);
        }

        public static byte[] Hmac(this byte[] data, byte[] key)
        {
            var blockSize = 136;
            var ipad = new byte[blockSize];
            var opad = new byte[blockSize];                                        

            if (key.Length > blockSize)
            { 
                key = new SHA3_256().ComputeVariable(key);                                             
            }
            else if (key.Length < blockSize)
            {
                var temp = new byte[blockSize];
                Array.Copy(key, temp, key.Length);
                Array.Clear(temp, 128, blockSize - 128);
                key = temp;
            }

            for (var i = 0; i < blockSize; i++)
            {
                ipad[i] = i < key.Length ? (byte)(key[i] ^ 0x36) : (byte)0x36;
                opad[i] = i < key.Length ? (byte)(key[i] ^ 0x5C) : (byte)0x5C;
            }

            var hash1 = new SHA3_256().ComputeVariable(ipad.Concat(data).ToArray());
            var hash2 = new SHA3_256().ComputeVariable(opad.Concat(hash1).ToArray());
            return hash2;
        }

        public static byte[] Hmac(this byte[] data, string key)
        {
            return data.Hmac(key.ToBytes());
        }
        
        public static byte[] Hmac(this string data, byte[] key)
        {
            return data.ToBytes().Hmac(key);
        }

        public static byte[] Hmac(this string data, string key)
        {
            return data.ToBytes().Hmac(key.ToBytes());
        }

        public static byte[] HkdfExpand(this byte[] prk, byte[] info, int outputLength)
        {
            var infoLen = info.Length;
            var hashLen = 32;
            var steps = Math.Ceiling((decimal)outputLength / hashLen);
            if (steps > 0xFF)
            {
                throw new Exception("OKM length ${length} is too long for sha3-256 hash");
            }

            var t = new byte[hashLen * (int)steps + infoLen + 1];

            for (int c = 1, start = 0, end = 0; c <= steps; ++c)
            {
                info.CopyTo(t, end);
                t[end + infoLen] = (byte)c;
                t.Slice(start, end + infoLen + 1).Hmac(prk).CopyTo(t, end);
                start = end; //used for T(C-1) start
                end += hashLen; // used for T(C-1) end & overall end
            }
            return t.Slice(0, outputLength);
        }  

        public static byte[] HkdfExpand(this byte[] prk, string info, int outputLength)
        {
            return prk.HkdfExpand(info.ToBytes(), outputLength);
        }    

        public static byte[] HkdfExpand(this string prk, byte[] info, int outputLength)
        {
            return prk.ToBytes().HkdfExpand(info, outputLength);
        } 

        public static byte[] HkdfExpand(this string prk, string info, int outputLength)
        {
            return prk.ToBytes().HkdfExpand(info.ToBytes(), outputLength);
        } 

        public static byte[] HkdfExtract(this byte[] ikm, byte[] salt)
        {
            return ikm.Hmac(salt);
        }

        public static byte[] HkdfExtract(this byte[] ikm, string salt)
        {
            return ikm.Hmac(salt.ToBytes());
        }

        public static byte[] HkdfExtract(this string ikm, byte[] salt)
        {
            return ikm.ToBytes().Hmac(salt);
        }

        public static byte[] HkdfExtract(this string ikm, string salt)
        {
            return ikm.ToBytes().Hmac(salt.ToBytes());
        }

        public static byte[] EddsaSign(this byte[] message, byte[] secretKey)
        {
            var ed = new Edwards25519(secretKey);
            return ed.Sign(message);
        }

        public static byte[] EddsaSign(this string message, byte[] secretKey)
        {
            return message.ToBytes().EddsaSign(secretKey);
        }

        public static bool EddsaVerify(this byte[] message, byte[] signature, byte[] publicKey)
        {
            var ed = new Edwards25519();
            return ed.Verify(message, publicKey, signature);
        }

        public static bool EddsaVerify(this string message, byte[] signature, byte[] publicKey)
        {
            return message.ToBytes().EddsaVerify(signature, publicKey);
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            int idx = 0;
            foreach (T item in enumerable)
                handler(item, idx++);
        }
    }
}
