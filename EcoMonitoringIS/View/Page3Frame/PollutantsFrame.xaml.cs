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
            string dangerClass = DangerClassTextBox.Text;
            double gdk;
            double mfr;

            if (Double.TryParse(GdkTextBox.Text, out double gdkValue))
            {
                gdk = gdkValue;
            }
            else
            {
                MessageBox.Show("Недійсне значення для Граничнодопустимих викидів.");
                return;
            }

            if (Double.TryParse(MfrTextBox.Text, out double mfrValue))
            {
                mfr = mfrValue;
            }
            else
            {
                MessageBox.Show("Недійсне значення для Величини масової витрати.");
                return;
            }

            // Створення нового запису в базі даних
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                var newPollutant = new Pollutant
                {
                    Name = name,
                    DangerClass = int.Parse(dangerClass),
                    Gdk = gdk,
                    Mfr = mfr
                };

                context.Pollutants.Add(newPollutant);

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Дані успішно додано до бази даних.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка: " + ex.Message);
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
