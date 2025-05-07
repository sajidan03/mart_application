using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Xsl;

namespace lks_test
{
    public partial class Laporan : Form
    {
        SqlCommand cmd;
        SqlDataAdapter sda;
        SqlConnection koneksi = connection.Connect();
        SqlDataReader rd;
        DataTable dt;
        public Laporan()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Start(); 
            timer.Interval = 1000;
            timer.Tick += tick;
        }
        public void tick(object sender, EventArgs e)
        {
            CultureInfo c = new CultureInfo("id-ID");
            tgl.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy", c);
            jam.Text = DateTime.Now.ToString("T");
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new SqlCommand("SELECT * FROM tbl_transaksi WHERE tgl_transaksi BETWEEN @awal AND @akhir", koneksi);
                cmd.Parameters.AddWithValue("@awal", awal.Value);
                cmd.Parameters.AddWithValue("@akhir", akhir.Value);
                dt = new DataTable();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new SqlCommand("SELECT SUM(total_bayar) as total, tgl_transaksi AS date FROM tbl_transaksi WHERE tgl_transaksi BETWEEN @awal AND @akhir GROUP BY tgl_transaksi", koneksi);
                cmd.Parameters.AddWithValue("@awal", awal.Value);
                cmd.Parameters.AddWithValue("@akhir", akhir.Value);
                rd = cmd.ExecuteReader();
                chart1.Series.Clear();
                chart1.Series.Add("Omset");
                int i = 0;
                while (rd.Read())
                {
                    string date = rd["date"].ToString();
                    double total = Convert.ToDouble(rd["total"]);
                    chart1.Series[i].Points.AddXY(date, total);
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            //    try
            //    {
            //        // Simpan chart sebagai gambar sementara
            //        string tempPath = Path.Combine(Path.GetTempPath(), "chart_image.png");
            //        chart1.SaveImage(tempPath, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);

            //        // Buat file Excel baru
            //        using (var workbook = new XLWorkbook())
            //        {
            //            var worksheet = workbook.Worksheets.Add("Chart Data");

            //            // Masukkan gambar ke dalam Excel
            //            var image = worksheet.AddPicture(tempPath)
            //                                 .MoveTo(worksheet.Cell("B2"))
            //                                 .Scale(1); // Ubah skala jika diperlukan

            //            // Simpan file Excel
            //            SaveFileDialog saveFileDialog = new SaveFileDialog
            //            {
            //                Filter = "Excel Files|*.xlsx",
            //                Title = "Simpan Chart ke Excel",
            //                FileName = "ChartExport.xlsx"
            //            };

            //            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //            {
            //                workbook.SaveAs(saveFileDialog.FileName);
            //                MessageBox.Show("Chart berhasil diekspor ke Excel!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //        }

            //        // Hapus gambar sementara
            //        if (File.Exists(tempPath))
            //        {
            //            File.Delete(tempPath);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error: " + ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            try
            { 
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Chart Data");
                    worksheet.Cell(1, 1).Value = "Tanggal pembelian";
                    worksheet.Cell(1, 2).Value = "Total bayar";
                    worksheet.Range("A1:B1").Style.Font.Bold = true;
                    for (int i = 0; i < chart1.Series[0].Points.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = chart1.Series[0].Points[i].AxisLabel;
                        worksheet.Cell(i + 2, 2).Value = chart1.Series[0].Points[i].YValues[0];
                    }
                    var range = worksheet.RangeUsed();
                    range.CreateTable();
                    worksheet.Column(1).Style.NumberFormat.Format = "yyyy-MM-dd";
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        Title = "Simpan Data Chart ke Excel",
                        FileName = "Laporan keuangan.xlsx"
                    };

                    //if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    //{
                    //    workbook.SaveAs(saveFileDialog.FileName);
                    //    MessageBox.Show("Data Chart berhasil diekspor ke Excel!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;
                        workbook.SaveAs(filePath);

                        DialogResult result = MessageBox.Show("Data berhasil diekspor!\nBuka file sekarang?", "Sukses",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = filePath,
                                UseShellExecute = true
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
