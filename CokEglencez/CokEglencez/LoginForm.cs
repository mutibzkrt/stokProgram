using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CokEglencez
{
    public partial class GirisFormu : Form
    {
        public GirisFormu()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtBoxKullanıcıGiris.Text;
            string sifre = txtBoxSifre.Text;
            if (KullaniciDogrula(kullaniciAdi,sifre))
            {
                AnaForm anaForm = new AnaForm();
                anaForm.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KullaniciDogrula(object kullaniciAdi, object sifre)
        {
            string connectionString = "Data Source=YG0234-22-01\\SQLEXPRESS;Initial Catalog=stokV8;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi AND Sifre = @Sifre";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@Sifre", sifre);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private void btnYardım_Click(object sender, EventArgs e)
        {
            Yardım yardımKısım = new Yardım();
            yardımKısım.Show();
        }
        private void btnAltay_Click_1(object sender, EventArgs e)
        {   // İstediğiniz internet sitesinin URL'sini aşağıdaki string değişkenine atayın
            string websiteUrl = "https://github.com/mutibzkrt";
            // Belirtilen internet sitesini varsayılan web tarayıcısıyla aç
            Process.Start(websiteUrl);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamayı kapatmak istiyor musunuz?", "Kapat", MessageBoxButtons.YesNo);
            // Kullanıcı "Evet"i seçtiyse uygulamayı kapat
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            // Kullanıcı "Hayır"ı seçtiyse hiçbir şey yapma
            else if (result == DialogResult.No)
            {
            }
        }

        private void btnSatinAl_Click(object sender, EventArgs e)
        {
            string Id = "muhammetBozkurt";
            string Sifre = "12345";
            if (txtBoxKullanıcıGiris.Text == Id && txtBoxSifre.Text == Sifre)
            {
               SatinAl ss = new SatinAl();
                ss.Show();  
            }
            else
            {
                MessageBox.Show("Kulanıcı bilgisi veya Kulanıcı şifresi eşleşmiyor.Tekrar deneyiniz.Sadece özel yetkinlik belgesi olanlar girebilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUye_Click(object sender, EventArgs e)
        {
            UyeOl uye = new UyeOl();
            uye.Show();
        }
    }
}

    
      
        
        
