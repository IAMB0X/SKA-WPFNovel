using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SKA_Novel.Classes.Game
{
    internal class CharacterView : System.Windows.Controls.Image
    {
        public Character Character { get; } 
        public SolidColorBrush CharacterColor; 
        public string Emotion = "neutral"; 
        public byte CurrentPosition;

        public DispatcherTimer AnimationTimer = new DispatcherTimer();
        private List<string> animationSprites;
        private int animationIndex = 0;

        public CharacterView(Character character, string characterColor, byte position)
        {
            Character = character;
            CharacterColor = (SolidColorBrush)new BrushConverter().ConvertFrom(characterColor);
            CurrentPosition = --position;
            SetPosition();
        }

        public void SetAnimation(List<string> sprites, int speedMilliseconds)
        {
            animationSprites = sprites;
            AnimationTimer.Interval = TimeSpan.FromMilliseconds(speedMilliseconds);
            AnimationTimer.Tick += UpdateSprite;
            AnimationTimer.Start();
        }

        public void StopAnimation()
        {
            AnimationTimer.Stop();
            animationIndex = 0;
            animationSprites = null;
        }

        public void UpdateSprite(object sender, EventArgs e)
        {
            if (animationIndex + 1 < animationSprites.Count)
                animationIndex++;
            else
                animationIndex = 0;

            UpdateImageSource(animationSprites[animationIndex]);
        }
        public void UpdateEmotion(string emotionName)
        {
            Margin = new System.Windows.Thickness(50);
            Emotion = emotionName;
            UpdateImageSource(Emotion);

            DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(30) };
            timer.Tick += SetSmallImage;
            timer.Start();
        }

        private void SetSmallImage(object sender, EventArgs e)
        {
            if (Margin.Top <= 10)
                (sender as DispatcherTimer).Stop();

            Margin = new System.Windows.Thickness(Margin.Top - 10);
        }

        private void UpdateImageSource(string imageName)
        {
            if (CurrentPosition == 0)
                FlowDirection = System.Windows.FlowDirection.RightToLeft;
            else 
                FlowDirection = System.Windows.FlowDirection.LeftToRight;

            Source = new BitmapImage(new Uri(Technical.MediaHelper.ImagesDirectory + "\\" + Character.FullName.ToUpper() + "\\" + imageName + ".png"));
        }

        public void SetPosition()
        {
            UpdateImageSource(Emotion);
            Technical.ControlsManager.HeroPositions[CurrentPosition].Children.Add(this);
        }

        public void SetBlackout()
        {
            UpdateImageSource(Emotion + "_blackout");
        }

        public void TakeOffBlackout()
        {
            UpdateImageSource(Emotion);
        }

        public void MirrorImage()
        {
            if (FlowDirection == System.Windows.FlowDirection.LeftToRight)
                FlowDirection = System.Windows.FlowDirection.RightToLeft;
            else
                FlowDirection = System.Windows.FlowDirection.LeftToRight;
        }
    }
}
