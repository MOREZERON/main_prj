using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using Microsoft.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.Data;
using Microsoft.Win32;


namespace main_prj
{
    /// <summary>
    /// Interaction logic for ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
            LoadApprovedProductionRuns();
        }

        private void LoadApprovedProductionRuns()
        {
            string query = @"
        SELECT pr.BatchNumber, pr.QuantityProduced, pr.ProducedAt, qc.QualityRating
        FROM QualityChecks qc
        JOIN ProductionRuns pr ON qc.RunID = pr.RunID
        WHERE qc.Resolution = 'Готово к отправке'";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                try
                {
                    adapter.Fill(table);
                    ApprovedRunsGrid.ItemsSource = table.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
            }
        }

        private void ExportToExcel(DataTable table, string filePath)
        {
            if (table.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта");
                return;
            }

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Готовые партии");

                    // Заголовки
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = table.Columns[i].ColumnName;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    // Данные
                    int row = 2;
                    foreach (DataRow dataRow in table.Rows)
                    {
                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            var value = dataRow[col]?.ToString() ?? "";
                            worksheet.Cell(row, col + 1).Value = value;
                        }
                        row++;
                    }

                    // Автоширина колонок
                    worksheet.Columns().AdjustToContents();

                    // Сохранение файла
                    workbook.SaveAs(filePath);
                }

                MessageBox.Show("Файл успешно сохранён!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте: " + ex.Message);
            }
        }

        private DataTable GetApprovedProductionRuns()
        {
            string query = @"
        SELECT pr.BatchNumber, pr.QuantityProduced, pr.ProducedAt, qc.QualityRating, qc.Resolution
        FROM QualityChecks qc
        JOIN ProductionRuns pr ON qc.RunID = pr.RunID
        WHERE qc.Resolution = 'Готово к отправке'";

            var table = new DataTable();

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                try
                {
                    adapter.Fill(table);
                    ApprovedRunsGrid.ItemsSource = table.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
            }

            return table;
        }

        private void BtnAddQualityCheck_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddQualityCheckWindow();

            if (addWindow.ShowDialog() == true)
            {
                LoadApprovedProductionRuns();
            }
        }

        private void BtnDefectsReport_Click(object sender, RoutedEventArgs e)
        {
            var table = GetApprovedProductionRuns();

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel файл (*.xlsx)|*.xlsx",
                FileName = "Готовые_партии_" + DateTime.Now.ToString("yyyy-MM-dd"),
                DefaultExt = ".xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                ExportToExcel(table, dialog.FileName);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
