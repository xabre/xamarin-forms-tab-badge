using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace Plugin.Badge.WPF.Converters
{
    public class BadgePositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            var isHorizontal = (parameter as string) == bool.TrueString;
            if (value is Element element)
            {
                var position = TabBadge.GetBadgePosition(element);
                switch (position)
                {
                    case BadgePosition.PositionTopRight:
                        return isHorizontal ? (object)HorizontalAlignment.Right : VerticalAlignment.Top;
                    case BadgePosition.PositionTopLeft:
                        return isHorizontal ? (object)HorizontalAlignment.Left : VerticalAlignment.Top;
                    case BadgePosition.PositionBottomRight:
                        return isHorizontal ? (object)HorizontalAlignment.Right : VerticalAlignment.Bottom;
                    case BadgePosition.PositionBottomLeft:
                        return isHorizontal ? (object)HorizontalAlignment.Left : VerticalAlignment.Bottom;
                    case BadgePosition.PositionCenter:
                        return isHorizontal ? (object)HorizontalAlignment.Center : VerticalAlignment.Center;
                    case BadgePosition.PositionTopCenter:
                        return isHorizontal ? (object)HorizontalAlignment.Center : VerticalAlignment.Top;
                    case BadgePosition.PositionBottomCenter:
                        return isHorizontal ? (object)HorizontalAlignment.Center : VerticalAlignment.Bottom;
                    case BadgePosition.PositionLeftCenter:
                        return isHorizontal ? (object)HorizontalAlignment.Left : VerticalAlignment.Center;
                    case BadgePosition.PositionRightCenter:
                        return isHorizontal ? (object)HorizontalAlignment.Right : VerticalAlignment.Center;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return isHorizontal ? (object)HorizontalAlignment.Center : VerticalAlignment.Center;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
