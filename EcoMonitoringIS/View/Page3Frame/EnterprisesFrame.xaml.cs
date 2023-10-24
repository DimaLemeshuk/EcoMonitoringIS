using EcoMonitoringIS.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace EcoMonitoringIS.View.Page3Frame
{
    /// <summary>
    /// Interaction logic for EnterprisesFrame.xaml
    /// </summary>
    public partial class EnterprisesFrame : Page
    {
        public EnterprisesFrame()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;
            string Activity = ActivityTextBox.Text;
            string Belonging = BelongingTextBox.Text;
            string Addres = AddresTextBox.Text;
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Activity) || string.IsNullOrWhiteSpace(Belonging) || string.IsNullOrWhiteSpace(Addres))
            {
                MessageBox.Show("Заповніть всі поля!");
            }
            else
            {
                if (int.TryParse(Belonging, out int BelongingId))
                {
                    using (var db = new EcomonitoringdbContext())
                    {
                        var b = db.Belongings.FirstOrDefault(b => b.Idbelonging == BelongingId);
                        if (b != null)
                        {                         
                            try
                            {
                                db.Enterprises.Add(new Enterprise(Name, Activity, BelongingId, Addres));
                                db.SaveChanges();
                                MessageBox.Show("Дані успішно додано");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Сталася помилка: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Значення Належність не має відповідника\nв таблиці Belongings");
                            return;
                        }
                    }
                }
                else
                {
                    using (var db = new EcomonitoringdbContext())
                    {
                        var b = db.Belongings.FirstOrDefault(b => b.Name == Belonging);
                        if (b != null)
                        {
                            try
                            {
                                db.Enterprises.Add(new Enterprise(Name, Activity, b.Idbelonging, Addres));
                                db.SaveChanges();
                                MessageBox.Show("Дані успішно додано");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Сталася помилка: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Значення Належність не має відповідника\nв таблиці Belongings");
                            return;
                        }
                    }
                }
            }
        }
    }
}
