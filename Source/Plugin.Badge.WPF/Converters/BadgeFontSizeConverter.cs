using System;
using System.Globalization;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgeFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            var element = value as Element;
            if (element != null)
            {
                var font = TabBadge.GetBadgeFont(element);
                if(font == Font.Default)
                {
                    return 11;
                }

                return font.FontSize;
            }

            return 11;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
