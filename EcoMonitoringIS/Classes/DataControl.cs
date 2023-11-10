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

    }
}
    

