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
using System.Data;

namespace main_prj
{
    /// <summary>
    /// Interaction logic for AddRejectionWindow.xaml
    /// </summary>
    public partial class AddRejectionWindow : Window
    {
        private string connectionString = "Server=MOREZERON;Database=main_db;Trusted_Connection=True;Encrypt=False;";
        public AddRejectionWindow()
        {
            InitializeComponent();
            LoadProductionRuns();
            LoadDefects();
        }

        private void LoadProductionRuns()
        {
            string query = "SELECT RunID, BatchNumber FROM ProductionRuns";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                cbProductionRun.ItemsSource = table.DefaultView;
                cbProductionRun.DisplayMemberPath = "BatchNumber";
                cbProductionRun.SelectedValuePath = "RunID";
            }
        }

        private void LoadDefects()
        {
            string query = "SELECT DefectID, Name FROM Defects";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                cbDefect.ItemsSource = table.DefaultView;
                cbDefect.DisplayMemberPath = "Name";
                cbDefect.SelectedValuePath = "DefectID";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbProductionRun.SelectedValue == null || cbDefect.SelectedValue == null || string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            int runId = (int)cbProductionRun.SelectedValue;
            int defectId = (int)cbDefect.SelectedValue;
            int quantity = int.Parse(txtQuantity.Text);

            string query = @"
                INSERT INTO Rejections (RunID, DefectID, Quantity, Comment)
                VALUES (@runId, @defectId, @quantity, '')";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@runId", runId);
                cmd.Parameters.AddWithValue("@defectId", defectId);
                cmd.Parameters.AddWithValue("@quantity", quantity);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Брак успешно добавлен!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
