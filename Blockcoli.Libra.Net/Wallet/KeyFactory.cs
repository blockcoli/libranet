using System;
using Blockcoli.Libra.Net.Crypto;

namespace Blockcoli.Libra.Net.Wallet
{
    public class KeyFactory
    {
        readonly Seed seed;
        byte[] masterPrk;

        public KeyFactory(Seed seed)
        {
            this.seed = seed;
            this.masterPrk = seed.ToBytes().HkdfExtract(Constant.KeyPrefixes.MasterKeySalt, HashAlgorithm.SHA3_256);
        }

        public Edwards25519 GenerateKey(ulong childDepth)
        {
            var childDepthBuffer = BitConverter.GetBytes(childDepth);
            var derivedKeyBytes = Constant.KeyPrefixes.DerivedKey.ToBytes();
            var info = new byte[childDepthBuffer.Length+derivedKeyBytes.Length];
            Array.Copy(derivedKeyBytes, info, derivedKeyBytes.Length);
            Array.Copy(childDepthBuffer, 0, info, derivedKeyBytes.Length, childDepthBuffer.Length);            
            var privateKey = masterPrk.HkdfExpand(info, 32, HashAlgorithm.SHA3_256);            
            var keyPair = new Edwards25519(privateKey);
            return keyPair;
        }
    }
}