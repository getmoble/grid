using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Grid.Infrastructure
{
    public static class HashHelper
    {
        public static string Hash(string text, HashType hashType = HashType.MD5)
        {
            string hashString;
            switch (hashType)
            {
                case HashType.MD5:
                    hashString = GetMD5(text);
                    break;
                case HashType.SHA1:
                    hashString = GetSHA1(text);
                    break;
                case HashType.SHA256:
                    hashString = GetSHA256(text);
                    break;
                case HashType.SHA512:
                    hashString = GetSHA512(text);
                    break;
                default:
                    hashString = "Invalid Hash Type";
                    break;
            }
            return hashString;
        }
        public static bool CheckHash(string original, string hashString, HashType hashType = HashType.MD5)
        {
            var originalHash = Hash(original, hashType);
            return originalHash == hashString;
        }

        private static string GetMD5(string text)
        {
            var ue = new UnicodeEncoding();
            var message = ue.GetBytes(text);

            MD5 hashString = new MD5CryptoServiceProvider();

            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }

        private static string GetSHA1(string text)
        {
            var ue = new UnicodeEncoding();
            var message = ue.GetBytes(text);

            var hashString = new SHA1Managed();

            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }

        private static string GetSHA256(string text)
        {
            var ue = new UnicodeEncoding();
            var message = ue.GetBytes(text);

            var hashString = new SHA256Managed();

            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }

        private static string GetSHA512(string text)
        {
            var ue = new UnicodeEncoding();
            var message = ue.GetBytes(text);

            var hashString = new SHA512Managed();

            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }
    }
}
