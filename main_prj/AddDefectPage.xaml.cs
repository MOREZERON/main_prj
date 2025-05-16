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
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;

namespace main_prj
{
    /// <summary>
    /// Interaction logic for AddDefectPage.xaml
    /// </summary>
    public partial class AddDefectPage : Window
    {
        public AddDefectPage()
        {
            InitializeComponent();
            LoadDefects();
        }

        private void LoadDefects()
        {
            string query = "SELECT DefectID, Name, Description FROM Defects";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                DefectsGrid.ItemsSource = table.DefaultView;
            }
        }

        private void BtnAddDefect_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtDefectName.Text.Trim();
            string description = TxtDefectDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название дефекта");
                return;
            }

            string insertQuery = @"
                INSERT INTO Defects (Name, Description) 
                VALUES (@name, @description)";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description ?? "");

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Дефект успешно добавлен");

                    // Очистка полей и обновление грида
                    TxtDefectName.Text = "";
                    TxtDefectDescription.Text = "";
                    LoadDefects();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении дефекта: " + ex.Message);
                }
            }
        }
        private void TxtDefectName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtDefectName.Text == "Название дефекта")
                TxtDefectName.Text = "";
        }

        private void TxtDefectName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDefectName.Text))
                TxtDefectName.Text = "Название дефекта";
        }

        private void TxtDefectDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtDefectDescription.Text == "Описание дефекта (необязательно)")
                TxtDefectDescription.Text = "";
        }

        private void TxtDefectDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDefectDescription.Text))
                TxtDefectDescription.Text = "Описание дефекта (необязательно)";
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
