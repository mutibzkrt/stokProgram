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

namespace CokEglencez
{
  
    public partial class Guncelleyici2 : Form
    {
        private string connectionString = "Data Source=YG0234-22-01\\SQLEXPRESS;Initial Catalog=stokV8;Integrated Security=True";
        private int girisID;
        private int urunID;

        public event EventHandler GuncellemeTamamlandi2;
        public Guncelleyici2(string urunAdi,string urunKodu,string miktar,string notlar ,string tarih ,string lotNumarasi ,string seriNumarasi, int girisID, int urunID)
        {
            InitializeComponent();
            textBoxGUrunAd.Text = urunAdi;
            textBoxGUrunKod.Text = urunKodu;
            textBoxGMiktar.Text = miktar;
            textBoxGNot.Text = notlar;
            textBoxGLotNumarasi.Text = lotNumarasi;
            textBoxGSeriNumaralari.Text = seriNumarasi;
            dateTimePicker1.Text = tarih;
            this.girisID = girisID;
            this.urunID = urunID;
        

        }
        private void buttonGuncelle_Click(object sender, EventArgs e)
        {
            string yeniMiktar = textBoxGMiktar.Text.Replace(',', '.');
            string yeniNotlar = textBoxGNot.Text;
            string yeniLotNumarasi = textBoxGLotNumarasi.Text;

            // Miktarın ve notların boş olup olmadığını kontrol ediyoruz
            if (string.IsNullOrEmpty(textBoxGMiktar.Text) || string.IsNullOrEmpty(textBoxGNot.Text))
            {
                MessageBox.Show("Lütfen Miktar ve Notları giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Veritabanı bağlantısını açıyoruz
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Ürün giriş bilgilerini güncelliyoruz
                string urunGirisUpdateQuery = "UPDATE UrunGiris SET Miktar = @Miktar, Notlar = @Notlar, LotNumarasi = @LotNumarasi WHERE GirisID = @GirisID";
                SqlCommand urunGirisCommand = new SqlCommand(urunGirisUpdateQuery, connection);
                urunGirisCommand.Parameters.AddWithValue("@Miktar", yeniMiktar);
                urunGirisCommand.Parameters.AddWithValue("@Notlar", yeniNotlar);
                urunGirisCommand.Parameters.AddWithValue("@LotNumarasi", yeniLotNumarasi);
                urunGirisCommand.Parameters.AddWithValue("@GirisID", girisID);
                urunGirisCommand.ExecuteNonQuery();

                // Seri numaralarını ayırıyoruz
                string seriNumarasiGirisi = textBoxGSeriNumaralari.Text;
                string[] seriNumaralari = seriNumarasiGirisi.Split(',');

                // Her seri numarası için INSERT işlemi gerçekleştiriyoruz
                foreach (string seriNo in seriNumaralari)
                {
                    string seriNoEkleQuery = @"
                        INSERT INTO SeriNumaralari (GirisID, UrunID, LotNumarasi, SeriNo) 
                        VALUES (@GirisID, @UrunID, @LotNumarasi, @SeriNo)";
                    SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection);
                    seriNoEkleCommand.Parameters.AddWithValue("@GirisID", girisID);
                    seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID); // Ürün ID'sini ekliyoruz
                    seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", yeniLotNumarasi);
                    seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNo.Trim()); // Boşlukları kaldırıyoruz
                    seriNoEkleCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Ürün girişi başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Güncelleme tamamlandığında event'i tetikliyoruz
                GuncellemeTamamlandi2?.Invoke(this, EventArgs.Empty);
                this.Close(); // Formu kapatıyoruz
            }
        }
     
    }
}
