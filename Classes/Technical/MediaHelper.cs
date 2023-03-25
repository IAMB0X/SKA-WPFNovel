using SKA_Novel.Classes.Game;
using SKA_Novel.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static readonly string AudioDirectory = BaseDirectory + "Audio\\";
        public static readonly string MusicDirectory = AudioDirectory + "Music\\" ;
        public static readonly string SoundDirectory = AudioDirectory + "Sound\\";
        public static readonly string EnvDirectory = AudioDirectory + "Enviroment\\";
        public static string CurrentFile;
        public static string CurrentMusic;
        public static string CurrentEnviroment;
        public static string CurrentBackground;

        public static int frameIndex = 0;
        public static List<string> EffectFrames;

        public static DispatcherTimer EffectTimer = new DispatcherTimer();


        public static MediaPlayer MainMusicPlayer = new MediaPlayer();
        public static MediaPlayer MainSoundPlayer = new MediaPlayer();
        public static MediaPlayer MainEnvPlayer   = new MediaPlayer();

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
            FileModule backgroundFile = new FileModule();
            backgroundFile.FileCheck(backgroundName, BackgroundDirectory);
            if (backgroundFile.FileCheck(backgroundName, BackgroundDirectory))
            {
                ControlsManager.AppMainWindow.Background = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(backgroundFile.checkedFile))
                };
                ControlsManager.BackgroundVideo.Stop();
                ControlsManager.BackgroundVideo.Visibility = System.Windows.Visibility.Hidden;
                StoryCompilator.AnimationBackground(backgroundFile.checkedFile);
            }    
        }

        public static void SetVideo(string videoName)
        {
            FileModule videoFile = new FileModule();
            videoFile.FileCheck(videoName, BackgroundDirectory);
            if (videoFile.FileCheck(videoName, BackgroundDirectory))
            {
                ControlsManager.BackgroundVideo.Visibility = System.Windows.Visibility.Visible;
                ControlsManager.BackgroundVideo.Source = new Uri (videoFile.checkedFile);
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
        public static void Cutscene (string videoName)
        {
            FileModule videoFile = new FileModule();
            videoFile.FileCheck(videoName, CutsceneDirectory);
            if (videoFile.FileCheck(videoName, CutsceneDirectory))
            {
                MainWindow.AllowKeys = false;
                MainMusicPlayer.Stop();
                MainSoundPlayer.Stop();
                MainEnvPlayer.Stop();
                ControlsManager.Cutscene.Visibility = System.Windows.Visibility.Visible;
                ControlsManager.Cutscene.Source = new Uri(videoFile.checkedFile);
                ControlsManager.Cutscene.MediaEnded += CutsceneFinish;
                ControlsManager.Cutscene.Play();
            };
        }

        private static void CutsceneFinish(object sender, EventArgs e)
        {
            MainWindow.AllowKeys = true;
            ControlsManager.Cutscene.Visibility = System.Windows.Visibility.Hidden;
            SetGameMusic(CurrentMusic);
            SetEnvsound(CurrentEnviroment);
        }

        public static void SetEnvsound(string envName)              // Звуки фонового окружения - зациклены
        {
            CurrentEnviroment = envName;
            MainEnvPlayer.Stop();                                   //Завершает
            FileModule envFile = new FileModule();
            envFile.FileCheck(envName, EnvDirectory);
            if (envFile.FileCheck(envName, EnvDirectory))
            {
                MainEnvPlayer.Open(new Uri(envFile.checkedFile));    //Открывает из своей директории название файла

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
            soundFile.FileCheck(soundName, SoundDirectory);
            if (soundFile.FileCheck(soundName, SoundDirectory))
            {
                MainSoundPlayer.Open(new Uri(soundFile.checkedFile));
                MainSoundPlayer.Play();
            }
        }
        public static void SetGameMusic(string musicName)           // Музыка - зациклена
        {                                                           //Работает также
            CurrentMusic = musicName;
            MainMusicPlayer.Stop();
            FileModule musicFile = new FileModule();
            if (musicFile.FileCheck(musicName, MusicDirectory))
            {
                MainMusicPlayer.Open(new Uri(musicFile.checkedFile));
                MainMusicPlayer.MediaEnded += MusicFinish;
                MainMusicPlayer.Play();
            }
        }
        private static void MusicFinish(object sender, EventArgs e)
        {
            MainMusicPlayer.Position = TimeSpan.Zero;
            MainMusicPlayer.Play();
        }
        public static void SaveGame()                                                           // Сохранение
        {
            StreamWriter writer = new StreamWriter(FilesDirectory + "\\SystemFiles\\Save.txt");
            writer.WriteLine(CurrentFile);                                                      //Записывает текущего название файла
            writer.WriteLine(StoryCompilator.LineOfStory);                                      //Записывает текущий номер строки
            writer.WriteLine(ControlsManager.KarmaLevel);                                       //Записывает текущую карму
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
