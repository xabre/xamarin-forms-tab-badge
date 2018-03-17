using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Content;

namespace Plugin.Badge.Droid
{
    /// <summary>
    /// Tab badge renderer for legacy (Non-AppCompat) FormsApplicationActivity 
    /// </summary>
    public class LegacyBadgedTabbedRenderer : TabbedRenderer
    {
        private const int DeleayBeforeTabAdded = 10;
        protected readonly Dictionary<Element, BadgeView> BadgeViews = new Dictionary<Element, BadgeView>();
        private LinearLayout _tabLinearLayout;

        public LegacyBadgedTabbedRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            // make sure we cleanup old event registrations
            Cleanup(e.OldElement);
            Cleanup(Element);

            Element.ChildAdded += OnTabAdded;
            Element.ChildRemoved += OnTabRemoved;
            Element.Appearing += OnAppearing;
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            Element.Appearing -= OnAppearing;

            IViewParent root = ViewGroup;
            while (root.Parent?.Parent != null)
            {
                root = root.Parent;
            }

            if (!(root is ViewGroup rootGroup))
            {
                return;
            }

            for (var i = 0; i < rootGroup.ChildCount; i++)
            {
                _tabLinearLayout = _tabLinearLayout ?? (rootGroup.GetChildAt(i) as ViewGroup)?.FindChildOfType<HorizontalScrollView>()?.FindChildOfType<LinearLayout>();
            }

            if (_tabLinearLayout == null)
            {
                Console.WriteLine("Plugin.Badge: No ActionBar bit ha tab layout found. Badges not added.");
                return;
            }

            for (var i = 0; i < _tabLinearLayout.ChildCount; i++)
            {
                AddTabBadge(i);
            }
        }

        private void AddTabBadge(int tabIndex)
        {
            if (!(_tabLinearLayout.GetChildAt(tabIndex) is ViewGroup view))
            {
                return;
            }

            var element = Element.Children[tabIndex];

            var badgeView = view.FindChildOfType<BadgeView>();

            if (badgeView == null)
            {
                var badgeTarget = view.FindChildOfType<TextView>();
                if (badgeTarget == null)
                {
                    Console.WriteLine("Plugin.Badge: No Text view found to attach badge");
                    return;
                }

                //create badge for tab
                badgeView = new BadgeView(Context, badgeTarget);
            }

            BadgeViews[element] = badgeView;

            badgeView.UpdateFromElement(element);

            element.PropertyChanged += OnTabbedPagePropertyChanged;
        }

        protected virtual void OnTabbedPagePropertyChanged(object sender, PropertyChangedEventArgs e)
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
            page.Appearing -= OnAppearing;

            BadgeViews.Clear();
            _tabLinearLayout = null;
        }
    }
}
