using System.ComponentModel;
using Android.Views;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

namespace Plugin.Badge.Droid
{
    internal static class BadgeViewExtensions
    {
        public static void UpdateFromElement(this BadgeView badgeView, Page element)
        {
            //get text
            var badgeText = TabBadge.GetBadgeText(element);
            badgeView.Text = badgeText;

            // set color if not default
            var tabColor = TabBadge.GetBadgeColor(element);
            if (tabColor != Color.Default)
            {
                badgeView.BadgeColor = tabColor.ToAndroid();
            }

            // set text color if not default
            var tabTextColor = TabBadge.GetBadgeTextColor(element);
            if (tabTextColor != Color.Default)
            {
                badgeView.TextColor = tabTextColor.ToAndroid();
            }

            // set font if not default
            var font = TabBadge.GetBadgeFont(element);
            if (font != Font.Default)
            {
                badgeView.Typeface = font.ToTypeface();
            }

            var margin = TabBadge.GetBadgeMargin(element);
            badgeView.SetMargins((float)margin.Left, (float)margin.Top, (float)margin.Right, (float)margin.Bottom);

            // set position
            badgeView.Postion = TabBadge.GetBadgePosition(element);
        }

        public static void UpdateFromPropertyChangedEvent(this BadgeView badgeView, Element element, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                badgeView.Text = TabBadge.GetBadgeText(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                badgeView.BadgeColor = TabBadge.GetBadgeColor(element).ToAndroid();
                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName)
            {
                badgeView.TextColor = TabBadge.GetBadgeTextColor(element).ToAndroid();
                return;
            }

            if (e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
            {
                badgeView.Typeface = TabBadge.GetBadgeFont(element).ToTypeface();
                return;
            }

            if (e.PropertyName == TabBadge.BadgePositionProperty.PropertyName)
            {
                badgeView.Postion = TabBadge.GetBadgePosition(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeMarginProperty.PropertyName)
            {
                var margin = TabBadge.GetBadgeMargin(element);
                badgeView.SetMargins((float)margin.Left, (float)margin.Top, (float)margin.Right, (float)margin.Bottom);
                return;
            }
        }

        public static T FindChildOfType<T>(this ViewGroup parent) where T : View
        {
            if (parent == null)
                return null;

            if (parent.ChildCount == 0)
                return null;

            for (var i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChildAt(i);


                var typedChild = child as T;
                if (typedChild != null)
                {
                    return typedChild;
                }

                if (!(child is ViewGroup))
                    continue;


                var result = FindChildOfType<T>(child as ViewGroup);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}