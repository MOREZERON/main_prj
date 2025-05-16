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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace main_prj
{
    /// <summary>
    /// Interaction logic for AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Window
    {
        public AddProductPage()
        {
            InitializeComponent();
        }

        private void BtnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtProductName.Text.Trim();
            string description = TxtProductDescription.Text.Trim();
            string country = CbManufacturerCountry.SelectedItem is ComboBoxItem item ? item.Content.ToString() : null;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(country))
            {
                MessageBox.Show("Название и страна производителя обязательны");
                return;
            }

            string insertQuery = @"
        INSERT INTO Products (Name, Description, ManufacturerCountry)
        VALUES (@name, @description, @country)";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description ?? "");
                cmd.Parameters.AddWithValue("@country", country);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Продукт успешно добавлен");

                    // Очистка полей
                    TxtProductName.Text = "";
                    TxtProductDescription.Text = "";
                    CbManufacturerCountry.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления продукта: " + ex.Message);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
