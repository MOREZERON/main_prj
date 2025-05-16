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
    /// Interaction logic for MainSto.xaml
    /// </summary>
    public partial class MainSto : Page
    {
        public MainSto()
        {
            InitializeComponent();
        }

        private void BtnProductionRuns_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductionRunsPage());
        }
    }
}
