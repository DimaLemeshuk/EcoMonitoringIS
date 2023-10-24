using EcoMonitoringIS.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EcoMonitoringIS.View.Page3Frame
{
    /// <summary>
    /// Interaction logic for BelongingsFrame.xaml
    /// </summary>
    public partial class BelongingsFrame : Page
    {
        public BelongingsFrame()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;
            string Additional = AdditionalTextBox.Text;

            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Заповніть поле відомча належність!");
                return;
            }
            else
            {
                using (EcomonitoringdbContext context = new EcomonitoringdbContext())
                {
                    var newBelonging = new Belonging
                    {
                        Name = Name,
                    };

                    try
                    {
                        context.Belongings.Add(newBelonging);
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
            AdditionalTextBox.Clear();
        }        
    }
}
