using System;
using System.Globalization;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgeTextConverter : IValueConverter
    {        

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is Element element)
            {
                return TabBadge.GetBadgeText(element);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
