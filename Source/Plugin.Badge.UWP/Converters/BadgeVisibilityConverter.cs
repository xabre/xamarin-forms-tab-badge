using Plugin.Badge.Abstractions;
using System;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace Plugin.Badge.UWP
{
    public class BadgeVisibilityConverter : Windows.UI.Xaml.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var element = value as Element;
            if (element != null)
            {
                return string.IsNullOrEmpty(TabBadge.GetBadgeText(element)) ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
