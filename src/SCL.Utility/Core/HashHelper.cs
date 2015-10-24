using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Core
{
    public class HashHelper
    {
        public enum ServiceProviderEnum
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }

        private readonly HashAlgorithm mCryptoService;

        public HashHelper()
        {
            mCryptoService = new SHA1Managed();
        }

        public HashHelper(ServiceProviderEnum serviceProvider)
        {
            switch (serviceProvider)
            {
                case ServiceProviderEnum.SHA1:
                    mCryptoService = new SHA1Managed();
                    break;
                case ServiceProviderEnum.SHA256:
                    mCryptoService = new SHA256Managed();
                    break;
                case ServiceProviderEnum.SHA384:
                    mCryptoService = new SHA384Managed();
                    break;
                case ServiceProviderEnum.SHA512:
                    mCryptoService = new SHA512Managed();
                    break;
                case ServiceProviderEnum.MD5:
                    mCryptoService = new MD5CryptoServiceProvider();
                    break;
            }
        }

        public HashHelper(string serviceProviderName)
        {
            try
            {
                mCryptoService = (HashAlgorithm) CryptoConfig.CreateFromName(serviceProviderName.ToUpper());
            }
            catch
            {
                throw;
            }
        }

        public string Salt { get; set; }

        public static string ComputeHash(FileInfo fi)
        {
            return new HashHelper(ServiceProviderEnum.SHA1).Encrypt(fi);
        }

        public static string ComputeHash(string text)
        {
            return new HashHelper(ServiceProviderEnum.SHA1).Encrypt(text);
        }

        public virtual string Encrypt(FileInfo fi)
        {
            var str = string.Empty;
            var fileStream = fi.OpenRead();
            try
            {
                str = BitConverter.ToString(mCryptoService.ComputeHash(fileStream));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                fileStream.Close();
            }
            return str;
        }

        public virtual string Encrypt(string plainText)
        {
            var hash = mCryptoService.ComputeHash(Encoding.ASCII.GetBytes(plainText + Salt));
            return Convert.ToBase64String(hash, 0, hash.Length);
        }

        public bool Match(string input, string hashValue)
        {
            return StringHelper.Match(Encrypt(input), hashValue, true);
        }
    }
}