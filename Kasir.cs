using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace lks_test
{
    public partial class Kasir : Form
    {
        SqlConnection koneksi = connection.Connect();
        SqlCommand cmd;
        SqlDataAdapter sda;
        SqlDataReader rd;
        DataTable dt;
        int number = 1;
        double diskon = 0;
        public Kasir()
        {
            InitializeComponent();
            txtSatuan.ReadOnly = true;
            menu_load();
            table_load();
            btnPrint.Enabled = false;
            btnSimpan.Enabled = false;
            printDocument1 = new PrintDocument();
            PaperSize customPaperSize = new PaperSize("Struk 80mm", 300, 800);
            printDocument1.DefaultPageSettings.PaperSize = customPaperSize;
            printDocument1.PrintPage += PrintReceipt2;
        }
        public void table_load()
        {
            dgv.Columns.Clear();
             dgv.ColumnCount = 7;
            dgv.Columns[0].Name = "No Transaksi";
            dgv.Columns[1].Name = "Kode Barang";
            dgv.Columns[2].Name = "Nama Barang";
            dgv.Columns[3].Name = "Harga Barang";
            dgv.Columns[4].Name = "QTY";
            dgv.Columns[5].Name = "total";
            dgv.Columns[6].Name = "id";

            dgv.Columns[6].Visible = false;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog pd = new PrintPreviewDialog
            {
                Document = printDocument1,
                Width = 800,
                Height = 600
            };
            pd.ShowDialog();
        }
        public void menu_load()
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new SqlCommand("SELECT * FROM [tbl_barang]", koneksi);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    cmbMenu.Items.Add(rd["kode_barang"].ToString() + " - " + rd["nama_barang"].ToString());
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }

        }
        private void btnBayar_Click(object sender, EventArgs e)
        {
            double ub;
            double total;
            if (double.TryParse(txtUbay.Text, out ub) && double.TryParse(totBay.Text, out total))
            {
                if (ub < total)
                {
                    MessageBox.Show("Nominal uang bayar kurang!");
                }
                else
                {
                    btnPrint.Enabled = true;
                    btnSimpan.Enabled = true;
                }
                double uk = 0;
                uk = ub - total;
                lbKembali.Text = uk.ToString();
            }
        }
        private void PrintReceipt2(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Courier New", 12);
            float yPos = 10;
            float leftMargin = 0;
            StringBuilder receiptText = new StringBuilder();
            receiptText.AppendLine("           STRUK PENJUALAN          ");
            receiptText.AppendLine("Nama Kasir : " + Session.username);
            receiptText.AppendLine("------------------------------------");
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[2].Value != null)
                {
                    receiptText.AppendLine("Nama Barang : " + row.Cells[2].Value);
                    receiptText.AppendLine("Harga       : " + row.Cells[5].Value);
                    receiptText.AppendLine("Qty         : " + row.Cells[4].Value);
                    receiptText.AppendLine("Total       : " + row.Cells[5].Value);
                    receiptText.AppendLine("------------------------------------");
                }
            }
            receiptText.AppendLine("Terima Kasih!");
            e.Graphics.DrawString(receiptText.ToString(), font, Brushes.Black, leftMargin, yPos);
        }
        public void hapus()
        {
            txtHarga.Text = null;
            txtQtt.Text = null;
            txtSatuan.Text = null;
            cmbMenu.Text = null;
            dgv.Columns.Clear();
        }
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string gn = GenerateId.noTransaksi();
            int idTransaksi = 0;
            koneksi.Open();
            //cmd = new SqlCommand("INSERT INTO [tbl_transaksi] OUTPUT INSERTED.id_transaksi VALUES ('" + gn + "', @waktu, '" + Session.username + "', @totbay, '" + Session.id_user + "')", koneksi);
            //cmd.Parameters.AddWithValue("@waktu", DateTime.Now);
            //cmd.Parameters.AddWithValue("@totbay", Convert.ToInt32(totBay.Text));
            //cmd.ExecuteNonQuery();
            //idTransaksi = (int)cmd.ExecuteScalar();
            cmd = new SqlCommand("INSERT INTO [tbl_transaksi] VALUES (@no, @tgl, @nama, @total ,@id)", koneksi);
            cmd.Parameters.AddWithValue("@no", gn);
            cmd.Parameters.AddWithValue("@tgl", DateTime.Now);
            cmd.Parameters.AddWithValue("@nama", Session.username);
            cmd.Parameters.AddWithValue("@total", totBay.Text);
            cmd.Parameters.AddWithValue("@id", Session.id_user);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Input berhasil");
            hapus();
            koneksi.Close();
            //for (int i = 0; i < dgv.Rows.Count - 1; i++)
            //{
            //    koneksi.Open();
            //    cmd = new SqlCommand("INSERT INTO [tbl_detail] VALUES( '" + idTransaksi + "', '" + dgv.Rows[i].Cells[2].Value + "', '" + dgv    .Rows[i].Cells[3].Value + "', '" + dgv.Rows[i].Cells[4].Value + "', '" + dgv.Rows[i].Cells[6].Value + "')", koneksi);
            //    cmd.ExecuteNonQuery();
            //    koneksi.Close();
            //}
            MessageBox.Show("Transaksi berhasil.");
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            string no = "TRS00" + number.ToString();
            string kode = cmbMenu.Text.ToString().Split('-')[0];
            string nama = cmbMenu.Text.ToString().Split('-')[1];
            dgv.Rows.Add(1);
            dgv.Rows[dgv.Rows.Count - 2].Cells[0].Value = no;
            dgv.Rows[dgv.Rows.Count - 2].Cells[1].Value = kode;
            dgv.Rows[dgv.Rows.Count - 2].Cells[2].Value = nama;
            dgv.Rows[dgv.Rows.Count - 2].Cells[3].Value = txtSatuan.Text;
            dgv.Rows[dgv.Rows.Count - 2].Cells[4].Value = txtQtt.Text;
            dgv.Rows[dgv.Rows.Count - 2].Cells[5].Value = txtHarga.Text;
            dgv.Rows[dgv.Rows.Count - 2].Cells[6].Value = txtId.Text;
            txtSatuan.Text = null;
            txtHarga.Text = null;
            txtQtt.Text = null;
            cmbMenu.SelectedIndex = -1;
            int total = 0;
            foreach (DataGridViewRow dr in dgv.Rows)
            {
                int tt = Convert.ToInt32(dr.Cells[5].Value);
                total += tt;
            }

            totHar.Text = total.ToString();
            //
            if (Convert.ToInt32(totHar.Text) > 500000)
            {
                diskon = 25000;
                lbDiskon.Text = diskon.ToString();
            }
            double totalBayar = total - diskon;
            totBay.Text = totalBayar.ToString();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            koneksi.Open();
            cmd = new SqlCommand("INSERT INTO [tbl_log] VALUES (@waktu, 'Logout', '" + Session.id_user + "')", koneksi);
            cmd.Parameters.AddWithValue("@waktu", DateTime.Now);
            cmd.ExecuteNonQuery();
            koneksi.Close();
            Login login = new Login();
            MessageBox.Show("Logout berhasil");
            login.Show();
            this.Hide();
        }

        private void cmbMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kode = cmbMenu.Text.ToString().Split('-')[0];
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new SqlCommand("SELECT * FROM [tbl_barang] WHERE kode_barang = '" + kode + "' ", koneksi);
                rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    txtSatuan.Text = rd["harga_satuan"].ToString();
                    txtId.Text = rd["id_barang"].ToString();
                    rd.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void txtQtt_KeyUp(object sender, KeyEventArgs e)
        {
            if (double.TryParse(txtQtt.Text, out double angka1) && double.TryParse(txtSatuan.Text, out double angka2))
            {
                double hasil = angka1 * angka2;
                txtHarga.Text = hasil.ToString();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            txtSatuan.Text = null;
            txtQtt.Text = null;
            txtHarga.Text=null;
            if (dgv.SelectedRows.Count == 1)
            {
                dgv.Rows.Clear();
            }
        }

        private void Kasir_Load(object sender, EventArgs e)
        {

        }

        private void txtQtt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void txtQtt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
