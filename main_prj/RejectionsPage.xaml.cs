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
    /// Interaction logic for RejectionsPage.xaml
    /// </summary>
    public partial class RejectionsPage : Page
    {
        public RejectionsPage()
        {
            InitializeComponent();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddRejection_Click(object sender, RoutedEventArgs e)
        {
            AddRejectionWindow addRejection = new AddRejectionWindow();
            addRejection.Show();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

    }
}
