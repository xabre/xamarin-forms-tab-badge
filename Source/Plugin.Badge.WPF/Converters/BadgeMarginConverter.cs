using System;
using System.Globalization;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;
using Thickness = System.Windows.Thickness;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgeMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is Element element)
            {
                var margin = TabBadge.GetBadgeMargin(element);
                return new Thickness(margin.Left, margin.Top, margin.Right, margin.Bottom);
            }

            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
