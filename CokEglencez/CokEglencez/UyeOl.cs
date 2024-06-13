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
using System.Text.RegularExpressions;


namespace CokEglencez
{
    public partial class UyeOl : Form
    {
        public UyeOl()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string connectionString = "Data Source=YG0234-22-01\\SQLEXPRESS;Initial Catalog=stokV8;Integrated Security=True";

        // Girişin sadece harf ve sayıları içerip içermediğini kontrol eden fonksiyon
        private bool IsAlphaNumeric(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
        }
        private void btnKayitOl_Click(object sender, EventArgs e)
        { // Kullanıcı adı ve şifre değerlerini al
            string kullaniciAdi = txtBoxIsim.Text;
            string sifre = txtBoxSoy.Text;

            // Kullanıcı adı kontrolü
            if (!IsAlphaNumeric(kullaniciAdi))
            {
                MessageBox.Show("Kullanıcı adı yalnızca harf ve sayı içermelidir.");
                return;
            }

            // Şifre kontrolü
            if (!IsAlphaNumeric(sifre))
            {
                MessageBox.Show("Şifre yalnızca harf ve sayı içermelidir.");
                return;
            }

            // Veritabanı bağlantısı oluştur
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // INSERT sorgusu
                string query = "INSERT INTO Kullanicilar (KullaniciAdi, Sifre) VALUES (@KullaniciAdi, @Sifre)";

                // SqlCommand nesnesi oluştur
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@Sifre", sifre);

                    try
                    {
                        // Bağlantıyı aç
                        connection.Open();
                        // Sorguyu çalıştır
                        int rowsAffected = command.ExecuteNonQuery();
                        // Başarılı bir şekilde eklendiyse
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kullanıcı başarıyla kaydedildi.");
                            // Formu temizle
                            txtBoxIsim.Clear();
                            txtBoxSoy.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }

        }
    }
}

