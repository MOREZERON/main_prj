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
    /// Interaction logic for RegisterUserPage.xaml
    /// </summary>
    public partial class RegisterUserPage : Page
    {
        private List<string> Roles = new List<string> { "Admin", "Storekeeper", "OTC" };

        public RegisterUserPage()
        {
            InitializeComponent();
            CbRole.ItemsSource = Roles;
            CbRole.SelectedIndex = 0;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = PwbPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            string selectedRole = CbRole.SelectedItem as string;

            string query = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", selectedRole);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Пользователь зарегистрирован");
                    NavigationService.GoBack(); // Возврат на предыдущую страницу
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Ошибка регистрации: " + ex.Message);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
