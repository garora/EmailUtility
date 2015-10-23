using System;
using System.Security.Cryptography;

namespace Utility.Core
{
    public sealed class CryptoFactory
    {
        private CryptoFactory()
        {
        }

        public static ICryptoHelper Create(CryptographyAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case CryptographyAlgorithm.Des:
                case CryptographyAlgorithm.Rc2:
                case CryptographyAlgorithm.Rijndael:
                case CryptographyAlgorithm.TripleDes:
                    return new SymmetricCryptographyHelper(algorithm);
                default:
                    throw new CryptographicException("Algorithm '" + algorithm + "' not supported.");
            }
        }

        public static ICryptoHelper Create(CryptographyAlgorithm algorithm, string entropy)
        {
            switch (algorithm)
            {
                case CryptographyAlgorithm.Des:
                case CryptographyAlgorithm.Rc2:
                case CryptographyAlgorithm.Rijndael:
                case CryptographyAlgorithm.TripleDes:
                    return new SymmetricCryptographyHelper(algorithm, entropy);
                default:
                    throw new CryptographicException("Algorithm '" + algorithm + "' not supported.");
            }
        }

        public static ICryptoHelper Create(string algorithmName, string entropy)
        {
            return Create((CryptographyAlgorithm) Enum.Parse(typeof (CryptographyAlgorithm), algorithmName, true),
                entropy);
        }
    }
}