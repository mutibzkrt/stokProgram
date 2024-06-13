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
    public partial class Guncelleyici : Form
    {
        private string connectionString = "Data Source=YG0234-22-01\\SQLEXPRESS;Initial Catalog=stokV8;Integrated Security=True";
        private DataGridViewGuncelleyici guncelleyici;

        public event EventHandler GuncellemeTamamlandi;
        public Guncelleyici(string urunAdi, string urunKodu, string kategori)
        {
            InitializeComponent();
            // TextBox'lara bilgileri yerleştir
            textBox1.Text = urunAdi;
            textBox2.Text = urunKodu;
            comboBoxGuncelleyici.Text = kategori;
            this.Load += Guncelleyici_Load;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yeniUrunAdi = textBox1.Text;
            string yeniUrunKodu = textBox2.Text;
            string yeniKategori = comboBoxGuncelleyici.Text;
            // Veritabanında güncellenen ürün hariç aynı isim veya kodla başka bir ürün var mı kontrol et
            if (UrunVarMi(yeniUrunAdi, yeniUrunKodu))
            {
                MessageBox.Show("Girdiğiniz ürün adı veya ürün kodu zaten mevcut!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                // Veritabanında güncelleme işlemleri yap
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Urunler SET UrunAdi = @YeniUrunAdi, Kategori = @YeniKategori WHERE UrunKodu = @UrunKodu";
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@YeniUrunAdi", yeniUrunAdi);
                    command.Parameters.AddWithValue("@YeniKategori", yeniKategori);
                    command.Parameters.AddWithValue("@UrunKodu", yeniUrunKodu); 
                    int rowsAffected = command.ExecuteNonQuery();
                  
                    if (rowsAffected == 0)
                    {
                        // Güncelleme işlemleri başarıyla tamamlandıysa, kullanıcıya bilgi ver.
                        MessageBox.Show("Ürün bilgileri başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Güncelleme tamamlandığında olayı tetikle
                        GuncellemeTamamlandi?.Invoke(this, EventArgs.Empty);
                        // Formu kapat
                        this.Close();

                    }
                    else
                    {
                        // Güncelleme işlemleri başarısız olduysa, kullanıcıya bilgi ver.
                        MessageBox.Show("Ürün bilgileri güncellenirken bir hata oluştu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool UrunVarMi(string yeniUrunAdi, string yeniUrunKodu)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Aynı ürün adına ve ürün koduna sahip ürünleri kontrol etmek için sorgu
                string kontrolQuery = "SELECT COUNT(*) FROM Urunler WHERE UrunAdi = @UrunAdi OR UrunKodu = @UrunKodu";
                // Kontrol komutu oluşturma
                SqlCommand kontrolCommand = new SqlCommand(kontrolQuery, connection);
                kontrolCommand.Parameters.AddWithValue("@UrunAdi", yeniUrunAdi);
                kontrolCommand.Parameters.AddWithValue("@UrunKodu", yeniUrunKodu);
                int urunSayisi = (int)kontrolCommand.ExecuteScalar();
                // Aynı ürün adına veya ürün koduna sahip ürün varsa true döndür
                return urunSayisi > 0;
            }
        }

        private void Guncelleyici_Load(object sender, EventArgs e)
        {
            ComboBoxDoldurucu.KategorileriDoldur2(comboBoxGuncelleyici);

        }
    }
}
