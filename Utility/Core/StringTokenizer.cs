using System.Collections;

namespace Utility.Core
{
    public class StringTokenizer
    {
        private int CurrIndex;
        private int NumTokens;
        private readonly ArrayList tokens;

        public StringTokenizer(string source, string delimiter)
        {
            tokens = new ArrayList(10);
            Source = source;
            Delim = delimiter;
            if (delimiter.Length == 0)
                Delim = " ";
            Tokenize();
        }

        public StringTokenizer(string source, char[] delimiter)
            : this(source, new string(delimiter))
        {
        }

        public StringTokenizer(string source)
            : this(source, "")
        {
        }

        public StringTokenizer()
            : this("", "")
        {
        }

        public string Source { get; private set; }

        public string Delim { get; private set; }

        private void Tokenize()
        {
            var str1 = Source;
            NumTokens = 0;
            tokens.Clear();
            CurrIndex = 0;
            if (str1.IndexOf(Delim) < 0 && str1.Length > 0)
            {
                NumTokens = 1;
                CurrIndex = 0;
                tokens.Add(str1);
                tokens.TrimToSize();
                str1 = "";
            }
            else if (str1.IndexOf(Delim) < 0 && str1.Length <= 0)
            {
                NumTokens = 0;
                CurrIndex = 0;
                tokens.TrimToSize();
            }
            while (str1.IndexOf(Delim) >= 0)
            {
                if (str1.IndexOf(Delim) == 0)
                {
                    str1 = str1.Length <= Delim.Length ? "" : str1.Substring(Delim.Length);
                }
                else
                {
                    var str2 = str1.Substring(0, str1.IndexOf(Delim));
                    tokens.Add(str2);
                    str1 = str1.Length <= Delim.Length + str2.Length ? "" : str1.Substring(Delim.Length + str2.Length);
                }
            }
            if (str1.Length > 0)
                tokens.Add(str1);
            tokens.TrimToSize();
            NumTokens = tokens.Count;
        }

        public void NewSource(string newSrc)
        {
            Source = newSrc;
            Tokenize();
        }

        public void NewDelim(string newDel)
        {
            Delim = newDel.Length != 0 ? newDel : " ";
            Tokenize();
        }

        public void NewDelim(char[] newDel)
        {
            var str = new string(newDel);
            Delim = str.Length != 0 ? str : " ";
            Tokenize();
        }

        public int CountTokens()
        {
            return tokens.Count;
        }

        public bool HasMoreTokens()
        {
            return CurrIndex <= tokens.Count - 1;
        }

        public string NextToken()
        {
            if (CurrIndex > tokens.Count - 1)
                return null;
            var str = (string) tokens[CurrIndex];
            ++CurrIndex;
            return str;
        }
    }
}