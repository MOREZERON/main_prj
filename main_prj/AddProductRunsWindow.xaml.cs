using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace main_prj
{
    /// <summary>
    /// Interaction logic for AddProductRunsWindow.xaml
    /// </summary>
    public partial class AddProductRunsWindow : Window
    {
        public AddProductRunsWindow()
        {
            InitializeComponent();
            LoadProducts();
            LoadDefects();
            DpProducedAt.SelectedDate = DateTime.Now;
        }
        private void TxtQuantity_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void LoadProducts()
        {
            string query = "SELECT ProductID, Name FROM Products";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                CbProduct.ItemsSource = table.DefaultView;
            }
        }

        /// <summary>
        /// Загружает список дефектов
        /// </summary>
        private void LoadDefects()
        {
            string query = "SELECT DefectID, Name FROM Defects";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                // Добавляем опцию "Нет дефекта"
                var newRow = table.NewRow();
                newRow["DefectID"] = DBNull.Value;
                newRow["Name"] = "Нет дефекта";
                table.Rows.InsertAt(newRow, 0);

                CbDefect.ItemsSource = table.DefaultView;
                CbDefect.DisplayMemberPath = "Name";
                CbDefect.SelectedValuePath = "DefectID";
                CbDefect.SelectedIndex = 0;
            }
        }



       

        /// <summary>
        /// Обновляет количество принятых изделий при изменении полей
        /// </summary>
        private void TxtQuantityProduced_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateQuantityAccepted();
        }

        /// <summary>
        /// Обновляет количество принятых при изменении брака
        /// </summary>
        private void TxtQuantityRejected_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateQuantityAccepted();
        }

        /// <summary>
        /// Включает/выключает выбор дефекта в зависимости от количества брака
        /// </summary>
        private void UpdateQuantityAccepted()
        {
            if (!int.TryParse(TxtQuantityProduced.Text, out int produced))
                return;

            int rejected = int.TryParse(TxtQuantityRejected.Text, out int r) ? r : 0;

            if (produced >= rejected && produced > 0)
            {
                TxtQuantityAccepted.Text = (produced - rejected).ToString();
                CbDefect.IsEnabled = rejected > 0;
            }
            else
            {
                MessageBox.Show("Брак не может быть больше общего количества");
                TxtQuantityRejected.Text = "0";
            }
        }

        

        

        /// <summary>
        /// Обработчик нажатия на кнопку "Сохранить"
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CbProduct.SelectedItem == null ||
                string.IsNullOrWhiteSpace(TxtBatchNumber.Text) ||
                !int.TryParse(TxtQuantityProduced.Text, out int quantityProduced) ||
                !int.TryParse(TxtQuantityRejected.Text, out int quantityRejected))
            {
                MessageBox.Show("Заполните все поля корректно");
                return;
            }

            var productRow = (CbProduct.SelectedItem as DataRowView);
            int productId = (int)productRow["ProductID"];
            string batchNumber = TxtBatchNumber.Text.Trim();
            int quantityAccepted = quantityProduced - quantityRejected;
            object defectId = CbDefect.SelectedValue ?? DBNull.Value;
            DateTime producedAt = DpProducedAt.SelectedDate ?? DateTime.Now;

            string insertQuery = @"
                INSERT INTO ProductionRuns 
                (ProductID, BatchNumber, QuantityProduced, QuantityAccepted, QuantityRejected, DefectID, ProducedAt)
                VALUES 
                (@productId, @batchNumber, @quantityProduced, @quantityAccepted, @quantityRejected, @defectId, @producedAt)";

            try
            {
                using (SqlConnection conn = new SqlConnection(Con.srv))
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@batchNumber", batchNumber);
                    cmd.Parameters.AddWithValue("@quantityProduced", quantityProduced);
                    cmd.Parameters.AddWithValue("@quantityAccepted", quantityAccepted);
                    cmd.Parameters.AddWithValue("@quantityRejected", quantityRejected);
                    cmd.Parameters.AddWithValue("@defectId", defectId);
                    cmd.Parameters.AddWithValue("@producedAt", producedAt);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Партия успешно добавлена");

                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении партии: " + ex.Message);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Отмена"
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}