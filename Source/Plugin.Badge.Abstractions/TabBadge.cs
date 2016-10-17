using System;
using Xamarin.Forms;
namespace Plugin.Badge.Abstractions
{
    public class TabBadge //: TabbedPage
    {
        public static BindableProperty BadgeTextProperty = BindableProperty.CreateAttached("BadgeText", typeof(string), typeof(TabBadge), default(string), BindingMode.OneWay);

        public static string GetBadgeText(BindableObject view)
        {
            return (string)view.GetValue(BadgeTextProperty);
        }

        public static void SetBadgeText(BindableObject view, string value)
        {
            view.SetValue(BadgeTextProperty, value);
        }

         
        public static BindableProperty BadgeColorProperty = BindableProperty.CreateAttached("BadgeColor", typeof(Color), typeof(TabBadge), default(Color), BindingMode.OneWay);

        public static Color GetBadgeColor(BindableObject view)
        {
            return (Color)view.GetValue(BadgeColorProperty);
        }

        public static void SetBadgeColor(BindableObject view, Color value)
        {
            view.SetValue(BadgeColorProperty, value);
        }
    }
}
