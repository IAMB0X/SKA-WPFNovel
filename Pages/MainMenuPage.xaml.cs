using SKA_Novel.Classes.Technical;
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
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
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

        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.White;
            (sender as TextBlock).Effect = null;
        }

        private void btClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btLoadGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            menuSectionFrame.Navigate(new SavesPage());
            //MediaHelper.LoadGame();
            //ControlsManager.MainMenuFrame.Visibility = Visibility.Collapsed;
        }

        private void btStartGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlsManager.MainMenuFrame.Visibility = Visibility.Collapsed;
            StoryCompilator.GoNextFile("Startfile"); // Стартовый файл истории, сейчас: тестовый
            StoryCompilator.GoNextLine();
        }

        private void btSaveGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MediaHelper.SaveGame(false);
            new ModalWindows.SaveSuccessWindow().ShowDialog();
        }

        private void btSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
