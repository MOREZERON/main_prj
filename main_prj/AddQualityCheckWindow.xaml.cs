using Microsoft.Data.SqlClient;
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
using System.Data.SqlClient;
using System.Data;
using Microsoft.IdentityModel.Tokens;

namespace main_prj
{
    /// <summary>
    /// Interaction logic for AddQualityCheckWindow.xaml
    /// </summary>
    public partial class AddQualityCheckWindow : Window
    {
        private string _resolution = null;
        public AddQualityCheckWindow()
        {
            InitializeComponent();
            LoadProductionRuns();
            InitializeQualityRatings();
        }

        private void LoadProductionRuns()
        {
            string query = "SELECT RunID, BatchNumber FROM ProductionRuns";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                CbProductionRun.ItemsSource = table.DefaultView;
                CbProductionRun.DisplayMemberPath = "BatchNumber";
                CbProductionRun.SelectedValuePath = "RunID";
            }
        }

        private void InitializeQualityRatings()
        {
            var ratings = new List<string> { "Отлично", "Удовлетворительно", "Ужасно" };
            CbQualityRating.ItemsSource = ratings;
            CbQualityRating.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CbProductionRun.SelectedValue == null ||
                CbQualityRating.SelectedItem == null ||
                string.IsNullOrEmpty(_resolution))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            int runId = (int)CbProductionRun.SelectedValue;
            string rating = CbQualityRating.SelectedItem.ToString();

            // Убедимся, что _resolution корректен
            if (!new[] { "Готово к отправке", "На доработке" }.Contains(_resolution))
            {
                MessageBox.Show("Неверное значение для Resolution");
                return;
            }

            string insertQuery = @"
        INSERT INTO QualityChecks (RunID, CheckedAt, QualityRating, Resolution)
        VALUES (@runId, GETDATE(), @rating, @resolution)";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@runId", runId);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@resolution", _resolution);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Проверка качества успешно добавлена");
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
                }
            }
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            _resolution = "На доработке";

            BtnApprove.Background = System.Windows.Media.Brushes.LightGray;
            BtnReject.Background = System.Windows.Media.Brushes.Red;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void CbProductionRun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbProductionRun.SelectedItem is DataRowView selectedRow)
            {
                int runId = Convert.ToInt32(selectedRow["RunID"]); // Убедимся, что это число

                // Можно добавить проверку
                if (!int.TryParse(selectedRow["RunID"].ToString(), out _))
                {
                    MessageBox.Show("Ошибка: Неверный формат RunID");
                    return;
                }

                // Дополнительная логика
                MessageBox.Show($"Выбрана партия с ID: {runId}");
            }
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            _resolution = "Готово к отправке";

            BtnApprove.Background = System.Windows.Media.Brushes.Green;
            BtnReject.Background = System.Windows.Media.Brushes.LightGray;
        }
    }
}
