using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lks_test
{
    public partial class AdminForm : Form
    {
        SqlCommand cmd;
        SqlConnection koneksi = connection.Connect();
        SqlDataAdapter sda;
        SqlDataReader rd;
        DataTable dt;
        public AdminForm()
        {
            InitializeComponent();
            Timer timer = new Timer();  
            timer.Start();
            timer.Interval = 1000;
            timer.Tick += display;
        }
        public void display(object sender, EventArgs e)
        {
            CultureInfo c = new CultureInfo("id-ID");
            tgl.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy", c);
            jam.Text = DateTime.Now.ToString("T");
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            foreach (Control control in guna2Panel2.Controls)
            {
                if (control is Form ex)
                {
                    ex.Dispose();
                    ex.Close();
                }
            }
            Kelola kelola = new Kelola();
            AdminForm admin = new AdminForm();
            kelola.TopLevel = false;
            guna2Panel2.Controls.Add(kelola);
            kelola.FormBorderStyle = FormBorderStyle.None;
            kelola.Dock = DockStyle.Fill;
            kelola.BringToFront();
            admin.Hide();
            kelola.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            koneksi.Open();
            cmd = new SqlCommand("SELECT * FROM tbl_log WHERE waktu BETWEEN @awal AND @akhir", koneksi);
            cmd.Parameters.AddWithValue("@awal", awal.Value);
            cmd.Parameters.AddWithValue("@akhir", akhir.Value);
            dt = new DataTable();
            sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            koneksi.Close();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            koneksi.Open();
            cmd = new SqlCommand("INSERT INTO tbl_log VALUES(@waktu, @akt, @id)", koneksi);
            cmd.Parameters.AddWithValue("@waktu", DateTime.Now);
            cmd.Parameters.AddWithValue("@akt", "Logout");
            cmd.Parameters.AddWithValue("@id", Session.id_user);
            cmd.ExecuteNonQuery();
            koneksi.Close();
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            foreach (Control control in guna2Panel2.Controls)
            {
                if (control is Form ex)
                {
                    ex.Dispose();
                    ex.Close();
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            foreach (Control control in guna2Panel2.Controls)
            {
                if (control is Form ex)
                {
                    ex.Dispose();
                    ex.Close();
                }
            }
            Laporan laporan = new Laporan();
            AdminForm admin = new AdminForm();
            laporan.TopLevel = false;
            guna2Panel2.Controls.Add(laporan);
            laporan.FormBorderStyle = FormBorderStyle.None;
            laporan.Dock = DockStyle.Fill;
            laporan.BringToFront();
            admin.Hide();
            laporan.Show();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void awal_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
