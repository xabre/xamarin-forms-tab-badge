using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using UIKit;
using System;

namespace Plugin.Badge.iOS
{
    [Preserve]
    public class BadgedTabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // make sure we cleanup old event registrations
            Cleanup(e.OldElement as TabbedPage);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // make sure we cleanup old event registrations
            Cleanup(Tabbed);

            for (var i = 0; i < TabBar.Items.Length; i++)
            {
                AddTabBadge(i);
            }

            Tabbed.ChildAdded += OnTabAdded;
            Tabbed.ChildRemoved += OnTabRemoved;
        }

        private void AddTabBadge(int tabIndex)
        {
            var element = Tabbed.GetChildPageWithBadge(tabIndex);
            element.PropertyChanged += OnTabbedPagePropertyChanged;

            if (TabBar.Items.Length > tabIndex)
            {
                var tabBarItem = TabBar.Items[tabIndex];
                UpdateTabBadgeText(tabBarItem, element);
                UpdateTabBadgeColor(tabBarItem, element);
                UpdateTabBadgeTextAttributes(tabBarItem, element);
            }
        }

        private void UpdateTabBadgeText(UITabBarItem tabBarItem, Element element)
        {
            var text = TabBadge.GetBadgeText(element);

            tabBarItem.BadgeValue = string.IsNullOrEmpty(text) ? null : text;
        }

        private void UpdateTabBadgeTextAttributes(UITabBarItem tabBarItem, Element element)
        {
            if (!tabBarItem.RespondsToSelector(new ObjCRuntime.Selector("setBadgeTextAttributes:forState:")))
            {
                // method not available, ios < 10
                Console.WriteLine("Plugin.Badge: badge text attributes only available starting with iOS 10.0.");
                return;
            }

            var attrs = new UIStringAttributes();

            var textColor = TabBadge.GetBadgeTextColor(element);
            if (textColor != Color.Default)
            {
                attrs.ForegroundColor = textColor.ToUIColor();
            }

            var font = TabBadge.GetBadgeFont(element);
            if (font != Font.Default)
            {
                attrs.Font = font.ToUIFont();
            }

            tabBarItem.SetBadgeTextAttributes(attrs, UIControlState.Normal);
        }

        private void UpdateTabBadgeColor(UITabBarItem tabBarItem, Element element)
        {
            if (!tabBarItem.RespondsToSelector(new ObjCRuntime.Selector("setBadgeColor:")))
            {
                // method not available, ios < 10
                Console.WriteLine("Plugin.Badge: badge color only available starting with iOS 10.0.");
                return;
            }

            var tabColor = TabBadge.GetBadgeColor(element);
            if (tabColor != Color.Default)
            {
                tabBarItem.BadgeColor = tabColor.ToUIColor();
            }
        }

        private void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var page = sender as Page;
            if (page == null)
                return;

            if (e.PropertyName == Page.IconProperty.PropertyName)
            {
                // #65 update badge properties if icon changed
                if (CheckValidTabIndex(page, out int tabIndex))
                {
                    UpdateTabBadgeText(TabBar.Items[tabIndex], page);
                    UpdateTabBadgeColor(TabBar.Items[tabIndex], page);
                    UpdateTabBadgeTextAttributes(TabBar.Items[tabIndex], page);
                }

                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                if (CheckValidTabIndex(page, out int tabIndex))
                    UpdateTabBadgeText(TabBar.Items[tabIndex], page);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                if (CheckValidTabIndex(page, out int tabIndex))
                    UpdateTabBadgeColor(TabBar.Items[tabIndex], page);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName || e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
            {
                if (CheckValidTabIndex(page, out int tabIndex))
                    UpdateTabBadgeTextAttributes(TabBar.Items[tabIndex], page);
                return;
            }
        }

        protected bool CheckValidTabIndex(Page page, out int tabIndex)
        {
            tabIndex = Tabbed.Children.IndexOf(page);
            if (tabIndex == -1 && page.Parent != null)
                tabIndex = Tabbed.Children.IndexOf(page.Parent);
            return tabIndex >= 0 && tabIndex < TabBar.Items.Length;
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            //workaround for XF, tabbar is not updated at this point and we have no way of knowing for sure when it will be updated. so we have to wait ... 
            await Task.Delay(10);
            var page = e.Element as Page;
            if (page == null)
                return;

            var tabIndex = Tabbed.Children.IndexOf(page);
            AddTabBadge(tabIndex);
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= OnTabbedPagePropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            Cleanup(Tabbed);

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

            page.ChildAdded -= OnTabAdded;
            page.ChildRemoved -= OnTabRemoved;
        }
    }
}
