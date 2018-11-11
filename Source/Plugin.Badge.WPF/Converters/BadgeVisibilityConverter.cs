using System;
using System.Globalization;
using System.Windows;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgeVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is Element element)
            {
                return string.IsNullOrEmpty(TabBadge.GetBadgeText(element)) ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
