using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml.Office2010.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenXmlPowerTools;
using System.Text;
using System.Web;
using System.Net.Http;
using DocumentFormat.OpenXml.Drawing;
using YourNamespace;

namespace CokEglencez
{
    public partial class AnaForm : Form
    {
        // Veritabanı bağlantı dizesi
        private string connectionString = "Data Source=YG0234-22-01\\SQLEXPRESS;Initial Catalog=stokV8;Integrated Security=True";
        private DataGridViewGuncelleyici guncelleyici;
        private QRIslemleri qrIslemleri;
        public AnaForm()
        {
            InitializeComponent();
            // Form yüklenirken DataGridView'i güncelle
            guncelleyici = new DataGridViewGuncelleyici(connectionString);
            qrIslemleri=new QRIslemleri(connectionString);
            this.Load += AnaForm_Load;
            RefreshComboBox();
            RefreshComboBox2();
            RefreshComboBox3();
            RefreshComboBox4();
            RefreshComboBox5();
            RefreshComboBox6();
            RefreshComboBox7();
            comboBoxIcerigeBak1();
        }
        private void AnaForm_Load(object sender, EventArgs e)
        {
            ComboBoxDoldurucu.KategorileriDoldur(cmbBoxKat);
            ComboBoxDoldurucu.CikisKategorileriDoldur(cmbCikisKategori);
            ComboBoxDoldurucu.CikisNedenleriDoldur(cmbCikisNedeni);
            ComboBoxDoldurucu.KategoriDoldur3(comboBoxBomOlustur);
            ComboBoxDoldurucu.SonUrunDoldur(comboBoxTamKategori);
            // DataGridView'leri güncelle
            guncelleyici.UrunListesiniGuncelle(dataGridView1);
            guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
            guncelleyici.stokGuncelle(dataGridView3);
            guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
            guncelleyici.stokGuncelle(dataGridView5);
            guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
            guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
            guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
            guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
            guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
            guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");

        }




