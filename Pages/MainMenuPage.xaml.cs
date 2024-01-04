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
            mainTitle.Text = StoryCompilator.Title;
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
            brdMainFrame.Visibility = Visibility.Visible;
            menuSectionFrame.Navigate(new SavesPage(false));
        }

        private void btStartGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlsManager.MainText.Visibility = Visibility.Visible;
            ControlsManager.SpeakerName.Visibility = Visibility.Visible;
            ControlsManager.OptionPanel.Children.Clear();
            ControlsManager.MainMenuFrame.Visibility = Visibility.Collapsed;
            MainWindow.AllowKeys = true;
            StoryCompilator.GoNextFile("::Startfile"); // Стартовый файл истории, сейчас: тестовый
            StoryCompilator.GoNextLine();
            StoryCompilator.IsGameStarted = true;
        }

        private void btSaveGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            brdMainFrame.Visibility = Visibility.Visible;
            menuSectionFrame.Navigate(new SavesPage());
        }

        private void btSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            brdMainFrame.Visibility = Visibility.Visible;
            menuSectionFrame.Navigate(new SettingsPage());

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            brdMainFrame.Visibility = Visibility.Collapsed;
        }
    }
}
