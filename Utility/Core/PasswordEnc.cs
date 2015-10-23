namespace Utility.Core
{
    public class PasswordEnc
    {
        public static bool Compare(string enc_pw, string pw)
        {
            return new HashHelper().Match(pw, enc_pw);
        }

        public static string Encrypt(string pw)
        {
            return new HashHelper().Encrypt(pw);
        }
    }
}