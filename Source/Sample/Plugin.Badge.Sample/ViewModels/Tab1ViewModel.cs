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
            Color.Transparent
        };
    
        public Color BadgeColor { get; private set; }

        public ICommand ChangeColorCommand => new Command((obj) =>
        {
            _color++;
            if (_color >= Colors.Count)
                _color = 0;

            BadgeColor = Colors[_color];
            RaisePropertyChanged(nameof(BadgeColor));
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

        private int _count = 0;
        private int _color = 0;
        public string Count => _count <= 0 ? null : _count.ToString();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
