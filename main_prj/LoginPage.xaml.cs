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
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using main_prj.models;

namespace main_prj
{
    

    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
       
        private string connectionString = Con.srv;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = PwbPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            var user = AuthenticateUser(username, password);

            if (user != null)
            {
                switch (user.Role)
                {
                    case "Admin":
                        Manager.MainFrame.Navigate(new MainAdm());
                        break;

                    case "Storekeeper":
                        Manager.MainFrame.Navigate(new MainSto());
                        break;

                    case "OTC":
                        Manager.MainFrame.Navigate(new MainOTC());
                        break;

                    default:
                        MessageBox.Show("Неизвестная роль");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        
        private User AuthenticateUser(string username, string password)
        {
            string query = @"
        SELECT UserID, Username, Password, Role
        FROM Users
        WHERE Username = @username AND Password = @password";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new User
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
                }
            }

            return null;
        }

    }
}
