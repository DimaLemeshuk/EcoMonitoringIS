using EcoMonitoringIS.View;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace EcoMonitoringIS.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new Page1();
            //Page1Button.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x6F, 0x46));

            var timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1); // Оновлювати кожну секунду
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Оновлення даних в текстових полях
            TimeTextBlock.Text = DateTime.Now.ToString("HH:mm");
            DateTextBlock.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private void Page1Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page1();
        }

        private void Page2Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page2();
        }

        private void Page3Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page3();
        }
    }
}
