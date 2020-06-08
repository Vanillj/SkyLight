using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Managers
{
    class CryptoManager
    {
        public static string ToHash(string s)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(6);
            return BCrypt.Net.BCrypt.HashPassword(s, salt);
        }

        public static bool CheckHash(string comparedString, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(comparedString, hash);
        }
    }
}
