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
    internal class GameChoiseView : System.Windows.Controls.TextBlock
    {
        public readonly string Title;
        public readonly string Value;
        public readonly string TargetFile;
        public readonly int KarmaWeight;

        public GameChoiseView( string fileName, string optionText,
                               string title = "", string value = "", int karmaWeight = 0)
        {
            Title = title;
            Value = value;
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
            StoryCompilator.KarmaLevel += KarmaWeight;

            if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Value))
                if (StoryCompilator.OptionResults.FirstOrDefault(u => u.Title.Equals(Title)) != null)
                    StoryCompilator.OptionResults.FirstOrDefault(u => u.Title.Equals(Title)).Result = Value;
                else
                    StoryCompilator.OptionResults.Add(new GameChoiseResult(Title, Value));

            StoryCompilator.GoNextFile(TargetFile);
            ControlsManager.OptionPanel.Children.Clear();
            ControlsManager.MainText.Visibility = Visibility.Visible;
            ControlsManager.SpeakerName.Visibility = Visibility.Visible;
            StoryCompilator.GoNextLine();
            MainWindow.AllowKeys = true;
        }
    }
}
