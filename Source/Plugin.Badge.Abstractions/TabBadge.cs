using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Plugin.Badge.Abstractions
{
    public static class TabBadge
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

        public static BindableProperty BadgeColorProperty = BindableProperty.CreateAttached("BadgeColor", typeof(Color), typeof(TabBadge), Color.Default, BindingMode.OneWay);

        public static Color GetBadgeColor(BindableObject view)
        {
            return (Color)view.GetValue(BadgeColorProperty);
        }

        public static void SetBadgeColor(BindableObject view, Color value)
        {
            view.SetValue(BadgeColorProperty, value);
        }

        public static BindableProperty BadgeTextColorProperty = BindableProperty.CreateAttached("BadgeTextColor", typeof(Color), typeof(TabBadge), Color.Default, BindingMode.OneWay);

        public static Color GetBadgeTextColor(BindableObject view)
        {
            return (Color)view.GetValue(BadgeTextColorProperty);
        }

        public static void SetBadgeTextColor(BindableObject view, Color value)
        {
            view.SetValue(BadgeTextColorProperty, value);
        }

        public static BindableProperty BadgeFontProperty = BindableProperty.CreateAttached("BadgeFont", typeof(Font), typeof(TabBadge), Font.Default, BindingMode.OneWay);

        public static Font GetBadgeFont(BindableObject view)
        {
            return (Font)view.GetValue(BadgeFontProperty);
        }

        public static void SetBadgeFont(BindableObject view, Font value)
        {
            view.SetValue(BadgeFontProperty, value);
        }

        public static BindableProperty BadgePositionProperty = BindableProperty.CreateAttached("BadgePosition", typeof(BadgePosition), typeof(TabBadge), BadgePosition.PositionTopRight, BindingMode.OneWay);

        public static BadgePosition GetBadgePosition(BindableObject view)
        {
            return (BadgePosition)view.GetValue(BadgePositionProperty);
        }

        public static void SetBadgePosition(BindableObject view, BadgePosition value)
        {
            view.SetValue(BadgePositionProperty, value);
        }

        public static BindableProperty BadgeMarginProperty = BindableProperty.CreateAttached("BadgeMargin", typeof(Thickness), typeof(TabBadge), DefaultMargins, BindingMode.OneWay);

        public static Thickness GetBadgeMargin(BindableObject view)
        {
            return (Thickness)view.GetValue(BadgeMarginProperty);
        }

        public static void SetBadgeMargin(BindableObject view, Thickness value)
        {
            view.SetValue(BadgeMarginProperty, value);
        }

        public static Thickness DefaultMargins 
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return new Thickness(-10, -5);
                    case Device.UWP:
                    case Device.macOS:
                    case Device.iOS:
                        return new Thickness(0);
                }

                return new Thickness(0);
            }
        }

        /// <summary>
        /// Internal use only. Attempts to get the badged child of a tabbed page (either navigation page or content page)
        /// </summary>
        /// <param name="parentTabbedPage">Tabbed page</param>
        /// <param name="tabIndex">Index</param>
        /// <returns>Page</returns>
        public static Page GetChildPageWithBadge(this TabbedPage parentTabbedPage, int tabIndex)
        {
            var element = parentTabbedPage.Children[tabIndex];
            return GetPageWithBadge(element);
        }

        public static Page GetPageWithBadge(this Page element)
        {
            if (GetBadgeText(element) != (string) BadgeTextProperty.DefaultValue)
            {
                return element;
            }

            if (element is NavigationPage navigationPage)
            {
                //if the child page is a navigation page get its root page
                return navigationPage.RootPage;
            }

            return element;
        }
    }
}
