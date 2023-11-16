using EcoMonitoringIS.Classes;
using EcoMonitoringIS.Models;
using MySqlX.XDevAPI.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Result = EcoMonitoringIS.Models.Result;

namespace EcoMonitoringIS.View
{
    /// <summary>
    /// Interaction logic for Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid2);
                DBGrid2.ItemsSource = context.Results.ToList();
                DBGridControl.AddColumn(DBGrid2, "id", "Idresults", true);
                DBGridControl.AddColumn(DBGrid2, "Величина  популяційного\nризику", "PCR", true);
                DBGridControl.AddColumn(DBGrid2, "Величина  індивідуального\nризику", "CR", true);
                DBGridControl.AddColumn(DBGrid2, "Величина надходження\nмг/кг-доба", "LADD", true);

                DBGridControl.AddColumn(DBGrid2, "id забрудника", "PollutionId", true);

                DBGridControl.AddColumn(DBGrid2, "Концентрація реч. в приміщенні\nмг/куб.м ", "Ch");
                DBGridControl.AddColumn(DBGrid2, "Час поза приміщенням\nгод/доба", "Tout");
                DBGridControl.AddColumn(DBGrid2, "Час всередині приміщення\nгод/доба", "Tin");
                DBGridControl.AddColumn(DBGrid2, "Шв. дих. поза приміщенням\nкуб.м/год", "Vout");
                DBGridControl.AddColumn(DBGrid2, "Шв. дих. в середині приміщення\nкуб.м/год", "Vin");
                DBGridControl.AddColumn(DBGrid2, "Частота впливу\nднів/рік", "EF");
                DBGridControl.AddColumn(DBGrid2, "Тривалість впливу\nроків", "ED");
                DBGridControl.AddColumn(DBGrid2, "Маса тіла\nкг", "BW");
                DBGridControl.AddColumn(DBGrid2, "Осереднення експозиції\nроків", "AT");
                DBGridControl.AddColumn(DBGrid2, "Кількість  населення\nосіб", "POP_");


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
            catch(Exception ex)
            {
                MessageBox.Show("Дані не можуть бути змінені");
            }
            
        }

        private void СalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBGrid1.SelectedItems != null) 
                {
                //var select = SelectedItems as Pollution;

                using (var context = new EcomonitoringdbContext())
                {
                    foreach (var selectedItem in DBGrid1.SelectedItems)
                    {
                        if(selectedItem is Pollution pollution)
                        {
                            try
                            {
                                context.Results.Add(new Result(context.Pollutions.Find(pollution.Idpollution)));
                                context.SaveChanges();
                                DBGrid2.ItemsSource = context.Results.ToList();
                            }
                            catch(Exception ex)
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
    }
}
