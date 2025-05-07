using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace lks_mart
{
    internal class HashPassword
    {
        //public static string hash(string password)
        //{
        //    using (SHA1Managed sha1 = new SHA1Managed()) 
        //    {
        //        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        var sb = new StringBuilder(hash.Length * 2);
        //        foreach (byte b in hash)
        //        {
        //            sb.Append(b.ToString("X2"));
        //        }
        //        return sb.ToString();
        //    }
        //}
        public static string hash(string password)
        {
            using (SHA256 sh = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sh.ComputeHash(bytes);
                return
                    Convert.ToBase64String(hash);
            }
        }
    }
}
