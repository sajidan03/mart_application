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
        private static SqlConnection koneksi;
        private static string sqlCommand= "Data Source=DESKTOP-MOUI7DH\\SQLEXPRESS;Initial Catalog=lksmart;Integrated Security=True;TrustServerCertificate=True";
        public static SqlConnection Connect()
        {
            if (koneksi == null)
            {
                koneksi = new SqlConnection(sqlCommand);
            }
            return koneksi;
        }
    }
}
