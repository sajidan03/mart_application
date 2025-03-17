using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lks_test
{
    internal class Session
    {
        public static int id_user;
        public static string username;
        public static void start(int id, string nama)
        {
            id_user = id;
            username = nama;
        }
    }
}
