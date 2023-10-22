﻿using SKA_Novel.Classes.Game;
using SKA_Novel.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SKA_Novel.Classes.Technical
{
    public static class MediaHelper
    {
        public static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "Resources\\";
        public static readonly string ImagesDirectory = BaseDirectory + "Images\\";
        public static readonly string BackgroundDirectory = ImagesDirectory + "Backgrounds\\";
        public static readonly string CutsceneDirectory = ImagesDirectory + "Cutscene\\";
        public static readonly string FilesDirectory = BaseDirectory + "Files\\";
        public static readonly string SaveDirectory = FilesDirectory + "SystemFiles\\Saves\\";
        public static readonly string AudioDirectory = BaseDirectory + "Audio\\";
        public static readonly string MusicDirectory = AudioDirectory + "Music\\" ;
        public static readonly string SoundDirectory = AudioDirectory + "Sound\\";
        public static readonly string EnvDirectory = AudioDirectory + "Enviroment\\";
        public static string CurrentFile;
        public static string CurrentMusic;
        public static string CurrentEnviroment;
        public static string CurrentBackground { get; set; }

        public static int frameIndex = 0;
        public static List<string> EffectFrames;

        public static DispatcherTimer EffectTimer = new DispatcherTimer();

        public static MediaPlayer MainMusicPlayer = new MediaPlayer();
        public static MediaPlayer MainSoundPlayer = new MediaPlayer();
        public static MediaPlayer MainEnvPlayer   = new MediaPlayer();

        public static string GetTextFromFile(string fileName)
        {
            CurrentFile = fileName.Trim();
            StreamReader reader = new StreamReader(FilesDirectory + fileName.Trim() + ".txt");
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
            FileModule backgroundFile = new FileModule();
            if (backgroundFile.CheckFile(backgroundName, BackgroundDirectory))
            {
                ControlsManager.AppMainWindow.Background = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(backgroundFile.CheckedFile))
                };
                ControlsManager.BackgroundVideo.Stop();
                ControlsManager.BackgroundVideo.Visibility = Visibility.Hidden;
            }    
        }

        public static void SetVideo(string videoName)
        {
            FileModule videoFile = new FileModule();
            videoFile.CheckFile(videoName, BackgroundDirectory);
            if (videoFile.CheckFile(videoName, BackgroundDirectory))
            {
                ControlsManager.BackgroundVideo.Visibility = Visibility.Visible;
                ControlsManager.BackgroundVideo.Source = new Uri (videoFile.CheckedFile);
                ControlsManager.BackgroundVideo.MediaEnded += VideoFinish;
                ControlsManager.BackgroundVideo.Play();
            };
        }

        private static void VideoFinish(object sender, EventArgs e)
        {
            ControlsManager.BackgroundVideo.Position = TimeSpan.Zero;
            ControlsManager.BackgroundVideo.Play();
        }

        public static void SetEffectAnimation()
        {
            int speed = 10;
            byte framesCount = 34;
            byte spriteNumber = 0;

            List<string> sprites = new List<string>();      //Лист спрайтов

            while (framesCount != 0)
            {
                framesCount--;                                                                           //Вычитаем из общего кол-ва кадров -1
                spriteNumber++;                                                                          //Прибавляем к номеру спрайта 1
                string currentLine = "Filmgrain" + "_" + Convert.ToString(spriteNumber);
                sprites.Add(currentLine);                                                            //То добавляем имя спрайта
            }
            EffectFrames = sprites;                                             //Получает спрайты для анимации
            EffectTimer.Interval = TimeSpan.FromMilliseconds(speed);            //Задает интервал времени между спрайтами
            EffectTimer.Tick += UpdateFrame;                                    //По истечению времени таймера обновляет спрайт 
            EffectTimer.Start();                                                //Стартует таймер анимации
        }

        public static void UpdateFrame(object sender, EventArgs e)
        {
            ControlsManager.EffectScreen.Background = new ImageBrush(new BitmapImage(new Uri(BaseDirectory + "Effect\\" + "Filmgrain\\" + EffectFrames[frameIndex] + ".png")));

            if (frameIndex + 1 < EffectFrames.Count)     
                frameIndex++;                                
            else
                frameIndex = 0;                             
        }
        public static void SetCutscene (string videoName)
        {
            FileModule videoFile = new FileModule();
            videoFile.CheckFile(videoName, CutsceneDirectory);
            if (videoFile.CheckFile(videoName, CutsceneDirectory))
            {
                MainWindow.AllowKeys = false;
                MainMusicPlayer.Stop();
                MainSoundPlayer.Stop();
                MainEnvPlayer.Stop();
                ControlsManager.Cutscene.Visibility = System.Windows.Visibility.Visible;
                ControlsManager.Cutscene.Source = new Uri(videoFile.CheckedFile);
                ControlsManager.Cutscene.MediaEnded += CutsceneFinish;
                ControlsManager.Cutscene.Play();
            };
        }

        private static void CutsceneFinish(object sender, EventArgs e)
        {
            MainWindow.AllowKeys = true;
            ControlsManager.Cutscene.Visibility = System.Windows.Visibility.Hidden;
            SetGameMusic(CurrentMusic);
            SetEnvSound(CurrentEnviroment);
        }

        public static void SetEnvSound(string envName)              // Звуки фонового окружения - зациклены
        {
            CurrentEnviroment = envName;
            MainEnvPlayer.Stop();                                   //Завершает
            FileModule envFile = new FileModule();
            envFile.CheckFile(envName, EnvDirectory);
            if (envFile.CheckFile(envName, EnvDirectory))
            {
                MainEnvPlayer.Open(new Uri(envFile.CheckedFile));    //Открывает из своей директории название файла

                MainEnvPlayer.MediaEnded += EnvFinish;                  //Медиа заканчивается обращается к EnvFinish
                MainEnvPlayer.Play();                                   //Воспроизводит

            }
        }

        private static void EnvFinish(object sender, EventArgs e)   // Звуки фонового окружения заново
        {
            MainEnvPlayer.Position = TimeSpan.Zero;                 //Перематывает на начало 0:00
            MainEnvPlayer.Play();                                   //Воспроизводит
        }

        public static void SetSound(string soundName)               // Звуки - не зациклены
                                                                    //Работает также
        {
            MainSoundPlayer.Stop();
            FileModule soundFile = new FileModule();
            soundFile.CheckFile(soundName, SoundDirectory);
            if (soundFile.CheckFile(soundName, SoundDirectory))
            {
                MainSoundPlayer.Open(new Uri(soundFile.CheckedFile));
                MainSoundPlayer.Play();
            }
        }

        public static void SetGameMusic(string musicName)           // Музыка - зациклена
        {                                                           //Работает также
            CurrentMusic = musicName;
            MainMusicPlayer.Stop();
            FileModule musicFile = new FileModule();
            if (musicFile.CheckFile(musicName, MusicDirectory))
            {
                MainMusicPlayer.Open(new Uri(musicFile.CheckedFile));
                MainMusicPlayer.MediaEnded += MusicFinish;
                MainMusicPlayer.Play();
            }
        }

        private static void MusicFinish(object sender, EventArgs e)
        {
            MainMusicPlayer.Position = TimeSpan.Zero;
            MainMusicPlayer.Play();
        }

        public static void SaveGame(string saveName = "QuickSave.txt")
        {
            FileStream stream = new FileStream(SaveDirectory + saveName, FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, new DataToSave(saveName, CurrentFile, CurrentBackground,
                                        StoryCompilator.LineOfStory, ControlsManager.KarmaLevel,
                                        DateTime.Now, ControlsManager.OptionResults));
            stream.Close();
        }

        public static void LoadGame(string saveName = "QuickSave.txt")
        {

			FileStream stream = new FileStream(SaveDirectory + saveName, FileMode.OpenOrCreate);
			BinaryFormatter bf = new BinaryFormatter();
            DataToSave data = (DataToSave)bf.Deserialize(stream);
            stream.Close();

            StoryCompilator.GoNextFile(data.File);
            int lastString = data.Line;
            StoryCompilator.LineOfStory = 0;
            ControlsManager.KarmaLevel = data.KarmaLevel;

            while (StoryCompilator.LineOfStory < lastString)
				StoryCompilator.GoNextLine();

			ControlsManager.DarkScreenTimer.Stop();
			ControlsManager.DarkScreen.Visibility = Visibility.Collapsed;
            SetBackground(CurrentBackground);
			ControlsManager.MainText.Visibility = Visibility.Visible;
			ControlsManager.SpeakerName.Visibility = Visibility.Visible;
			ControlsManager.OptionPanel.Children.Clear();
			MainWindow.AllowKeys = true;
		}
    }
}
