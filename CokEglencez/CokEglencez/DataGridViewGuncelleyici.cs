using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace CokEglencez
{
    public class DataGridViewGuncelleyici
    {
        private string connectionString;

        public DataGridViewGuncelleyici(string connectionString)
        {
            this.connectionString = connectionString;
        }
        // Ürünlerin listesini DataGridView1 kontrolüne yükleyen metod
        public void UrunListesiniGuncelle(DataGridView dataGridView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string urunQuery = "SELECT UrunID, UrunAdi, UrunKodu, Kategori FROM Urunler";
                SqlDataAdapter urunDataAdapter = new SqlDataAdapter(urunQuery, connection);
                DataTable urunDataTable = new DataTable();
                urunDataAdapter.Fill(urunDataTable);
                dataGridView.DataSource = urunDataTable;
                dataGridView.Columns["UrunID"].Visible = false;
                dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                dataGridView.Columns["Kategori"].HeaderText = "Kategori";
                
                //dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        public void Fhrist(DataGridView dataGridView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
               
            {
                string fihristQuery = "SELECT FihristID, SirketIsmi, CalisanIsmi,     MailAdresi , TelefonNumarasi1, TelefonNumarasi2 FROM Fihrist";
                SqlDataAdapter fihristDataAdapter = new SqlDataAdapter(fihristQuery, connection);
                DataTable fihristDataTable = new DataTable();
                fihristDataAdapter.Fill(fihristDataTable);
                dataGridView.DataSource = fihristDataTable;
                dataGridView.Columns["FihristID"].Visible = false;
               // dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // Ürün girişlerinin listesini DataGridView2 kontrolüne yükleyen metod
        public void UrunGirisListesiniGuncelle(DataGridView dataGridView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string urunGirisQuery = "SELECT Urunler.UrunAdi, Urunler.UrunKodu, CONVERT(DECIMAL(10, 1), UrunGiris.Miktar) AS Miktar, UrunGiris.Tarih, UrunGiris.Notlar, UrunGiris.LotNumarasi, SeriNumaralari.SeriNo " +
                                        "FROM Urunler " +
                                        "INNER JOIN UrunGiris ON Urunler.UrunID = UrunGiris.UrunID " +
                                        "LEFT JOIN SeriNumaralari ON UrunGiris.GirisID = SeriNumaralari.GirisID";
                SqlDataAdapter urunGirisDataAdapter = new SqlDataAdapter(urunGirisQuery, connection);
                DataTable urunGirisDataTable = new DataTable();
                urunGirisDataAdapter.Fill(urunGirisDataTable);
                dataGridView.DataSource = urunGirisDataTable;
                // Sütun başlıklarını ayarla
                dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                dataGridView.Columns["Miktar"].HeaderText = "Miktar";
                dataGridView.Columns["Tarih"].HeaderText = "Tarih";
                dataGridView.Columns["Notlar"].HeaderText = "Notlar";
                dataGridView.Columns["LotNumarasi"].HeaderText = "Lot Numarası";
                dataGridView.Columns["SeriNo"].HeaderText = "Seri Numarası";
               // dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            //2eski versiyon
            /*using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string urunGirisQuery = "SELECT Urunler.UrunAdi, Urunler.UrunKodu, CONVERT(DECIMAL(10, 1), UrunGiris.Miktar) AS Miktar, UrunGiris.Tarih, UrunGiris.Notlar, UrunGiris.LotNumarasi " +
                                        "FROM Urunler " +
                                        "INNER JOIN UrunGiris ON Urunler.UrunID = UrunGiris.UrunID";

                SqlDataAdapter urunGirisDataAdapter = new SqlDataAdapter(urunGirisQuery, connection);
                DataTable urunGirisDataTable = new DataTable();
                urunGirisDataAdapter.Fill(urunGirisDataTable);
                dataGridView.DataSource = urunGirisDataTable;

                // Sütun başlıklarını ayarla
                dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                dataGridView.Columns["Miktar"].HeaderText = "Miktar";
                dataGridView.Columns["Tarih"].HeaderText = "Tarih";
                dataGridView.Columns["Notlar"].HeaderText = "Notlar";
                dataGridView.Columns["LotNumarasi"].HeaderText = "Lot Numarası";
            }*/
            //eski sorgu sadece ürün girişi yapar lot yoktu ilerii de lazım olur diye silmiyorum.
            /* using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 string urunGirisQuery = "SELECT Urunler.UrunAdi, CONVERT(DECIMAL(10, 3), UrunGiris.Miktar) AS Miktar, UrunGiris.Tarih, UrunGiris.Notlar " +
                         "FROM Urunler INNER JOIN UrunGiris ON Urunler.UrunID = UrunGiris.UrunID";

                 SqlDataAdapter urunGirisDataAdapter = new SqlDataAdapter(urunGirisQuery, connection);
                 DataTable urunGirisDataTable = new DataTable();
                 urunGirisDataAdapter.Fill(urunGirisDataTable);
                 dataGridView.DataSource = urunGirisDataTable;
                 dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                 dataGridView.Columns["Miktar"].HeaderText = "Miktar";
                 dataGridView.Columns["Tarih"].HeaderText = "Tarih";
                 dataGridView.Columns["Notlar"].HeaderText = "Notlar";
                 foreach (DataGridViewColumn column in dataGridView.Columns)
                 {
                     if (column.HeaderText != "Ürün Adı" && column.HeaderText != "Miktar" && column.HeaderText != "Tarih" && column.HeaderText != "Notlar")
                     {
                         column.Visible = false;
                     }
                 }
             }*/
        }

        public void UrunVeSeriNumaralariGetir(DataGridView dataGridView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT Urunler.UrunAdi, Urunler.UrunKodu, Urunler.Kategori, UrunGiris.Tarih, UrunGiris.LotNumarasi, SeriNumaralari.SeriNo
                FROM Urunler
                LEFT JOIN UrunGiris ON Urunler.UrunID = UrunGiris.UrunID
                LEFT JOIN SeriNumaralari ON UrunGiris.GirisID = SeriNumaralari.GirisID
            ";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
                dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                dataGridView.Columns["Kategori"].HeaderText = "Kategori";
                dataGridView.Columns["Tarih"].HeaderText = "Tarih";
                dataGridView.Columns["LotNumarasi"].HeaderText = "Lot Numarası";
                dataGridView.Columns["SeriNo"].HeaderText = "Seri Numarası";
                // dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }
        

        public void UrunVeSeriNumaralariGetir2(DataGridView dataGridView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
    SELECT Urunler.UrunAdi, Urunler.UrunKodu, Urunler.Kategori, UrunCikis.CikisTarihi, UrunCikis.LotNumarasi, SeriNumaralari.SeriNo
    FROM Urunler
    LEFT JOIN UrunCikis ON Urunler.UrunID = UrunCikis.UrunID
    LEFT JOIN SeriNumaralari ON UrunCikis.CikisID = SeriNumaralari.CikisID
    ";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
                dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                dataGridView.Columns["Kategori"].HeaderText = "Kategori";
                dataGridView.Columns["CikisTarihi"].HeaderText = "Çıkış Tarihi";
                dataGridView.Columns["LotNumarasi"].HeaderText = "Lot Numarası";
                dataGridView.Columns["SeriNo"].HeaderText = "Seri Numarası";
                // dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }
        public void UrunVeSeriNumaralariGetir3(DataGridView dataGridView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                /* string query = @"
                     SELECT Urunler.UrunAdi, Urunler.UrunKodu, Urunler.Kategori, 
                            UrunGiris.Tarih AS GirisTarihi, UrunGiris.LotNumarasi, SeriNumaralari.SeriNo
                     FROM Urunler
                     INNER JOIN UrunGiris ON Urunler.UrunID = UrunGiris.UrunID
                     INNER JOIN SeriNumaralari ON UrunGiris.GirisID = SeriNumaralari.GirisID
                     WHERE SeriNumaralari.CikisID IS NULL

                     EXCEPT

                     SELECT Urunler.UrunAdi, Urunler.UrunKodu, Urunler.Kategori, 
                            UrunCikis.CikisTarihi AS CikisTarihi, UrunCikis.LotNumarasi, SeriNumaralari.SeriNo
                     FROM Urunler
                     INNER JOIN UrunCikis ON Urunler.UrunID = UrunCikis.UrunID
                     INNER JOIN SeriNumaralari ON UrunCikis.CikisID = SeriNumaralari.CikisID
                 ";*/
                string query = @"
    SELECT 
        Urunler.UrunKodu AS UrunKodu,
        Urunler.UrunAdi AS UrunAdi,
        CAST(ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) AS INT) AS ToplamGirisMiktari,
        CAST(ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0) AS INT) AS ToplamCikisMiktari,
        CAST((ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) - ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0)) AS INT) AS StokMiktari,
        STUFF((
            SELECT ', ' + SN.SeriNo
            FROM SeriNumaralari SN
            WHERE SN.UrunID = Urunler.UrunID
            FOR XML PATH('')), 1, 2, '') AS SeriNumaralari,
        STUFF((
            SELECT ', ' + L.LotNumarasi
            FROM (SELECT LotNumarasi FROM UrunGiris WHERE UrunID = Urunler.UrunID
                  UNION ALL
                  SELECT LotNumarasi FROM UrunCikis WHERE UrunID = Urunler.UrunID) AS L
            FOR XML PATH('')), 1, 2, '') AS LotNumaralari
    FROM 
        Urunler
    LEFT JOIN 
        (SELECT UrunID, SUM(Miktar) AS ToplamGirisMiktari FROM UrunGiris GROUP BY UrunID) AS GirisMiktarlari
    ON 
        Urunler.UrunID = GirisMiktarlari.UrunID
    LEFT JOIN 
        (SELECT UrunID, SUM(Miktar) AS ToplamCikisMiktari FROM UrunCikis GROUP BY UrunID) AS CikisMiktarlari
    ON 
        Urunler.UrunID = CikisMiktarlari.UrunID;";
                // SQL sorgusu
                //        string query = @"
                //    WITH KalanUrunler AS (
                //        SELECT
                //            U.UrunID,
                //            U.UrunAdi,
                //            U.UrunKodu,
                //            ISNULL(SUM(G.Miktar), 0) - ISNULL(SUM(C.Miktar), 0) AS KalanMiktar
                //        FROM
                //            Urunler U
                //        LEFT JOIN
                //            UrunGiris G ON U.UrunID = G.UrunID
                //        LEFT JOIN
                //            UrunCikis C ON U.UrunID = C.UrunID
                //        GROUP BY
                //            U.UrunID, U.UrunAdi, U.UrunKodu
                //        HAVING
                //            ISNULL(SUM(G.Miktar), 0) - ISNULL(SUM(C.Miktar), 0) > 0
                //    )
                //    SELECT
                //        K.UrunID,
                //        K.UrunAdi,
                //        K.UrunKodu,
                //        K.KalanMiktar,
                //        (
                //            SELECT TOP 1 UG.LotNumarasi
                //            FROM UrunGiris UG 
                //            WHERE UG.UrunID = K.UrunID
                //        ) AS LotNumarasi,
                //        (
                //            SELECT TOP 1 SN.SeriNo
                //            FROM SeriNumaralari SN 
                //            WHERE SN.UrunID = K.UrunID
                //        ) AS SeriNo
                //    FROM
                //        KalanUrunler K;
                //";


                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
               // dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            }




        }
        //Anlık stok kontrolünü yapan method
        public void stokGuncelle(DataGridView dataGridView)
        {
            /*using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string stokQuery = @"
    SELECT 
        Urunler.UrunKodu AS UrunKodu,
        Urunler.UrunAdi AS UrunAdi,
        CAST(ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) AS DECIMAL(10, 3)) AS ToplamGirisMiktari,
        CAST(ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0) AS DECIMAL(10, 3)) AS ToplamCikisMiktari,
        CAST((ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) - ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0)) AS DECIMAL(10, 3)) AS StokMiktari
    FROM 
        Urunler
    LEFT JOIN 
        (SELECT UrunID, SUM(Miktar) AS ToplamGirisMiktari FROM UrunGiris GROUP BY UrunID) AS GirisMiktarlari
    ON 
        Urunler.UrunID = GirisMiktarlari.UrunID
    LEFT JOIN 
        (SELECT UrunID, SUM(Miktar) AS ToplamCikisMiktari FROM UrunCikis GROUP BY UrunID) AS CikisMiktarlari
    ON 
        Urunler.UrunID = CikisMiktarlari.UrunID";

                SqlCommand command = new SqlCommand(stokQuery, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable stokDataTable = new DataTable();
                stokDataTable.Load(reader);
                dataGridView.DataSource = stokDataTable;
                reader.Close();
            } */
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string stokQuery = @"
SELECT 
    Urunler.UrunKodu AS UrunKodu,
    Urunler.UrunAdi AS UrunAdi,
    CAST(ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) AS INT) AS ToplamGirisMiktari,
    CAST(ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0) AS INT) AS ToplamCikisMiktari,
    CAST((ISNULL(GirisMiktarlari.ToplamGirisMiktari, 0) - ISNULL(CikisMiktarlari.ToplamCikisMiktari, 0)) AS INT) AS StokMiktari
FROM 
    Urunler
LEFT JOIN 
    (SELECT UrunID, SUM(Miktar) AS ToplamGirisMiktari FROM UrunGiris GROUP BY UrunID) AS GirisMiktarlari
ON 
    Urunler.UrunID = GirisMiktarlari.UrunID
LEFT JOIN 
    (SELECT UrunID, SUM(Miktar) AS ToplamCikisMiktari FROM UrunCikis GROUP BY UrunID) AS CikisMiktarlari
ON 
    Urunler.UrunID = CikisMiktarlari.UrunID";

                SqlCommand command = new SqlCommand(stokQuery, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable stokDataTable = new DataTable();
                stokDataTable.Load(reader);
                dataGridView.DataSource = stokDataTable;
                dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView.Columns["StokMiktari"].HeaderText = "Stok Miktarı";
                dataGridView.Columns["ToplamGirisMiktari"].HeaderText = "Toplam Giriş Miktarı";
                dataGridView.Columns["ToplamCikisMiktari"].HeaderText = "Toplam Çıkış Miktarı";


                reader.Close();
               // dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }
        // Ürün çıkışlarının listesini DataGridView4 kontrolüne yükleyen metod
        public void UrunCikisListesiniGuncelle(DataGridView dataGridView)
        {

            /*  using (SqlConnection connection = new SqlConnection(connectionString))
              {
                  string urunCikisQuery = "SELECT Urunler.UrunKodu, UrunCikis.CikisTarihi AS Tarih, CONVERT(DECIMAL(10, 1), UrunCikis.Miktar) AS Miktar, Urunler.Kategori, UrunCikis.CikisNedeni, UrunCikis.Notlar, UrunCikis.LotNumarasi, SeriNumaralari.SeriNo " +
                                          "FROM Urunler " +
                                          "INNER JOIN UrunCikis ON Urunler.UrunID = UrunCikis.UrunID " +
                                          "LEFT JOIN SeriNumaralari ON UrunCikis.CikisID = SeriNumaralari.CikisID";
                  SqlDataAdapter urunCikisDataAdapter = new SqlDataAdapter(urunCikisQuery, connection);
                  DataTable urunCikisDataTable = new DataTable();
                  urunCikisDataAdapter.Fill(urunCikisDataTable);
                  dataGridView.DataSource = urunCikisDataTable;
                  // Sütun başlıklarını ayarla

                  dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                  dataGridView.Columns["Tarih"].HeaderText = "Tarih";
                  dataGridView.Columns["Miktar"].HeaderText = "Miktar";
                  dataGridView.Columns["Kategori"].HeaderText = "Kategori";
                  dataGridView.Columns["CikisNedeni"].HeaderText = "Çıkış Nedeni";
                  dataGridView.Columns["Notlar"].HeaderText = "Notlar";
                  dataGridView.Columns["LotNumarasi"].HeaderText = "Lot Numarası";
                  dataGridView.Columns["SeriNo"].HeaderText = "Seri Numarası";
              }*/
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string urunCikisQuery = "SELECT Urunler.UrunAdi, Urunler.UrunKodu, CONVERT(DECIMAL(10, 1), UrunCikis.Miktar) AS Miktar, UrunCikis.CikisTarihi, UrunCikis.Notlar, UrunCikis.LotNumarasi, SeriNumaralari.SeriNo " +
                                        "FROM Urunler " +
                                        "INNER JOIN UrunCikis ON Urunler.UrunID = UrunCikis.UrunID " +
                                        "LEFT JOIN SeriNumaralari ON UrunCikis.CikisID = SeriNumaralari.CikisID";
                SqlDataAdapter urunCikisDataAdapter = new SqlDataAdapter(urunCikisQuery, connection);
                DataTable urunCikisDataTable = new DataTable();
                urunCikisDataAdapter.Fill(urunCikisDataTable);
                dataGridView.DataSource = urunCikisDataTable;
                // Sütun başlıklarını ayarla
                dataGridView.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                dataGridView.Columns["Miktar"].HeaderText = "Miktar";
                dataGridView.Columns["CikisTarihi"].HeaderText = "Çıkış Tarihi";
                dataGridView.Columns["Notlar"].HeaderText = "Notlar";
                dataGridView.Columns["LotNumarasi"].HeaderText = "Lot Numarası";
                dataGridView.Columns["SeriNo"].HeaderText = "Seri Numarası";
              //  dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        // bu methodu kalan seri numaralarrı göstermek için kulanacam
        //daha sonra
        public void SeriNumaralariListesiniGuncelle(DataGridView dataGridView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT U.UrunAdi, U.UrunKodu, SN.LotNumarasi, SN.SeriNo
            FROM SeriNumaralari SN
            INNER JOIN Urunler U ON SN.UrunID = U.UrunID
            LEFT JOIN UrunCikis UC ON SN.CikisID = UC.CikisID
            WHERE UC.CikisID IS NULL";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
                // Diğer sütunları hizala
               
            }
        }
        public void GizliSeriNumaralariGuncelle(DataGridView dataGridView, string columnName)
        {
            // Hücrelerin bulunduğu sütunun dizinini al
            int columnIndex = dataGridView.Columns.Cast<DataGridViewColumn>()
                .FirstOrDefault(column => column.Name == columnName)?.Index ?? -1;

            if (columnIndex == -1)
            {
                // Hata: Verilen sütun adı bulunamadı
                return;
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                // Belirtilen sütun adındaki hücreleri al
                DataGridViewCell cell = row.Cells[columnName];

                if (cell.Value != null)
                {
                    // Hücredeki değeri virgüllerle ayırarak diziye dönüştür
                    string[] degerler = cell.Value.ToString().Split(',').Select(s => s.Trim()).ToArray();

                    // Tekrar eden değerleri filtrelemek için bir HashSet kullan
                    HashSet<string> geciciSet = new HashSet<string>();

                    // Filtrelenmiş değerleri tutacak yeni bir liste oluştur
                    List<string> yeniDegerler = new List<string>();

                    foreach (string deger in degerler)
                    {
                        // Tekrar eden değerleri filtrele
                        if (geciciSet.Add(deger)) // Ekleme başarılıysa (yani eklendiyse), deger benzersizdir
                        {
                            yeniDegerler.Add(deger);
                        }
                        else
                        {
                            // Eğer değer zaten daha önce eklendiyse, onu kaldır
                            yeniDegerler.Remove(deger);
                        }
                    }

                    // Yeni değerleri hücreye ayarla
                    cell.Value = string.Join(", ", yeniDegerler);
                }
                dataGridView.Columns[0].Visible = false;//urun ıdler gözükmesin diye yaptım...
                // Diğer sütunları hizala
                //   dataGridView.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

    }
}
