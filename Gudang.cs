using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls.WebParts;
using Guna.UI2.WinForms;
using lks_test;

namespace lks_mart
{
    public partial class gudang : Form
    {
        SqlCommand cmd;
        SqlDataAdapter sda;
        SqlConnection koneksi = connection.Connect();
        DataTable dt;
        SqlDataReader rd;
        string imagePath = "";
        string lokasi = @"E:\Sajidan\C#\lks_test\assets";
        public gudang()
        {
            InitializeComponent();
            display();
        }
        public void display()
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new SqlCommand("SELECT * FROM [tbl_barang]", koneksi);
                dt = new DataTable();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                guna2DataGridView1.DataSource = dt;
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
        public void hapus()
        {
            txtHarga.Text = null;
            txtNama.Text = null;
            txtHarga.Text = null ;
            txtKode.Text = null;
            txtSatuan.Text = null;
        }
        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void txtNama_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                if (txtHarga.Text.Length == 0 || txtJml.Text.Length == 0 || txtKode.Text.Length == 0 || txtNama.Text.Length == 0 || txtSatuan.Text.Length == 0)
                {
                    MessageBox.Show("Tolong isi semua inputan!");
                }
                else
                {
                    string namaFile = Path.GetFileName(imagePath);
                    string lokasiSave = Path.Combine(lokasi, namaFile);
                    File.Copy(imagePath, lokasiSave, true);
                    cmd = new SqlCommand("INSERT INTO [tbl_barang] VALUES (@kode, @nama, @exp, @jml, @satuan, @harga, @image)", koneksi);
                    cmd.Parameters.AddWithValue("@kode", txtKode.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@exp", dtpEx.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@jml", txtJml.Text.Trim());
                    cmd.Parameters.AddWithValue("@satuan", txtSatuan.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", txtHarga.Text.Trim());
                    cmd.Parameters.AddWithValue("@image", namaFile);
                    cmd.ExecuteNonQuery();
                    display();
                    hapus();
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

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void gudang_Load(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                if (guna2DataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Harap pilih data yang ingin di ubah!");
                }
                int selectedId = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["id_barang"].Value);
                string namaFile = "";
                if (!string.IsNullOrEmpty(imagePath))
                {
                    namaFile = Path.GetFileName(imagePath);
                    string lokasiSimpan = Path.Combine(lokasi, namaFile);
                    File.Copy(imagePath, lokasiSimpan, true);
                }
                else
                {
                    namaFile = guna2DataGridView1.SelectedRows[0].Cells["image"].Value.ToString();
                }
                cmd = new SqlCommand("UPDATE [tbl_barang] SET kode_barang = @kode, nama_barang = @nama, expired_date = @exp, jumlah_barang = @jml, satuan = @satuan, harga_satuan = @harga, image = @image WHERE id_barang = @id", koneksi);
                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.Parameters.AddWithValue("@kode", txtKode.Text.Trim());
                cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                cmd.Parameters.AddWithValue("@exp", dtpEx.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@jml", txtJml.Text.Trim());
                cmd.Parameters.AddWithValue("@satuan", txtSatuan.Text.Trim());
                cmd.Parameters.AddWithValue("@harga", txtHarga.Text.Trim());
                cmd.Parameters.AddWithValue("@image", namaFile);

                cmd.ExecuteNonQuery();
                display();
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

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                int selectedId = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["id_barang"].Value);
                cmd = new SqlCommand("DELETE FROM [tbl_barang] WHERE id_barang = @id", koneksi);
                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.ExecuteNonQuery();
                display();
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
                txtNama.Text = row.Cells["nama_barang"].Value.ToString();
                txtKode.Text = row.Cells["kode_barang"].Value.ToString();
                txtJml.Text = row.Cells["jumlah_barang"].Value.ToString();
                txtSatuan.Text = row.Cells["satuan"].Value.ToString();
                txtHarga.Text = row.Cells["harga_satuan"].Value.ToString();
                string namaFile = row.Cells["image"].Value.ToString();
                string lokasiSimpan = Path.Combine(lokasi, namaFile);
                if (File.Exists(lokasiSimpan))
                {
                    using (FileStream fs = new FileStream(lokasiSimpan, FileMode.Open, FileAccess.Read))
                    {
                        guna2PictureBox2.Image?.Dispose();
                        guna2PictureBox2.Image = Image.FromStream(fs);
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Name files|*.jpg;|*.png;|*.jpeg";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                imagePath = opf.FileName;
                if (guna2PictureBox2.Image != null)
                {
                    guna2PictureBox2.Image.Dispose();
                    guna2PictureBox2.Image = null;
                }
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                guna2PictureBox2.Image = Image.FromStream(fs);
                fs.Close();
            }
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new SqlCommand("SELECT * FROM [tbl_barang] WHERE nama_barang LIKE '%"+ txtCari.Text+"%'", koneksi);
                cmd.Parameters.AddWithValue("@cari", txtCari.Text.Trim());
                dt = new DataTable();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                guna2DataGridView1.DataSource = dt;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error : "+ ex.Message);
            }finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void txtNama_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void txtJml_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void S(object sender, EventArgs e)
        {

        }
    }
}

