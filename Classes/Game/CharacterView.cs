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

        public DispatcherTimer AnimationTimer = new DispatcherTimer(); //Таймер анимации
        private List<string> animationSprites;                         //Лист спрайтов анимации, он же список кадров
        public int animationIndex = 0;                                //Индекс анимации, он же номер текущий кадр. Далее индекс кадра.
        public bool animationcycle;

        public CharacterView(Character character, string characterColor, byte position)
        {
            Character = character;
            CharacterColor = (SolidColorBrush)new BrushConverter().ConvertFrom(characterColor);
            CurrentPosition = --position;
            SetPosition();
        }

        public void SetAnimation(List<string> sprites, int speedMilliseconds, bool animationLoop)
        {
            animationcycle = animationLoop;
            animationSprites = sprites;                                             //Получает спрайты для анимации
            AnimationTimer.Interval = TimeSpan.FromMilliseconds(speedMilliseconds); //Задает интервал времени между спрайтами
            AnimationTimer.Tick += UpdateSprite;                                    //По истечению времени таймера обновляет спрайт 
            AnimationTimer.Start();                                                 //Стартует таймер анимации
        }

        public void StopAnimation()     // Остановка анимации
        {
            AnimationTimer.Stop();      //Останавливает таймер анимации
            animationIndex = 0;         //Задаёт индекс кадра = 0 
            animationSprites = null;    //Обнуляет спрайты анимации
        }

        public void UpdateSprite(object sender, EventArgs e)    // Обновление спрайта/кадра в анимации
        {
            UpdateImageSource(animationSprites[animationIndex]); //Обновляет кадр обращаясь к списку спрайтов и индексу кадра, который мы ранее задали = 0

            if (animationIndex + 1 < animationSprites.Count)     //Если "индекс кадра" + 1 < общее количество кадров
                    animationIndex++;                                //То прибавляет +1 к индексу кадра 
                else
                    if (animationcycle)
                    animationIndex = 0;                              //Задает индекс кадра = 0, что по сути возвращает к первому кадру
                else
                    StopAnimation();
            
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
