using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CokEglencez
{
    public class ComboBoxDoldurucu
    {

        //Kategori doldurmamıza yarayan clasımız 
        public static void KategorileriDoldur(ComboBox cmbBoxKat)
        {

         //   cmbBoxKat.Items.Add("Takımlar");
            cmbBoxKat.Items.Add("Mekanik");
            cmbBoxKat.Items.Add("Optik");
            cmbBoxKat.Items.Add("Elektronik Kart");
            cmbBoxKat.Items.Add("Kablaj");
            cmbBoxKat.Items.Add("Satın Alınanlar");
            cmbBoxKat.Items.Add("Civata-Pul-Rondela-Pabuç");
            cmbBoxKat.Items.Add("Sarf Malzeme");
            cmbBoxKat.Items.Add("Yari Mamül");
            cmbBoxKat.Items.Add("Mamül");
            cmbBoxKat.SelectedIndex = 0;
        }
        public static void KategorileriDoldur2(ComboBox cmbBoxKat)
        {
            //GÜnceleyicinin combobox'ı.

            cmbBoxKat.Items.Add("Takımlar");
            cmbBoxKat.Items.Add("Mekanik");
            cmbBoxKat.Items.Add("Optik");
            cmbBoxKat.Items.Add("Elektronik Kart");
            cmbBoxKat.Items.Add("Kablaj");
            cmbBoxKat.Items.Add("Satın Alınanlar");
            cmbBoxKat.Items.Add("Civata-Pul-Rondela-Pabuç");
            cmbBoxKat.Items.Add("Sarf Malzeme");
        
        }
        public static void KategoriDoldur3(ComboBox comboBoxBomOlustur)
        {
            //bom comboxı
            comboBoxBomOlustur.Items.Add("Genel Takımlar");
            comboBoxBomOlustur.Items.Add("Elektronik Takımlar");
            comboBoxBomOlustur.Items.Add("Optik Takımlar");
            comboBoxBomOlustur.Items.Add("Mekanik Takımlar");
            comboBoxBomOlustur.Items.Add("Pil Yuvası Takımı");
            comboBoxBomOlustur.Items.Add("Tuş Takımı Kartı");
            comboBoxBomOlustur.Items.Add("Oled Olightek Sürücü Kartı");
            comboBoxBomOlustur.Items.Add("Tuş Takımı Kablajı");
            comboBoxBomOlustur.Items.Add("Dış Dünya Konektör Kablajı");
            comboBoxBomOlustur.Items.Add("Oled Kablajı");
            comboBoxBomOlustur.Items.Add("Lazer Kablajı");
            comboBoxBomOlustur.SelectedIndex = 0;
        }
        public static void CikisKategorileriDoldur(ComboBox cmbCikisKategori)
        {
            cmbCikisKategori.Items.Add("Mini-TSD");
            cmbCikisKategori.Items.Add("AKILLI-TSD");
            cmbCikisKategori.Items.Add("DVS-40");
            cmbCikisKategori.Items.Add("DVS-120");
            cmbCikisKategori.Items.Add("EOSS");
            cmbCikisKategori.Items.Add("AIR LP12");
            cmbCikisKategori.Items.Add("AIR LP17");
            cmbCikisKategori.Items.Add("AIR LP35");
            cmbCikisKategori.Items.Add("AIR lP");
            cmbCikisKategori.SelectedIndex = 0;
        }

        public static void CikisNedenleriDoldur(ComboBox cmbCikisNedeni)
        {
            cmbCikisNedeni.Items.Add("RTV(İade)");
            cmbCikisNedeni.Items.Add("Teslim Edildi.");
            cmbCikisNedeni.Items.Add("Ödünç Verildi");
            cmbCikisNedeni.Items.Add("Ar-ge Kulanıldı");
            cmbCikisNedeni.Items.Add("B/O kullanıldı.");
            cmbCikisNedeni.Items.Add("Fire Hurda");
            cmbCikisNedeni.Items.Add("İş emrinde kullanıldı");
            cmbCikisNedeni.SelectedIndex = 0;
        }

        public static void SonUrunDoldur(ComboBox comboBoxTamKategori)
        {
            comboBoxTamKategori.Items.Add("Mini-TSD");
            comboBoxTamKategori.Items.Add("AKILLI-TSD");
            comboBoxTamKategori.Items.Add("DVS-40");
            comboBoxTamKategori.Items.Add("DVS-120");
            comboBoxTamKategori.Items.Add("EOSS");
            comboBoxTamKategori.Items.Add("AIR LP12");
            comboBoxTamKategori.Items.Add("AIR LP17");
            comboBoxTamKategori.Items.Add("AIR LP35");
            comboBoxTamKategori.Items.Add("AIR lP");
            comboBoxTamKategori.SelectedIndex = 0;

        }
    }
}
