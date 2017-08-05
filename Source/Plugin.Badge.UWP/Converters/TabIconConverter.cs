using System;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;

namespace Plugin.Badge.UWP
{
    public class TabIconConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var source = (value as FileImageSource)?.File ?? value as string;

            if (source == null)
                return null;

            return new BitmapImage(new Uri("ms-appx:///" + source, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
