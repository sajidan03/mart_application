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
    public partial class Login : Form
    {
        SqlCommand cmd;
        SqlConnection koneksi = connection.Connect();
        SqlDataAdapter sda;
        SqlDataReader rd;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                if (txtUsn.Text.Length == 0 && txtPswd.Text.Length == 0)
                {
                    MessageBox.Show("Tolong isi kolom username dan password");
                }
                else
                {
                    string pw = txtPswd.Text;
                    string hash = HashPassword.hash(pw);
                    cmd = new SqlCommand("SELECT * FROM tbl_users WHERE username = @username AND password = @password", koneksi);
                    cmd.Parameters.AddWithValue("@username", txtUsn.Text);
                    cmd.Parameters.AddWithValue("@password", hash);
                    rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        rd.Read();
                        int id = Convert.ToInt32(rd["id_user"]);
                        string nama = rd["nama"].ToString();
                        string tipe = rd["tipe_user"].ToString();
                        Session.start(id, nama);
                        rd.Close();
                        //
                        cmd = new SqlCommand("INSERT INTO [tbl_log] VALUES (@waktu, @akt, @id)", koneksi);
                        cmd.Parameters.AddWithValue("@waktu", DateTime.Now);
                        cmd.Parameters.AddWithValue("@akt", "Login");
                        cmd.Parameters.AddWithValue("@id", Session.id_user);
                        cmd.ExecuteNonQuery();
                        //
                        if (tipe == "gudang")
                        {
                            gudang gudang = new gudang();
                            this.Hide();
                            gudang.ShowDialog();
                        }
                        if (tipe == "admin")
                        {
                            AdminForm admin = new AdminForm();
                            admin.Show();
                            this.Hide();
                        }
                        if (tipe == "kasir")
                        {
                            Kasir kasir = new Kasir();
                            this.Hide();
                            kasir.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username atau password salah!");
                    }
                }
              
            }
            catch (Exception ex) {
                MessageBox.Show("Error connection : "+ ex.Message);
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            txtPswd.Text = null;
            txtUsn.Text = null;
        }
    }
}
