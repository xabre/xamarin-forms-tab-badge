using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
namespace Plugin.Badge.Sample.ViewModels
{
    public class Tab1ViewModel : INotifyPropertyChanged
    {
        private static readonly List<Color> Colors = new List<Color>()
        {
            Color.Accent,
            Color.Aqua,
            Color.Black,
            Color.Blue,
            Color.Fuchsia,
            Color.Fuchsia,
            Color.Gray,
            Color.Green,
            Color.Lime,
            Color.Maroon,
            Color.Navy,
            Color.Pink,
            Color.Purple,
            Color.Red,
            Color.Silver,
            Color.Teal,
            Color.White,
            Color.Wheat,
            Color.WhiteSmoke
        };

        private static readonly List<Font> Fonts = new List<Font>()
        {
            Font.Default.WithAttributes(FontAttributes.Bold),
            Font.Default.WithAttributes(FontAttributes.Italic),
            Font.Default.WithAttributes(FontAttributes.Bold | FontAttributes.Italic),
            Font.Default.WithAttributes(FontAttributes.None)
        };

        public Color BadgeColor { get; private set; }
        public Color BadgeTextColor { get; private set; }

        public ICommand ChangeColorCommand => new Command((obj) =>
        {
            _color++;
            if (_color >= Colors.Count)
                _color = 0;

            BadgeColor = Colors[_color];
            RaisePropertyChanged(nameof(BadgeColor));
        });

        public ICommand ChangeTextColorCommand => new Command((obj) =>
        {
            _textColor--;
            if (_textColor < 0)
                _textColor = Colors.Count - 1;

            BadgeTextColor = Colors[_textColor];
            RaisePropertyChanged(nameof(BadgeTextColor));
        });

        public ICommand IncrementCommand => new Command((obj) =>
        {
            _count++;
            RaisePropertyChanged(nameof(Count));
        });

        public ICommand DecrementCommand => new Command((obj) =>
       {
           _count--;
           if (_count < 0)
               _count = 0;
           RaisePropertyChanged(nameof(Count));
       });

        public ICommand ChangeFontAttributesCommand => new Command((obj) =>
        {
            _fontIndex++;
            if (_fontIndex >= Fonts.Count)
                _fontIndex = 0;
            BadgeFont = Fonts[_fontIndex];
            RaisePropertyChanged(nameof(BadgeFont));
        });

        private int _count = 0;
        private int _color = 0;
        private int _textColor = 0;
        private int _fontIndex = 0;

        public string Count => _count <= 0 ? string.Empty : _count.ToString();

        public Font BadgeFont { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
