using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SKA_Novel.Classes.Technical
{
    internal class TypingTimer
    {
        public static int Interval { get; set; } = 30;

        public readonly DispatcherTimer Timer = new DispatcherTimer();

        public bool IsTyping { get; set; } = true;
        private int _letterIndex = 0;
        private int _textLength;
        private TextBlock _targetTextBlock;
        private string _content;
        
        public TypingTimer(TextBlock textBlock, string text)
        {
            Timer.Interval = TimeSpan.FromMilliseconds(Interval);
            _targetTextBlock = textBlock;
            _textLength = text.Length;
            _content = text;
            _targetTextBlock.Text = "";
            Timer.Tick += TypingText;
            Timer.Start();
            if (ControlsManager.TypingTimer != null)
                ControlsManager.TypingTimer.Timer.Stop();
            ControlsManager.TypingTimer = this;
        }

        public void TypingText(object sender, EventArgs e)
        {
            if (_letterIndex != _textLength)
            {
                if (_content[_letterIndex] == '\\' && _content[_letterIndex + 1] == 'r')
                {
                    _targetTextBlock.Text += "\n";
                    _letterIndex++;
                }
                else
                {
                    _targetTextBlock.Text += _content[_letterIndex];
                    _letterIndex++;
                }
            }
            else
            {
                IsTyping = false;
                Timer.Stop();
            }
        }

        public void UpdateTypingInterval()
        {
            Timer.Interval = TimeSpan.FromMilliseconds(Interval);
        }

        public void FinishTyping()
        {
            Timer.Stop();
            _targetTextBlock.Text = _content;
            IsTyping = false;
        }
    }
}