        #region ürünü olusturuduk
        private void btnUrunOlustur_Click(object sender, EventArgs e)
        {
            string urunAdi = txtBoxUrunAdı.Text;
            string urunKodu = txtBoxUrunKod.Text;
            string kategori = cmbBoxKat.Text;
            // Girişlerin boş olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(urunAdi) || string.IsNullOrWhiteSpace(urunKodu) || string.IsNullOrWhiteSpace(kategori))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Ürünü veritabanına ekle
                if (UrunVarMi(urunAdi, urunKodu))
                {
                    MessageBox.Show("Aynı ürün adı veya ürün kodu zaten var!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBoxUrunAdı.Text = "";
                    txtBoxUrunKod.Text = "";
                }
                else
                {
                    EkleUrun(urunAdi, urunKodu, kategori);
                    MessageBox.Show("Malzeme oluşturuldu."+"Malzeme adı:"+urunAdi+" "+"Malzeme Kodu:"+urunKodu, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // DataGridView'i güncelle
                    guncelleyici.UrunListesiniGuncelle(dataGridView1);
                    guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                    guncelleyici.stokGuncelle(dataGridView3);
                    guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                    guncelleyici.stokGuncelle(dataGridView5);
                    guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                    guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                    txtBoxUrunAdı.Text = "";
                    txtBoxUrunKod.Text = "";
                }
            }
        }
        private bool UrunVarMi(string urunAdi, string urunKodu)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Aynı ürün adına ve ürün koduna sahip ürünleri kontrol etmek için sorgu
                string kontrolQuery = "SELECT COUNT(*) FROM Urunler WHERE UrunAdi = @UrunAdi OR UrunKodu = @UrunKodu";
                // Kontrol komutu oluşturma
                SqlCommand kontrolCommand = new SqlCommand(kontrolQuery, connection);
                kontrolCommand.Parameters.AddWithValue("@UrunAdi", urunAdi);
                kontrolCommand.Parameters.AddWithValue("@UrunKodu", urunKodu);
                int urunSayisi = (int)kontrolCommand.ExecuteScalar();
                // Aynı ürün adına veya ürün koduna sahip ürün varsa true döndür
                return urunSayisi > 0;
            }
        }
        private void EkleUrun(string urunAdi, string urunKodu, string kategori)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Ürün ekleme sorgusu
                string urunEkleQuery = "INSERT INTO Urunler (UrunAdi, UrunKodu, Kategori) VALUES (@UrunAdi, @UrunKodu, @Kategori)";
                // Ürün ekleme komutu oluşturma
                SqlCommand urunEkleCommand = new SqlCommand(urunEkleQuery, connection);
                urunEkleCommand.Parameters.AddWithValue("@UrunAdi", urunAdi);
                urunEkleCommand.Parameters.AddWithValue("@UrunKodu", urunKodu);
                urunEkleCommand.Parameters.AddWithValue("@Kategori", kategori);
                // Ürünü veritabanına ekleme
                urunEkleCommand.ExecuteNonQuery();
            }
        }
        #endregion
        #region Ürünü stoğa sokalım
        private void btnUrunArtsın_Click(object sender, EventArgs e)
        { // Diğer giriş bilgilerini almak
            string kodu = txtUrunKoduGiris.Text;
            string miktarStr = txtMiktar.Text;
            string notlar = txtNotlar.Text;
            string lotNo = textBoxLotNumrası.Text;
            DateTime tarih = dateTime1.Value;
            // Seri numaralarını almak için TextBox'tan girişi al ve virgüllerle ayır
            string seriNumaraGirisi = textBoxSeriNumaralari.Text;
            string[] seriNumaralari = seriNumaraGirisi.Split(',');
            List<string> seriNumaralariListesi = new List<string>();
            //cccaltayccc
            foreach (var seriNo in seriNumaralari)
            {
                // Eğer seri numarası boş değilse ve sadece boşluklardan oluşmuyorsa seri numarası listesine ekle
                if (!string.IsNullOrWhiteSpace(seriNo))
                {
                    seriNumaralariListesi.Add(seriNo.Trim()); // Seri numarasının başındaki ve sonundaki boşlukları kaldır
                }
            }
            // Girişlerin doğrulanması
            if (string.IsNullOrWhiteSpace(kodu) || string.IsNullOrWhiteSpace(miktarStr) || string.IsNullOrWhiteSpace(lotNo) /*  || seriNumaralariListesi.Count == 0 */ )
            {
                MessageBox.Show("Lütfen tüm alanları doldurun! (Ürün kodu, Miktar, Lot numarası girilmelidir.)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!float.TryParse(miktarStr, out float miktar))
                {
                    MessageBox.Show("Miktar için geçerli bir sayı girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Hata durumunda işlemi sonlandır
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        // Ürün ID'sini almak için ÜrünKodu'na göre sorgulama yap
                        string urunIDQuery = "SELECT UrunID FROM Urunler WHERE UrunKodu = @UrunKodu";
                        SqlCommand urunIDCommand = new SqlCommand(urunIDQuery, connection);
                        urunIDCommand.Parameters.AddWithValue("@UrunKodu", kodu);
                        int urunID = Convert.ToInt32(urunIDCommand.ExecuteScalar());
                        if (urunID == 0)
                        {
                            MessageBox.Show("Girilen ürün veritabanında bulunmamaktadır! Lütfen önce ürün oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // Ürün girişi yap
                        string urunGirisQuery = @"
    INSERT INTO UrunGiris (UrunID, Miktar, Tarih, Notlar, LotNumarasi)
    OUTPUT INSERTED.GirisID
    VALUES (@UrunID, @Miktar, @Tarih, @Notlar, @LotNumarasi)
";
                        SqlCommand urunGirisCommand = new SqlCommand(urunGirisQuery, connection);
                        urunGirisCommand.Parameters.AddWithValue("@UrunID", urunID);
                        urunGirisCommand.Parameters.AddWithValue("@Miktar", miktar);
                        urunGirisCommand.Parameters.AddWithValue("@Tarih", tarih);
                        urunGirisCommand.Parameters.AddWithValue("@Notlar", notlar);
                        urunGirisCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                        // Insert komutunu çalıştırarak girisID değerini al
                        int girisID = (int)urunGirisCommand.ExecuteScalar();
                        // Eğer girisID 0 ise bir hata oluşmuş demektir
                        if (girisID == 0)
                        {
                            MessageBox.Show("Ürün girişi sırasında bir hata oluştu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                        foreach (string seriNo in seriNumaralariListesi)
                        {
                            string seriNoEkleQuery = "INSERT INTO SeriNumaralari (GirisID, UrunID, LotNumarasi, SeriNo) VALUES (@GirisID, @UrunID, @LotNumarasi, @SeriNo)";
                            SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection);
                            seriNoEkleCommand.Parameters.AddWithValue("@GirisID", girisID);
                            seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                            seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                            seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNo);
                            seriNoEkleCommand.ExecuteNonQuery();
                        }
                        MessageBox.Show("Ürün girişi yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        guncelleyici.UrunListesiniGuncelle(dataGridView1);
                        guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                        guncelleyici.stokGuncelle(dataGridView3);
                        guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                        guncelleyici.stokGuncelle(dataGridView5);
                        guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                        guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                        guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                        guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                        guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                        guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion
        #region ARAMA datagrid de arama işlemlerini burada çağırdım.
        //guıde arama kısmını yaptık.
        private void btnCikanAraLot_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGridView7, txtLotSeriAra.Text);
        }
        private void btnAr_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara3 = new DataGridViewSearcher();
            ara3.Search(dataGridView3, txtUrunAdiAra.Text);
        }
        private void btnUrunAra2_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGridView5, txtUrunAra2.Text);
        }
        private void btnUrunOlAra_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGridView1, textBoxUrunOlAra.Text);
        }
        private void UrunGirisUrunAra_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara2 = new DataGridViewSearcher();
            ara2.Search(dataGridView2, textUrunGirisUrunAra.Text);
        }
        private void btnCikisAra_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGridView4, textbtnCikisAra.Text);
        }
        private void btnSeriAra_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGridViewSeriNo, textBox1.Text);
        }

        private void buttonYariAra_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara =new DataGridViewSearcher();
            ara.Search(dataGridView10, textBoxYariMamulOl.Text);
        }
        private void buttonAraTamUrun_Click(object sender, EventArgs e)
        {
            DataGridViewSearcher ara = new DataGridViewSearcher();
            ara.Search(dataGridView12, textBoxAraTamUrun.Text);
        }
        #endregion
        private void btnUrunCikisYap_Click(object sender, EventArgs e)
        {

            string kodu = txtUrunKoduCikis.Text;
            string miktarStr = txtMiktarCikis.Text;
            string notlar = txtNotlarCikis.Text;
            string lotNo = textBoxCikisLot.Text;
            DateTime cikisTarihi = dateTimePickerCikis.Value;
            string cikisNedeni = cmbCikisNedeni.Text;
            string cikisKategori = cmbCikisKategori.Text;
            // Girişlerin doğrulanması
            if (string.IsNullOrWhiteSpace(kodu) || string.IsNullOrWhiteSpace(miktarStr) || string.IsNullOrWhiteSpace(lotNo))
            {
                MessageBox.Show("Lütfen ürün kodu, miktar ve lot numarası alanlarını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Miktarın doğrulanması
            if (!float.TryParse(miktarStr, out float miktar) || miktar <= 0)
            {
                MessageBox.Show("Geçerli bir miktar girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Veritabanı bağlantısı oluştur
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Ürün ID'sini almak için ÜrünKodu'na göre sorgulama yap
                    string urunIDQuery = "SELECT UrunID FROM Urunler WHERE UrunKodu = @UrunKodu";
                    SqlCommand urunIDCommand = new SqlCommand(urunIDQuery, connection);
                    urunIDCommand.Parameters.AddWithValue("@UrunKodu", kodu);
                    int urunID = Convert.ToInt32(urunIDCommand.ExecuteScalar());
                    if (urunID == 0)
                    {
                        MessageBox.Show("Girilen ürün veritabanında bulunmamaktadır! Lütfen önce ürün oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Ürün çıkışı yap
                    string urunCikisQuery = @"
                INSERT INTO UrunCikis (UrunID, Miktar, CikisTarihi, Notlar, Kategori, CikisNedeni, LotNumarasi)
                OUTPUT INSERTED.CikisID
                VALUES (@UrunID, @Miktar, @CikisTarihi, @Notlar, @Kategori, @CikisNedeni, @LotNumarasi)";
                    SqlCommand urunCikisCommand = new SqlCommand(urunCikisQuery, connection);
                    urunCikisCommand.Parameters.AddWithValue("@UrunID", urunID);
                    urunCikisCommand.Parameters.AddWithValue("@Miktar", miktar);
                    urunCikisCommand.Parameters.AddWithValue("@CikisTarihi", cikisTarihi);
                    urunCikisCommand.Parameters.AddWithValue("@Notlar", notlar);
                    urunCikisCommand.Parameters.AddWithValue("@Kategori", cikisKategori);
                    urunCikisCommand.Parameters.AddWithValue("@CikisNedeni", cikisNedeni);
                    urunCikisCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                    // Insert komutunu çalıştırarak cikisID değerini al
                    int cikisID = (int)urunCikisCommand.ExecuteScalar();
                    // Eğer cikisID 0 ise bir hata oluşmuş demektir
                    if (cikisID == 0)
                    {
                        MessageBox.Show("Ürün çıkışı sırasında bir hata oluştu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                    if (!string.IsNullOrWhiteSpace(textBoxSeriNumara.Text))
                    {
                        string seriNumaraCikisi = textBoxSeriNumara.Text;
                        string[] seriNumaralariCikis = seriNumaraCikisi.Split(',');
                        foreach (string seriNo in seriNumaralariCikis)
                        {
                            string seriNoEkleQuery = "INSERT INTO SeriNumaralari (CikisID, UrunID, LotNumarasi, SeriNo) VALUES (@CikisID, @UrunID, @LotNumarasi, @SeriNo)";
                            SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection);
                            seriNoEkleCommand.Parameters.AddWithValue("@CikisID", cikisID);
                            seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                            seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                            seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNo.Trim());
                            seriNoEkleCommand.ExecuteNonQuery();
                        }
                    }
                    // DataGridView'i güncelle
                    guncelleyici.UrunListesiniGuncelle(dataGridView1);
                    guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                    guncelleyici.stokGuncelle(dataGridView3);
                    guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                    guncelleyici.stokGuncelle(dataGridView5);
                    guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                    guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                    MessageBox.Show("Ürün çıkışı yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #region yazdır ve excell taşı

        private void btnYazdir1_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridView1); //Ürünler yazdırır
        }

        private void btnYazdir2_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridView2); // Ürün girişini yazdırır
        }
        private void btnYazdir3_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridView3); // anlık stoğu yazdırır
        }

        private void btnYazdir4_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridView4); // ürün çıkışın yazdırır
        }
        private void buttnYazdir_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridView6);
        }
        private void buttnYazdirKac_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridView9);
        }
        private void btnYazdir_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridViewSeriNo);

        }
        private void buttonYazdirSeriLot_Click(object sender, EventArgs e)
        {
            CiktiAlmaSinifi ciktiAlmaSinifi = new CiktiAlmaSinifi();
            ciktiAlmaSinifi.Yazdir(dataGridView7);
        }
        private void buttonTasiExcell_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridView7);
        }
        private void btnTasi_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridViewSeriNo);
        }
        private void btnTasiBomİc_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridView9);
        }
        private void butTasi_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridView6);
        }

        private void btnExcelTasi_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridView1);//Ürün oluşturur excell taşır.
        }
        private void btnExcelTasi2_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridView2);//Ürün girişini excel taşır.
        }
        private void btnExcelTasi3_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridView3);//anlık stoğu excel taşır
        }
        private void btnExcelTasi4_Click(object sender, EventArgs e)
        {
            DatagriwToExcel.Export(dataGridView4);//ürün çıkışını excell taşır.
        }
        #endregion;
        #region hadi Poşeti dolduralım
        private List<string> icerikListesi = new List<string>();
        private void btnIcerikEkle_Click(object sender, EventArgs e)
        {
            string urunKodu = txtUrunKoduEkle.Text;
            string miktarText = txtUrunMiktarEkle.Text;
            if (string.IsNullOrWhiteSpace(urunKodu) || string.IsNullOrWhiteSpace(miktarText))
            {
                MessageBox.Show("Ürün kodu veya miktar boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int miktar;
            if (!int.TryParse(miktarText, out miktar))
            {
                MessageBox.Show("Miktar geçerli bir tamsayı olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string icerik = $"{urunKodu}: {miktar} adet";
            icerikListesi.Add(icerik);
            PosetList.Items.Add(icerik);
            txtUrunKoduEkle.Clear();
            txtUrunMiktarEkle.Clear();
        }
        private void buttonPosetSil_Click(object sender, EventArgs e)
        {
            // ListBox'ta seçili bir öğe var mı kontrol edin
            if (PosetList.SelectedItem != null)
            {
                // Seçili öğeyi sil
                PosetList.Items.Remove(PosetList.SelectedItem);
            }
            else
            {
                // Seçili öğe yoksa kullanıcıya bilgi verin
                MessageBox.Show("Lütfen silmek için bir öğe seçin.");
            }
        }
        #endregion
        #region bom
        private void btnBomOlusutur_Click(object sender, EventArgs e)
        {
            string posetAdi = txtBomAdi2.Text;
            string posetKodu = textBoxBomKodu.Text; // Yeni eklenen satır
            string malzeme;
            if (radioButton1Yari.Checked)
            {
                malzeme = "Yarı Mamül";
            }
            else if (radioButton2Tam.Checked)
            {
                malzeme = "Mamül";
            }
            else
            {
                malzeme = "Yarı Mamül";
            }

            if (string.IsNullOrWhiteSpace(posetAdi) || string.IsNullOrWhiteSpace(posetKodu) || string.IsNullOrEmpty(malzeme)) // Değiştirildi
            {
                MessageBox.Show("Bom adı ve Bom kodu boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(posetAdi) || string.IsNullOrWhiteSpace(posetKodu))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                // Ürünü veritabanına ekle
                if (UrunVarMi(posetAdi, posetKodu))
                {
                    MessageBox.Show("Aynı ürün adı veya ürün kodu zaten var! Ürün Oluşturmadan devam ediliyor", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EkleUrun(posetAdi, posetKodu, malzeme);
                    guncelleyici.UrunListesiniGuncelle(dataGridView1);
                    guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                    guncelleyici.stokGuncelle(dataGridView3);
                    guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                    guncelleyici.stokGuncelle(dataGridView5);
                    guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                    guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Poset adını ve kodunu kaydet (INSERT INTO Posetler sorgusuna posetKodu eklendi)
                string posetKaydetQuery = "INSERT INTO Posetler (PosetAdi, PosetKodu) VALUES (@PosetAdi, @PosetKodu); SELECT SCOPE_IDENTITY();";
                SqlCommand posetKaydetCommand = new SqlCommand(posetKaydetQuery, connection);
                posetKaydetCommand.Parameters.AddWithValue("@PosetAdi", posetAdi);
                posetKaydetCommand.Parameters.AddWithValue("@PosetKodu", posetKodu); // Yeni eklenen satır
                int posetID = Convert.ToInt32(posetKaydetCommand.ExecuteScalar());
                // Poset içeriğini kaydet
                foreach (string icerik in icerikListesi)
                {
                    string[] parts = icerik.Split(':');
                    string urunKodu = parts[0].Trim();
                    int miktar = Convert.ToInt32(parts[1].Trim().Split()[0]);
                    // Ürünün veritabanında olup olmadığını kontrol et
                    string urunKontrolQuery = "SELECT COUNT(*) FROM Urunler WHERE UrunKodu = @UrunKodu";
                    SqlCommand urunKontrolCommand = new SqlCommand(urunKontrolQuery, connection);
                    urunKontrolCommand.Parameters.AddWithValue("@UrunKodu", urunKodu);
                    int urunSayisi = Convert.ToInt32(urunKontrolCommand.ExecuteScalar());
                    if (urunSayisi == 0)
                    {
                        // Ürün tablosunda bulunmayan ürün, uyarı ver ve işlemi atla
                        MessageBox.Show($"Ürün tablosunda bulunmayan bir ürün: {urunKodu}. Bu ürün eklenmeyecek.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue; // Ürün eklenmeyecek, bir sonraki ürünü işle
                    }
                    // Ürün tablosunda bulunan bir ürün ise, işlemi devam ettir
                    // Ürün ID'sini al
                    string urunIDQuery = "SELECT UrunID FROM Urunler WHERE UrunKodu = @UrunKodu";
                    SqlCommand urunIDCommand = new SqlCommand(urunIDQuery, connection);
                    urunIDCommand.Parameters.AddWithValue("@UrunKodu", urunKodu);
                    int urunID = Convert.ToInt32(urunIDCommand.ExecuteScalar());
                    // Poset içeriğini PosetIcerikleri tablosuna ekle
                    string posetIcerikKaydetQuery = "INSERT INTO PosetIcerikleri (PosetID, UrunID, Miktar) VALUES (@PosetID, @UrunID, @Miktar)";
                    SqlCommand posetIcerikKaydetCommand = new SqlCommand(posetIcerikKaydetQuery, connection);
                    posetIcerikKaydetCommand.Parameters.AddWithValue("@PosetID", posetID);
                    posetIcerikKaydetCommand.Parameters.AddWithValue("@UrunID", urunID);
                    posetIcerikKaydetCommand.Parameters.AddWithValue("@Miktar", miktar);
                    posetIcerikKaydetCommand.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Bom oluştu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Temizle
            icerikListesi.Clear();
            PosetList.Items.Clear();
            txtBomAdi2.Clear();
            textBoxBomKodu.Clear(); // Yeni eklenen satır
            RefreshComboBox();
            RefreshComboBox2();
            comboBoxIcerigeBak1();
            RefreshComboBox3();
            RefreshComboBox4();
            RefreshComboBox5();
            RefreshComboBox6();
            RefreshComboBox7();
        }
        // ComboBox'ı güncelleyen metod
        private void RefreshComboBox()
        {
            // ComboBox'ı temizle
            cmbBoxBom.Items.Clear();
            // Veritabanından poset başlıklarını çek
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    cmbBoxBom.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }
        private void RefreshComboBox2()
        {
            // ComboBox'ı temizle
            comboBox2.Items.Clear();
            // Veritabanından poset başlıklarını çek
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    comboBox2.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }
        private void RefreshComboBox3()
        {
            comboBox3.Items.Clear();////
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    comboBox3.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }
        private void RefreshComboBox4()
        {
            comboBoxBomYarisEmri.Items.Clear();////
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    comboBoxBomYarisEmri.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }
        private void RefreshComboBox5()
        {
            comboBoxMamul.Items.Clear();////
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    comboBoxMamul.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }
        private void RefreshComboBox6()
        {
            comboBox1.Items.Clear();////
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    comboBox1.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }
        private void RefreshComboBox7()
        {
            comboBoxIsEmri.Items.Clear();////
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    comboBoxIsEmri.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }

        private void comboBoxIcerigeBak1()
        {
            comboBoxIcerigeBak.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetAdi FROM Posetler";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                // ComboBox'a başlıkları ekle
                while (reader.Read())
                {
                    string posetAdi = reader["PosetAdi"].ToString();
                    comboBoxIcerigeBak.Items.Add(posetAdi);
                }
                // Veritabanı bağlantısını kapat
                reader.Close();
            }
        }

        private void btnPosetOlustur_Click_1(object sender, EventArgs e)
        {
            // ComboBox'tan seçilen poset adını al
            string selectedPosetAdi = cmbBoxBom.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedPosetAdi))
            {
                MessageBox.Show("Lütfen bir poset seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Seçilen poset adına göre SQL sorgusunu çalıştırıp sonuçları yükle
            LoadPosetData(selectedPosetAdi);

        }
        private void LoadPosetData(string posetAdi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Poset adına göre SQL sorgusu
                string query = @"
            SELECT 
                Posetler.PosetKodu AS 'Poset Kodu',
                Posetler.PosetAdi AS 'Poset Adı',
                Urunler.UrunAdi AS 'Ürün Adı',
                PosetIcerikleri.Miktar AS 'Miktar'
            FROM 
                PosetIcerikleri
            INNER JOIN 
                Urunler ON PosetIcerikleri.UrunID = Urunler.UrunID
            INNER JOIN 
                Posetler ON PosetIcerikleri.PosetID = Posetler.PosetID
            WHERE 
                Posetler.PosetAdi = @PosetAdi"; // Değiştirildi
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PosetAdi", posetAdi); // Değiştirildi
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                // DataGridView'e verileri yükle
                dataGridView6.DataSource = dataTable;
                // Sütun başlıklarını değiştir
                dataGridView6.Columns["Poset Kodu"].HeaderText = "Bom Kodu";
                dataGridView6.Columns["Poset Adı"].HeaderText = "Bom Adı";
                dataGridView6.Columns["Ürün Adı"].HeaderText = "Ürün Adı";
                dataGridView6.Columns["Miktar"].HeaderText = "Miktar";
            }
        }
        private void LoadPosetData2(string posetAdi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Poset adına göre SQL sorgusu
                string query = @"
            SELECT 
                Posetler.PosetKodu AS 'Poset Kodu',
                Posetler.PosetAdi AS 'Poset Adı',
                Urunler.UrunAdi AS 'Ürün Adı',
                PosetIcerikleri.Miktar AS 'Miktar'
            FROM 
                PosetIcerikleri
            INNER JOIN 
                Urunler ON PosetIcerikleri.UrunID = Urunler.UrunID
            INNER JOIN 
                Posetler ON PosetIcerikleri.PosetID = Posetler.PosetID
            WHERE 
                Posetler.PosetAdi = @PosetAdi"; // Değiştirildi
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PosetAdi", posetAdi); // Değiştirildi
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                // DataGridView'e verileri yükle
                dataGridView10.DataSource = dataTable;
            }

        }
        private void btnPosetKacTane_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir poset seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string posetAdi = comboBox2.SelectedItem.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                WITH GirisMiktarlari AS (
                    SELECT UrunID, SUM(Miktar) AS ToplamGirisMiktari
                    FROM UrunGiris
                    GROUP BY UrunID
                ),
                CikisMiktarlari AS (
                    SELECT UrunID, SUM(Miktar) AS ToplamCikisMiktari
                    FROM UrunCikis
                    GROUP BY UrunID
                ),
                StokDurumu AS (
                    SELECT 
                        GirisMiktarlari.UrunID,
                        ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) AS ToplamGirisMiktari,
                        ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0) AS ToplamCikisMiktari,
                        (ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) - ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0)) AS StokMiktari
                    FROM 
                        GirisMiktarlari
                    FULL OUTER JOIN 
                        CikisMiktarlari ON GirisMiktarlari.UrunID = CikisMiktarlari.UrunID
                )
                SELECT 
                    Posetler.PosetID,
                    Posetler.PosetAdi,
                    COUNT(PosetIcerikleri.IcerikID) AS PosetIcerikSayisi,
                    MIN(StokDurumu.StokMiktari / PosetIcerikleri.Miktar) AS OlusturulabilecekAdet
                FROM 
                    Posetler
                JOIN 
                    PosetIcerikleri ON Posetler.PosetID = PosetIcerikleri.PosetID
                JOIN 
                    StokDurumu ON PosetIcerikleri.UrunID = StokDurumu.UrunID
                WHERE
                    Posetler.PosetAdi = @PosetAdi
                GROUP BY 
                    Posetler.PosetID, Posetler.PosetAdi;
            ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PosetAdi", posetAdi);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView9.DataSource = dataTable;
                }
            }
        }
        #endregion
        #region Ürünü Bozuk Ürünü Bulalım.
        private void buttonBozukUrunAra_Click(object sender, EventArgs e)
        {
            if (radioButtonSeri.Checked)
            {
                string arananSeriNo = textBoxUrunSeriAra.Text;
                if (string.IsNullOrEmpty(arananSeriNo))
                {
                    MessageBox.Show("Lütfen bir seri numarası girin.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string lotQuery = "SELECT DISTINCT LotNumarasi FROM SeriNumaralari WHERE SeriNo = @SeriNumarasi";
                    SqlCommand lotCommand = new SqlCommand(lotQuery, connection);
                    lotCommand.Parameters.AddWithValue("@SeriNumarasi", arananSeriNo);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter lotDataAdapter = new SqlDataAdapter(lotCommand))
                    {
                        DataTable lotTable = new DataTable();
                        lotDataAdapter.Fill(lotTable);
                        foreach (DataRow lotRow in lotTable.Rows)
                        {
                            string lotNumarasi = lotRow["LotNumarasi"].ToString();
                            string query = @"
                        SELECT 
                            Urunler.UrunAdi, 
                            Urunler.UrunKodu, 
                            UrunGiris.Tarih, 
                            SeriNumaralari.SeriNo,
                            SeriNumaralari.LotNumarasi
                        FROM 
                            Urunler
                        INNER JOIN 
                            UrunGiris ON Urunler.UrunID = UrunGiris.UrunID
                        LEFT JOIN 
                            SeriNumaralari ON UrunGiris.GirisID = SeriNumaralari.GirisID
                        WHERE 
                            UrunGiris.LotNumarasi = @LotNumarasi";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@LotNumarasi", lotNumarasi);
                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable tempTable = new DataTable();
                            adapter.Fill(tempTable);
                            dataTable.Merge(tempTable);
     
                        }
                    }
                    dataGridView8.DataSource = dataTable;
                    textBoxUrunSeriAra.Text = "";
                }
            }
            else if (radioButtonLot.Checked)
            {
                string arananLotNumarasi = textBoxUrunSeriAra.Text;
                if (string.IsNullOrEmpty(arananLotNumarasi))
                {
                    MessageBox.Show("Lütfen bir Lot numarası girin.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    Urunler.UrunAdi, 
                    Urunler.UrunKodu, 
                    UrunGiris.Tarih, 
                    UrunGiris.LotNumarasi,
                    UrunGiris.Miktar
                FROM 
                    Urunler
                INNER JOIN 
                    UrunGiris ON Urunler.UrunID = UrunGiris.UrunID
                WHERE 
                    UrunGiris.LotNumarasi = @LotNumarasi";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@LotNumarasi", arananLotNumarasi);
                    DataTable dataTable = new DataTable();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    dataGridView8.DataSource = dataTable;
                    textBoxUrunSeriAra.Text = "";
                    
                }
            }
            else
            {
                MessageBox.Show("Lütfen arama yapacağınız butonu seçiniz.(Seri veya Lot numarasını lütfen seçiniz.)", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxUrunSeriAra.Text = "";
            }
        }
        #endregion
        #region datagriw güncelle sadece ürüneleri
        //ürün olustur güncellemesi
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
  // Eğer kullanıcı başlık satırına tıklarsa, işlem yapma
            if (e.RowIndex == -1)
                return;
            // Seçilen satırı al
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            // Seçilen satırdaki verileri al
            string urunAdi = row.Cells["UrunAdi"].Value.ToString();
            string urunKodu = row.Cells["UrunKodu"].Value.ToString();
            string kategori = row.Cells["Kategori"].Value.ToString();
            // Güncelleme formunu aç ve verileri aktar
            Guncelleyici guncelleFormu = new Guncelleyici(urunAdi, urunKodu, kategori);
            guncelleFormu.GuncellemeTamamlandi += GuncellemeTamamlandiHandler;
            guncelleFormu.ShowDialog();  
        }
        private void GuncellemeTamamlandiHandler(object sender, EventArgs e)
        {
            guncelleyici.UrunListesiniGuncelle(dataGridView1);
        }
        //ürün girişin güncellemesi
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           /* //// Eğer kullanıcı başlık satırına tıklarsa, işlem yapma
            if (e.RowIndex == -1)
                return;
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            string urunAdi = row.Cells["UrunAdi"].Value.ToString();
            string urunKodu = row.Cells["UrunKodu"].Value.ToString();
            string miktar = row.Cells["Miktar"].Value.ToString();
            string notlar = row.Cells["Notlar"].Value.ToString();
            string tarih = row.Cells["Tarih"].Value.ToString();
            string lotNumarasi = row.Cells["LotNumarasi"].Value.ToString();
            string seriNumarasi = row.Cells["SeriNo"].Value.ToString();
            int girisID = GetGirisID(urunAdi);
            Guncelleyici2 guncelleFormu2 = new Guncelleyici2(urunAdi, urunKodu, miktar, notlar, tarih, lotNumarasi, seriNumarasi, girisID,urunID);
            guncelleFormu2.GuncellemeTamamlandi2 += GuncellemeTamamlandiHandler2;
            guncelleFormu2.ShowDialog(); */
        }

        private int GetGirisID(string urunAdi)
        {
            int girisID = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT UrunGiris.GirisID FROM UrunGiris INNER JOIN Urunler ON UrunGiris.UrunID = Urunler.UrunID WHERE Urunler.UrunAdi = @UrunAdi";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UrunAdi", urunAdi);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    girisID = Convert.ToInt32(reader["GirisID"]);
                }
                reader.Close();
            }
            return girisID;
        }
        private void GuncellemeTamamlandiHandler2(object sender, EventArgs e)
        {
            UrunGirisListesiniGuncelle();
        }
        private void UrunGirisListesiniGuncelle()
        {
            guncelleyici.UrunListesiniGuncelle(dataGridView1);
            guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
            guncelleyici.stokGuncelle(dataGridView3);
            guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
            guncelleyici.stokGuncelle(dataGridView5);
            guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
            guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
            guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
            guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
            guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
            guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
        }
        #endregion
        //Yarimamül kısmı
        private void btnİcerigeBak_Click(object sender, EventArgs e)
        {
            // ComboBox'tan seçilen poset adını al
            string selectedPosetAdi = comboBoxIcerigeBak.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedPosetAdi))
            {
                MessageBox.Show("Lütfen bir poset seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetKodu FROM Posetler WHERE PosetAdi = @PosetAdi";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@PosetAdi", selectedPosetAdi);
                string posetKodu = cmd.ExecuteScalar()?.ToString(); // ExecuteScalar kullanarak tek bir değer döndürüyoruz
                textBoxBurunAd.Text = selectedPosetAdi;
                textBoxBurunKod.Text = posetKodu;
                // Posetin içeriğini ListBox'ta göster
                query = @"
        SELECT Urunler.UrunAdi, Urunler.UrunKodu, PosetIcerikleri.Miktar 
        FROM PosetIcerikleri 
        INNER JOIN Urunler ON PosetIcerikleri.UrunID = Urunler.UrunID 
        INNER JOIN Posetler ON PosetIcerikleri.PosetID = Posetler.PosetID 
        WHERE Posetler.PosetAdi = @PosetAdi";
                cmd.CommandText = query;
                SqlDataReader reader = cmd.ExecuteReader();
                listBox1.Items.Clear();
                while (reader.Read())
                {
                    string urunAdi = reader["UrunAdi"].ToString();
                    string urunKodu = reader["UrunKodu"].ToString();
                    int miktar = Convert.ToInt32(reader["Miktar"]);
                    // Miktar 1'den fazlaysa her bir ürünü miktar kadar listeleyelim
                    for (int i = 0; i < miktar; i++)
                    {
                        listBox1.Items.Add($"Ürün Adı: {urunAdi}, Ürün Kodu: {urunKodu}, Miktar: 1");
                    }
                }
                reader.Close();
            }
        }
        #region  yari mamülü olusuturuduk ve stoğa soktuk
        private void buttonEkle_Click(object sender, EventArgs e)
        {
            /* if (!string.IsNullOrWhiteSpace(txtLotNumarasi.Text) && !string.IsNullOrWhiteSpace(txtSeriNo.Text) && !string.IsNullOrEmpty(seciliUrun))
             {
                 // TextBox'lardan lot ve seri numaralarını al
                 string lotNumarasi = txtLotNumarasi.Text;
                 string seriNo = txtSeriNo.Text;
                 // Seçili öğenin sonuna lot ve seri numaralarını ekleyerek ListBox'ta güncelle
                 listBox1.Items[listBox1.SelectedIndex] = $"{seciliUrun} , Lot: {lotNumarasi}, Seri No: {seriNo}";
             }
             else
             {
                 MessageBox.Show("Lütfen tüm alanları doldurun ve bir öğe seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
             }*/


            if (!string.IsNullOrWhiteSpace(txtLotNumarasi.Text) && !string.IsNullOrWhiteSpace(txtSeriNo.Text) && !string.IsNullOrEmpty(seciliUrun))
            {
                // TextBox'lardan lot ve seri numaralarını al
                string lotNumarasi = txtLotNumarasi.Text;
                string seriNo = txtSeriNo.Text;
                // Seçili öğenin sonuna lot ve seri numaralarını ekleyerek ListBox'ta güncelle
                listBox1.Items[listBox1.SelectedIndex] = $"{seciliUrun} , Lot: {lotNumarasi}, Seri No: {seriNo}";
            }
            else
            {
                // Herhangi bir textbox boşsa, "0" yazdır
                string lotNumarasi = string.IsNullOrWhiteSpace(txtLotNumarasi.Text) ? "0" : txtLotNumarasi.Text;
                string seriNo = string.IsNullOrWhiteSpace(txtSeriNo.Text) ? "0" : txtSeriNo.Text;

                // ListBox'ta güncelleme
                if (listBox1.SelectedIndex != -1)
                {
                    listBox1.Items[listBox1.SelectedIndex] = $"{seciliUrun} , Lot: {lotNumarasi}, Seri No: {seriNo}";
                }
                else
                {
                    // ListBox'ta seçili bir öğe yoksa, yeni bir öğe olarak ekle
                    listBox1.Items.Add($"{seciliUrun} , Lot: {lotNumarasi}, Seri No: {seriNo}");
                }

            }
        }
        //listboxdan seç yapıştır daha sonra..
        private string seciliUrun;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                // ListBox'ta seçili olan öğenin bilgisini al ve sakla
                seciliUrun = listBox1.SelectedItem.ToString();
            }
        }
        private void buttonSil_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                // Seçili öğenin içeriğini al
                string seciliUrun = listBox1.SelectedItem.ToString();
                // Öğenin sonunda çizgi (-) işareti varsa ve içinde "Lot:" ve "Seri No:" ifadeleri varsa
                int index = seciliUrun.LastIndexOf("-");
                if (index != -1 && seciliUrun.Contains("Lot:") && seciliUrun.Contains("Seri No:"))
                {
                    // Öğenin sonundaki çizgi ve lot/seri numaralarını içeren kısmı sil
                    seciliUrun = seciliUrun.Substring(0, index).Trim();
                    listBox1.Items[listBox1.SelectedIndex] = seciliUrun;
                }
                else
                {
                    // Sadece seçili öğeyi sil
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir öğe seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void buttonOlustur_Click(object sender, EventArgs e)
        {
            string urunAdi2 = textBoxBurunAd.Text;
            string urunKodu2 = textBoxBurunKod.Text;
            string kategori2 = comboBoxBomOlustur.Text;
            string ad = textBoxSeriNoVer.Text;
            // Girişlerin boş olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(urunAdi2) || string.IsNullOrWhiteSpace(urunKodu2) || string.IsNullOrWhiteSpace(kategori2)||string.IsNullOrEmpty(ad))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                // Ürünü veritabanına ekle
                if (UrunVarMi(urunAdi2, urunKodu2))
                {
                    MessageBox.Show("Aynı ürün adı veya ürün kodu zaten var! Ürün Oluşturmadan devam ediliyor", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EkleUrun(urunAdi2, urunKodu2, kategori2);
                    guncelleyici.UrunListesiniGuncelle(dataGridView1);
                    guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                    guncelleyici.stokGuncelle(dataGridView3);
                    guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                    guncelleyici.stokGuncelle(dataGridView5);
                    guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                    guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                }
            }
            
            // Veritabanına bağlandık using methodu ile ve toplu çıkış yaptık..
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (var item in listBox1.Items)
                    {
                        string urunAdi = "";
                        string urunKodu = "";
                        int miktar = 0;
                        string lotNo = "";
                        List<string> seriNumaralari = new List<string>();
                        string[] bilgiler = item.ToString().Split(',');
                        // Her satırdaki bilgileri işle
                        foreach (var bilgi in bilgiler)
                        {
                            if (bilgi.Contains("Ürün Adı"))
                            {
                                urunAdi = bilgi.Split(':')[1].Trim();
                            }
                            else if (bilgi.Contains("Ürün Kodu"))
                            {
                                urunKodu = bilgi.Split(':')[1].Trim();
                            }
                            else if (bilgi.Contains("Miktar"))
                            {
                                int.TryParse(bilgi.Split(':')[1].Trim(), out miktar);
                            }
                            else if (bilgi.Contains("Lot"))
                            {
                                lotNo = bilgi.Split(':')[1].Trim();
                            }
                            else if (bilgi.Contains("Seri No"))
                            {
                                string seriBilgisi = bilgi.Split(':')[1].Trim();
                                string[] seriNumaraDizisi = seriBilgisi.Split(',');
                                // Her bir seri numarasını ana seri numara listesine ekle
                                seriNumaralari.AddRange(seriNumaraDizisi.Select(s => s.Trim()));
                            }
                        }
                        // Ürün ID'sini almak için ÜrünKodu'na göre sorgulama yap
                        string urunIDQuery = "SELECT UrunID FROM Urunler WHERE UrunKodu = @UrunKodu";
                        SqlCommand urunIDCommand = new SqlCommand(urunIDQuery, connection);
                        urunIDCommand.Parameters.AddWithValue("@UrunKodu", urunKodu);
                        int urunID = Convert.ToInt32(urunIDCommand.ExecuteScalar());
                        if (urunID == 0)
                        {
                            MessageBox.Show("Girilen ürün veritabanında bulunmamaktadır! Lütfen önce ürün oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        string kategori = comboBoxBomOlustur.Text;
                        // Ürün çıkışı yap
                        string urunCikisQuery = @"
INSERT INTO UrunCikis (UrunID, Miktar, CikisTarihi, Notlar, Kategori, CikisNedeni, LotNumarasi)
OUTPUT INSERTED.CikisID
VALUES (@UrunID, @Miktar, @CikisTarihi, @Notlar, @Kategori, @CikisNedeni, @LotNumarasi)";
                        SqlCommand urunCikisCommand = new SqlCommand(urunCikisQuery, connection);
                        urunCikisCommand.Parameters.AddWithValue("@UrunID", urunID);
                        urunCikisCommand.Parameters.AddWithValue("@Miktar", miktar);
                        urunCikisCommand.Parameters.AddWithValue("@CikisTarihi", DateTime.Now); // Şu anki tarihi kullanabilirsiniz
                        urunCikisCommand.Parameters.AddWithValue("@Notlar", "Yarı Mamülde Kullanıldı."); // Notlar boş olacaksa
                        urunCikisCommand.Parameters.AddWithValue("@Kategori", kategori); // Kategori boş olacaksa
                        urunCikisCommand.Parameters.AddWithValue("@CikisNedeni", "Üretimde Kullanıldı."); // Çıkış nedeni boş olacaksa
                        urunCikisCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                        // Insert komutunu çalıştırarak cikisID değerini al
                        int cikisID = (int)urunCikisCommand.ExecuteScalar();
                        // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                        foreach (string seriNo in seriNumaralari)
                        {
                            string seriNoEkleQuery = "INSERT INTO SeriNumaralari (CikisID, UrunID, LotNumarasi, SeriNo) VALUES (@CikisID, @UrunID, @LotNumarasi, @SeriNo)";
                            SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection);
                            seriNoEkleCommand.Parameters.AddWithValue("@CikisID", cikisID);
                            seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                            seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                            seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNo.Trim());
                            seriNoEkleCommand.ExecuteNonQuery();
                        }
                        // Birleşik ürün adını ve kodunu al
                        string birlesikUrunAdi = textBoxBurunAd.Text;
                        string birlesikUrunKodu = textBoxSeriNoVer.Text;
                        // Birleşik ürünü veritabanına ekle
                        string birlesikUrunEkleQuery = @"
INSERT INTO BirlesikUrunler (BirlesikUrunAdi, BirlesikUrunKodu)
VALUES (@BirlesikUrunAdi, @BirlesikUrunKodu);
";
                        SqlCommand birlesikUrunEkleCommand = new SqlCommand(birlesikUrunEkleQuery, connection);
                        birlesikUrunEkleCommand.Parameters.AddWithValue("@BirlesikUrunAdi", birlesikUrunAdi);
                        birlesikUrunEkleCommand.Parameters.AddWithValue("@BirlesikUrunKodu", birlesikUrunKodu);
                        birlesikUrunEkleCommand.ExecuteNonQuery();
                        birlesikUrunEkleCommand.ExecuteNonQuery();
                        // Eklenen birleşik ürünün ID'sini al
                        string birlesikUrunIDQuery = "SELECT TOP 1 BirlesikUrunID FROM BirlesikUrunler ORDER BY BirlesikUrunID DESC";
                        SqlCommand birlesikUrunIDCommand = new SqlCommand(birlesikUrunIDQuery, connection);
                        int birlesikUrunID = Convert.ToInt32(birlesikUrunIDCommand.ExecuteScalar());
                        // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                        foreach (string seriNo in seriNumaralari)
                        {
                            string seriNoEkleQuery = "INSERT INTO BirlesikUrunSeriNumaralari (BirlesikUrunID, UrunID, LotNumarasi, SeriNo) VALUES (@BirlesikUrunID, @UrunID, @LotNumarasi, @SeriNo)";
                            SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection);
                            seriNoEkleCommand.Parameters.AddWithValue("@BirlesikUrunID", birlesikUrunID);
                            seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                            seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                            seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNo.Trim());
                            seriNoEkleCommand.ExecuteNonQuery();
                        }

                        //MessageBox.Show("Toplu ürün girişi başarıyla yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //MessageBox.Show("Ürünler düzgün bir Şekilde çıkartıldı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Ürün girişi yap
                    // Diğer giriş bilgilerini almak
                    string kodu = textBoxBurunKod.Text;
                    int miktar2 = 1; // Toplu ürün çıkışı yapıyoruz ama o ürünler birleşiyor bir ürün oluşuyor.
                    string lotGiris = textBoxSeriNoVer.Text;
                    DateTime tarihGiris = DateTime.Today;
                    string seriNumaralariGiris = textBoxSeriNoVer.Text;

                    using (SqlConnection connection2 = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection2.Open();
                            // Ürün ID'sini almak için ÜrünKodu'na göre sorgulama yap
                            string urunIDQuery = "SELECT UrunID FROM Urunler WHERE UrunKodu = @UrunKodu";
                            SqlCommand urunIDCommand = new SqlCommand(urunIDQuery, connection2);
                            urunIDCommand.Parameters.AddWithValue("@UrunKodu", kodu);
                            int urunID = Convert.ToInt32(urunIDCommand.ExecuteScalar());

                            // Ürün girişi yap
                            string urunGirisQuery = @"
            INSERT INTO UrunGiris (UrunID, Miktar, Tarih, LotNumarasi, Notlar)
            VALUES (@UrunID, @Miktar, @Tarih, @LotNumarasi, @Notlar);
            SELECT SCOPE_IDENTITY();"; // Eklenen girişin ID'sini döndürür
                            SqlCommand urunGirisCommand = new SqlCommand(urunGirisQuery, connection2);
                            urunGirisCommand.Parameters.AddWithValue("@UrunID", urunID);
                            urunGirisCommand.Parameters.AddWithValue("@Miktar", miktar2);
                            urunGirisCommand.Parameters.AddWithValue("@Tarih", tarihGiris);
                            urunGirisCommand.Parameters.AddWithValue("@LotNumarasi", "");
                            urunGirisCommand.Parameters.AddWithValue("@Notlar", "Yarı Mamül Ürün Girişi");
                            int girisID = Convert.ToInt32(urunGirisCommand.ExecuteScalar()); // Giriş ID'sini al

                            // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                            if (!string.IsNullOrEmpty(seriNumaralariGiris))
                            {
                                foreach (string seriNoGiris in seriNumaralariGiris.Split(','))
                                {
                                    string seriNoEkleQuery = "INSERT INTO SeriNumaralari (GirisID, UrunID, LotNumarasi, SeriNo) VALUES (@GirisID, @UrunID, @LotNumarasi, @SeriNo)";
                                    SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection2);
                                    seriNoEkleCommand.Parameters.AddWithValue("@GirisID", girisID); // UrunGiris tablosundan alınan GirisID'yi ekleyin
                                    seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                                    seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotGiris);
                                    seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNoGiris.Trim());
                                    seriNoEkleCommand.ExecuteNonQuery(); // Bu satır komutu çalıştırır ve seri numarasını veritabanına ekler
                                }
                            }
                         //   MessageBox.Show("Ürün girişi başarıyla yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // DataGridView'i güncelle
                            guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                            guncelleyici.stokGuncelle(dataGridView3);
                            guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    // DataGridView'i güncelle
                    MessageBox.Show("Yarı Mamül oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guncelleyici.UrunListesiniGuncelle(dataGridView1);
                    guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                    guncelleyici.stokGuncelle(dataGridView3);
                    guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                    guncelleyici.stokGuncelle(dataGridView5);
                    guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                    guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void buttonYarıMamulGoster_Click(object sender, EventArgs e)
        {
             string veri = yariMamulSorgu.Text;
             using (SqlConnection conn = new SqlConnection(connectionString))
             {
                 try
                 {
                     conn.Open();
                     string query = @"
         SELECT 
             BU.BirlesikUrunKodu AS 'Birleşik Ürün Kodu',
             BU.BirlesikUrunAdi AS 'Birleşik Ürün Adı',
             BS.SeriNo AS 'Seri Numarası',
             UC.LotNumarasi AS 'Lot Numarası',
             U.UrunAdi AS 'Ürün Adı'
         FROM 
             BirlesikUrunler BU
         JOIN 
             BirlesikUrunSeriNumaralari BS ON BU.BirlesikUrunID = BS.BirlesikUrunID
         JOIN 
             Urunler U ON BS.UrunID = U.UrunID
         JOIN 
             UrunCikis UC ON U.UrunID = UC.UrunID
         WHERE 
             BU.BirlesikUrunKodu = @BirlesikUrunKodu";

                     SqlCommand command = new SqlCommand(query, conn);

                     // Parametre ekleyerek SQL sorgusunu hazırlama
                     command.Parameters.AddWithValue("@BirlesikUrunKodu", veri);

                     // Veri tabanından verileri okuyalım
                     SqlDataReader reader = command.ExecuteReader();

                     // DataGridView'e verileri ekleme
                     DataTable dataTable = new DataTable();
                     dataTable.Load(reader);
                    // Aynı seri numarasına sahip ürünleri filtreleme
                    var filteredRows = dataTable.AsEnumerable()
                                                .GroupBy(row => row.Field<string>("Seri Numarası"))
                                                .Select(group => group.First())
                                                .CopyToDataTable();
                    // DataGridView'e verileri ekleme
                    dataGridView11.DataSource = filteredRows;
                    reader.Close();
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             }

        }
     
        #endregion
        #region tam mamül kısmı stoğa sokalım
        //tam mamül kısmı

        private void buttonTamUrunBak_Click(object sender, EventArgs e)
        {
            // ComboBox'tan seçilen poset adını al
            string selectedPosetAdi = comboBox3.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedPosetAdi))
            {
                MessageBox.Show("Lütfen bir poset seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PosetKodu FROM Posetler WHERE PosetAdi = @PosetAdi";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@PosetAdi", selectedPosetAdi);
                string posetKodu = cmd.ExecuteScalar()?.ToString(); // ExecuteScalar kullanarak tek bir değer döndürüyoruz
                textBoxTamUrunAdi.Text = selectedPosetAdi;
                textBoxTamUrunKodu.Text = posetKodu;
                // Posetin içeriğini ListBox'ta göster
                query = @"
        SELECT Urunler.UrunAdi, Urunler.UrunKodu, PosetIcerikleri.Miktar 
        FROM PosetIcerikleri 
        INNER JOIN Urunler ON PosetIcerikleri.UrunID = Urunler.UrunID 
        INNER JOIN Posetler ON PosetIcerikleri.PosetID = Posetler.PosetID 
        WHERE Posetler.PosetAdi = @PosetAdi";
                cmd.CommandText = query;
                SqlDataReader reader = cmd.ExecuteReader();
                listBox2.Items.Clear();
                while (reader.Read())
                {
                    string urunAdi = reader["UrunAdi"].ToString();
                    string urunKodu = reader["UrunKodu"].ToString();
                    int miktar = Convert.ToInt32(reader["Miktar"]);
                    // Miktar 1'den fazlaysa her bir ürünü miktar kadar listeleyelim
                    for (int i = 0; i < miktar; i++)
                    {
                        listBox2.Items.Add($"Ürün Adı: {urunAdi}, Ürün Kodu: {urunKodu}, Miktar: 1");
                    }
                }
                reader.Close();
            }
        }
        private void buttonTamUrunEkle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxLotEkle.Text) && !string.IsNullOrWhiteSpace(textBoxSeriEkle.Text) && !string.IsNullOrEmpty(seciliUrun2))
            {
                // TextBox'lardan lot ve seri numaralarını al
                string lotNumarasi = textBoxLotEkle.Text;
                string seriNo = textBoxSeriEkle.Text;
                // Seçili öğenin sonuna lot ve seri numaralarını ekleyerek ListBox'ta güncelle
                listBox2.Items[listBox2.SelectedIndex] = $"{seciliUrun2} , Lot: {lotNumarasi}, Seri No: {seriNo}";
            }
            else
            {
                // Herhangi bir textbox boşsa, "0" yazdır
                string lotNumarasi = string.IsNullOrWhiteSpace(textBoxLotEkle.Text) ? "0" : textBoxLotEkle.Text;
                string seriNo = string.IsNullOrWhiteSpace(textBoxSeriEkle.Text) ? "0" : textBoxSeriEkle.Text;

                // Uyarı mesajı yerine direkt "0" yazdır
                if (listBox2.SelectedIndex != -1)
                {
                    listBox2.Items[listBox2.SelectedIndex] = $"{seciliUrun2} , Lot: {lotNumarasi}, Seri No: {seriNo}";
                }
                else
                {
                    // ListBox'ta seçili bir öğe yoksa, yeni bir öğe olarak ekle
                    listBox2.Items.Add($"{seciliUrun2} , Lot: {lotNumarasi}, Seri No: {seriNo}");
                }


            }
        }
        private string seciliUrun2;
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                // ListBox'ta seçili olan öğenin bilgisini al ve sakla
                seciliUrun2 = listBox2.SelectedItem.ToString();
            }
        }
        private void buttonTamSil_Click(object sender, EventArgs e)
        {

            if (listBox2.SelectedItem != null)
            {
                // Seçili öğenin içeriğini al
                string seciliUrun = listBox2.SelectedItem.ToString();
                // Öğenin sonunda çizgi (-) işareti varsa ve içinde "Lot:" ve "Seri No:" ifadeleri varsa
                int index = seciliUrun.LastIndexOf("-");
                if (index != -1 && seciliUrun.Contains("Lot:") && seciliUrun.Contains("Seri No:"))
                {
                    // Öğenin sonundaki çizgi ve lot/seri numaralarını içeren kısmı sil
                    seciliUrun = seciliUrun.Substring(0, index).Trim();
                    listBox2.Items[listBox2.SelectedIndex] = seciliUrun;
                }
                else
                {
                    // Sadece seçili öğeyi sil
                    listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir öğe seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void buttonTamOlustur_Click(object sender, EventArgs e)
        {
            string urunAdi2 = textBoxTamUrunAdi.Text;
            string urunKodu2=textBoxTamUrunKodu.Text;
            string kategori2=comboBoxTamKategori.Text;
            string ad = textBoxTamSeri.Text;
            //girişlerin boş olup olmadığını kontrol edelim
            if (string.IsNullOrEmpty(urunAdi2)||string.IsNullOrEmpty(urunKodu2)||string.IsNullOrEmpty(kategori2))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun !","Hata",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //ürün varmi diye kontrol et

                if(UrunVarMi(urunAdi2,urunKodu2)) 
                {
                    MessageBox.Show("Aynı ürün adı veya ürün kodu zaten var! Ürün Oluşturmadan devam ediliyor", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    EkleUrun(urunAdi2, urunKodu2, kategori2);
                    // DataGridView'i güncelle
                    guncelleyici.UrunListesiniGuncelle(dataGridView1);
                    guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                    guncelleyici.stokGuncelle(dataGridView3);
                    guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                    guncelleyici.stokGuncelle(dataGridView5);
                    guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                    guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                    guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                    guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                }
                if (string.IsNullOrEmpty(ad))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    //seri numarası yoksa üürünü ile ilgili işlemler yaptırmammak için yaptım.
                }
                //Veri tabanından list box daki ürünlerin çıkışını yapalım....yine using methodu ile veri tabnaına bağlandık.
                using (SqlConnection connection =new SqlConnection(connectionString))
                {

                    try
                    {

                        connection.Open();
                        foreach (var item in listBox2.Items)
                        {
                            string urunAdi = "";
                            string urunKodu = "";
                            int miktar = 0;
                            string lotNo = "";
                            List<string> seriNumaralari = new List<string>();
                            string[] bilgiler = item.ToString().Split(',');
                            //her satirdaki bilgileri işle

                            foreach (var bilgi in bilgiler)
                            {
                                if (bilgi.Contains("Ürün Adı"))
                                {
                                    urunAdi = bilgi.Split(':')[1].Trim();
                                }
                                else if (bilgi.Contains("Ürün Kodu"))
                                {
                                    urunKodu = bilgi.Split(':')[1].Trim();
                                }
                                else if (bilgi.Contains("Miktar"))
                                {
                                    int.TryParse(bilgi.Split(':')[1].Trim(), out miktar);
                                }
                                else if (bilgi.Contains("Lot"))
                                {
                                    lotNo = bilgi.Split(':')[1].Trim();
                                }
                                else if (bilgi.Contains("Seri No"))
                                {
                                    string seriBilgisi = bilgi.Split(':')[1].Trim();
                                    string[] seriNumaraDizisi = seriBilgisi.Split(',');
                                    // Her bir seri numarasını ana seri numara listesine ekle
                                    seriNumaralari.AddRange(seriNumaraDizisi.Select(s => s.Trim()));
                                }

                            }
                            //Ürün ıd almak için ürün koduyla arama yapalım...
                            string urunIDQuery = "SELECT UrunID FROM Urunler WHERE UrunKodu = @UrunKodu";
                            SqlCommand urunIDCommand = new SqlCommand(urunIDQuery, connection);
                            urunIDCommand.Parameters.AddWithValue("@UrunKodu", urunKodu);
                            int urunID = Convert.ToInt32(urunIDCommand.ExecuteScalar());
                            if (urunID == 0)
                            {
                                MessageBox.Show("Girilen ürün veritabanında bulunmamaktadır! Lütfen önce ürün oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            string kategori = comboBoxTamKategori.Text;
                            // Ürün çıkışı yap
                            string urunCikisQuery = @"
INSERT INTO UrunCikis (UrunID, Miktar, CikisTarihi, Notlar, Kategori, CikisNedeni, LotNumarasi)
OUTPUT INSERTED.CikisID
VALUES (@UrunID, @Miktar, @CikisTarihi, @Notlar, @Kategori, @CikisNedeni, @LotNumarasi)";
                            SqlCommand urunCikisCommand = new SqlCommand(urunCikisQuery, connection);
                            urunCikisCommand.Parameters.AddWithValue("@UrunID", urunID);
                            urunCikisCommand.Parameters.AddWithValue("@Miktar", miktar);
                            urunCikisCommand.Parameters.AddWithValue("@CikisTarihi", DateTime.Now); // Şu anki tarihi kullanabilirsiniz
                            urunCikisCommand.Parameters.AddWithValue("@Notlar", "Yarı Mamülde Kullanıldı."); // Notlar boş olacaksa
                            urunCikisCommand.Parameters.AddWithValue("@Kategori", kategori); // Kategori boş olacaksa
                            urunCikisCommand.Parameters.AddWithValue("@CikisNedeni", "Üretimde Kullanıldı."); // Çıkış nedeni boş olacaksa
                            urunCikisCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                            // Insert komutunu çalıştırarak cikisID değerini al
                            int cikisID = (int)urunCikisCommand.ExecuteScalar();
                            // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                            foreach (string seriNo in seriNumaralari)
                            {
                                string seriNoEkleQuery = "INSERT INTO SeriNumaralari (CikisID, UrunID, LotNumarasi, SeriNo) VALUES (@CikisID, @UrunID, @LotNumarasi, @SeriNo)";
                                SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection);
                                seriNoEkleCommand.Parameters.AddWithValue("@CikisID", cikisID);
                                seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                                seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                                seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNo.Trim());
                                seriNoEkleCommand.ExecuteNonQuery();
                            }
                            // Tam Birleşik ürün adını ve kodunu al
                            string birlesikUrunAdi = textBoxTamUrunAdi.Text;
                            string birlesikUrunKodu = textBoxTamSeri.Text;
                            // Tam Birleşik ürünü veritabanına ekle
                            string birlesikUrunEkleQuery = @"
                INSERT INTO YeniTablo (YeniTabloAdi, YeniTabloKodu)
                VALUES (@YeniTabloAdi, @YeniTabloKodu);
            ";
                            using (SqlCommand birlesikUrunEkleCommand = new SqlCommand(birlesikUrunEkleQuery, connection))
                            {
                                birlesikUrunEkleCommand.Parameters.AddWithValue("@YeniTabloAdi", birlesikUrunAdi);
                                birlesikUrunEkleCommand.Parameters.AddWithValue("@YeniTabloKodu", birlesikUrunKodu);
                                birlesikUrunEkleCommand.ExecuteNonQuery();
                                // Eklenen birleşik ürünün ID'sini al
                                string birlesikUrunIDQuery = "SELECT TOP 1 YeniTabloID FROM YeniTablo ORDER BY YeniTabloID DESC";
                                SqlCommand birlesikUrunIDCommand = new SqlCommand(birlesikUrunIDQuery, connection);
                                int birlesikUrunID = Convert.ToInt32(birlesikUrunIDCommand.ExecuteScalar());
                                // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                                foreach (string seriNo in seriNumaralari)
                                {
                                    string seriNoEkleQuery = @"
                        INSERT INTO YeniTabloSeriNumaralari (YeniTabloID, UrunID, LotNumarasi, SeriNo) 
                        VALUES (@YeniTabloID, @UrunID, @LotNumarasi, @SeriNo)
                    ";
                                    SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection);
                                    seriNoEkleCommand.Parameters.AddWithValue("@YeniTabloID", birlesikUrunID);
                                    seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                                    seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotNo);
                                    seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNo.Trim());
                                    seriNoEkleCommand.ExecuteNonQuery();
                                }
                               // MessageBox.Show("Toplu ürün girişi yapıldı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                      //  MessageBox.Show("Ürünler düzgün bir Şekilde çıkartıldı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Ürün girişi yap
                        // Diğer giriş bilgilerini almak
                        string kodu3 = textBoxTamUrunKodu.Text;
                        int miktar3 = 1; // Toplu ürün çıkışı yapıyoruz ama o ürünler birleşiyor bir ürün oluşuyor.
                        string lotGiris3 = textBoxTamSeri.Text;
                        DateTime tarihGiris3 = DateTime.Today;
                        string seriNumaralariGiris3 = textBoxTamSeri.Text;
                        using (SqlConnection connection3 = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection3.Open();
                                // Ürün ID'sini almak için ÜrünKodu'na göre sorgulama yap
                                string urunIDQuery = "SELECT UrunID FROM Urunler WHERE UrunKodu = @UrunKodu";
                                SqlCommand urunIDCommand = new SqlCommand(urunIDQuery, connection3);
                                urunIDCommand.Parameters.AddWithValue("@UrunKodu", kodu3);
                                int urunID = Convert.ToInt32(urunIDCommand.ExecuteScalar());

                                // Ürün girişi yap
                                string urunGirisQuery = @"
            INSERT INTO UrunGiris (UrunID, Miktar, Tarih, LotNumarasi, Notlar)
            VALUES (@UrunID, @Miktar, @Tarih, @LotNumarasi, @Notlar);
            SELECT SCOPE_IDENTITY();"; // Eklenen girişin ID'sini döndürür
                                SqlCommand urunGirisCommand = new SqlCommand(urunGirisQuery, connection3);
                                urunGirisCommand.Parameters.AddWithValue("@UrunID", urunID);
                                urunGirisCommand.Parameters.AddWithValue("@Miktar", miktar3);
                                urunGirisCommand.Parameters.AddWithValue("@Tarih", tarihGiris3);
                                urunGirisCommand.Parameters.AddWithValue("@LotNumarasi", "");
                                urunGirisCommand.Parameters.AddWithValue("@Notlar","Tam Ürün Girişi");
                                int girisID = Convert.ToInt32(urunGirisCommand.ExecuteScalar()); // Giriş ID'sini al

                                // Seri numaralarını eklemek için her bir seri numarası için INSERT işlemi gerçekleştir
                                if (!string.IsNullOrEmpty(seriNumaralariGiris3))
                                {
                                    foreach (string seriNoGiris in seriNumaralariGiris3.Split(','))
                                    {
                                        string seriNoEkleQuery = "INSERT INTO SeriNumaralari (GirisID, UrunID, LotNumarasi, SeriNo) VALUES (@GirisID, @UrunID, @LotNumarasi, @SeriNo)";
                                        SqlCommand seriNoEkleCommand = new SqlCommand(seriNoEkleQuery, connection3);
                                        seriNoEkleCommand.Parameters.AddWithValue("@GirisID", girisID); // UrunGiris tablosundan alınan GirisID'yi ekleyin
                                        seriNoEkleCommand.Parameters.AddWithValue("@UrunID", urunID);
                                        seriNoEkleCommand.Parameters.AddWithValue("@LotNumarasi", lotGiris3);
                                        seriNoEkleCommand.Parameters.AddWithValue("@SeriNo", seriNoGiris.Trim());
                                        seriNoEkleCommand.ExecuteNonQuery(); // Bu satır komutu çalıştırır ve seri numarasını veritabanına ekler
                                    }
                                }
                              //  MessageBox.Show("Ürün girişi başarıyla yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // DataGridView'i güncelle
                                guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                                guncelleyici.stokGuncelle(dataGridView3);
                                guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        // DataGridView'i güncelle
                        guncelleyici.UrunListesiniGuncelle(dataGridView1);
                        guncelleyici.UrunGirisListesiniGuncelle(dataGridView2);
                        guncelleyici.stokGuncelle(dataGridView3);
                        guncelleyici.UrunCikisListesiniGuncelle(dataGridView4);
                        guncelleyici.stokGuncelle(dataGridView5);
                        guncelleyici.UrunVeSeriNumaralariGetir(dataGridViewSeriNo);//Girren
                        guncelleyici.UrunVeSeriNumaralariGetir2(dataGridView7);//çıkan
                        guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView10);//yari mamül olsurunca lazım olur
                        guncelleyici.UrunVeSeriNumaralariGetir3(dataGridView12);//Tam mamül olusunca lazım olur.
                        guncelleyici.GizliSeriNumaralariGuncelle(dataGridView10, "SeriNumaralari");
                        guncelleyici.GizliSeriNumaralariGuncelle(dataGridView12, "SeriNumaralari");
                        MessageBox.Show("Mamül oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //TAM MAMÜLÜN İÇ YAPISINI GÖSTER
        private void buttonTamMamulGoster_Click(object sender, EventArgs e)
        {

            // Yarı mamul sorgu metnini alalım
            string veri = textBoxTamMamul.Text;

            // SQL bağlantısı oluşturma
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // Bağlantıyı açma
                    conn.Open();

                    // SQL sorgusu
                    string query = @"
                SELECT 
                    YT.YeniTabloKodu AS 'Yeni Tablo Kodu',
                    YT.YeniTabloAdi AS 'Yeni Tablo Adı',
                    YTSN.SeriNo AS 'Seri Numarası',
                    YTSN.LotNumarasi AS 'Lot Numarası',
                    U.UrunAdi AS 'Ürün Adı'
                FROM 
                    YeniTablo YT
                JOIN 
                    YeniTabloSeriNumaralari YTSN ON YT.YeniTabloID = YTSN.YeniTabloID
                JOIN 
                    Urunler U ON YTSN.UrunID = U.UrunID
                JOIN 
                    UrunCikis UC ON U.UrunID = UC.UrunID
                WHERE 
                    YT.YeniTabloKodu = @YeniTabloKodu";

                    // Komut oluşturma
                    SqlCommand command = new SqlCommand(query, conn);

                    // Parametre ekleyerek SQL sorgusunu hazırlama
                    command.Parameters.AddWithValue("@YeniTabloKodu", veri);

                    // Veri tabanından verileri okuma
                    SqlDataReader reader = command.ExecuteReader();

                    // DataGridView'e verileri eklemek için bir DataTable oluşturalım
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    // Aynı seri numarasına sahip ürünleri filtreleme
                    var filteredRows = dataTable.AsEnumerable()
                                                .GroupBy(row => row.Field<string>("Seri Numarası"))
                                                .Select(group => group.First())
                                                .CopyToDataTable();
                    // DataGridView'e verileri ekleme
                    dataGridView13.DataSource = filteredRows;

                    // Okuyucuyu kapatma
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Hata mesajı gösterme
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }



        #endregion
        #region qr işlemleri
        private void btnQrAc_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    qrIslemleri.Baslat();
                    label70.Text = "QR okunuyor...";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
     
        // DataGridView'deki verileri içeren DataTable'ı gruplayıp seri numarasına göre filtreleyen metod
        private DataTable FilterDuplicateProducts(DataTable originalTable)
        {
            // Seri numarasına göre gruplayın ve aynı seri numarasına sahip olan ürünleri filtreleyin
            var filteredRows = originalTable.AsEnumerable()
                                            .GroupBy(row => row.Field<string>("Seri Numarası"))
                                            .Select(group => group.First())
                                            .CopyToDataTable();
            return filteredRows;
        }
        private void buttonOku_Click(object sender, EventArgs e)
        {
            qrIslemleri.Durdur();
            label70.Text = "";
            // QR kodunu oku
            string qrIcerik = qrIslemleri.QRKoduOku();
            // QR kodunun boş olup olmadığını kontrol et
            if (!string.IsNullOrEmpty(qrIcerik))
            {
                // Hangi radyo butonunun seçildiğine bak ve ilgili sorguyu yap
                if (radioButtonTam.Checked)
                {
                    DataTable filteredTable = qrIslemleri.SorgulaTamUrun(qrIcerik);
                    // Aynı üründe filtreleme yap
                    filteredTable = FilterDuplicateProducts(filteredTable);
                    dataGridView14.DataSource = filteredTable;
                    label70.Text = "QR okundu.";
                }
                else if (radioButtonYari.Checked)
                {
                    DataTable filteredTable = qrIslemleri.SorgulaYariMamul(qrIcerik);
                    // Aynı üründe filtreleme yap
                    filteredTable = FilterDuplicateProducts(filteredTable);
                    dataGridView14.DataSource = filteredTable;
                    label70.Text = "";
                }
                else
                {
                    MessageBox.Show("Lütfen bir seçenek seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("QR kodu okunmadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            qrIslemleri.Durdur(); // Kamerayı durdur
            label70.Text = ""; // Labeli temizle
        }


        #endregion


        //Özelden genele
        private void buttonOzeldenGenel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bu özellik V2 de gelecektir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            /*string seriNumarasi = textBoxOzeldenGenele.Text;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
SELECT DISTINCT 
    UC.SeriNo AS 'Yarı Mamül Kullanılan Cihazın Seri Numarası'
FROM 
    BirlesikUrunler BU
JOIN 
    BirlesikUrunSeriNumaralari BS ON BU.BirlesikUrunID = BS.BirlesikUrunID
JOIN 
    Urunler U ON BS.UrunID = U.UrunID
JOIN 
    UrunCikis UC ON U.UrunID = UC.UrunID
WHERE 
    BS.SeriNo = @SeriNo OR UC.LotNumarasi = @LotNumarasi";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@SeriNo", seriNumarasi);
                    command.Parameters.AddWithValue("@LotNumarasi", seriNumarasi);

                    SqlDataReader reader = command.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    // Sonuçları kullanıcıya göstermek için DataGridView'e verileri ekleme
                    dataGridView15.DataSource = dataTable;

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }*/
            //            using (SqlConnection conn = new SqlConnection(connectionString))
            //            {
            //                try
            //                {
            //                    conn.Open();
            //                    string query = @"
            //SELECT 
            //    BU.BirlesikUrunKodu AS 'Birleşik Ürün Kodu',
            //    BU.BirlesikUrunAdi AS 'Birleşik Ürün Adı',
            //    BS.SeriNo AS 'Seri Numarası',
            //    UC.LotNumarasi AS 'Lot Numarası',
            //    U.UrunAdi AS 'Ürün Adı'
            //FROM 
            //    BirlesikUrunler BU
            //JOIN 
            //    BirlesikUrunSeriNumaralari BS ON BU.BirlesikUrunID = BS.BirlesikUrunID
            //JOIN 
            //    Urunler U ON BS.UrunID = U.UrunID
            //JOIN 
            //    UrunCikis UC ON U.UrunID = UC.UrunID
            //WHERE 
            //    BS.SeriNo = @SeriNo OR UC.LotNumarasi = @LotNumarasi";

            //                    SqlCommand command = new SqlCommand(query, conn);
            //                    command.Parameters.AddWithValue("@SeriNo", seriNumarasi);
            //                    command.Parameters.AddWithValue("@LotNumarasi", seriNumarasi);

            //                    SqlDataReader reader = command.ExecuteReader();

            //                    DataTable dataTable = new DataTable();
            //                    dataTable.Load(reader);

            //                    // Sonuçları kullanıcıya göstermek için DataGridView'e verileri ekleme
            //                    dataGridView15.DataSource = dataTable;

            //                    reader.Close();
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }
            //            }


        }

        private void buttonIsEmriYari_Click(object sender, EventArgs e)
        {
            string isEmriNo = textBoxIsEmri.Text;
            string revizyon = textBoxRevizyon.Text;
            float miktar;
            if (!float.TryParse(textBoxMiktar.Text, out miktar))
            {
                MessageBox.Show("Miktar geçerli bir sayı olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string posetAdi = comboBoxBomYarisEmri.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(posetAdi))
            {
                MessageBox.Show("Lütfen bir BOM adı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DateTime tarihB = dateTimePicker1.Value;
            DateTime tarihK = dateTimePicker2.Value;
            string notlar = textBox2.Text;


            textBoxIsEmri.Text = "";
            textBoxRevizyon.Text = "";
            textBoxMiktar.Text = "";
            textBox2.Text = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int urunID = GetUrunIDByPosetAdi(posetAdi, connection);

                    if (urunID == -1)
                    {
                        MessageBox.Show("PosetAdi'na karşılık gelen bir ürün bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string query = @"
                    INSERT INTO YariMamulIsEmirleri (UrunID, Miktar, BaslangicTarihi, BitisTarihi, Notlar, Revizyon, IsEmriNo)
                    VALUES (@UrunID, @Miktar, @BaslangicTarihi, @BitisTarihi, @Notlar, @Revizyon, @IsEmriNo)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UrunID", urunID);
                        command.Parameters.AddWithValue("@Miktar", miktar);
                        command.Parameters.AddWithValue("@BaslangicTarihi", tarihB);
                        command.Parameters.AddWithValue("@BitisTarihi", tarihK);
                        command.Parameters.AddWithValue("@Notlar", notlar);
                        command.Parameters.AddWithValue("@Revizyon", revizyon);
                        command.Parameters.AddWithValue("@IsEmriNo", isEmriNo);

                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                        {
                            MessageBox.Show("Veri eklenirken bir hata oluştu.");
                        }
                        else
                        {
                            MessageBox.Show("İş emri başarıyla eklendi.");
                            RefreshComboBox6();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
        private int GetUrunIDByPosetAdi(string posetAdi, SqlConnection connection)
        {
            int urunID = -1;
            try
            {
                string query = "SELECT UrunID FROM Urunler WHERE UrunAdi = @UrunAdi";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UrunAdi", posetAdi);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        urunID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            return urunID;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Seçilen BOM adını al
            string selectedBom = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedBom))
            {
                MessageBox.Show("Lütfen bir BOM adı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // BOM adına göre ilgili iş emirlerini getir
            string query = "SELECT * FROM YariMamulIsEmirleri WHERE UrunID IN (SELECT UrunID FROM Posetler WHERE PosetAdi = @PosetAdi)";
            // Bağlantı oluştur
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Komut ve bağlantıyı hazırla
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PosetAdi", selectedBom);
                try
                {
                    // Bağlantıyı aç
                    connection.Open();

                    // Verileri oku
                    SqlDataReader reader = command.ExecuteReader();

                    // Verileri DataGridView'de göster
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView16.DataSource = dataTable;
                    dataGridView16.Columns[0].Visible = false; // İlk sütunu gizle
                    dataGridView16.Columns[1].Visible = false; // İkinci sütunu gizle
                    dataGridView16.Columns["IsEmriNo"].DisplayIndex = 0; // İş Emri No sütununu ilk sıraya yerleştir
                    dataGridView16.Columns["Revizyon"].DisplayIndex = 1; // Revizyon sütununu ikinci sıraya yerleştir
                    dataGridView16.Columns["Miktar"].DisplayIndex = 2; // Miktar sütununu üçüncü sıraya yerleştir
                    dataGridView16.Columns["BaslangicTarihi"].DisplayIndex = 3; // Başlangıç Tarihi sütununu dördüncü sıraya yerleştir
                    dataGridView16.Columns["BitisTarihi"].DisplayIndex = 4; // Bitiş Tarihi sütununu beşinci sıraya yerleştir
                                                                            // Geri kalan sütunlar otomatik olarak eklenmiş olacak
                    dataGridView16.Columns["IsEmriNo"].HeaderText = "İş Emri No";
                    dataGridView16.Columns["Revizyon"].HeaderText = "Revizyon";
                    dataGridView16.Columns["Miktar"].HeaderText = "Miktar";
                    dataGridView16.Columns["BaslangicTarihi"].HeaderText = "Başlangıç Tarihi";
                    dataGridView16.Columns["BitisTarihi"].HeaderText = "Bitiş Tarihi";
                    dataGridView16.Columns["Notlar"].HeaderText = "Notlar";
                    dataGridView16.Columns["Revizyon"].HeaderText = "Revizyon";



                    // Okuyucuyu kapat
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string isEmriNo = IsEmriNo.Text;
            string revizyon = textBoxRevMamul.Text;
            float miktar;
            if (!float.TryParse(textBoxMikMamul.Text, out miktar))
            {
                MessageBox.Show("Miktar geçerli bir sayı olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string posetAdi = comboBoxMamul.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(posetAdi))
            {
                MessageBox.Show("Lütfen bir BOM adı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DateTime tarihB = dateTimePicker4Bas.Value;
            DateTime tarihK = dateTimePicker3Bit.Value;
            string notlar = textBox3Not.Text;
            // Bağlantı oluştur
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Bağlantıyı aç
                    connection.Open();

                    // Ürün ID'sini al
                    int urunID = GetUrunIDByPosetAdi(posetAdi, connection);
                    // Eğer ürün bulunamadıysa
                    if (urunID == -1)
                    {
                        MessageBox.Show("PosetAdi'na karşılık gelen bir ürün bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Mamül iş emrini veritabanına ekle
                    string query = @"
            INSERT INTO MamulIsEmirleri (UrunID, Miktar, BaslangicTarihi, BitisTarihi, Notlar, Revizyon, IsEmriNo)
            VALUES (@UrunID, @Miktar, @BaslangicTarihi, @BitisTarihi, @Notlar, @Revizyon, @IsEmriNo)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UrunID", urunID);
                        command.Parameters.AddWithValue("@Miktar", miktar);
                        command.Parameters.AddWithValue("@BaslangicTarihi", tarihB);
                        command.Parameters.AddWithValue("@BitisTarihi", tarihK);
                        command.Parameters.AddWithValue("@Notlar", notlar);
                        command.Parameters.AddWithValue("@Revizyon", revizyon);
                        command.Parameters.AddWithValue("@IsEmriNo", isEmriNo);

                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                        {
                            MessageBox.Show("Veri eklenirken bir hata oluştu.");
                        }
                        else
                        {
                            MessageBox.Show("Mamül iş emri başarıyla eklendi.");
                            RefreshComboBox7(); // ComboBox'ı yenileme metodu
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Seçilen BOM adını al
            string selectedBom = comboBoxIsEmri.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedBom))
            {
                MessageBox.Show("Lütfen bir BOM adı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // BOM adına göre ilgili iş emirlerini getir
            string query = "SELECT * FROM MamulIsEmirleri WHERE UrunID IN (SELECT UrunID FROM Posetler WHERE PosetAdi = @PosetAdi)";

            // Bağlantı oluştur
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Komut ve bağlantıyı hazırla
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PosetAdi", selectedBom);

                try
                {
                    // Bağlantıyı aç
                    connection.Open();

                    // Verileri oku
                    SqlDataReader reader = command.ExecuteReader();

                    // Verileri DataGridView'de göster
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView17.DataSource = dataTable;
                    dataGridView17.Columns[0].Visible = false; // İlk sütunu gizle
                    dataGridView17.Columns[1].Visible = false; // İkinci sütunu gizle
                    dataGridView17.Columns["IsEmriNo"].DisplayIndex = 0; // İş Emri No sütununu ilk sıraya yerleştir
                    dataGridView17.Columns["Revizyon"].DisplayIndex = 1; // Revizyon sütununu ikinci sıraya yerleştir
                    dataGridView17.Columns["Miktar"].DisplayIndex = 2; // Miktar sütununu üçüncü sıraya yerleştir
                    dataGridView17.Columns["BaslangicTarihi"].DisplayIndex = 3; // Başlangıç Tarihi sütununu dördüncü sıraya yerleştir
                    dataGridView17.Columns["BitisTarihi"].DisplayIndex = 4; // Bitiş Tarihi sütununu beşinci sıraya yerleştir
                                                                            // Geri kalan sütunlar otomatik olarak eklenmiş olacak
                    dataGridView17.Columns["IsEmriNo"].HeaderText = "İş Emri No";
                    dataGridView17.Columns["Revizyon"].HeaderText = "Revizyon";
                    dataGridView17.Columns["Miktar"].HeaderText = "Miktar";
                    dataGridView17.Columns["BaslangicTarihi"].HeaderText = "Başlangıç Tarihi";
                    dataGridView17.Columns["BitisTarihi"].HeaderText = "Bitiş Tarihi";
                    dataGridView17.Columns["Notlar"].HeaderText = "Notlar";
                    dataGridView17.Columns["Revizyon"].HeaderText = "Revizyon";

                    // Okuyucuyu kapat
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
    }



}
