using EcoMonitoringIS.Classes;
using EcoMonitoringIS.Models;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
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
using Result = EcoMonitoringIS.Models.Result;

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
        EcomonitoringdbContext context;

        public Page1()
        {
            InitializeComponent();
            context = new EcomonitoringdbContext();
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
             
                DBGrid.CellEditEnding += DBGrid_CellEditEndingEnterprises;

                ChooseT.Text = ((Button)sender).Content.ToString();
                CurBtn = (Button)sender;
            }
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
            DBGrid.CellEditEnding += DBGrid_CellEditEndingPollutions;
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
            DBGrid.CellEditEnding += DBGrid_CellEditEndingPollutants;
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
            DBGrid.CellEditEnding += DBGrid_CellEditEndingBelongings;
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
            DBGrid.CellEditEnding += DBGrid_CellEditEndingResults;
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


        private void DBGrid_CellEditEndingEnterprises(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var editedItem = e.Row.Item;

            if (editedItem != null && dataGrid.CurrentColumn is DataGridBoundColumn column)
            {
                var newPropertyName = column.SortMemberPath;// Назва властивості
                var editingElement = e.EditingElement as TextBox;

                if (editingElement != null)
                {
                    var NewValue = editingElement.Text;// Нове значення                   
                    Enterprise Ent = context.Enterprises.FirstOrDefault(e => e == editedItem);
                    if (newPropertyName.Equals("Name"))
                    {
                        Ent.Name = NewValue;
                    }
                    else if (newPropertyName.Equals("Activity"))
                    {
                        Ent.Activity = NewValue;
                    }
                    else if (newPropertyName.Equals("Belonging.Name"))
                    {
                        Ent.Belonging = context.Belongings.FirstOrDefault(b => b.Name.Equals(NewValue));
                    }
                    else if (newPropertyName.Equals("Addres"))
                    {
                        Ent.Addres = NewValue;
                    }
                    else
                    {
                        MessageBox.Show("Параметр неможливо змінити");
                    }
                }
                else
                {
                    // Обробка помилки: якщо editingElement має значення null
                }
            }
            else
            {
                // Обробка помилки: якщо editedItem не було успішно отримано або поточна колонка не є DataGridBoundColumn
            }

        }

        private void DBGrid_CellEditEndingPollutions(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var editedItem = e.Row.Item;

            if (editedItem != null && dataGrid.CurrentColumn is DataGridBoundColumn column)
            {
                var newPropertyName = column.SortMemberPath;// Назва властивості
                var editingElement = e.EditingElement as TextBox;

                if (editingElement != null)
                {
                    var NewValue = editingElement.Text;// Нове значення                   
                    Pollution obj = context.Pollutions.FirstOrDefault(e => e == editedItem);
                    if (newPropertyName.Equals("Enterprise.Name"))
                    {
                        obj.Enterprise = context.Enterprises.FirstOrDefault(e => e.Name.Equals(NewValue));
                    }
                    else if (newPropertyName.Equals("Pollutant.Name"))
                    {
                        obj.Pollutant = context.Pollutants.FirstOrDefault(e => e.Name.Equals(NewValue));
                    }             
                    else if (newPropertyName.Equals("ValueMfr"))
                    {
                        obj.ValueMfr = double.Parse(NewValue);
                    }
                    else if (newPropertyName.Equals("Percent"))
                    {
                        obj.Percent = double.Parse(NewValue);
                    }
                    else if (newPropertyName.Equals("Year"))
                    {
                        obj.Year = int.Parse(NewValue);
                    }
                    else
                    {
                        MessageBox.Show("Параметр неможливо змінити");
                    }
                }
                else
                {
                    // Обробка помилки: якщо editingElement має значення null
                }
            }
            else
            {
                // Обробка помилки: якщо editedItem не було успішно отримано або поточна колонка не є DataGridBoundColumn
            }
        }

        private void DBGrid_CellEditEndingPollutants(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var editedItem = e.Row.Item;

            if (editedItem != null && dataGrid.CurrentColumn is DataGridBoundColumn column)
            {
                var newPropertyName = column.SortMemberPath;// Назва властивості
                var editingElement = e.EditingElement as TextBox;

                if (editingElement != null)
                {

                    var NewValue = editingElement.Text;// Нове значення                   
                    Pollutant obj = context.Pollutants.FirstOrDefault(e => e == editedItem);
                    if (newPropertyName.Equals("Name"))
                    {
                        obj.Name = NewValue;
                    }
                    else if (newPropertyName.Equals("DangerClass"))
                    {
                        obj.DangerClass = int.Parse(NewValue);
                    }
                    else if (newPropertyName.Equals("Gdk"))
                    {
                        obj.Gdk = double.Parse(NewValue);
                    }
                    else if (newPropertyName.Equals("Mfr"))
                    {
                        obj.Mfr = double.Parse(NewValue);
                    }
                    else
                    {
                        MessageBox.Show("Параметр неможливо змінити");
                    }
                }
                else
                {
                    // Обробка помилки: якщо editingElement має значення null
                }
            }
            else
            {
                // Обробка помилки: якщо editedItem не було успішно отримано або поточна колонка не є DataGridBoundColumn
            }
        }

        private void DBGrid_CellEditEndingBelongings(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var editedItem = e.Row.Item;

            if (editedItem != null && dataGrid.CurrentColumn is DataGridBoundColumn column)
            {
                var newPropertyName = column.SortMemberPath;// Назва властивості
                var editingElement = e.EditingElement as TextBox;

                if (editingElement != null)
                {
                    var NewValue = editingElement.Text;// Нове значення                   
                    Belonging obj = context.Belongings.FirstOrDefault(e => e == editedItem);
                    if (newPropertyName.Equals("Name"))
                    {
                        obj.Name = NewValue;
                    }               
                    else
                    {
                        MessageBox.Show("Параметр неможливо змінити");
                    }
                }
                else
                {
                    // Обробка помилки: якщо editingElement має значення null
                }
            }
            else
            {
                // Обробка помилки: якщо editedItem не було успішно отримано або поточна колонка не є DataGridBoundColumn
            }
        }

        private void DBGrid_CellEditEndingResults(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var editedItem = e.Row.Item;

            if (editedItem != null && dataGrid.CurrentColumn is DataGridBoundColumn column)
            {
                var newPropertyName = column.SortMemberPath;// Назва властивості
                var editingElement = e.EditingElement as TextBox;

                if (editingElement != null)
                {
                    DBGridControl.AddColumn(DBGrid, "id", "Idresults");
                    DBGridControl.AddColumn(DBGrid, "Перевищення", "Exceeding");
                    DBGridControl.AddColumn(DBGrid, "id забрудника", "PollutionId");

                    var NewValue = editingElement.Text;// Нове значення                   
                    Result obj = context.Results.FirstOrDefault(e => e == editedItem);
                    if (newPropertyName.Equals("PollutionId"))
                    {
                        obj.PollutionId = int.Parse(NewValue);
                    }
                    else
                    {
                        MessageBox.Show("Параметр неможливо змінити");
                    }
                }
                else
                {
                    // Обробка помилки: якщо editingElement має значення null
                }
            }
            else
            {
                // Обробка помилки: якщо editedItem не було успішно отримано або поточна колонка не є DataGridBoundColumn
            }
        }

        private void SaveChangeButton_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
        }
    }
}
