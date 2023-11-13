using EcoMonitoringIS.Classes;
using EcoMonitoringIS.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Result = EcoMonitoringIS.Models.Result;
using Type = System.Type;

namespace EcoMonitoringIS.View
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        DataTableCollection? collection;
        Button CurBtn;
        DataTable changedData;
        //EcomonitoringdbContext context;

        public Page1()
        {
            InitializeComponent();
            //context = new EcomonitoringdbContext();
        }

        private void EnterprisesTable_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new EcomonitoringdbContext())
            {

                DBGridControl.DelOllColumn(DBGrid);

                DBGrid.ItemsSource = context.Enterprises
                    .Include(e => e.Belonging)
                    .ToList();

                DBGridControl.AddColumn(DBGrid, "id", "Identerprise");
                DBGridControl.AddColumn(DBGrid, "Назва", "Name");
                DBGridControl.AddColumn(DBGrid, "Вид діяльності", "Activity");
                DBGridControl.AddColumn(DBGrid, "Належність", "Belonging.Name");
                DBGridControl.AddColumn(DBGrid, "Адреса", "Addres");
            }
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void pollution_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGrid.ItemsSource = context.Pollutions
                .Include(p => p.Enterprise)
                .Include(p => p.Pollutant)
                .ToList();
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Pollutions.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idpollution");
                DBGridControl.AddColumn(DBGrid, "Підприємство", "Enterprise.Name");
                DBGridControl.AddColumn(DBGrid, "Забрудник", "Pollutant.Name");
                DBGridControl.AddColumn(DBGrid, "Викидив(т/рік)", "ValueMfr");
                DBGridControl.AddColumn(DBGrid, "(%)від заг. викидів", "Percent");
                DBGridControl.AddColumn(DBGrid, "Рік", "Year");
            }
            //DBGrid.CellEditEnding += DBGrid_CellEditEndingPollutions;
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;

        }

        private void pollutant_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Pollutants.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idpollutant");
                DBGridControl.AddColumn(DBGrid, "Назва речовини", "Name");
                DBGridControl.AddColumn(DBGrid, "Клас небезпеки", "DangerClass");
                DBGridControl.AddColumn(DBGrid, "Граничнодопустимі\n викиди (мг/м3)", "Gdk");
                DBGridControl.AddColumn(DBGrid, "Величина масової\n витрати (г/год)", "Mfr");
            }
            //DBGrid.CellEditEnding += DBGrid_CellEditEndingPollutants;
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void belonging_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Belongings.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idbelonging");
                DBGridControl.AddColumn(DBGrid, "Відомча належність", "Name");

            }
            //DBGrid.CellEditEnding += DBGrid_CellEditEndingBelongings;
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void results_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Results.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idresults");
                DBGridControl.AddColumn(DBGrid, "Перевищення", "Exceeding");
                DBGridControl.AddColumn(DBGrid, "id забрудника", "PollutionId");
            }
            //DBGrid.CellEditEnding += DBGrid_CellEditEndingResults;
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurBtn != null)
            {
                CurBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }


        private void DBGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editedItem = e.Row.Item;
            DataControl.updateRow(e, editedItem);

        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBGrid.SelectedItems != null)
            {
                foreach (var selectedItem in DBGrid.SelectedItems)
                {
                    DataControl.deleteRov(DBGrid, selectedItem);
                }
                if (CurBtn != null)
                {
                    CurBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }

            }
            else
            {
                MessageBox.Show("Будь ласка, виділіть рядок, який ви хочете видалити.");
            }
        }
    }
}
