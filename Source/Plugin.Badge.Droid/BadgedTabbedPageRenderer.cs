using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Plugin.Badge.Abstractions;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Support.V4.View;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace Plugin.Badge.Droid
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
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

            switch (this.Element.OnThisPlatform().GetToolbarPlacement())
            {
                case ToolbarPlacement.Default:
                case ToolbarPlacement.Top:
                    _topTabLayout = ViewGroup.FindChildOfType<TabLayout>();
                    if (_topTabLayout == null)
                    {
                        Console.WriteLine("Plugin.Badge: No TabLayout found. Badge not added.");
                        return;
                    }

                    _topTabStrip = _topTabLayout.FindChildOfType<LinearLayout>();

                    for (var i = 0; i < _topTabLayout.TabCount; i++)
                    {
                        AddTabBadge(i);
                    }
                    break;
                case ToolbarPlacement.Bottom:
                    _bottomTabStrip = ViewGroup.FindChildOfType<BottomNavigationView>()?.GetChildAt(0) as ViewGroup;
                    if (_bottomTabStrip == null)
                    {
                        Console.WriteLine("Plugin.Badge: No bottom tab layout found. Badge not added.");
                        return;
                    }

                    for (var i = 0; i < _bottomTabStrip.ChildCount; i++)
                    {
                        AddTabBadge(i);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Element.ChildAdded += OnTabAdded;
            Element.ChildRemoved += OnTabRemoved;
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
            if (!(targetView is ViewGroup target))
            {
                Console.WriteLine("Plugin.Badge: Badge target cannot be null. Badge not added.");
                return;
            }

            var badgeView = target.FindChildOfType<BadgeView>();

            if (badgeView == null)
            {
                if (placement == ToolbarPlacement.Bottom)
                {
                    // create for entire tab layout
                    badgeView = BadgeView.ForLayout(Context, target);
                }
                else
                {
                    var imageView = target.FindChildOfType<ImageView>();

                    //create badge for tab image or text
                    badgeView = BadgeView.ForView(Context, imageView?.Drawable != null
                        ? (Android.Views.View)imageView
                        : target.FindChildOfType<TextView>());
                }
            }

            BadgeViews[element] = badgeView;
            badgeView.UpdateFromElement(element);
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
