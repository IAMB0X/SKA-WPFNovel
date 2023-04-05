﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;
using SKA_Novel.Classes.Technical;



namespace SKA_Novel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double volumeLevel;
        public static bool AllowKeys { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();

            ControlsManager.EffectScreen = EffectScreen;
            ControlsManager.AppMainWindow = this;
            ControlsManager.BackgroundVideo = BackgroundVideo;
            ControlsManager.Cutscene = Cutscene;
            ControlsManager.MainMenuFrame = frameMainMenu;
            ControlsManager.DarkScreen = DarkScreen;
            ControlsManager.OptionPanel = stckPnlOptions;
            ControlsManager.MainTextPanel = brdMainText;
            ControlsManager.MainText = txtMainText;
            ControlsManager.SpeakerName = txtCurrentCharacter;
            ControlsManager.HeroPositions[0] = HeroPosition1;
            ControlsManager.HeroPositions[1] = HeroPosition2;
            ControlsManager.HeroPositions[2] = HeroPosition3;

            MediaHelper.SetEffectAnimation();
            MediaHelper.SetGameMusic("808steps");
            MediaHelper.MainMusicPlayer.Volume = 0.03;

            frameMainMenu.Navigate(new Pages.MainMenuPage());
        }

        private void brdMainText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StoryCompilator.GoNextLine();
        }

        private void btVolume_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Volume.Visibility == Visibility.Visible)
            Volume.Visibility = Visibility.Collapsed;
            else
            Volume.Visibility = Visibility.Visible;
            /*
            if (MediaHelper.MainMusicPlayer.Volume > 0)
            {
                MediaHelper.MainMusicPlayer.Volume = 0;
                MediaHelper.MainSoundPlayer.Volume = 0;
                MediaHelper.MainEnvPlayer.Volume = 0;
                imgVolume.Source = new BitmapImage(new Uri(MediaHelper.ImagesDirectory + "mute.png"));
            }
            else
            {
                MediaHelper.MainMusicPlayer.Volume = 0.05;
                MediaHelper.MainSoundPlayer.Volume = 0.05;
                MediaHelper.MainEnvPlayer.Volume = 0.05;
                imgVolume.Source = new BitmapImage(new Uri(MediaHelper.ImagesDirectory + "volume.png"));
            }
            */
        }

        private void btSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MediaHelper.SaveGame();
            new ModalWindows.SaveSuccessWindow().ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && StoryCompilator.CurrentStory != null)
            {
                if (AllowKeys)
                {
                    frameMainMenu.Visibility = Visibility.Visible;
                    AllowKeys = false;
                    MediaHelper.MainMusicPlayer.Volume = 0;
                    MediaHelper.MainSoundPlayer.Volume = 0;
                    MediaHelper.MainEnvPlayer.Volume = 0;
                }
                else
                {
                    frameMainMenu.Visibility = Visibility.Collapsed;
                    AllowKeys = true;
                    MediaHelper.MainMusicPlayer.Volume = volumeLevel;
                    MediaHelper.MainSoundPlayer.Volume = volumeLevel;
                    MediaHelper.MainEnvPlayer.Volume = volumeLevel;
                }
            }
            else if (AllowKeys)
            {
                if (e.Key == Key.Space || e.Key == Key.Enter)
                    StoryCompilator.GoNextLine();
            }
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volumeLevel = Volume.Value;
            MediaHelper.MainMusicPlayer.Volume = volumeLevel;
            MediaHelper.MainSoundPlayer.Volume = volumeLevel;
            MediaHelper.MainEnvPlayer.Volume = volumeLevel;
        }

        private void btClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
