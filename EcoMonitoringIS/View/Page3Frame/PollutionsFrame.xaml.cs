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
            string enterpriseName = EnterpriseTextBox.Text;
            string pollutantName = PollutantTextBox.Text;
            double valueMfr = Convert.ToDouble(ValueMfrTextBox.Text);
            double percent = Convert.ToDouble(PercentTextBox.Text);
            int year = Convert.ToInt32(YearTextBox.Text);

            using (var context = new EcomonitoringdbContext())
            {
                // Отримання або створення підприємства та забрудника
                Enterprise enterprise = context.Enterprises.FirstOrDefault(ent => ent.Name == enterpriseName) ?? new Enterprise { Name = enterpriseName };
                Pollutant pollutant = context.Pollutants.FirstOrDefault(pol => pol.Name == pollutantName) ?? new Pollutant { Name = pollutantName };

                // Створення нового запису Pollution
                var newPollution = new Pollution
                {
                    Enterprise = enterprise,
                    Pollutant = pollutant,
                    ValueMfr = valueMfr,
                    Percent = percent,
                    Year = year
                };

                context.Pollutions.Add(newPollution);

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Дані успішно додано");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка: " + ex.Message);
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
