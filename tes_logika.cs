using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lks_test
{
    public partial class tes_logika: Form
    {
        public tes_logika()
        {
            InitializeComponent();
        }

        private void tes_logika_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //string[] sajidan = { "sajidan", "hendi", "januardi" };
            //StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < sajidan.Length; i++)
            //{
            //    sb.Append(sajidan[i]);
            //    if (i < sajidan.Length - 1)
            //    {
            //        sb.Append(", ");
            //    }
            //}

            //StringBuilder sb = new StringBuilder(); 
            //for (int i = 1; i < 10; i+=2)
            //{
            //    sb.Append(i.ToString()+ "");
            //}
            //output.Text = sb.ToString();

            StringBuilder sb = new StringBuilder();
            int[] nomor = { 1, 2, 3 , 4, 5};
            int total = 0;
            for (int i = 0; i < nomor.Length; i++)
            {
                total += nomor[i];
                sb.Append(i.ToString() + "");
            }
            output.Text = sb.ToString();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
