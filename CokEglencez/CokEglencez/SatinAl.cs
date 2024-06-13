using DocumentFormat.OpenXml.Drawing.Diagrams;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace CokEglencez
{
    public partial class SatinAl : Form
    {
        private string connectionString = "Data Source=YG0234-22-01\\SQLEXPRESS;Initial Catalog=stokV8;Integrated Security=True";
        private DataGridViewGuncelleyici guncelleyici2;
        public SatinAl()
        {
            InitializeComponent();
            guncelleyici2 = new DataGridViewGuncelleyici(connectionString); // guncelleyici2 nesnesini başlatıyoruz
            guncelleyici2.Fhrist(dataGriwGos); // Fhrist metodunu çağırıyoruz 
            LoadSatinAlData();
        }
        private void BtnKaydetFihrist_Click(object sender, EventArgs e)
        {
            string ad = txtSirketAd.Text;
            string calisanAd = txtSirketCalisanAd.Text;
            string sirketMail = txtSirketMail.Text;
            string sirketTelefon = txtSirketTelefon.Text;
            string sirketTelefonIki = txtSirketTelefonİki.Text;
            if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(calisanAd) || String.IsNullOrEmpty(sirketMail) || String.IsNullOrEmpty(sirketTelefon) || string.IsNullOrEmpty(sirketTelefonIki))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (urunVarmi(calisanAd))
                {
                    MessageBox.Show("Rehberde zaten o isimde kişi var ");
                    txtSirketAd.Text = "";
                    txtSirketCalisanAd.Text = "";
                    txtSirketMail.Text = "";
                    txtSirketTelefon.Text = "";
                    txtSirketTelefonİki.Text = "";
                }
                else
                {
                    EkleUrun(ad, calisanAd, sirketMail, sirketTelefon, sirketTelefonIki);
                    guncelleyici2.Fhrist(dataGriwGos);
                    txtSirketAd.Text = "";
                    txtSirketCalisanAd.Text = "";
                    txtSirketMail.Text = "";
                    txtSirketTelefon.Text = "";
                    txtSirketTelefonİki.Text = "";
                }
            }
        }
        private void EkleUrun(string ad, string calisanAd, string sirketMail, string sirketTelefon, string sirketTelefonIki)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Fihrist tablosuna ekleme sorgusu
                string urunEkleQuery = "INSERT INTO Fihrist (SirketIsmi, CalisanIsmi, Email, Telefon1, Telefon2) VALUES (@SirketIsmi, @CalisanIsmi, @MailAdresi, @TelefonNumarasi1, @TelefonNumarasi2)";
                // Ekleme komutu oluşturma
                SqlCommand urunEkleCommand = new SqlCommand(urunEkleQuery, connection);
                urunEkleCommand.Parameters.AddWithValue("@SirketIsmi", ad);
                urunEkleCommand.Parameters.AddWithValue("@CalisanIsmi", calisanAd);
                urunEkleCommand.Parameters.AddWithValue("@MailAdresi", sirketMail);
                urunEkleCommand.Parameters.AddWithValue("@TelefonNumarasi1", sirketTelefon);
                urunEkleCommand.Parameters.AddWithValue("@TelefonNumarasi2", sirketTelefonIki);
                // Veritabanına ekleme işlemi
                urunEkleCommand.ExecuteNonQuery();
            }
        }

        private bool urunVarmi(string calisanAd)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string kontrolQuery = "SELECT COUNT(*) FROM Fihrist WHERE CalisanIsmi = @CalisanIsmi";
                SqlCommand kontrolCommand = new SqlCommand(kontrolQuery, connection);
                kontrolCommand.Parameters.AddWithValue("@CalisanIsmi", calisanAd);
                int urunSayisi = (int)kontrolCommand.ExecuteScalar();
                return urunSayisi > 0;
            }
        }
        private void btnAraFhrist_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGriwGos, txtFara.Text);
        }
       
        private void btnUrunSatinAl_Click(object sender, EventArgs e)
        {
            // Kullanıcı girişlerini al
            string urunAdi = txtBoxUrunAdi.Text;
            string urunKodu = txtBoxUrunKodu.Text;

            // Ürünün mevcut olup olmadığını kontrol et
            if (!UrunVarMi(urunAdi, urunKodu))
            {
                MessageBox.Show("Girdiğiniz ürün veritabanında mevcut değil. Lütfen önce ürün oluşturun.");
                return;
            }

            // Eğer ürün mevcut ise, satın alma işlemine devam edilebilir...
            // Diğer girişleri al ve işlemi gerçekleştir
            string firma = txtBoxUrunFirma.Text;
            float miktar;
            float birimFiyat;

            // Diğer girişlerin kontrolü burada yapılacak...

            // Miktar ve birim fiyat girişlerini kontrol et
            if (!float.TryParse(txtBoxUrunMiktar.Text, out miktar) || miktar <= 0)
            {
                MessageBox.Show("Geçersiz miktar girildi. Lütfen pozitif bir sayı girin.");
                return; // Ekleme işlemi iptal ediliyor
            }

            if (!float.TryParse(txtBoxUrunBirimFiyat.Text, out birimFiyat) || birimFiyat <= 0)
            {
                MessageBox.Show("Geçersiz birim fiyat girildi. Lütfen pozitif bir sayı girin.");
                return; // Ekleme işlemi iptal ediliyor
            }

            string lotNumarasi = txtBoxUrunLot.Text;
            DateTime alisTarihi = dateTimePicker1.Value;
            string paraBirimi = "";

            // Seçilen para birimini kontrol et
            if (radioButtonDolar.Checked)
            {
                paraBirimi = "Dolar";
            }
            else if (radioButtonEuro.Checked)
            {
                paraBirimi = "Euro";
            }
            else if (radioButtonTl.Checked)
            {
                paraBirimi = "TL";
            }
            else if (radioButtonÇinYuanı.Checked)
            {
                paraBirimi = "Çin Yuanı";
            }
            else if (radioButtonRusRuble.Checked)
            {
                paraBirimi = "Rus Rublesi";
            }
            else
            {
                MessageBox.Show("Lütfen bir para birimi seçin.");
                return; // Ekleme işlemi iptal ediliyor
            }
            // Gerekli bilgileri doldurulmadıysa hata mesajı göster
            if (string.IsNullOrWhiteSpace(urunAdi) || string.IsNullOrWhiteSpace(urunKodu) || string.IsNullOrWhiteSpace(firma))
            {
                MessageBox.Show("Lütfen tüm gerekli bilgileri girin.");
                return; // Ekleme işlemi iptal ediliyor
            }
            // Veritabanı işlemleri
            try
            {
                // Veritabanı bağlantısı ve işlemleri
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // SQL sorgusu ve parametreler
                    string sqlQuery = "INSERT INTO Alislar (UrunID, LotNumarasi, BirimFiyat, AlisTarihi, Firma, Miktar, ToplamFiyat, ParaBirimi) " +
                                      "VALUES ((SELECT UrunID FROM Urunler WHERE UrunAdi = @UrunAdi AND UrunKodu = @UrunKodu), @LotNumarasi, @BirimFiyat, @AlisTarihi, @Firma, @Miktar, @ToplamFiyat, @ParaBirimi)";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Parametrelerin atanması
                        command.Parameters.AddWithValue("@UrunAdi", urunAdi);
                        command.Parameters.AddWithValue("@UrunKodu", urunKodu);
                        command.Parameters.AddWithValue("@LotNumarasi", lotNumarasi);
                        command.Parameters.AddWithValue("@BirimFiyat", birimFiyat);
                        command.Parameters.AddWithValue("@AlisTarihi", alisTarihi);
                        command.Parameters.AddWithValue("@Firma", firma);
                        command.Parameters.AddWithValue("@Miktar", miktar);
                        command.Parameters.AddWithValue("@ToplamFiyat", miktar * birimFiyat);
                        command.Parameters.AddWithValue("@ParaBirimi", paraBirimi);
                        // Bağlantı açma ve komutu çalıştırma
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        // İşlem sonucunu gösterme
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Satın alma işlemi başarıyla gerçekleştirildi.");
                            // Formdaki giriş kutularını temizleme
                            txtBoxUrunAdi.Text = "";
                            txtBoxUrunKodu.Text = "";
                            txtBoxUrunFirma.Text = "";
                            txtBoxUrunMiktar.Text = "";
                            txtBoxUrunBirimFiyat.Text = "";
                            txtBoxUrunLot.Text = "";
                            LoadSatinAlData(); // Satın alma verilerini yeniden yükleme
                        }
                        else
                        {
                            MessageBox.Show("Satın alma işlemi başarısız oldu.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        private bool UrunVarMi(string urunAdi, string urunKodu)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Urunler WHERE UrunAdi = @UrunAdi AND UrunKodu = @UrunKodu";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UrunAdi", urunAdi);
                command.Parameters.AddWithValue("@UrunKodu", urunKodu);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private void btnARA_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGridViewSatinAl, textBox1.Text);
        }
        private void LoadSatinAlData()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Sorgu
                string query = "SELECT Urunler.UrunAdi, Urunler.UrunKodu, Alislar.LotNumarasi, Alislar.BirimFiyat, Alislar.AlisTarihi, Alislar.Firma, Alislar.Miktar, Alislar.ToplamFiyat, Alislar.ParaBirimi FROM Alislar INNER JOIN Urunler ON Alislar.UrunID = Urunler.UrunID";

                // Veri seti ve veri adaptörü oluştur
                DataSet dataSet = new DataSet();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                try
                {
                    // Verileri doldur
                    dataAdapter.Fill(dataSet, "Alislar");

                    // DataGridView'e veri kaynağını ayarla
                    dataGridViewSatinAl.DataSource = dataSet.Tables["Alislar"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri yüklenirken hata oluştu: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void SatinAl_Load(object sender, EventArgs e)
        {

        }
    }
}



