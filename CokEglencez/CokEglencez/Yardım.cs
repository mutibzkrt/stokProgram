using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace CokEglencez
{
    public partial class Yardım : Form
    {
        public Yardım()
        {
            InitializeComponent();
        }

        private void Yardım_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
        }

        private void btnMail_Click(object sender, EventArgs e)
        {
            string emailAddress = "stok.eo@altay.com.tr";
            OpenDefaultMailClient(emailAddress);
        }

        private void OpenDefaultMailClient(string emailAddress)
        {
            try
            {
                Process.Start("mailto:" + emailAddress);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Varsayılan e-posta istemcisi açılırken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
