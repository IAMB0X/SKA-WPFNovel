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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SKA_Novel.ModalWindows
{
    /// <summary>
    /// Interaction logic for SaveSuccessWindow.xaml
    /// </summary>
    public partial class SaveSuccessWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(600) };

        public SaveSuccessWindow()
        {
            InitializeComponent();
            timer.Tick += CloseWindow;
            timer.Start();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            timer.Stop();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseWindow(sender, e);
        }
    }
}
