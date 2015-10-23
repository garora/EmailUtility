using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public class HashHelper
    {
        private string mSalt;
        private HashAlgorithm mCryptoService;

        public string Salt
        {
            get
            {
                return this.mSalt;
            }
            set
            {
                this.mSalt = value;
            }
        }

        public HashHelper()
        {
            this.mCryptoService = (HashAlgorithm)new SHA1Managed();
        }

        public HashHelper(HashHelper.ServiceProviderEnum serviceProvider)
        {
            switch (serviceProvider)
            {
                case HashHelper.ServiceProviderEnum.SHA1:
                    this.mCryptoService = (HashAlgorithm)new SHA1Managed();
                    break;
                case HashHelper.ServiceProviderEnum.SHA256:
                    this.mCryptoService = (HashAlgorithm)new SHA256Managed();
                    break;
                case HashHelper.ServiceProviderEnum.SHA384:
                    this.mCryptoService = (HashAlgorithm)new SHA384Managed();
                    break;
                case HashHelper.ServiceProviderEnum.SHA512:
                    this.mCryptoService = (HashAlgorithm)new SHA512Managed();
                    break;
                case HashHelper.ServiceProviderEnum.MD5:
                    this.mCryptoService = (HashAlgorithm)new MD5CryptoServiceProvider();
                    break;
            }
        }

        public HashHelper(string serviceProviderName)
        {
            try
            {
                this.mCryptoService = (HashAlgorithm)CryptoConfig.CreateFromName(serviceProviderName.ToUpper());
            }
            catch
            {
                throw;
            }
        }

        public static string ComputeHash(FileInfo fi)
        {
            return new HashHelper(HashHelper.ServiceProviderEnum.SHA1).Encrypt(fi);
        }

        public static string ComputeHash(string text)
        {
            return new HashHelper(HashHelper.ServiceProviderEnum.SHA1).Encrypt(text);
        }

        public virtual string Encrypt(FileInfo fi)
        {
            string str = string.Empty;
            FileStream fileStream = fi.OpenRead();
            try
            {
                str = BitConverter.ToString(this.mCryptoService.ComputeHash((Stream)fileStream));
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
            byte[] hash = this.mCryptoService.ComputeHash(Encoding.ASCII.GetBytes(plainText + this.mSalt));
            return Convert.ToBase64String(hash, 0, hash.Length);
        }

        public bool Match(string input, string hashValue)
        {
            return StringHelper.Match(this.Encrypt(input), hashValue, true);
        }

        public enum ServiceProviderEnum
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5,
        }
    }
}
