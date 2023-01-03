﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SKA_Novel.Classes.Technical
{
    internal class StoryCompilator
    {
        public static int LineOfStory { get; set; } = -1;
        public static string[] CurrentStory { get; set; }

        public delegate void Command(string codeString);

        //{"ChangeBackground", "Изменение заднего фона, принимает имя фона"},
        //{"ChangeMusic", "Изменение музыки, принимает имя музыки (.wav)"},
        //{ "GoNextFile", "Переход к новому файлу, принимает имя файла"}

        public static Dictionary<String, Command> Commands { get; } = new Dictionary<string, Command>()
        {
            {"SetBackground", SetBackground},           // imageName
            {"SetMusic", SetMusic},                     // musicName
            {"GoNextFile", GoNextFile},                 // fileName
            {"CreateOptionBlock", CreateOptionBlock},   // File1(karmaWeight), File2(karmaWeight), .... { Var1, Var2, ... }
            {"AddHero",  AddHero},                      // characterName, characterColor, position
            {"ClearHero", ClearHero},                   // position
            {"SetHeroEmotion", SetHeroEmotion},         // characterName, position, emotionName
            {"GoThisLine", GoThisLine},                 // lineNumber
            {"MirrorHero", MirrorHero},                 // characterName, position
            {"CheckKarma", CheckKarma},                 // needKarmaLevel, lineNumber (go to this line if KarmaLevel < needKarmaLevel)
            {"SetSound", SetSound},                     // soundName
            {"SetHeroAnimation", SetHeroAnimation}      // characterName, position, animationSpeedMilliseconds { Sprite1, Sprite2, ... }
        };


        public static void GoNextLine()
        {
            if (ControlsManager.TypingTimer == null || !ControlsManager.TypingTimer.IsTyping)
            {
                if ((LineOfStory + 1) < CurrentStory.Count())
                    LineOfStory++;

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
            string command = codeString.Substring(1, codeString.IndexOf(':') - 1).Trim();

            foreach (string commandName in Commands.Keys)
                if (command == commandName)
                    Commands[command](codeString);

            GoNextLine();
        }

        public static string GetArguments(string codeString)
        {
            return codeString.Substring(
                codeString.IndexOf(':') + 1,
                codeString.Length - (codeString.IndexOf(':') + 1)).Trim();
        }
        public static void GoThisLine(string codeString)
        {
            LineOfStory = Convert.ToInt16(GetArguments(codeString).Trim()) - 2; // -1 из-за нумерации строк (тут с 0, там с 1)
                                                                                // -1 из-за GoNextLine - переходит на след строку
            GoNextLine();
        }

        public static void SetBackground(string codeString)
        {
            if (MediaHelper.CurrentBackground != GetArguments(codeString))
            {
                MediaHelper.CurrentBackground = GetArguments(codeString);
                MainWindow.AllowKeys = false;
                ControlsManager.DarkScreen.Visibility = System.Windows.Visibility.Visible;
                _helpVariable = codeString;
                ControlsManager.DarkScreenTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(30) };
                ControlsManager.DarkScreenTimer.Tick += ChangeOpacityDarkScreen;
                ControlsManager.DarkScreenTimer.Start();
            }
            
        }

        private static string _helpVariable;
        private static bool _opacityIsGrowing = true;

        private static void ChangeOpacityDarkScreen(object sender, EventArgs e)
        {
            if (ControlsManager.DarkScreen.Opacity < 1 && _opacityIsGrowing)
                ControlsManager.DarkScreen.Opacity += 0.1;
            else
                if (!_opacityIsGrowing)
                ControlsManager.DarkScreen.Opacity -= 0.1;

            if (Math.Round(ControlsManager.DarkScreen.Opacity, 2) == 1)
            {
                MediaHelper.SetBackground(GetArguments(_helpVariable));
                _opacityIsGrowing = false;
            }

            if (Math.Round(ControlsManager.DarkScreen.Opacity, 2) == 0)
            {
                ControlsManager.DarkScreenTimer.Stop();
                ControlsManager.DarkScreen.Visibility = System.Windows.Visibility.Collapsed;
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

        public static void AddHero(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');
            byte position = Convert.ToByte(arguments[2].Trim());

            Game.Character character = new Game.Character(arguments[0]);
            new Game.CharacterView(character, arguments[1].Trim(), position);
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

            foreach(Game.CharacterView character in ControlsManager.HeroPositions[--position].Children)
                if (character.Character.FullName == arguments[0].Trim())
                {
                    character.UpdateEmotion(arguments[2].Trim());
                    break;
                }
        }

        public static void SetHeroAnimation(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');
            byte position = Convert.ToByte(arguments[1].Trim());

            foreach (Game.CharacterView character in ControlsManager.HeroPositions[--position].Children)
                if (character.Character.FullName == arguments[0].Trim())
                {
                    List<string> sprites = new List<string>();

                    LineOfStory++;
                    string currentLine = CurrentStory[LineOfStory];
                    while (currentLine != "}")
                    {
                        LineOfStory++;
                        currentLine = CurrentStory[LineOfStory];
                        if (currentLine != "}")
                            sprites.Add(currentLine);
                    }

                    character.SetAnimation(sprites, Convert.ToInt16(arguments[2]));
                    break;
                }
        }

        public static void MirrorHero(string codeString)
        {
            string[] arguments = GetArguments(codeString).Split(',');
            byte position = Convert.ToByte(arguments[1].Trim());

            foreach (Game.CharacterView character in ControlsManager.HeroPositions[--position].Children)
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
                ControlsManager.SpeakerName.Text = "Я:";
                ControlsManager.SpeakerName.Foreground = Brushes.LightBlue;
                foreach (DockPanel heroPosition in ControlsManager.HeroPositions)
                    if (heroPosition.Children.Count > 0)
                        foreach (Game.CharacterView character in heroPosition.Children)
                            character.SetBlackout();
            }
            else
            {
                foreach (DockPanel heroPosition in ControlsManager.HeroPositions)
                    if (heroPosition.Children.Count > 0)
                        foreach (Game.CharacterView character in heroPosition.Children)
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
            if (ControlsManager.KarmaLevel >= needKarma)
                GoThisLine(": " + arguments[1]);
        }

        public static void CreateOptionBlock(string codeString)
        {
            string[] options = GetArguments(codeString).Split(',');
            AddOptions(options);
            ControlsManager.MainTextPanel.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AllowKeys = false;
        }

        private static void AddOptions(string[] optionsFiles)
        {
            for (int i = 0; i < optionsFiles.Count(); i++)
            {
                string option = optionsFiles[i];
                string karma = option.Substring(option.IndexOf('(') + 1, option.IndexOf(')') - (option.IndexOf('(') + 1));
                ControlsManager.OptionPanel.Children.
                    Add(new Game.GameOption(
                        option.Substring(0, option.IndexOf('(')).Trim(),
                        Convert.ToInt16(karma),
                        ReadOptionsText()));
            }
        }

        private static string ReadOptionsText()
        {
            LineOfStory++;

            if (CurrentStory[LineOfStory].Trim()[0] == '{')
            {
                LineOfStory++;
                return CurrentStory[LineOfStory];
            }
            else
                if (CurrentStory[LineOfStory].Trim()[0] == '}')
            {
                LineOfStory++;
                LineOfStory++;
            }

            return CurrentStory[LineOfStory];
        }
    }
}
