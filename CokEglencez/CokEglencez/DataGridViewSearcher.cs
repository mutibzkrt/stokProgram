using System;
using System.Linq;
using System.Windows.Forms;

public class DataGridViewSearcher
{
    public void Search(DataGridView dataGridView, object searchValue)
    {
        try
        {
            string searchText = searchValue.ToString().ToLower(); // Arama değerini stringe çevir ve küçük harfe dönüştür

            if (string.IsNullOrWhiteSpace(searchText))
            {
                ShowAllRows(dataGridView); // Arama değeri boşsa tüm satırları göster
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        bool found = row.Cells.Cast<DataGridViewCell>()
                                                .Any(cell => cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText));
                        row.Visible = found;
                        if (found)
                        {
                            row.Selected = true;
                            dataGridView.FirstDisplayedScrollingRowIndex = row.Index;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void ShowAllRows(DataGridView dataGridView)
    {
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            if (!row.IsNewRow)
            {
                row.Visible = true;
            }
        }
    }
}
