using RentDesktop.Models.Base;
using System;

namespace RentDesktop.Models
{
    public class Captcha : ReactiveModel, ICaptcha
    {
        public Captcha(int length = 5)
        {
            Length = length;
            UpdateText();
        }

        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            private set => RaiseAndSetIfChanged(ref _text, value);
        }

        private int _length = 0;
        public int Length
        {
            get => _length;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Length must be greater than zero.", nameof(value));

                RaiseAndSetIfChanged(ref _length, value);
            }
        }

        public void UpdateText()
        {
            const string symbols = "abcdefghijklmnopqrstuvwxyz0123456789";
            string text = string.Empty;

            var rand = new Random();

            for (int i = 0; i < _length; ++i)
            {
                int index = rand.Next(0, symbols.Length - 1);

                text += index % 2 == 0
                    ? symbols[index]
                    : char.ToUpper(symbols[index]);
            }

            Text = text;
        }
    }
}
