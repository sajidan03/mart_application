using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lks_test
{
    internal class connection
    {
        private static string sqlCmd = "Data Source=DESKTOP-MOUI7DH\\SQLEXPRESS;Initial Catalog=lks_mart;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        private static SqlConnection koneksi;
        public static SqlConnection Connect()
        {
            if (koneksi == null)
            {
                koneksi = new SqlConnection(sqlCmd);
            }
            return koneksi;
        }
    }
}
