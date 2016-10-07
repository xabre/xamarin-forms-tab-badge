using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;
namespace Plugin.Badge.Sample.ViewModels
{
    public class Tab1ViewModel : INotifyPropertyChanged
    {
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
        public string Count => _count <= 0 ? null : _count.ToString();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
