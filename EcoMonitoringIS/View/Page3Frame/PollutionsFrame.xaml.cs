using EcoMonitoringIS.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EcoMonitoringIS.View.Page3Frame
{
    /// <summary>
    /// Interaction logic for PollutionsFrame.xaml
    /// </summary>
    public partial class PollutionsFrame : Page
    {
        public PollutionsFrame()
        {
            InitializeComponent();
        }

        private void FillBtn_Click(object sender, RoutedEventArgs e)
        {
            // Отримання введених даних з TextBox
            Enterprise? enterprise;
            Pollutant? pollutant;
            double valueMfr;
            double percent;
            int year;


            if (string.IsNullOrWhiteSpace(EnterpriseTextBox.Text) || string.IsNullOrWhiteSpace(PollutantTextBox.Text) || string.IsNullOrWhiteSpace(ValueMfrTextBox.Text) || string.IsNullOrWhiteSpace(PercentTextBox.Text) || string.IsNullOrWhiteSpace(YearTextBox.Text))
            {
                MessageBox.Show("Заповніть всі поля!");
            }
            else
            {
                using (var context = new EcomonitoringdbContext())
                {

                    enterprise = context.Enterprises.FirstOrDefault(ent => ent.Name == EnterpriseTextBox.Text);
                    pollutant = context.Pollutants.FirstOrDefault(pol => pol.Name == PollutantTextBox.Text);
                    if (enterprise == null)
                    {
                        MessageBox.Show("Недійсне значення підприємства.");
                        return;
                    }
                    else if (pollutant == null)
                    {
                        MessageBox.Show("Недійсне значення забрудника");
                        return;
                    }
                    else if (!double.TryParse(ValueMfrTextBox.Text.Replace('.', ','), out valueMfr))
                    {
                        MessageBox.Show("Недійсне значення викидів");
                        return;
                    }
                    else if (!double.TryParse(PercentTextBox.Text.Replace('.', ','), out percent))
                    {
                        MessageBox.Show("Недійсне значення відсотку викидів");
                        return;
                    }
                    else if (!int.TryParse(PercentTextBox.Text, out year))
                    {
                        MessageBox.Show("Недійсне значення року");
                        return;
                    }


                    // Створення нового запису Pollution
                    var newPollution = new Pollution
                    {
                        Enterprise = enterprise,
                        Pollutant = pollutant,
                        ValueMfr = valueMfr,
                        Percent = percent,
                        Year = year
                    };         

                    try
                    {
                        context.Pollutions.Add(newPollution);
                        context.SaveChanges();
                        MessageBox.Show("Дані успішно додано");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Сталася помилка: " + ex.Message);
                    }
                }

            }

            // Очищення полів для введення даних
            EnterpriseTextBox.Clear();
            PollutantTextBox.Clear();
            ValueMfrTextBox.Clear();
            PercentTextBox.Clear();
            YearTextBox.Clear();
        }
    }
}
