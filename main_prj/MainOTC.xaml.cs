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
    /// Interaction logic for MainOTC.xaml
    /// </summary>
    public partial class MainOTC : Page
    {
        public MainOTC()
        {
            InitializeComponent();
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу статистики
            NavigationService.Navigate(new StatisticsPage());
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу отчётов
            NavigationService.Navigate(new ReportsPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
