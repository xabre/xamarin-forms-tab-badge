using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using View = Android.Views.View;

namespace Plugin.Badge.Droid
{
    /// <summary>
    /// Tab badge renderer for legacy (Non-AppCompat) FormsApplicationActivity 
    /// </summary>
    public class LegacyBadgedTabbedRenderer : TabbedRenderer
    {
        protected const int DelayBeforeTabAdded = 10;
        protected readonly Dictionary<Element, BadgeView> BadgeViews = new Dictionary<Element, BadgeView>();
        protected LinearLayout _tabLinearLayout;

        /// <summary>
        /// The legacy renderer has an issue which causes the badges not to appear if the initialization is done too soon before the tab layout is set-up. 
        /// Unfortunately events like OnAppearing and attached to window do not guarantee that the tab layout is initialized.
        /// A workaround for this is setting this initialization delay and ensure that the layouting has finished before the tabs are initialized.
        /// </summary>
        public static int InitializationDelayInMiliseconds { get; set; } = 600;

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
        }

        protected override async void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            await Task.Delay(InitializationDelayInMiliseconds);
            Initialize();
        }

        protected virtual void Initialize()
        {
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

            if (_tabLinearLayout == null || _tabLinearLayout.ChildCount == 0)
            {
                Console.WriteLine("Plugin.Badge: No ActionBar bit ha tab layout found or has no children. Badges not added.");
                return;
            }

            for (var i = 0; i < _tabLinearLayout.ChildCount; i++)
            {
                AddTabBadge(i);
            }
        }

        private void AddTabBadge(int tabIndex)
        {
            if (!(_tabLinearLayout.GetChildAt(tabIndex) is ViewGroup view) || tabIndex >= Element.Children.Count)
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
                badgeView = BadgeView.WithWrapView(Context, badgeTarget);
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
            await Task.Delay(DelayBeforeTabAdded);

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
            _tabLinearLayout = null;
        }
    }
}
