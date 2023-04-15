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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SKA_Novel.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void btDefaultSettings_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.White;
            (sender as TextBlock).Effect = new DropShadowEffect
            {
                Color = new Color { R = 255, G = 255, B = 255 },
                Direction = 320,
                ShadowDepth = 5,
                Opacity = 0.5,
                BlurRadius = 10
            };
        }

        private void btDefaultSettings_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.White;
            (sender as TextBlock).Effect = null;
        }

        private void btDefaultSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void slTypeInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Classes.Technical.TypingTimer.Interval = (int)slTypeInterval.Value;
        }

        private void slLayoutSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Узнаем текущий масштаб Windows
            var scaleX = VisualTreeHelper.GetDpi(this).DpiScaleX;
            var scaleY = VisualTreeHelper.GetDpi(this).DpiScaleY;
            double size = slLayoutSize.Value / 100;
            // Трансформируем контент окна до масштаба 100%
            ((UIElement)(App.Current.MainWindow.Content)).RenderTransform = new ScaleTransform(size / scaleX, size / scaleY);
        }
    }
}
