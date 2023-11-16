using EcoMonitoringIS.Classes;
using EcoMonitoringIS.Models;
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
using Microsoft.EntityFrameworkCore;
//using Loss = EcoMonitoringIS.Models.Loss;

namespace EcoMonitoringIS.View
{
    /// <summary>
    /// Interaction logic for Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        public Page5()
        {
            InitializeComponent();
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid2);
                DBGrid2.ItemsSource = context.Losses.ToList();
                DBGridControl.AddColumn(DBGrid2, "id", "idLoss", true);
                DBGridControl.AddColumn(DBGrid2, "розмір збитків\nгрн", "LossUAH", true);
                DBGridControl.AddColumn(DBGrid2, "Маса наднормативного\nвикиду", "Mi", true);
                DBGridControl.AddColumn(DBGrid2, "Розмір мінімальної\nзаробітної  плати", "P");

                DBGridControl.AddColumn(DBGrid2, "Показник відносної\nнебезпечності", "Ai", true);

                DBGridControl.AddColumn(DBGrid2, "Коефіцієнт\nчисельності  жителів", "Kpop", true);
                DBGridControl.AddColumn(DBGrid2, "Коефіцієнт\nнародногосподарського значення", "Kf");
                DBGridControl.AddColumn(DBGrid2, "Коефіцієнт\nрівня забруднення", "Kzi", true);
                DBGridControl.AddColumn(DBGrid2, "значення  ОВГП", "Qv");
                DBGridControl.AddColumn(DBGrid2, "Час роботи\nгод", "T");
                DBGridControl.AddColumn(DBGrid2, "Кількість  населення\nосіб", "POP_");
                DBGridControl.AddColumn(DBGrid2, "id забрудника", "pollution_id", true);



                DBGrid1.ItemsSource = context.Pollutions
                .Include(p => p.Enterprise)
                .Include(p => p.Pollutant)
                .ToList();
                DBGridControl.DelOllColumn(DBGrid1);
                DBGrid1.ItemsSource = context.Pollutions.ToList();
                DBGridControl.AddColumn(DBGrid1, "id", "Idpollution");
                DBGridControl.AddColumn(DBGrid1, "Підприємство", "Enterprise.Name");
                DBGridControl.AddColumn(DBGrid1, "Забрудник", "Pollutant.Name");
                DBGridControl.AddColumn(DBGrid1, "Викидив(т/рік)", "ValueMfr");
                DBGridControl.AddColumn(DBGrid1, "(%)від заг. викидів", "Percent");
                DBGridControl.AddColumn(DBGrid1, "Рік", "Year");
                DBGridControl.AddColumn(DBGrid1, "Концентрація(мг/м3)", "Concentration");
                DBGridControl.AddColumn(DBGrid1, "Коефіцієнт небезпеки", "HQ", true);
                DBGridControl.AddColumn(DBGrid1, "Характеристика ризику", "rating", true);

            }
        }

        private void DBGrid_CellEditEnding2(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                var editedItem = e.Row.Item;
                DataControl.updateRow(e, editedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Дані не можуть бути змінені");
            }
        }

        private void RefreshButton1_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGrid1.ItemsSource = context.Pollutions
                .Include(p => p.Enterprise)
                .Include(p => p.Pollutant)
                .ToList();
            }
        }

        private void RefreshButton2_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGrid2.ItemsSource = context.Results.ToList();
            }
        }

        private void СalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBGrid1.SelectedItems != null)
            {

                using (var context = new EcomonitoringdbContext())
                {
                    foreach (var selectedItem in DBGrid1.SelectedItems)
                    {
                        if (selectedItem is Pollution pollution)
                        {
                            try
                            {
                                context.Losses.Add(new Loss(context.Pollutions.Find(pollution.Idpollution)));
                                context.SaveChanges();
                                DBGrid2.ItemsSource = context.Losses.ToList();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Виникла помилка:  " + ex);
                            }

                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Будь ласка, виділіть рядок, який ви хочете видалити.");
            }
        }
    }
}
