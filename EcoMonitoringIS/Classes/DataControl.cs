using EcoMonitoringIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Data.Entity;
using System.Reflection;
using System.Windows.Data;

namespace EcoMonitoringIS.Classes
{
    class DataControl
    {
        public static void deleteRov(DataGrid DBGrid, System.Object selectedItem)
        {
            using (var context = new EcomonitoringdbContext())
            {

                if (selectedItem is Enterprise enterprise)
                {
                    try
                    {
                        context.Remove(enterprise);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при видаленні Enterprise : " + ex.Message);
                    }

                }
                else if (selectedItem is Pollution pollution)
                {
                    try
                    {
                        context.Remove(pollution);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при видаленні Pollution: " + ex.Message);
                    }
                }
                else if (selectedItem is Pollutant pollutant)
                {
                    try
                    {
                        context.Remove(pollutant);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при видаленні Pollutant: " + ex.Message);
                    }

                }
                else if (selectedItem is Belonging belonging)
                {
                    try
                    {
                        context.Remove(belonging);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при видаленні Belonging: " + ex.Message);
                    }

                }
                else if (selectedItem is Result result)
                {
                    try
                    {
                        context.Remove(result);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при видаленні Result: " + ex.Message);
                    }

                }
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка під час видалення: " + ex.Message);
                }
            }
        }

        public static void updateRow(DataGridCellEditEndingEventArgs e, System.Object editedItem)
        {
            var editedValue = (e.EditingElement as TextBox).Text;// Отримайте значення комірки, яку редагуєте

            using (var context = new EcomonitoringdbContext())
            {
                if (editedItem is Enterprise enterprise)
                {
                    try
                    {
                        var curr = context.Enterprises.Find(enterprise.Identerprise);
                        Binding binding = (e.Column as DataGridBoundColumn).Binding as Binding;
                        string propertyName = binding.Path.Path;//змінене поле   
                        PropertyInfo propertyInfo = (curr.GetType()).GetProperty(propertyName);
                        propertyInfo.SetValue(curr, ConvertToNumberOrString(editedValue));
                        context.SaveChanges();
                        MessageBox.Show("Зміни успішно збережено");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при оновленні: " + ex.Message);
                    }
                }
                else if (editedItem is Pollution pollution)
                {
                    try
                    {
                        var curr = context.Pollutions.Find(pollution.Idpollution);
                        Binding binding = (e.Column as DataGridBoundColumn).Binding as Binding;
                        string propertyName = binding.Path.Path;//змінене поле   
                        PropertyInfo propertyInfo = (curr.GetType()).GetProperty(propertyName);
                        propertyInfo.SetValue(curr, ConvertToNumberOrString(editedValue));
                        context.SaveChanges();
                        MessageBox.Show("Зміни успішно збережено");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при оновленні: " + ex.Message);
                    }
                }
                else if (editedItem is Pollutant pollutant)
                {
                    try
                    {
                        var curr = context.Pollutants.Find(pollutant.Idpollutant);
                        Binding binding = (e.Column as DataGridBoundColumn).Binding as Binding;
                        string propertyName = binding.Path.Path;//змінене поле   
                        PropertyInfo propertyInfo = (curr.GetType()).GetProperty(propertyName);
                        propertyInfo.SetValue(curr, ConvertToNumberOrString(editedValue));
                        context.SaveChanges();
                        MessageBox.Show("Зміни успішно збережено");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при оновленні: " + ex.Message);
                    }
                }
                else if (editedItem is Belonging belonging)
                {
                    try
                    {
                        var curr = context.Belongings.Find(belonging.Idbelonging);
                        Binding binding = (e.Column as DataGridBoundColumn).Binding as Binding;
                        string propertyName = binding.Path.Path;//змінене поле   
                        PropertyInfo propertyInfo = (curr.GetType()).GetProperty(propertyName);
                        propertyInfo.SetValue(curr, ConvertToNumberOrString(editedValue));
                        context.SaveChanges();
                        MessageBox.Show("Зміни успішно збережено");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при оновленні: " + ex.Message);
                    }
                }
                else if (editedItem is Result result)
                {
                    try
                    {
                        var curr = context.Results.Find(result.Idresults);
                        Binding binding = (e.Column as DataGridBoundColumn).Binding as Binding;
                        string propertyName = binding.Path.Path;//змінене поле   
                        PropertyInfo propertyInfo = (curr.GetType()).GetProperty(propertyName);
                        propertyInfo.SetValue(curr, ConvertToNumberOrString(editedValue));
                        curr.CalculateCR(curr.PollutionId);
                        context.SaveChanges();
                        MessageBox.Show("Зміни успішно збережено");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при оновленні: " + ex.Message);
                    }
                }
                else if (editedItem is Loss loss)
                {
                    try
                    {
                        var curr = context.Results.Find(loss.idLoss);
                        Binding binding = (e.Column as DataGridBoundColumn).Binding as Binding;
                        string propertyName = binding.Path.Path;//змінене поле   
                        PropertyInfo propertyInfo = (curr.GetType()).GetProperty(propertyName);
                        propertyInfo.SetValue(curr, ConvertToNumberOrString(editedValue));
                        curr.CalculateCR(curr.PollutionId);
                        context.SaveChanges();
                        MessageBox.Show("Зміни успішно збережено");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка при оновленні: " + ex.Message);
                    }
                }
            }

        }

        public static object ConvertToNumberOrString(string input)
        {

            if (int.TryParse(input, out int intValue))
            {
                return intValue;
            }

            input = input.Replace('.', ',');
            if (double.TryParse(input, out double doubleValue))
            {
                return doubleValue;
            }

            // Якщо конвертація в int не вдалася, повернемо вихідну строку
            return input;
        }
    }
}
    

