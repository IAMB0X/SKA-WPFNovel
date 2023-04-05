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
    /// Interaction logic for SavesPage.xaml
    /// </summary>
    public partial class SavesPage : Page
    {
        public SavesPage()
        {
            InitializeComponent();


        }

        private void btLoadQuickSave_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btLoadQuickSave_MouseEnter(object sender, MouseEventArgs e)
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

        private void btLoadQuickSave_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.White;
            (sender as TextBlock).Effect = null;
        }
    }
}
