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
using System.Windows.Shapes;
using System.Data;
using Microsoft.Data.SqlClient;

namespace main_prj
{
    /// <summary>
    /// Interaction logic for ProductionRunsPage.xaml
    /// </summary>
    public partial class ProductionRunsPage : Page
    {
        public ProductionRunsPage()
        {
            InitializeComponent();
            LoadProductionRuns();
        }


        public void LoadProductionRuns()
        {
            string query = @"
                SELECT 
                    pr.RunID, 
                    p.Name AS ProductName,
                    pr.BatchNumber,
                    pr.QuantityProduced,
                    pr.QuantityAccepted,
                    pr.QuantityRejected,
                    ISNULL(d.Name, 'Нет') AS DefectName,
                    pr.ProducedAt
                FROM ProductionRuns pr
                JOIN Products p ON pr.ProductID = p.ProductID
                LEFT JOIN Defects d ON pr.DefectID = d.DefectID";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                try
                {
                    adapter.Fill(table);
                    ProductionRunsGrid.ItemsSource = table.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
            }
        }

        private void BtnAddProductionRun_Click(object sender, RoutedEventArgs e)
        {
            AddProductRunsWindow addProductRunsWindow = new AddProductRunsWindow();
            addProductRunsWindow.ShowDialog();
        }

        private void BtnAddDefect_Click(object sender, RoutedEventArgs e)
        {
            AddDefectPage addDefectPage = new AddDefectPage();
            addDefectPage.Show();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductPage addProductPage = new AddProductPage();
            addProductPage.Show();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProductionRuns();
        }
    }
}
