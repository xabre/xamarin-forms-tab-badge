using System;
using System.Globalization;
using System.Windows.Media;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Color = Xamarin.Forms.Color;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgeTextColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is Element element)
            {
                var color = TabBadge.GetBadgeTextColor(element);
                if (color == Color.Default)
                {
                    color = Color.Black;
                }

                return color.ToBrush();
            }

            return new SolidColorBrush(new System.Windows.Media.Color());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
