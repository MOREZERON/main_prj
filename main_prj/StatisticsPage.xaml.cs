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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Data.SqlClient;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace main_prj
{
    /// <summary>
    /// Interaction logic for StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : System.Windows.Controls.Page
    {
        public PlotModel MonthlyPlotModel { get; set; }


        public StatisticsPage()
        {
            InitializeComponent();
            LoadMonthlyDefects();
        }

        private void LoadMonthlyDefects()
        {
            var data = new Dictionary<string, int>();

            string query = @"
                SELECT 
                    FORMAT(pr.ProducedAt, 'yyyy-MM') AS Month,
                    SUM(pr.QuantityRejected) AS TotalRejected
                FROM ProductionRuns pr
                GROUP BY FORMAT(pr.ProducedAt, 'yyyy-MM')
                ORDER BY Month DESC";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string month = reader["Month"].ToString();
                        int rejected = Convert.ToInt32(reader["TotalRejected"]);
                        data[month] = rejected;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                    return;
                }
            }

            if (data.Count == 0)
            {
                MessageBox.Show("Нет данных для отображения");
                return;
            }

            // Переворачиваем данные, чтобы месяцы шли слева направо
            var sortedData = data.Reverse().ToDictionary(x => x.Key, x => x.Value);

            // Привязываем данные к графику
            MonthlyDefectsChart.DataContext = sortedData.Select(kvp => new KeyValuePair<string, int>(kvp.Key, kvp.Value));
        }



        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadMonthlyDefects();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
