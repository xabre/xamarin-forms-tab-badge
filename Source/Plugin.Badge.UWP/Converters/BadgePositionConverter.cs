using Plugin.Badge.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Plugin.Badge.UWP
{
    public enum PanelAlignemnet
    {
        AlignRightWithPanel = 0,
        AlignTopWithPanel = 1,
        AlignBottomWithPanel = 2,
        AlignLeftWithPanel = 3,
        AlignHorizontalCenterWithPanel = 4,
        AlignVerticalCenterWithPanel = 5
    }

    public class BadgePositionConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        private static Dictionary<BadgePosition, bool[]> Alignments = new Dictionary<BadgePosition, bool[]>()
        {
            { BadgePosition.PositionBottomLeft, new[] { false, false, true, true, false, false } },
            { BadgePosition.PositionBottomRight, new[] { true, false, true, false, false, false } },
            { BadgePosition.PositionTopLeft, new[] { false, true, false, true, false, false } },
            { BadgePosition.PositionTopRight, new[] { true, true, false, false, false, false } },
            { BadgePosition.PositionCenter, new[] { false, false, false, false, true, true } }
        };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var element = value as Element;
            if (element != null)
            {
                var position = TabBadge.GetBadgePosition(element);

                if (Enum.TryParse<PanelAlignemnet>(parameter?.ToString(), out var panelAlignement))
                {
                    return Alignments[position][(int)panelAlignement];
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
