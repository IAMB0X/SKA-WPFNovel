﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
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
using SKA_Novel.Classes.Technical;

namespace SKA_Novel.Pages
{
    /// <summary>
    /// Interaction logic for SavesPage.xaml
    /// </summary>
    public partial class SavesPage : Page
    {
        public bool IsSavingPage;

        public SavesPage(bool isSavingPage = true)
        {
            InitializeComponent();
            
            IsSavingPage = isSavingPage;

            if (!IsSavingPage)
                txtTitle.Text = "ЗАГРУЗКИ";

            UpdateSavesList();
        }

        private void UpdateSavesList()
        {
            string[] saveFiles = Directory.GetFiles(MediaHelper.SaveDirectory);

            lvSaves.Items.Clear();

            foreach (string fileName in saveFiles)
            {
                if (fileName != MediaHelper.SaveDirectory + "QuickSave")
                {
                    FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate);
                    BinaryFormatter bf = new BinaryFormatter();
                    DataToSave data = (DataToSave)bf.Deserialize(stream);
                    stream.Close();

					FileModule background = new FileModule();
                    background.CheckFile(data.Background, MediaHelper.BackgroundDirectory);

                    TextBlock time = new TextBlock
                    {
                        Text = data.Time.ToString("dd.MM.yyyy HH:mm"),
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(5),
                        FontSize = 20,
                        FontWeight = FontWeights.Bold
                    };

                    Image img = new Image
                    {
                        Source = new BitmapImage(new Uri(background.CheckedFile)),
                        Margin = new Thickness(5),
                        Height = 100,
                        Cursor = Cursors.Hand,
                        DataContext = data.SaveName
                    };

                    TextBlock delete = new TextBlock
                    {
                        Text = "удалить сохранение",
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 200, 0)),
                        TextAlignment = TextAlignment.Center,
                        FontSize = 12,
                        Cursor = Cursors.Hand,
                        DataContext = fileName
                    };

                    if (IsSavingPage)
                        img.MouseDown += RewriteSaveFile;
                    else
                        img.MouseDown += LoadSaveFile;

                    delete.MouseDown += DeleteSave;

                    StackPanel panel = new StackPanel { Margin = new Thickness(10) };

                    panel.Children.Add(img);
                    panel.Children.Add(time);
                    panel.Children.Add(delete);

                    lvSaves.Items.Add(panel);
                }
            }

            if (StoryCompilator.IsGameStarted && IsSavingPage && lvSaves.Items.Count != 8)
            {
                Image createImg = new Image
                {
                    Source = new BitmapImage(new Uri(MediaHelper.ImagesDirectory + "floppy-disk.png")),
                    Margin = new Thickness(5),
                    Height = 100,
                    Cursor = Cursors.Hand
                };

                TextBlock create = new TextBlock
                {
                    Text = "новое сохранение",
                    Foreground = new SolidColorBrush(Color.FromRgb(155, 255, 0)),
                    TextAlignment = TextAlignment.Center,
                    FontSize = 12,
                    Cursor = Cursors.Hand
                };

                StackPanel createPanel = new StackPanel { Margin = new Thickness(10) };

                createPanel.MouseDown += CreateSave;

                createPanel.Children.Add(createImg);
                createPanel.Children.Add(create);

                lvSaves.Items.Add(createPanel);
            }
        }

        private void DeleteSave(object sender, EventArgs e)
        {
            try
            {
                File.Delete((sender as TextBlock).DataContext.ToString());
                UpdateSavesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RewriteSaveFile(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (StoryCompilator.IsGameStarted && e.ClickCount == 2)
                {
                    MediaHelper.SaveGame((sender as Image).DataContext.ToString());
                    UpdateSavesList();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadSaveFile(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    MediaHelper.LoadGame((sender as Image).DataContext.ToString());
                    ControlsManager.MainMenuFrame.Visibility = Visibility.Collapsed;
                    StoryCompilator.IsGameStarted = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CreateSave(object sender, EventArgs e)
        {
            try
            {
                string saveName = "Save_" + (int)DateTime.Now.TimeOfDay.TotalSeconds;
                MediaHelper.SaveGame(saveName);
                UpdateSavesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btLoadQuickSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MediaHelper.LoadGame();
                ControlsManager.MainMenuFrame.Visibility = Visibility.Collapsed;
                StoryCompilator.IsGameStarted = true;
            }
            catch
            {
                MessageBox.Show("Сохранение ГГ", "Внимание");
            }
        }

        private void btLoadQuickSave_MouseEnter(object sender, MouseEventArgs e)
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

        private void btLoadQuickSave_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.White;
            (sender as TextBlock).Effect = null;
        }
    }
}
