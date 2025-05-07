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
                string password = HashPassword.hash(txtPswd.Text);
                cmd = new SqlCommand("SELECT * FROM tbl_users where username = @username AND password = @password", koneksi);
                cmd.Parameters.AddWithValue("@username", txtUsn.Text.Trim());
                cmd.Parameters.AddWithValue("@password", password);
                rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    int id = Convert.ToInt32(rd["id_user"]);
                    string nama = rd["username"].ToString();
                    string tipe = rd["tipe_user"].ToString();
                    Session.start(id, nama);
                    rd.Close();
                    cmd = new SqlCommand("INSERT INTO tbl_log VALUES (@waktu,@akt,@id)", koneksi);
                    cmd.Parameters.AddWithValue("@waktu", DateTime.Now);
                    cmd.Parameters.AddWithValue("@akt", "Login");
                    cmd.Parameters.AddWithValue("@id", Session.id_user);
                    cmd.ExecuteNonQuery();
                    if (tipe == "admin")
                    {
                        AdminForm admin = new AdminForm();
                        admin.Show();
                        this.Hide();
                    }
                    if (tipe == "gudang")
                    {
                        gudang gudang = new gudang();
                        gudang.Show();
                        this.Hide();
                    }
                    if (tipe == "kasir")
                    {
                        Kasir kasir = new Kasir();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Username atau password salah!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error koneksi" + ex.Message);
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
