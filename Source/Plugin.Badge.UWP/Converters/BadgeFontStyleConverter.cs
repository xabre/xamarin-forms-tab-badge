using Plugin.Badge.Abstractions;
using System;
using Xamarin.Forms;

namespace Plugin.Badge.UWP
{
    public class BadgeFontStyleConverter : Windows.UI.Xaml.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var element = value as Element;
            if (element != null)
            {
                var font = TabBadge.GetBadgeFont(element);
                if (font.FontAttributes.HasFlag(FontAttributes.Italic))
                {
                    return Windows.UI.Text.FontStyle.Italic;
                }
            }

            return Windows.UI.Text.FontStyle.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
