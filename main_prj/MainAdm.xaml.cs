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
    /// Interaction logic for MainAdm.xaml
    /// </summary>
    public partial class MainAdm : Page
    {
        public MainAdm()
        {
            InitializeComponent();
        }

        private void BtnRegisterUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterUserPage());
        }

        // Переход к статистике
        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StatisticsPage());
        }

        // Переход к отчётам
        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportsPage());
        }

        // Назад (например, на форму логина)
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage();
            Window.GetWindow(this).Close(); // или используй NavigationService
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DeleteUserPage());
        }
    }
}
