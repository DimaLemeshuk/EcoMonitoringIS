using EcoMonitoringIS.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EcoMonitoringIS.View.Page3Frame
{
    /// <summary>
    /// Interaction logic for PollutantsFrame.xaml
    /// </summary>
    public partial class PollutantsFrame : Page
    {
        public PollutantsFrame()
        {
            InitializeComponent();
        }

        private void FillBtn_Click(object sender, RoutedEventArgs e)
        {
            // Отримайте дані із TextBox для створення нового запису в базі даних
            string name = NameTextBox.Text;
            int dangerClass;
            double gdk;
            double mfr;

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(DangerClassTextBox.Text) || string.IsNullOrWhiteSpace(GdkTextBox.Text) || string.IsNullOrWhiteSpace(MfrTextBox.Text))
            {
                MessageBox.Show("Заповніть всі поля!");
            }
            else
            {
                if (!Double.TryParse(GdkTextBox.Text.Replace('.', ','), out gdk))
                {
                    MessageBox.Show("Недійсне значення для Граничнодопустимих викидів.");
                    return;
                }
                else if (!Double.TryParse(MfrTextBox.Text.Replace('.', ','), out mfr))
                {
                    MessageBox.Show("Недійсне значення для Величини масової витрати.");
                    return;
                }
                else if (!int.TryParse(DangerClassTextBox.Text, out dangerClass))
                {
                    MessageBox.Show("Недійсне значення для Класу небезпеки.");
                    return;
                }

                // Створення нового запису в базі даних
                using (EcomonitoringdbContext context = new EcomonitoringdbContext())
                {
                    var newPollutant = new Pollutant
                    {
                        Name = name,
                        DangerClass = dangerClass,
                        Gdk = gdk,
                        Mfr = mfr
                    };

                    try
                    {
                        context.Pollutants.Add(newPollutant);
                        context.SaveChanges();
                        MessageBox.Show("Дані успішно додано до бази даних.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка: " + ex.Message);
                    }
                }
            }

            // Очистка TextBox після додавання даних
            NameTextBox.Clear();
            DangerClassTextBox.Clear();
            GdkTextBox.Clear();
            MfrTextBox.Clear();
        }
    }
}
