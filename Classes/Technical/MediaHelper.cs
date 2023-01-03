﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace SKA_Novel.Classes.Technical
{
    public static class MediaHelper
    {
        public static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "Resources\\";
        public static readonly string ImagesDirectory = BaseDirectory + "Images\\";
        public static readonly string BackgroundsDirectory = ImagesDirectory + "Backgrounds\\";
        public static readonly string FilesDirectory = BaseDirectory + "Files\\";
        public static readonly string MusicDirectory = BaseDirectory + "Music\\";
        public static string CurrentFile;
        public static string CurrentMusic;
        public static string CurrentBackground;


        public static MediaPlayer MainMusicPlayer = new MediaPlayer();
        public static MediaPlayer MainSoundPlayer = new MediaPlayer();

        public static string GetTextFromFile(string fileName)
        {
            CurrentFile = fileName;
            StreamReader reader = new StreamReader(FilesDirectory + fileName + ".txt");
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        public static string[] BeatStringToLines(string targetText, string separator = "\r\n")
        {
            return targetText.Split(new string[] {separator}, StringSplitOptions.None);
        }

        public static void SetBackground(string backgroundName)
        {
            CurrentBackground = backgroundName;
            ControlsManager.AppMainWindow.Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(BackgroundsDirectory + backgroundName))
            };
        }

        public static void SetGameMusic(string musicName)
        {
            CurrentMusic = musicName;
            MainMusicPlayer.Stop();
            MainMusicPlayer.Open(new Uri(MusicDirectory + musicName));

            MainMusicPlayer.MediaEnded += MusicFinish;
            MainMusicPlayer.Play();
        }

        public static void SetSound(string soundName)
        {
            MainSoundPlayer.Stop();
            MainSoundPlayer.Open(new Uri(MusicDirectory + soundName));

            MainSoundPlayer.Play();
        }

        private static void MusicFinish(object sender, EventArgs e)
        {
            MainMusicPlayer.Position = TimeSpan.Zero;
            MainMusicPlayer.Play();
        }

        public static void SaveGame()
        {
            StreamWriter writer = new StreamWriter(FilesDirectory + "\\SystemFiles\\Save.txt");
            writer.WriteLine(CurrentFile);
            writer.WriteLine(StoryCompilator.LineOfStory);
            writer.WriteLine(ControlsManager.KarmaLevel);
            writer.Close();
        }

        public static void LoadGame()
        {
            StreamReader reader = new StreamReader(FilesDirectory + "\\SystemFiles\\Save.txt");
            StoryCompilator.GoNextFile(reader.ReadLine().Trim());
            int lastString = Convert.ToInt16(reader.ReadLine());
            StoryCompilator.LineOfStory = 0;
            ControlsManager.KarmaLevel = Convert.ToInt16(reader.ReadLine());
            reader.Close();

            while (StoryCompilator.LineOfStory < lastString)
            {
                if ('*' == StoryCompilator.CurrentStory[StoryCompilator.LineOfStory][0])
                    if ("*SetBack" == StoryCompilator.CurrentStory[StoryCompilator.LineOfStory].Trim().Substring(0, 8))
                        SetBackground(StoryCompilator.GetArguments(StoryCompilator.CurrentStory[StoryCompilator.LineOfStory].Trim()));

                StoryCompilator.GoNextLine();
            }
        }
    }
}