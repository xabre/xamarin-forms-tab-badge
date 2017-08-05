using Plugin.Badge.Abstractions;
using System;
using Xamarin.Forms;

namespace Plugin.Badge.UWP
{
    public class BadgeTextConverter : Windows.UI.Xaml.Data.IValueConverter
    {        

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var element = value as Element;
            if (element != null)
            {
                return TabBadge.GetBadgeText(element);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
