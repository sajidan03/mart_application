using lks_mart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lks_test
{
    public partial class Kelola : Form
    {
        SqlConnection koneksi = connection.Connect();
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        SqlDataReader rd;
        public Kelola()
        {
            InitializeComponent();
            cmbTipe.Items.Add("gudang");
            cmbTipe.Items.Add("kasir");
            cmbTipe.Items.Add("admin");
            display();
            hapus();
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void display()
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new SqlCommand("SELECT * FROM tbl_users", koneksi);
                dt = new DataTable();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error koneksi : " + ex.Message);
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
            txtNama.Text = null;
            txtAlamat.Text = null;
            txtTelepon.Text = null;
            txtUsername.Text = null;
            txtPswd.Text = null;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (txtPswd.Text.Length == 0 && txtNama.Text.Length == 0 && txtAlamat.Text.Length == 0 && txtUsername.Text.Length == 0 && txtTelepon.Text.Length == 0)
            {
                MessageBox.Show("Tolong isi semua inputan!");
            }
            else
            {
                string pw = txtPswd.Text;
                string hash = HashPassword.hash(pw);
                koneksi.Open();
                cmd = new SqlCommand("INSERT INTO tbl_users VALUES (@tipe, @nama, @alamat, @telepon,@username, @password)", koneksi);
                cmd.Parameters.AddWithValue("@tipe", cmbTipe.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                cmd.Parameters.AddWithValue("@telepon", txtTelepon.Text.Trim());
                cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@password", hash);
                cmd.ExecuteNonQuery();
                display();
                hapus();
                MessageBox.Show("Input berhasil!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                koneksi.Close();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (txtPswd.Text.Length == 0 && txtNama.Text.Length == 0 && txtAlamat.Text.Length == 0 && txtUsername.Text.Length == 0 && txtTelepon.Text.Length == 0)
            {
                MessageBox.Show("Tolong isi semua inputan!");
            }
            else
            {
                koneksi.Open();
                int selectedId;
                if (!int.TryParse(ID.Text, out selectedId))
                {
                    MessageBox.Show("ID tidak valid");
                    return;
                }
                cmd = new SqlCommand("UPDATE tbl_users SET tipe_user = @tipe, nama = @nama, alamat = @alamat, username = @username, password = @password, telepon = @telepon WHERE id_user = @id", koneksi);
                cmd.Parameters.AddWithValue("@tipe", cmbTipe.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                cmd.Parameters.AddWithValue("@telepon", txtTelepon.Text.Trim());
                cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@password", txtPswd.Text.Trim());
                cmd.ExecuteNonQuery();
                display();
                hapus();
                MessageBox.Show("Input berhasil!");
                koneksi.Close();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (txtPswd.Text.Length == 0 && txtNama.Text.Length == 0 && txtAlamat.Text.Length == 0 && txtUsername.Text.Length == 0 && txtTelepon.Text.Length == 0)
            {
                MessageBox.Show("Tolong isi semua inputan!");
            }
            else
            {
                DialogResult result = MessageBox.Show("Apakah anda yakin ingin menghapus data ini?", "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    koneksi.Open();
                    int selectedId = Convert.ToInt32(ID.Text.ToString());
                    cmd = new SqlCommand("DELETE FROM tbl_users WHERE id_user = @id", koneksi);
                    cmd.Parameters.AddWithValue("@id", selectedId);
                    cmd.ExecuteNonQuery();
                    display();
                    hapus();
                    MessageBox.Show("Input berhasil!");
                    koneksi.Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtNama.Text = row.Cells["nama"].Value.ToString();
                txtAlamat.Text = row.Cells["alamat"].Value.ToString();
                txtTelepon.Text = row.Cells["telepon"].Value.ToString();
                txtUsername.Text = row.Cells["username"].Value.ToString();
                txtPswd.Text = row.Cells["password"].Value.ToString();
                cmbTipe.Text = row.Cells["tipe_user"].Value.ToString();
                ID.Text = row.Cells["id_user"].Value.ToString();
            }
        }

        private void txtTelepon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
