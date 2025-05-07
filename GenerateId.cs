using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lks_test
{
    internal class GenerateId
    {
        public static SqlCommand cmd;
        public static SqlConnection koneksi = connection.Connect();
        public static SqlDataReader rd;
        public static string noTransaksi()
        {
            koneksi.Open();
            string hasil;
            cmd =  new SqlCommand("SELECT * FROM tb;l_transaksi order by no_transaksi desc", koneksi);
            rd = cmd.ExecuteReader();
            if (rd.HasRows)
            {
                rd.Read();
                var urut = int.Parse(rd["no_transaksi"].ToString().Substring(4)) + 1;
                rd.Close();
                hasil = "TRS00" + urut.ToString();
            }
            else
            {
                hasil = "TRS001";
            }
            koneksi.Close();
            return hasil;
        }
        public static string nomor()
        {
            koneksi.Open();
            string hasil;
            cmd = new SqlCommand("SELECT * FROM tbl_transaksi ORDER BY no_transaksi DESC", koneksi);
            rd = cmd.ExecuteReader();
            if (rd.HasRows)
            {
                rd.Read();
                var urut = int.Parse(rd["no_transaksi"].ToString().Substring(4)) + 1;
                rd.Close();
                hasil = "TRS00" + urut.ToString();
            }
            else
            {
                hasil = "TRS001";
            }
            koneksi.Close();
            return hasil;
        }
    }
}
