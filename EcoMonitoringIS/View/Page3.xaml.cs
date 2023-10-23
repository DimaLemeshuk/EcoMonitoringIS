using EcoMonitoringIS.View.Page3Frame;
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

namespace EcoMonitoringIS.View
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            MainFrame.Content = new EnterprisesFrame();
            ChooseT.Text = EnterprisesTable.Content.ToString();
        }

        private void EnterprisesTable_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EnterprisesFrame();
            ChooseT.Text = ((Button)sender).Content.ToString();
        }

        private void pollution_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PollutionsFrame();
            ChooseT.Text = ((Button)sender).Content.ToString();
        }

        private void pollutant_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PollutantsFrame();
            ChooseT.Text = ((Button)sender).Content.ToString();
        }

        private void belonging_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new BelongingsFrame();
            ChooseT.Text = ((Button)sender).Content.ToString();
        }

    }
}
