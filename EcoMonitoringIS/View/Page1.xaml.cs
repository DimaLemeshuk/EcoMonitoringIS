using EcoMonitoringIS.Classes;
using EcoMonitoringIS.Models;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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
using static System.Net.Mime.MediaTypeNames;
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
                CleanCellEditEnding();
                DBGrid.CellEditEnding += DBGrid_CellEditEndingEnterprises;
            }
            //DBGrid.CellEditEnding += DBGrid_CellEditEndingEnterprises;
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
                CleanCellEditEnding();
                DBGrid.CellEditEnding += DBGrid_CellEditEndingPollutions;
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
                CleanCellEditEnding();
                DBGrid.CellEditEnding += DBGrid_CellEditEndingPollutants;
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
                CleanCellEditEnding();
                DBGrid.CellEditEnding += DBGrid_CellEditEndingBelongings;
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
                CleanCellEditEnding();
                DBGrid.CellEditEnding += DBGrid_CellEditEndingResults;
            }
            //DBGrid.CellEditEnding += DBGrid_CellEditEndingResults;
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void CleanCellEditEnding()
        {
            DBGrid.CellEditEnding -= DBGrid_CellEditEndingEnterprises;
            DBGrid.CellEditEnding -= DBGrid_CellEditEndingPollutions;
            DBGrid.CellEditEnding -= DBGrid_CellEditEndingPollutants;
            DBGrid.CellEditEnding -= DBGrid_CellEditEndingBelongings;
            DBGrid.CellEditEnding -= DBGrid_CellEditEndingResults;
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
                    Enterprise? Ent = context.Enterprises.FirstOrDefault(en => en == editedItem);
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
                        Belonging? test;
                        test = context.Belongings.FirstOrDefault(b => b.Name.Equals(NewValue));
                        if (test != null)
                        {
                            Ent.Belonging = test;
                        }
                        else
                        {
                            MessageBox.Show("Значення не має відповідника\nв таблиці Belongings");
                        }
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
                    MessageBox.Show(" editingElement має значення null");
                }
            }
            else
            {
                // Обробка помилки: якщо editedItem не було успішно отримано або поточна колонка не є DataGridBoundColumn
                MessageBox.Show("12 editedItem не було успішно отримано \nабо поточна колонка не є DataGridBoundColumn");
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
                    Pollution? obj = context.Pollutions.FirstOrDefault(po => po == editedItem);
                    if (newPropertyName.Equals("Enterprise.Name"))
                    {
                        Enterprise test = context.Enterprises.FirstOrDefault(en => en.Name.Equals(NewValue));
                        if (test != null)
                        {
                            obj.Enterprise = test;
                        }
                        else
                        {
                            MessageBox.Show("Значення не має відповідника\nв таблиці Enterprise");
                        }                     
                    }
                    else if (newPropertyName.Equals("Pollutant.Name"))
                    {                     
                        Pollutant? test = context.Pollutants.FirstOrDefault(po => po.Name.Equals(NewValue));
                        if (test != null)
                        {
                            obj.Pollutant = test;
                        }
                        else
                        {
                            MessageBox.Show("Значення не має відповідника\nв таблиці Pollutant");
                        }
                    }
                    else if (newPropertyName.Equals("ValueMfr"))
                    {
                        if (double.TryParse(NewValue, out double parsedValue))
                        {
                            obj.ValueMfr = parsedValue;
                        }
                        else
                        {
                            MessageBox.Show("Значення має невірний тип (це не число)");
                        }
                    }
                    else if (newPropertyName.Equals("Percent"))
                    {
                        if (double.TryParse(NewValue, out double parsedValue))
                        {
                            obj.Percent = parsedValue;
                        }
                        else
                        {
                            MessageBox.Show("Значення має невірний тип (це не число)");
                        }
                    }
                    else if (newPropertyName.Equals("Year"))
                    {
                        if (int.TryParse(NewValue, out int parsedValue))
                        {
                            obj.Year = parsedValue;
                        }
                        else
                        {
                            MessageBox.Show("Значення має невірний тип (це не ціле число)");
                        }
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
                    Pollutant? obj = context.Pollutants.FirstOrDefault(pl => pl == editedItem);
                    if (newPropertyName.Equals("Name"))
                    {
                        obj.Name = NewValue;
                    }
                    else if (newPropertyName.Equals("DangerClass"))
                    {
                        if (int.TryParse(NewValue, out int parsedValue))
                        {
                            obj.DangerClass = parsedValue;
                        }
                        else
                        {
                            MessageBox.Show("Значення має невірний тип (це не ціле число)");
                        }
                    }
                    else if (newPropertyName.Equals("Gdk"))
                    {
                        if (double.TryParse(NewValue, out double parsedValue))
                        {
                            obj.Gdk = parsedValue;
                        }
                        else
                        {
                            MessageBox.Show("Значення має невірний тип (це не число)");
                        }
                    }
                    else if (newPropertyName.Equals("Mfr"))
                    {
                        if (double.TryParse(NewValue, out double parsedValue))
                        {
                            obj.Mfr = parsedValue;
                        }
                        else
                        {
                            MessageBox.Show("Значення має невірний тип (це не число)");
                        }
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
                    Belonging? obj = context.Belongings.FirstOrDefault(b => b == editedItem);
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
                    Result obj = context.Results.FirstOrDefault(r => r == editedItem);
                    if (newPropertyName.Equals("PollutionId"))
                    {
                        if (int.TryParse(NewValue, out int parsedValue))
                        {
                            obj.PollutionId = parsedValue;
                        }
                        else
                        {
                            MessageBox.Show("Значення має невірний тип (це не ціле число)");
                        }
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
            try
            {
                context.SaveChanges();
                MessageBox.Show("Зміни успішно збережено");
                if (CurBtn != null)
                {
                    CurBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            }
            catch (Exception ex)
            {
                // Обробка винятку
                MessageBox.Show("Сталася помилка: " + ex.Message);
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBGrid.SelectedItem != null)
            {
                var selectedItem = DBGrid.SelectedItem;

                if (selectedItem is Enterprise enterprise)
                {
                    var id = enterprise.Identerprise;
                    var entityToDelete = new Enterprise { Identerprise = id };
                    context.Entry(entityToDelete).State = EntityState.Deleted;
                }
                else if (selectedItem is Pollution pollution)
                {
                    var id = pollution.Idpollution;
                    var entityToDelete = new Pollution { Idpollution = id };
                    context.Entry(entityToDelete).State = EntityState.Deleted;
                }
                else if (selectedItem is Pollutant pollutant)
                {
                    var id = pollutant.Idpollutant;
                    var entityToDelete = new Pollutant { Idpollutant = id };
                    context.Entry(entityToDelete).State = EntityState.Deleted;
                }
                else if (selectedItem is Belonging belonging)
                {
                    var id = belonging.Idbelonging;
                    var entityToDelete = new Belonging { Idbelonging = id };
                    context.Entry(entityToDelete).State = EntityState.Deleted;
                }
                else if (selectedItem is Result result)
                {
                    var id = result.Idresults;
                    var entityToDelete = new Result { Idresults = id };
                    context.Entry(entityToDelete).State = EntityState.Deleted;
                }

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Дані видалено успішно");
                    if (CurBtn != null)
                    {
                        CurBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка під час видалення: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виділіть рядок, який ви хочете видалити.");
            }
        }
    

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DBGridAdd.ItemsSource = null; // Припиняємо відображення даних
            DBGridAdd.Items.Clear();
            DBGridControl.CopyColumnNames(DBGrid , DBGridAdd);
        }

        private void AddObjButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChooseT.Text == "Enterprises")
            {
                var lastRow = DBGridAdd.Items[0] as DataRowView;

                if (lastRow != null)
                {
                    using (var db = new EcomonitoringdbContext())
                    {
                        string name = lastRow[1].ToString();
                        string activity = lastRow[2].ToString();
                        string belongingName = lastRow[3].ToString();
                        string address = lastRow[4].ToString();
                        Belonging? test;
                        test = db.Belongings.FirstOrDefault(b => b.Name.Equals(belongingName));
                        if (test == null)
                        {
                            MessageBox.Show("Значення Належність не має відповідника\nв таблиці Belongings");
                        }
                        else if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(activity) || !string.IsNullOrWhiteSpace(address))
                        {
                            var newEnterprise = new Enterprise();

                            try
                            {
                                newEnterprise.Initialize(name, activity, belongingName, address);
                                db.Enterprises.Add(newEnterprise);
                                db.SaveChanges();
                                MessageBox.Show("Дані успішно додано");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Сталася помилка: " + ex.Message);
                            }
                            ClearAndResetDBGridAdd();
                        }
                        else
                        {
                            MessageBox.Show("Заповніть всі поля!");
                        }

                    }

                }
                else
                {
                    MessageBox.Show("Помилка при отриманні даних з DBGridAdd.");
                }
            }
            else if (ChooseT.Text == "Pollutions")
            {
                var lastRow = DBGridAdd.Items[0] as DataRowView;

                if (lastRow != null)
                {
                    using (var db = new EcomonitoringdbContext())
                    {
                        string enterpriseName = lastRow[1].ToString();
                        string pollutantName = lastRow[2].ToString();
                        string valueMfrStr = lastRow[3].ToString();
                        string percentStr = lastRow[4].ToString();
                        string yearStr = lastRow[5].ToString();

                        if (!string.IsNullOrWhiteSpace(enterpriseName) && !string.IsNullOrWhiteSpace(pollutantName) &&
                            double.TryParse(valueMfrStr, out double valueMfr) &&
                            double.TryParse(percentStr, out double percent) &&
                            int.TryParse(yearStr, out int year))
                        {
                            Enterprise? enterprise = db.Enterprises.FirstOrDefault(e => e.Name.Equals(enterpriseName));
                            Pollutant? pollutant = db.Pollutants.FirstOrDefault(p => p.Name.Equals(pollutantName));

                            if (enterprise != null && pollutant != null)
                            {
                                // Блокування режиму редагування
                                //DBGridAdd.CancelEdit();

                                var newPollution = new Pollution
                                {
                                    EnterpriseId = enterprise.Identerprise,
                                    PollutantId = pollutant.Idpollutant,
                                    ValueMfr = valueMfr,
                                    Percent = percent,
                                    Year = year
                                };

                                try
                                {
                                    db.Pollutions.Add(newPollution);
                                    db.SaveChanges();
                                    MessageBox.Show("Дані успішно додано");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Сталася помилка: " + ex.Message);
                                }

                                // Оновлення джерела даних
                                DBGridAdd.ItemsSource = db.Pollutions.ToList();
                            }
                            else
                            {
                                MessageBox.Show("Помилка: Підприємство або забруднювач не знайдено.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Помилка: Перевірте введені дані.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Помилка при отриманні даних з DBGridAdd.");
                }
            }
            else if (ChooseT.Text == "Pollutants")
            {
                var lastRow = DBGridAdd.Items[0] as DataRowView;

                if (lastRow != null)
                {
                    using (var db = new EcomonitoringdbContext())
                    {
                        string name = lastRow[1].ToString();
                        string dangerClass = lastRow[2].ToString();
                        string gdk = lastRow[3].ToString();
                        string mfr = lastRow[4].ToString();

                        var newPollutant = new Pollutant();

                        newPollutant.Name = name;
                        newPollutant.DangerClass = dangerClass;

                        double? gdkValue = null;
                        if (double.TryParse(gdk, out double gdkResult))
                        {
                            gdkValue = gdkResult;
                        }
                        newPollutant.Gdk = gdkValue;

                        double? mfrValue = null;
                        if (double.TryParse(mfr, out double mfrResult))
                        {
                            mfrValue = mfrResult;
                        }
                        newPollutant.Mfr = mfrValue;

                        try
                        {
                            db.Pollutants.Add(newPollutant);
                            db.SaveChanges();
                            MessageBox.Show("Дані успішно додано");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Сталася помилка: " + ex.Message);
                        }

                        DBGridAdd.ItemsSource = null; // Припиняємо відображення даних
                        DBGridAdd.Items.Clear();
                        DBGridControl.CopyColumnNames(DBGrid, DBGridAdd); // заново відображаємо пусті колонки
                    }
                }
                else
                {
                    MessageBox.Show("Помилка при отриманні даних з DBGridAdd.");
                }
            }
            else if (ChooseT.Text == "Belongings")
            {

            }
            else if (ChooseT.Text == "Results")
            {

            }         
        }

        private void ClearAndResetDBGridAdd()
        {
            DBGridAdd.ItemsSource = null; // Припиняємо відображення даних
            DBGridAdd.Items.Clear();
            DBGridControl.CopyColumnNames(DBGrid, DBGridAdd);// заново відображаємо пусті колонки
        }
    }
}
