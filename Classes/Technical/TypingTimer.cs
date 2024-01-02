using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
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
        private Color _foreground;
        private bool _isBold;
        private bool _isItalic;
        private int _fontSize;
        private string _content;
        
        public TypingTimer(TextBlock textBlock, string text)
        {
            Timer.Interval = TimeSpan.FromMilliseconds(Interval);
            _targetTextBlock = textBlock;
            _textLength = text.Length;
            _content = text;
            _targetTextBlock.Text = "";
            
            _isBold = textBlock.FontWeight != FontWeights.Regular;
            _isItalic = textBlock.FontStyle != FontStyles.Normal;
            _fontSize = (int)textBlock.FontSize;
            _foreground = ((SolidColorBrush)textBlock.Foreground).Color;

            Timer.Tick += TypingText;
            Timer.Start();
            if (ControlsManager.TypingTimer != null)
                ControlsManager.TypingTimer.Timer.Stop();
            ControlsManager.TypingTimer = this;
        }

        private void FormatText()
        {
            if (_content[_letterIndex] == '#')
            {
                _foreground = (Color)ColorConverter.
                    ConvertFromString(_content.Substring(_letterIndex, 7));

                _letterIndex += 7;
                return;
            }

            if (_content[_letterIndex] == 'f')
            {
                _fontSize = Convert.ToInt16(_content.Substring(_letterIndex + 1, 2));
                _letterIndex += 3;
                return;
            }

            if (_content[_letterIndex] == 'b')
                _isBold = true;

            if (_content[_letterIndex] == 'i')
                _isItalic = true;

            _letterIndex++;
        }

        private void UnformatText()
        {
            if (_content[_letterIndex] == '#')
                _foreground = ((SolidColorBrush)(_targetTextBlock.Foreground)).Color;

            if (_content[_letterIndex] == 'b')
                _isBold = false;

            if (_content[_letterIndex] == 'i')
                _isItalic = false;

            if (_content[_letterIndex] == 'f')
                _fontSize = (int)_targetTextBlock.FontSize;

            _letterIndex++;
        }


        public void TypingText(object sender, EventArgs e)
        {
            if (_letterIndex != _textLength)
            {
                if (_content[_letterIndex] == '\\' && _content[_letterIndex + 1] == 'r')
                {
                    _targetTextBlock.Text += "\n";
                    _letterIndex++;

                    _fontSize = 30;
                    _isBold = _targetTextBlock.FontWeight != FontWeights.Regular;
                    _isItalic = _targetTextBlock.FontStyle != FontStyles.Normal;
                    _foreground = Colors.White;
                }
                else
                {
                    while (_content[_letterIndex] == '>' || _content[_letterIndex] == '<')
                    {
                        _letterIndex++;

                        if (_content[_letterIndex - 1] == '>')
                            FormatText();

                        if (_content[_letterIndex - 1] == '<')
                            UnformatText();
                    }

                    Run letter = new Run(_content[_letterIndex].ToString())
                    {
                        FontSize = _fontSize,
                        Foreground = new SolidColorBrush(_foreground)
                    };

                    if (_isBold)
                        letter.FontWeight = FontWeights.Bold;
                    if (_isItalic)
                        letter.FontStyle = FontStyles.Italic;

                    _targetTextBlock.Inlines.Add(letter);
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
