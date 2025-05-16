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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void LoadProducts_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new context.AppDbContext())
            {
                var products = db.Products.ToList();

            }
        }

        private void BtnProductionRuns_Click(object sender, RoutedEventArgs e)
        {

            Manager.MainFrame.Navigate(new ProductionRunsPage());

            
        }

        private void BtnRejections_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RejectionsPage());
        }

        /* private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
         {
             var result = MessageBox.Show("Вы действительно хотите закрыть приложение?",
                                          "Подтверждение выхода",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

             if (result == MessageBoxResult.No)
             {
                 e.Cancel = true; // Отменяем закрытие
             }
         }*/

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ReportsPage());
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new StatisticsPage());
        }
    }
}
