using SKA_Novel.Classes.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SKA_Novel.Classes.Game
{
    internal class GameOption : System.Windows.Controls.TextBlock
    {
        public readonly string TargetFile;
        public readonly int KarmaWeight;

        public GameOption(string fileName, int karmaWeight, string optionText)
        {
            TargetFile = fileName;
            KarmaWeight = karmaWeight;
            Text = optionText;
            MouseDown += GameOption_MouseDown;
            MouseEnter += GameOption_MouseEnter;
            MouseLeave += GameOption_MouseLeave;

            TextAlignment = TextAlignment.Center;
            TextWrapping = TextWrapping.Wrap;
            FontSize = 42;
            FontWeight = FontWeights.Bold;
            FontStyle = FontStyles.Italic;
            Foreground = Brushes.White;
            Margin = new Thickness(20);
        }


        private void GameOption_MouseEnter(object sender, MouseEventArgs e)
        {
            Foreground = Brushes.Yellow;
        }

        private void GameOption_MouseLeave(object sender, MouseEventArgs e)
        {
            Foreground = Brushes.White;
        }

        private void GameOption_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Technical.ControlsManager.KarmaLevel += KarmaWeight;
            Technical.StoryCompilator.GoNextFile(TargetFile);
            Technical.ControlsManager.OptionPanel.Children.Clear();
            Technical.ControlsManager.MainText.Visibility = Visibility.Visible;
            Technical.ControlsManager.SpeakerName.Visibility = Visibility.Visible;
            Technical.StoryCompilator.GoNextLine();
            MainWindow.AllowKeys = true;
        }
    }
}
