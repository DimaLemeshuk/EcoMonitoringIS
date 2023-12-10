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
            PrintAllTaxes();
            PrintTaxes();

        }

        private void RefreshButton1_Click(object sender, RoutedEventArgs e)
        {
            if (Year_Name_Button.Content.Equals("Групувати за роком"))
            {
                PrintAllTaxes();
            }
            else if (Year_Name_Button.Content.Equals("Групувати по підприємствах"))
            {
                PrintYearTaxes();
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

        private void Year_Name_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Year_Name_Button.Content.Equals("Групувати за роком"))
            {
                PrintYearTaxes();               
                Year_Name_Button.Content = "Групувати по підприємствах";
            }
            else if (Year_Name_Button.Content.Equals("Групувати по підприємствах")) 
            {
                PrintAllTaxes();
                Year_Name_Button.Content = "Групувати за роком";
            }
        }

        private void PrintYearTaxes()
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {

                DBGrid1.ItemsSource = context.Taxes
                .GroupBy(tax => new { tax.Pollution.Enterprise.Name, tax.Pollution.Year })
                .Select(group => new
                {
                    EnterpriseName = group.Key.Name,
                    Year = group.Key.Year,
                    TotalTaxUAH = group.Sum(tax => tax.TaxUAH)
                })
                .ToList();

                DBGridControl.DelOllColumn(DBGrid1);
                DBGridControl.AddColumn(DBGrid1, "Назва об'єкту", "EnterpriseName");
                DBGridControl.AddColumn(DBGrid1, "Рік", "Year");
                DBGridControl.AddColumn(DBGrid1, "Загальний податок\n(грн)", "TotalTaxUAH", true, "#0.##");

            }
        }

        private void PrintAllTaxes()
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
                DBGridControl.DelOllColumn(DBGrid1);
                DBGridControl.AddColumn(DBGrid1, "Назва об'єкту", "EnterpriseName");
                DBGridControl.AddColumn(DBGrid1, "Загальний податок\n(грн)", "TotalTaxUAH", true, "#0.##");

            }
        }

        private void PrintTaxes()
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {

                DBGrid2.ItemsSource = context.Taxes
                .Include(p => p.Pollution)
                .Include(p => p.Pollution.Enterprise)
                .Include(p => p.Pollution.Pollutant)
                .ToList();
                DBGridControl.DelOllColumn(DBGrid2);
                DBGridControl.AddColumn(DBGrid2, "id", "Idtax");
                DBGridControl.AddColumn(DBGrid2, "Назва об'єкту", "Pollution.Enterprise.Name");
                DBGridControl.AddColumn(DBGrid2, "Забрудник", "Pollution.Pollutant.Name");
                DBGridControl.AddColumn(DBGrid2, "id забрудника", "Pollution.Idpollution");
                DBGridControl.AddColumn(DBGrid2, "Ставки податку\n(грн/т)", "Rate", true, "#0.##");
                DBGridControl.AddColumn(DBGrid2, "Сума податку\n(грн)", "TaxUAH", true, "#0.##");

            }
        }
    }
}
