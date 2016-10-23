using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Android.Database;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Plugin.Badge.Droid;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Plugin.Badge.Abstractions;
using Xamarin.Forms.Platform.Android;

namespace Plugin.Badge.Droid
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        private const int DeleayBeforeTabAdded = 10;
        protected readonly Dictionary<Element, BadgeView> BadgeViews = new Dictionary<Element, BadgeView>();
        private TabLayout _tabLayout;
        private TabLayout.SlidingTabStrip _tabStrip;

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);



            _tabLayout = ViewGroup.FindChildOfType<TabLayout>();
            if (_tabLayout == null)
            {
                Console.WriteLine("Plugin.Badge: No TabLayout found. Bedge not added.");
                return;
            }

            _tabStrip = _tabLayout.FindChildOfType<TabLayout.SlidingTabStrip>();

            for (var i = 0; i < _tabLayout.TabCount; i++)
            {
                AddTabBadge(i);
            }

            Element.ChildAdded += OnTabAdded;
            Element.ChildRemoved += OnTabRemoved;
        }


        private void AddTabBadge(int tabIndex)
        {
            var element = Element.Children[tabIndex];
            var view = _tabLayout?.GetTabAt(tabIndex).CustomView ?? _tabStrip?.GetChildAt(tabIndex);

            var badgeView = (view as ViewGroup)?.FindChildOfType<BadgeView>();

            if (badgeView == null)
            {
                var imageView = (view as ViewGroup)?.FindChildOfType<ImageView>();

                var badgeTarget = imageView?.Drawable != null
                    ? (Android.Views.View) imageView
                    : (view as ViewGroup)?.FindChildOfType<TextView>();

                //create badge for tab
                badgeView = new BadgeView(Context, badgeTarget);
            }

            BadgeViews[element] = badgeView;

            //get text
            var badgeText = TabBadge.GetBadgeText(element);
            badgeView.Text = badgeText;

            // set color if not default
            var tabColor = TabBadge.GetBadgeColor(element);
            if (tabColor != Color.Default)
            {
                badgeView.BadgeColor = tabColor.ToAndroid();
            }

            element.PropertyChanged += OnTabbedPagePropertyChanged;
        }

        protected virtual void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var element = sender as Element;
            if (element == null)
                return;

            BadgeView badgeView;
            if (!BadgeViews.TryGetValue(element, out badgeView))
            {
                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                badgeView.Text = TabBadge.GetBadgeText(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                badgeView.BadgeColor = TabBadge.GetBadgeColor(element).ToAndroid();
            }
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= OnTabbedPagePropertyChanged;
            BadgeViews.Remove(e.Element);
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            await Task.Delay(DeleayBeforeTabAdded);

            var page = e.Element as Page;
            if (page == null)
                return;

            var tabIndex = Element.Children.IndexOf(page);
            AddTabBadge(tabIndex);
        }

        protected override void Dispose(bool disposing)
        {
            if (Element != null)
            {
                foreach (var tab in Element.Children)
                {
                    tab.PropertyChanged -= OnTabbedPagePropertyChanged;
                }

                Element.ChildRemoved -= OnTabRemoved;
                Element.ChildAdded -= OnTabAdded;

                BadgeViews.Clear();
            }

            base.Dispose(disposing);
        }

      
    }
}
