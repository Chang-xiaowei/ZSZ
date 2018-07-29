using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Common
{
   public static class CommonHelper
    {
        public static string CalMD5(this string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return CalMD5(bytes);
        }
        public static string CalMD5(byte[]bytes)
        {
            using (MD5 md5= MD5.Create())
            {
                byte[]computerBytes= md5.ComputeHash(bytes);
                string result = "";
                for (int i = 0; i < computerBytes.Length; i++)
                {
                    result += computerBytes[i].ToString("X").Length == 1 ? "0" + computerBytes[i].ToString("X"):computerBytes[i].ToString("X");
                }
                return result;
            }
        }
        public static string CalMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computerBytes = md5.ComputeHash(stream);
                string result = "";
                for (int i = 0; i < computerBytes.Length; i++)
                {
                    result += computerBytes[i].ToString("X").Length == 1 ? "0" + computerBytes[i].ToString("X") : computerBytes[i].ToString("X");
                }
                return result;
            }
        }
        public static string GenerateCaptchaCode(int len)
        {
            char[] data = { 'a', 'c', 'd', 'e', 'f', 'g', 'k', 'm', 'p', 'r', 's', 't', 'w', 'x', 'y', '3', '4', '5', '7', '8'};
            StringBuilder sbCode = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                char ch = data[rand.Next(data.Length)];
                sbCode.Append(ch);
            }
            return sbCode.ToString();
        }
    }
   
}
