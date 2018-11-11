using System;
using System.Globalization;
using System.Windows;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgeFontStyleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is Element element)
            {
                var font = TabBadge.GetBadgeFont(element);
                if (font.FontAttributes.HasFlag(FontAttributes.Italic))
                {
                    return FontStyles.Italic;
                }
            }

            return FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
