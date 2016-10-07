using System;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Plugin.Badge.Sample.Droid;
using Android.Support.Design.Widget;
using Android.Views;
using Java.Lang;
using System.Collections.Generic;
using Android.Widget;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
namespace Plugin.Badge.Sample.Droid
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {

        protected BadgeView[] BadgeViews;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            var tabLayout = FindChildOfType<TabLayout>(this.ViewGroup) as TabLayout;
            if (tabLayout != null)
            {
                Console.WriteLine(PrintAllChildren(tabLayout, 0));

                var tabStrip = FindChildOfType<TabLayout.SlidingTabStrip>(tabLayout) as TabLayout.SlidingTabStrip;

                BadgeViews = new BadgeView[tabLayout.TabCount];
                for (int i = 0; i < tabLayout.TabCount; i++)
                {
                    var tab = tabLayout.GetTabAt(i);
                    var view = tab.CustomView ?? tabStrip?.GetChildAt(i);
                    var imageView = FindChildOfType<ImageView>(view as ViewGroup);
                    var badgeTarget = imageView.Drawable !=null ? (Android.Views.View)imageView : FindChildOfType<TextView>(view as ViewGroup);

                    var badgeText = BadgedTabbedPage.GetBadgeText(Element.Children[i]);
                    BadgeViews[i] = new BadgeView(Context, badgeTarget);
                    BadgeViews[i].Text = badgeText;
                    BadgeViews[i].show();

                    Element.Children[i].PropertyChanged += TabbedPage_PropertyChanged;

                }
            }

        }

        protected virtual void TabbedPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BadgedTabbedPage.BadgeTextProperty.PropertyName)
            {
                var tabIndex = Element.Children.IndexOf(sender as Page);
                BadgeViews[tabIndex].Text = BadgedTabbedPage.GetBadgeText(sender as Page);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Element != null)
            {
                foreach (var tab in Element.Children)
                {
                    tab.PropertyChanged -= TabbedPage_PropertyChanged;
                }
            }

            base.Dispose(disposing);
        }

        private static string PrintAllChildren(ViewGroup parent, int level)
        {
            if (parent == null)
                return "";

            if (parent.ChildCount == 0)
                return "";

            var spaces = new string(' ', level * 2);
            var result = new StringBuilder();
            for (var i = 0; i < parent.ChildCount; i++)
            {

                var child = parent.GetChildAt(i);

                result.Append(spaces + child.ToString() + "\n");

                var partialResult = PrintAllChildren(child as ViewGroup, level + 1);
                if (!string.IsNullOrEmpty(partialResult))
                    result.Append(partialResult + "\n");
            }

            return result.ToString();
        }

        private static IEnumerable<TabLayout.TabView> FindAllStandardTabs(TabLayout.SlidingTabStrip parent)
        {

            for (var i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChildAt(i);

                if (child is TabLayout.TabView)
                {
                    yield return (TabLayout.TabView)child;
                }
            }
        }

        private static T FindChildOfType<T>(ViewGroup parent) where T: Android.Views.View
        {
            if (parent == null)
                return null;
            
            if (parent.ChildCount == 0)
                return null;

            for (var i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChildAt(i);

              
                if (child is T)
                {
                    return (T)child;
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
