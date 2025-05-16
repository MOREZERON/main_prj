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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace main_prj
{
    /// <summary>
    /// Interaction logic for DeleteUserPage.xaml
    /// </summary>
    public partial class DeleteUserPage : Page
    {
        private string selectedUsername = null;
        public DeleteUserPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            string query = "SELECT Username, Role FROM Users";

            using (SqlConnection conn = new SqlConnection(Con.srv))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                UsersGrid.ItemsSource = table.DefaultView;
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is DataRowView row)
            {
                string username = row["Username"].ToString();

                // Запрет админ
                if (username == "admin")
                {
                    MessageBox.Show("Невозможно удалить администратора");
                    return;
                }

                var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {username}?",
                                            "Подтверждение удаления",
                                            MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    string query = "DELETE FROM Users WHERE Username = @username";

                    using (SqlConnection conn = new SqlConnection(Con.srv))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        try
                        {
                            conn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Пользователь успешно удалён");
                                LoadUsers(); // Обновляем список
                            }
                            else
                            {
                                MessageBox.Show("Ошибка при удалении");
                            }
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("Ошибка базы данных: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя из списка");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
