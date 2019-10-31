using Xunit;

namespace Blockcoli.Libra.Net.Test
{
    public class CryptoTest
    {
        [Fact]
        public void Hmac_Sha3_256_KeyLess()
        {
            var data = "abc";
            var key = "12345678";

            var actual = data.Hmac(key);
            var expected = new byte[]
            {
                188, 209, 246, 94, 119, 24, 223, 103, 3, 69, 115, 92, 120, 135, 93, 7, 173,
                52, 183, 180, 246, 246, 120, 10, 255, 193, 147, 189, 220, 31, 82, 204
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Hmac_Sha3_256_KeyMore()
        {
            var data = "abc";
            var key =
                "1234567890123456789012345678901234567890123456789012345678901234567890" +
                "1234567890123456789012345678901234567890123456789012345678901234567890";

            var actual = data.Hmac(key);
            var expected = new byte[]
            {
                64, 221, 158, 59, 234, 94, 244, 230, 235, 212, 117, 117, 254, 167, 166, 241,
                89, 121, 158, 104, 168, 138, 105, 250, 156, 16, 118, 3, 31, 255, 10, 25
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Pbkdf_Sha3_256()
        {
            var password = "abc";
            var salt = "12345678";
            var iterations = 5;
            var outputLen = 32;

            var actual = password.Pbkdf(salt, iterations, outputLen);
            var expected = new byte[]
            {
                188, 73, 36, 65, 91, 49, 226, 57, 48, 16, 206, 49, 237, 78, 50, 38, 81,
                116, 152, 11, 178, 55, 82, 104, 194, 50, 182, 213, 199, 236, 174, 195
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HkdfExtract_Sha3_256()
        {
            var ikm = "abc";
            var salt = "12345678";

            var actual = ikm.HkdfExtract(salt);
            var expected = new byte[]
            {
                188, 209, 246, 94, 119, 24, 223, 103, 3, 69, 115, 92, 120, 135, 93, 7, 173,
                52, 183, 180, 246, 246, 120, 10, 255, 193, 147, 189, 220, 31, 82, 204
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HkdfExpand_Sha3_256()
        {
            var prk = "abc";
            var info = "12345678";
            var length = 32;

            var actual = prk.HkdfExpand(info, length);
            var expected = new byte[]
            {
                218, 155, 54, 135, 214, 38, 114, 114, 6, 146, 5, 69, 46, 96, 1, 247,
                27, 147, 82, 1, 50, 29, 31, 182, 131, 195, 91, 132, 167, 29, 220, 164
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Eddsa_VerifyMessage_WithPublicKey()
        {
            var message = "abc";
            var publicKey = new byte[]
            {
                97, 201, 21, 44, 110, 6, 3, 224, 125, 217, 81, 193, 88, 188, 86, 161,
                221, 213, 133, 36, 32, 72, 125, 206, 145, 125, 132, 87, 81, 79, 130, 95
            };
            var signature = new byte[]
            {
                107, 128, 14, 183, 175, 48, 41, 153, 216, 246, 104, 205, 2, 123, 94, 12,
                15, 219, 51, 125, 2, 163, 250, 88, 247, 239, 165, 176, 122, 140, 83, 43,
                37, 154, 170, 82, 170, 180, 202, 29, 73, 211, 102, 141, 252, 169, 20, 236,
                97, 201, 124, 222, 180, 219, 137, 98, 212, 128, 225, 42, 184, 131, 170, 5
            };

            var isVerify = message.EddsaVerify(signature, publicKey);
            Assert.True(isVerify);
        }
    }
}