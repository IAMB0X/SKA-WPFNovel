using System;
using System.Collections.Generic;
using System.IO;
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
using SKA_Novel.Classes.Technical;

namespace SKA_Novel.Pages
{
    /// <summary>
    /// Interaction logic for SavesPage.xaml
    /// </summary>
    public partial class SavesPage : Page
    {
        public SavesPage()
        {
            InitializeComponent();

            UpdateSavesList();
        }

        private void UpdateSavesList()
        {
            string[] saveFiles = Directory.GetFiles(MediaHelper.SaveDirectory);

            lvSaves.Items.Clear();

            foreach (string fileName in saveFiles)
            {
                StreamReader reader = new StreamReader(fileName);
                reader.ReadLine(); reader.ReadLine(); reader.ReadLine();

                FileModule background = new FileModule();
                background.CheckFile(reader.ReadLine(), MediaHelper.BackgroundDirectory);

                Image img = new Image
                {
                    Source = new BitmapImage(new Uri(background.CheckedFile)),
                    Margin = new Thickness(5),
                    Height = 100,
                    Cursor = Cursors.Hand
                };

                TextBlock time = new TextBlock
                {
                    Text = reader.ReadLine(),
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold
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

                delete.MouseDown += DeleteSave;

                Border border = new Border
                {
                    BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(10)
                };

                StackPanel panel = new StackPanel { Margin = new Thickness(5) };

                border.Child = panel;
                panel.Children.Add(img);
                panel.Children.Add(time);
                panel.Children.Add(delete);

                lvSaves.Items.Add(border);
            }
        }

        private void DeleteSave(object sender, EventArgs e)
        {
            try
            {
                File.Delete((sender as TextBlock).DataContext.ToString());
                UpdateSavesList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btLoadQuickSave_MouseDown(object sender, MouseButtonEventArgs e)
        {

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
