using System;
using System.Globalization;
using System.Windows;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgeFontWeightConverter : IValueConverter
    {        

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            var element = value as Element;
            if (element != null)
            {
                var font = TabBadge.GetBadgeFont(element);

                if (font.FontAttributes.HasFlag(FontAttributes.Bold))
                {
                    return FontWeights.Bold;
                }

                return FontWeights.SemiBold;
            }

            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
