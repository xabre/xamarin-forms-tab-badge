using System;
using Xamarin.Forms;
namespace Plugin.Badge.Sample
{
    public class BadgedTabbedPage : TabbedPage
    {

        public static BindableProperty BadgeTextProperty = BindableProperty.CreateAttached("BadgeText", typeof(string), typeof(BadgedTabbedPage), default(string), BindingMode.OneWay);

        public static string GetBadgeText(BindableObject view)
        {
            return (string)view.GetValue(BadgeTextProperty);
        }

        public static void SetBadgeText(BindableObject view, string value)
        {
            view.SetValue(BadgeTextProperty, value);
        }

        public BadgedTabbedPage()
        {
            
        }
    }
}
