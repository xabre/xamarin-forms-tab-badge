using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;
using Plugin.Badge.Abstractions;

namespace Plugin.Badge.Droid
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        private static Thickness defaultThicknessForBottomPlacement = new Thickness(10, 5);
        private const int DeleayBeforeTabAdded = 10;
        protected readonly Dictionary<Element, BadgeView> BadgeViews = new Dictionary<Element, BadgeView>();
        private TabLayout _topTabLayout;
        private LinearLayout _topTabStrip;
        private ViewGroup _bottomTabStrip;

        public BadgedTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            // make sure we cleanup old event registrations
            Cleanup(e.OldElement);
            Cleanup(Element);

            var tabCount = InitLayout();
            for (var i = 0; i < tabCount; i++)
            {
                AddTabBadge(i);
            }

            Element.ChildAdded += OnTabAdded;
            Element.ChildRemoved += OnTabRemoved;
        }

        private int InitLayout()
        {
            switch (this.Element.OnThisPlatform().GetToolbarPlacement())
            {
                case ToolbarPlacement.Default:
                case ToolbarPlacement.Top:
                    _topTabLayout = ViewGroup.FindChildOfType<TabLayout>();
                    if (_topTabLayout == null)
                    {
                        Console.WriteLine("Plugin.Badge: No TabLayout found. Badge not added.");
                        return 0;
                    }

                    _topTabStrip = _topTabLayout.FindChildOfType<LinearLayout>();
                    return _topTabLayout.TabCount;
                case ToolbarPlacement.Bottom:
                    _bottomTabStrip = ViewGroup.FindChildOfType<BottomNavigationView>()?.GetChildAt(0) as ViewGroup;
                    if (_bottomTabStrip == null)
                    {
                        Console.WriteLine("Plugin.Badge: No bottom tab layout found. Badge not added.");
                        return 0;
                    }

                    return _bottomTabStrip.ChildCount;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddTabBadge(int tabIndex)
        {
            var element = Element.Children[tabIndex];
            if (element is NavigationPage navigationPage)
            {
                //if the child page is a navigation page get its root page
                element = navigationPage.RootPage;
            }

            var placement = Element.OnThisPlatform().GetToolbarPlacement();
            var targetView = placement == ToolbarPlacement.Bottom ? _bottomTabStrip?.GetChildAt(tabIndex) : _topTabLayout?.GetTabAt(tabIndex).CustomView ?? _topTabStrip?.GetChildAt(tabIndex);
            if (!(targetView is ViewGroup targetLayout))
            {
                Console.WriteLine("Plugin.Badge: Badge target cannot be null. Badge not added.");
                return;
            }

            var badgeView = targetLayout.FindChildOfType<BadgeView>();

            if (badgeView == null)
            {
                var imageView = targetLayout.FindChildOfType<ImageView>();
                if (placement == ToolbarPlacement.Bottom)
                {
                    // create for entire tab layout
                    badgeView = BadgeView.WithViewLayout(Context, imageView);
                }
                else
                {
                    //create badge for tab image or text
                    badgeView = BadgeView.WithWrapView(Context, imageView?.Drawable != null
                        ? (Android.Views.View)imageView
                        : targetLayout.FindChildOfType<TextView>());
                }
            }

            BadgeViews[element] = badgeView;
            badgeView.UpdateFromElement(element);

            // adjust default margins for bottom placement
            if (placement == ToolbarPlacement.Bottom && TabBadge.GetBadgeMargin(element) == TabBadge.DefaultMargins)
            {
                badgeView.SetMargins(0, 0, 0, 0);
            }

            element.PropertyChanged -= OnTabbedPagePropertyChanged;
            element.PropertyChanged += OnTabbedPagePropertyChanged;
        }

        protected virtual void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is Element element))
                return;

            if (BadgeViews.TryGetValue(element, out var badgeView))
            {
                badgeView.UpdateFromPropertyChangedEvent(element, e);
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

            if (!(e.Element is Page page))
                return;

            AddTabBadge(Element.Children.IndexOf(page));
        }

        protected override void Dispose(bool disposing)
        {
            Cleanup(Element);

            base.Dispose(disposing);
        }

        private void Cleanup(TabbedPage page)
        {
            if (page == null)
            {
                return;
            }

            foreach (var tab in page.Children)
            {
                tab.PropertyChanged -= OnTabbedPagePropertyChanged;
            }

            page.ChildRemoved -= OnTabRemoved;
            page.ChildAdded -= OnTabAdded;

            BadgeViews.Clear();
            _topTabLayout = null;
            _topTabStrip = null;
            _bottomTabStrip = null;
        }
    }
}
