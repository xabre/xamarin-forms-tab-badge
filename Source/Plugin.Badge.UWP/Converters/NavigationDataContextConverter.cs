using System;
using Xamarin.Forms;
using IValueConverter = Windows.UI.Xaml.Data.IValueConverter;

namespace Plugin.Badge.UWP
{
    public class NavigationDataContextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as NavigationPage)?.RootPage ?? value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
