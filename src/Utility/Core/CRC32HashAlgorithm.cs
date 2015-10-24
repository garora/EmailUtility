using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Core
{
    public class CRC32HashAlgorithm : HashAlgorithm
    {
        protected static uint AllOnes = uint.MaxValue;
        protected static Hashtable cachedCRC32Tables = Hashtable.Synchronized(new Hashtable());
        protected static bool autoCache = true;
        protected uint[] crc32Table;
        private uint m_crc;

        public CRC32HashAlgorithm()
            : this(DefaultPolynomial)
        {
        }

        public CRC32HashAlgorithm(uint aPolynomial)
            : this(aPolynomial, AutoCache)
        {
        }

        public CRC32HashAlgorithm(uint aPolynomial, bool cacheTable)
        {
            HashSizeValue = 32;
            crc32Table = (uint[]) cachedCRC32Tables[aPolynomial];
            if (crc32Table == null)
            {
                crc32Table = BuildCRC32Table(aPolynomial);
                if (cacheTable)
                    cachedCRC32Tables.Add(aPolynomial, crc32Table);
            }
            Initialize();
        }

        public static uint DefaultPolynomial
        {
            get { return 79764919U; }
        }

        public static bool AutoCache
        {
            get { return autoCache; }
            set { autoCache = value; }
        }

        public static void ClearCache()
        {
            cachedCRC32Tables.Clear();
        }

        protected static uint[] BuildCRC32Table(uint ulPolynomial)
        {
            var numArray = new uint[256];
            for (var index1 = 0; index1 < 256; ++index1)
            {
                var num = (uint) index1;
                for (var index2 = 8; index2 > 0; --index2)
                {
                    if (((int) num & 1) == 1)
                        num = num >> 1 ^ ulPolynomial;
                    else
                        num >>= 1;
                }
                numArray[index1] = num;
            }
            return numArray;
        }

        public override void Initialize()
        {
            m_crc = AllOnes;
        }

        protected override void HashCore(byte[] buffer, int offset, int count)
        {
            for (var index1 = offset; index1 < count; ++index1)
            {
                ulong index2 = m_crc & byte.MaxValue ^ buffer[index1];
                m_crc >>= 8;
                m_crc ^= crc32Table[index2];
            }
        }

        protected override byte[] HashFinal()
        {
            var numArray = new byte[4];
            ulong num = m_crc ^ AllOnes;
            numArray[0] = (byte) (num >> 24 & byte.MaxValue);
            numArray[1] = (byte) (num >> 16 & byte.MaxValue);
            numArray[2] = (byte) (num >> 8 & byte.MaxValue);
            numArray[3] = (byte) (num & byte.MaxValue);
            return numArray;
        }

        public new byte[] ComputeHash(Stream inputStream)
        {
            var numArray = new byte[4096];
            int cbSize;
            while ((cbSize = inputStream.Read(numArray, 0, 4096)) > 0)
                HashCore(numArray, 0, cbSize);
            return HashFinal();
        }

        public new byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, 0, buffer.Length);
        }

        public byte[] ComputeHash(string s)
        {
            var bytes = Encoding.ASCII.GetBytes(s);
            return ComputeHash(bytes, 0, bytes.Length);
        }

        public new byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            HashCore(buffer, offset, count);
            return HashFinal();
        }
    }
}