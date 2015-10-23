using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public interface ICryptoHelper : IDisposable
    {
        string Entropy { get; set; }

        CryptographyAlgorithm Algorithm { get; }

        byte[] Encrypt(byte[] plaintext);

        string Encrypt(string plaintext, StringEncodingType encoding);

        string Encrypt(string plaintext);

        byte[] Decrypt(byte[] cipherText);

        string Decrypt(string cipherText, StringEncodingType encoding);

        string Decrypt(string cipherText);
    }
}
