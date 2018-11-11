using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class TabIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var source = (value as FileImageSource)?.File ?? value as string;

            if (source == null)
                return null;

            return new BitmapImage(new Uri("/" + source, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
