using EcoMonitoringIS.Classes;
using EcoMonitoringIS.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for Page6.xaml
    /// </summary>
    public partial class Page6 : Page
    {
        public Page6()
        {
            InitializeComponent();

            Tax.reload();
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {            

                DBGrid1.ItemsSource = context.Taxes
                .GroupBy(tax => tax.Pollution.Enterprise.Name)
                .Select(group => new
                {
                    EnterpriseName = group.Key,
                    TotalTaxUAH = group.Sum(tax => tax.TaxUAH)
                })
                .ToList();
                DBGridControl.DelOllColumn(DBGrid1);
                DBGridControl.AddColumn(DBGrid1, "Назва об'єкту", "EnterpriseName");
                DBGridControl.AddColumn(DBGrid1, "Загальний податок\n(грн)", "TotalTaxUAH");

                DBGrid2.ItemsSource = context.Taxes
                .Include(p => p.Pollution)
                .Include(p => p.Pollution.Enterprise)
                .Include(p => p.Pollution.Pollutant)
                .ToList();
                DBGridControl.DelOllColumn(DBGrid2);          
                DBGridControl.AddColumn(DBGrid2, "id", "Idtax");
                DBGridControl.AddColumn(DBGrid2, "Назва об'єкту", "Pollution.Enterprise.Name");
                DBGridControl.AddColumn(DBGrid2, "Забрудник", "Pollution.Pollutant.Name");
                DBGridControl.AddColumn(DBGrid2, "Ставки податку\n(грн/т)", "Rate");
                DBGridControl.AddColumn(DBGrid2, "Сума податку\n(грн)", "TaxUAH");

            }
        }

        private void RefreshButton1_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGrid1.ItemsSource = context.Taxes
                .GroupBy(tax => tax.Pollution.Enterprise.Name)
                .Select(group => new
                {
                    EnterpriseName = group.Key,
                    TotalTaxUAH = group.Sum(tax => tax.TaxUAH)
                })
                .ToList();
            }
        }

        private void RefreshButton2_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGrid2.ItemsSource = context.Taxes
                .Include(p => p.Pollution)
                .Include(p => p.Pollution.Enterprise)
                .Include(p => p.Pollution.Pollutant)
                .ToList();
            }
        }
    }
}
