using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

public class CiktiAlmaSinifi
{
    private DataGridView dataGridViewToPrint;

    public void Yazdir(DataGridView dataGridView)
    {
        try
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // Yazdırılacak DataGridView'i kaydet
                dataGridViewToPrint = dataGridView;

                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings = printDialog.PrinterSettings;

                // Yazdırma işlemi için PrintDocument nesnesine PrintPage olayı atanır
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

                // Yazdırma işlemi başlatılır
                pd.Print();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Çıktı alınırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void pd_PrintPage(object sender, PrintPageEventArgs e)
    {
        // Yazdırılacak DataGridView'in resmini al
        Bitmap bm = new Bitmap(dataGridViewToPrint.Width, dataGridViewToPrint.Height);
        dataGridViewToPrint.DrawToBitmap(bm, new Rectangle(0, 0, dataGridViewToPrint.Width, dataGridViewToPrint.Height));

        // Resmi yazdır
        e.Graphics.DrawImage(bm, 0, 0);
    }
}
