using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security;
using System.Text;

using IvoryCrow.Utilities;

namespace IvoryCrow.Extensions
{
    public static class StringExtensions
    {
        // Hash inputData and compare against hash
        public static bool VerifyHash(string inputData, string hash)
        {
            string hashOfInput = ComputeMD5Hash(inputData);
            
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (comparer.Compare(hashOfInput, hash) == 0)
            {
                return true;
            }

            return false;
        }

        // Get MD5 hash as a hex string
        public static string ComputeMD5Hash(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                
                return stringBuilder.ToString();
            }
        }

        private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int ByteSize = 0x100;
        public static string GetSecurelyRandomString(int length)
        {
            Throw.IfTrue(length <= 0, "length cannot be less than or equal to zero");

            HashSet<char> hashCharacterSet = new HashSet<char>(ValidCharacters);
            char[] stringArray = new char[hashCharacterSet.Count];
            hashCharacterSet.CopyTo(stringArray);

            var cryptoRng = System.Security.Cryptography.RandomNumberGenerator.Create();
            byte[] cryptoBuf = new byte[128];
            StringBuilder result = new StringBuilder();
            
            while (result.Length < length)
            {
                cryptoRng.GetBytes(cryptoBuf);
                for (var i = 0; i < cryptoBuf.Length && result.Length < length; ++i)
                {
                    // Divide the byte into allowedCharSet-sized groups. If the
                    // random value falls into the last group and the last group is
                    // too small to choose from the entire allowedCharSet, ignore
                    // the value in order to avoid biasing the result.
                    int outOfRangeStart = ByteSize - (ByteSize % ValidCharacters.Length);
                    if (outOfRangeStart <= cryptoBuf[i])
                    {
                        continue;
                    }
                    
                    result.Append(ValidCharacters[cryptoBuf[i] % ValidCharacters.Length]);
                }
            }

            return result.ToString();
        }
    }
}