using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace YourNamespace
{
   
    public class QRIslemleri
    {
        private VideoCaptureDevice videoSource;
        private string connectionString;
        public string qrIcerik;
        public QRIslemleri(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Baslat()
        {
            // Kamera başlatma işlemleri burada gerçekleştirilecek
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame; // Yeni kare geldiğinde tetiklenecek olayı tanımla
                videoSource.Start(); // Kamerayı başlat
            }
            else
            {
                MessageBox.Show("Kamera bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Her yeni kare geldiğinde bu metot tetiklenecek
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone(); // Yeni kareyi klonlayarak işlem yap
            BarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(frame);
            if (result != null)
            {
                  qrIcerik= result.Text; // QR kodunun içeriğini al
              

            }
        }

        public void Durdur()
        {
            // Kamera durdurma işlemleri burada gerçekleştirilecek
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop(); // Kamerayı durdur
                videoSource.WaitForStop(); // Kameranın durmasını bekle
            }
        }

        public string QRKoduOku()
        {
            // QR kod okuma işlemleri...
            // Okunan QR kodunun içeriğini döndür
            return qrIcerik;
        }

        public DataTable SorgulaTamUrun(string qrIcerik)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
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

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@YeniTabloKodu", qrIcerik);
                    SqlDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Hata mesajı gösterme
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dataTable;
        }

        public DataTable SorgulaYariMamul(string qrIcerik)
        {
            DataTable dataTable = new DataTable();
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
                    command.Parameters.AddWithValue("@BirlesikUrunKodu", qrIcerik);
                    SqlDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Hata mesajı gösterme
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dataTable;
        }
    }
}
