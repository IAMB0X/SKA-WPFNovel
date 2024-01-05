using SKA_Novel.Classes.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SKA_Novel.Classes.Technical
{
    public static class StoryCompilator
    {
        public static string Title { get; set; } = "Хроники \"НОСТ\"";
        public static string MainHeroName { get; set; } = "Уинстон";
        public static int LineOfStory { get; set; } = -1;
        public static string[] CurrentStory { get; set; }
		public static bool IsGameStarted { get; set; } = false;
		public static int KarmaLevel { get; set; } = 0;
		public static List<GameChoiseResult> OptionResults { get; set; } = new List<GameChoiseResult>();

		public delegate void Command(string codeString);

        //СПРАВКА ПО КОМАНДАМ (*имякоманды )

        //Файлы

        //{"ChangeBackground", "Изменение заднего фона, принимает: имя фона + расширение"},
        //{"SetMusic", "Воспроизведение музыки, принимает: имя музыки + расширение"},
        //{"SetSound", "Воспроизведение звука, принимает: имя звука + расширение"},
        //{"SetEnv", "Воспроизведение окружения, принимает: имя звука окружения + расширение"},
        //{"GoNextFile", "Переход к новому файлу, принимает: имя файла" P.s. можно ходить по директориям через /}

        //Текст

        //{"(MND", Мысли ГГ без имени}
        //{"(I", Слова ГГ с иненем и цветом} 
        //{"(ПЕР", Слова героя "Персонаж", сокращается до 3-ёх букв}

        //{"GoThisLine", "Переход на строку текущ. файла: номер строки "}
        //{"CheckKarma", "Проверка кармы, если ниже то перейти на строку: число кармы, номер строки"

        //Персонажи

        //{"AddHero", "Добавляет героя: имя героя, цвет (HEX), позиция"}
        //{"ClearHero", "Стерает героя с позиции: позиция"}
        //{"SetHeroEmotion", "Меняет эмоцию: имя героя, позиция, название эмоции"}
        //{"MirrorHero", "Отражает героя: имя героя, позиция"}
        //{"SetHeroAnimation", Задаёт анимацию: имя героя, позиция, скорость в милисекундах {Спрайт 1, Спрайт 2, ... } } Потом упрощу, чтобы сам прибавлял Название анимации_++Цифра.png
        //{"StopAnimation", Останавливает анимацию: имя героя, позиция }

        //Выбор

        //{"CreateOptionBlock", "Выбор, отправляет в выбранный файл + число кармы, при соответсвуйщем варианте"}




        public static Dictionary<String, Command> Commands { get; } = new Dictionary<string, Command>()
        {
            {"SetBackground", SetBackground},           // imageName + .extension
            {"SetMusic", SetMusic},                     // musicName + .extension
            {"SetSound", SetSound},                     // soundName + .extension
            {"SetEnv", SetEnv},                         // envName + .extension
            {"GoNextFile", GoNextFile},                 // fileName
            {"CreateChoiseBlock", CreateChoiseBlock},   // File1(karmaWeight), File2(karmaWeight), .... { Var1, Var2, ... }
            {"CreateChoise", CreateChoise },   // File1(karmaWeight), File2(karmaWeight), .... { Var1, Var2, ... }
            {"AddHero",  AddHero},                      // characterName, characterColor, position
            {"ClearHero", ClearHero},                   // position
            {"SetHeroEmotion", SetHeroEmotion},         // characterName, position, emotionName
            {"GoThisLine", GoThisLine},                 // lineNumber
            {"MirrorHero", MirrorHero},                 // characterName, position
            {"CheckKarma", CheckKarma},                 // needKarmaLevel, lineNumber (go to this line if KarmaLevel < needKarmaLevel)
            {"CheckChoise", CheckChoise},                 // needKarmaLevel, lineNumber (go to this line if KarmaLevel < needKarmaLevel)
            {"SetHeroAnimation", SetHeroAnimation},     // characterName, position, animationSpeedMilliseconds { Sprite1, Sprite2, ... }
            {"StopAnimation", StopAnimation},           // characterName, position
            {"SetVideo", SetVideo},                     // videoName
            {"SetCutscene", SetCutscene},               // videoName
        };


        public static void GoNextLine()
        {
            if (ControlsManager.TypingTimer == null || !ControlsManager.TypingTimer.IsTyping)
            {
                if ((LineOfStory + 1) < CurrentStory.Count())
                    LineOfStory++;

                if (string.IsNullOrWhiteSpace(CurrentStory[LineOfStory].Trim()))
                {
                    GoNextLine();
                    return;
                }

                if (CurrentStory[LineOfStory].Trim()[0] == '(')
                    UpdateSpeaker(CurrentStory[LineOfStory].Trim());

                if (CurrentStory[LineOfStory].Trim()[0] == '*')
                    CompilateString(CurrentStory[LineOfStory]);

                new TypingTimer(ControlsManager.MainText, CurrentStory[LineOfStory]);
            }
            else
                if (ControlsManager.TypingTimer != null)
                ControlsManager.TypingTimer.FinishTyping();
        }

        private static void CompilateString(string codeString)
        {
            string command = codeString.Substring(1).Trim();

            if (command.Contains("::"))
                command = command.Substring(0, command.IndexOf("::") - 1);

			foreach (string commandName in Commands.Keys)
                if (command == commandName)
                    Commands[command](codeString);

            GoNextLine();
        }

        public static string GetArguments(string codeString)
        {
            if (codeString.Contains("::"))
				return codeString.Substring(codeString.IndexOf("::") + 2);
			else
				return codeString;
		}
        public static void GoThisLine(string codeString)
        {
            LineOfStory = Convert.ToInt16(GetArguments(codeString).Trim()) - 2; // -1 из-за нумерации строк (тут с 0, там с 1)
                                                                                // -1 из-за GoNextLine - переходит на след строку
            GoNextLine();
        }

        public static void SetBackground(string codeString)
        {
                DarkScreen(GetArguments(codeString));
        }

        public static void SetVideo(string codeString)
        {
            MediaHelper.SetVideo(GetArguments(codeString));
        }


        public static void SetCutscene(string codeString)
        {
            MediaHelper.SetCutscene(GetArguments(codeString));
        }

        public static void DarkScreen (string background)
        {
            if (MediaHelper.CurrentBackground != GetArguments(background))
            {
				string[] arguments = GetArguments(background).Split(',');
				MediaHelper.CurrentBackground = GetArguments(arguments[0]);
                MainWindow.AllowKeys = false;

                if (arguments.Length > 1)
                    ControlsManager.DarkScreen.Background = new SolidColorBrush(
                                                (Color)ColorConverter.ConvertFromString(arguments[1]));
                else
                    ControlsManager.DarkScreen.Background = Brushes.Black;

				ControlsManager.DarkScreen.Visibility = Visibility.Visible;
                ControlsManager.DarkScreen.Opacity = 0;
				_helpVariable = arguments[0];
                ControlsManager.DarkScreenTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(10) };
                ControlsManager.DarkScreenTimer.Tick += ChangeOpacityDarkScreen;
                ControlsManager.DarkScreenTimer.Start();
            }
        }

        private static string _helpVariable;
        private static bool _opacityIsGrowing = true;

        private static void ChangeOpacityDarkScreen(object sender, EventArgs e)
        {
			if (ControlsManager.DarkScreen.Opacity < 1 && _opacityIsGrowing)
				ControlsManager.DarkScreen.Opacity += 0.01;
			else
				if (!_opacityIsGrowing)
				ControlsManager.DarkScreen.Opacity -= 0.01;

			if (Math.Round(ControlsManager.DarkScreen.Opacity, 2) == 1)
			{
				ControlsManager.DarkScreen.Opacity = 1;
				MediaHelper.SetBackground(_helpVariable);
				_opacityIsGrowing = false;
			}

			if (Math.Round(ControlsManager.DarkScreen.Opacity, 2) == 0)
			{
				ControlsManager.DarkScreen.Opacity = 0;
				ControlsManager.DarkScreenTimer.Stop();
				ControlsManager.DarkScreen.Visibility = Visibility.Collapsed;
				_opacityIsGrowing = true;
				MainWindow.AllowKeys = true;
				ControlsManager.AppMainWindow.Focus();
			}

			ControlsManager.DarkScreen.Opacity = Math.Round(ControlsManager.DarkScreen.Opacity, 2);
		}

        public static void SetMusic(string codeString)
        {
            if (MediaHelper.CurrentMusic != GetArguments(codeString))
            {
                MediaHelper.SetGameMusic(GetArguments(codeString));
            }
        }

        public static void SetSound(string codeString)
        {
            MediaHelper.SetSound(GetArguments(codeString));
        }

        public static void SetEnv(string codeString)
        {
                MediaHelper.SetEnvSound(GetArguments(codeString));
        }

        public static void AddHero(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');
            byte position = Convert.ToByte(arguments[2].Trim());

            Character character = new Character(arguments[0].Trim());
            new CharacterView(character, arguments[1].Trim(), position);
        }

        public static void ClearHero(string codeString)
        {
            byte position = Convert.ToByte(GetArguments(codeString));

            ControlsManager.HeroPositions[--position].Children.Clear();
        }

        public static void SetHeroEmotion(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');
            byte position = Convert.ToByte(arguments[1].Trim());

            foreach(CharacterView character in ControlsManager.HeroPositions[--position].Children)
                if (character.Character.FullName == arguments[0].Trim())
                {
                    character.UpdateEmotion(arguments[2].Trim());
                    break;
                }
        }

        public static void SetHeroAnimation(string codeString)      // Анимация
        {
            string[] arguments = GetArguments(codeString).Split(',');                                   //Разбивает текст на аргументы разделяемые ","
            byte position = Convert.ToByte(arguments[1].Trim());                                        //Определяет позицию из 1-го аргумента
            byte framesCount = Convert.ToByte(arguments[3].Trim());
            byte spriteNumber = 0;
            bool animationLoop = (arguments[4].Trim() == "loop");

            foreach (CharacterView character in ControlsManager.HeroPositions[--position].Children)
                if (character.Character.FullName == arguments[0].Trim())                                //Если имя соответствуем имени в аргументе 0, то
                {
                    List<string> sprites = new List<string>();      //Лист спрайтов

                    LineOfStory++;
                    string currentLine = CurrentStory[LineOfStory];
                    while (currentLine != "}")
                    {
                        LineOfStory++;
                        while (framesCount != 0) 
                            {
                                framesCount--;                                                                           //Вычитаем из общего кол-ва кадров -1
                                spriteNumber++;                                                                          //Прибавляем к номеру спрайта 1
                                currentLine = "anim/" + CurrentStory[LineOfStory] + "_" + Convert.ToString(spriteNumber);//Преобразуем текст строки в название спрайта
                                if (currentLine != "}")                                                                  //Если строка не "}"
                                    sprites.Add(currentLine);                                                            //То добавляем имя спрайта
                            }
                        character.SetAnimation(sprites, Convert.ToInt16(arguments[2]), animationLoop);
                        LineOfStory++;
                        break;
                    }
                    
                }

        }

        public static void StopAnimation(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');                                   //Разбивает текст на аргументы разделяемые ","
            byte position = Convert.ToByte(arguments[1].Trim());                                        //Определяет позицию из 1-го аргумента

            foreach (CharacterView character in ControlsManager.HeroPositions[--position].Children)
                if (character.Character.FullName == arguments[0].Trim())
                {
                    character.StopAnimation();
                break;
                }

        }

        public static void MirrorHero(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');
            byte position = Convert.ToByte(arguments[1].Trim());

            foreach (CharacterView character in ControlsManager.HeroPositions[--position].Children)
                if (character.Character.FullName == arguments[0].Trim())
                {
                    character.MirrorImage();
                    break;
                }
        }

        public static void UpdateSpeaker(string speakerString)
        {
            string shortName = speakerString.Substring(1, speakerString.Trim().Length - 1).ToUpper();

            if (shortName == "MND")
            {
                ControlsManager.SpeakerName.Text = "";
                foreach (DockPanel heroPosition in ControlsManager.HeroPositions)
                    if (heroPosition.Children.Count > 0)
                        foreach (Game.CharacterView character in heroPosition.Children)
                            character.TakeOffBlackout();
            }
            else if (shortName == "I")
            {
                ControlsManager.SpeakerName.Text = MainHeroName + ":";
                ControlsManager.SpeakerName.Foreground = Brushes.LightBlue;
                foreach (DockPanel heroPosition in ControlsManager.HeroPositions)
                    if (heroPosition.Children.Count > 0)
                        foreach (CharacterView character in heroPosition.Children)
                            character.SetBlackout();
            }
            else
            {
                foreach (DockPanel heroPosition in ControlsManager.HeroPositions)
                    if (heroPosition.Children.Count > 0)
                        foreach (CharacterView character in heroPosition.Children)
                            if (character.Character.ShortName == shortName)
                            {
                                ControlsManager.SpeakerName.Foreground = character.CharacterColor;
                                ControlsManager.SpeakerName.Text = character.Character.FullName.ToUpper();
                                character.TakeOffBlackout();
                            }
                            else 
                            {
                                character.SetBlackout();
                            }
            }
            LineOfStory++;
        }

        public static void GoNextFile(string codeString)
        {
            CurrentStory = MediaHelper.BeatStringToLines(MediaHelper.GetTextFromFile(GetArguments(codeString)));
            LineOfStory = -1;
        }

        public static void CheckKarma(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');
            int needKarma = Convert.ToInt16(arguments[0]);
            if (KarmaLevel < needKarma)
                GoThisLine("::" + arguments[1]);
        }
        public static void CheckChoise(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(new string[] { "=>" }, StringSplitOptions.None);
            string title = arguments[0].Split('[')[0].Trim();
            string result = arguments[0].Split('[')[1].Trim().Substring(0, arguments[0].Split('[')[1].Trim().Length - 1);
            if (OptionResults.FirstOrDefault(u => u.Title.ToLower().Equals(title.ToLower()) && u.Result.ToLower().Equals(result.ToLower())) != null)
                GoNextFile("::" + arguments[1]);
        }

        public static void CreateChoiseBlock(string codeString)
        {
			string title = GetArguments(codeString).Trim();

			LineOfStory++;
			while (CurrentStory[LineOfStory].Trim()[0] != '}')
			{
				if (CurrentStory[LineOfStory].Trim()[0] == '{')
				{
					LineOfStory++;
					CreateChoiseValue(title);
				}
				else
					CreateChoiseValue(title);

				LineOfStory++;
			}

			ControlsManager.MainText.Visibility = Visibility.Hidden;
			ControlsManager.SpeakerName.Visibility = Visibility.Hidden;
			MainWindow.AllowKeys = false;
		}

		public static void CreateChoise(string codeString)
		{
			LineOfStory++;
            while (CurrentStory[LineOfStory].Trim()[0] != '}')
            {
                if (CurrentStory[LineOfStory].Trim()[0] == '{')
                {
                    LineOfStory++;
                    CreateChoise();
                }
                else
					CreateChoise();

				LineOfStory++;
            }

            ControlsManager.MainText.Visibility = Visibility.Hidden;
			ControlsManager.SpeakerName.Visibility = Visibility.Hidden;
			MainWindow.AllowKeys = false;
		}

		private static void CreateChoiseValue(string title)
		{
			string[] choiseContent = CurrentStory[LineOfStory].Split(new string[] { "::" }, StringSplitOptions.None);
			string[] choiseProperties = choiseContent[0].Split(new string[] { "=>" }, StringSplitOptions.None);
			string[] choiseHeader = null;

			if (choiseProperties[0].Contains('(') && choiseProperties[0].Contains(')'))
				choiseHeader = choiseProperties[0].Split('(');

			string content = choiseContent[1].Trim();
			string fileName = choiseProperties[1].Trim();

			if (choiseHeader != null)
				ControlsManager.OptionPanel.Children.
					Add(new GameChoiseView(fileName, content, title, choiseHeader[0].Trim(),
					Convert.ToInt16(choiseHeader[1].Trim().Substring(0, choiseHeader[1].Trim().Length - 1))));
			else
				ControlsManager.OptionPanel.Children.
					Add(new GameChoiseView(fileName, content, title, choiseProperties[0].Trim()));
		}

		private static void CreateChoise()
		{
			string[] choiseContent = CurrentStory[LineOfStory].Split(new string[] { "::" }, StringSplitOptions.None);

			string content = choiseContent[1].Trim();
			string fileName = choiseContent[0].Trim();

			ControlsManager.OptionPanel.Children.
				Add(new GameChoiseView(fileName, content));
		}
	}
}
