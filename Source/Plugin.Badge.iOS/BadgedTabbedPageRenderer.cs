using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Internals;

namespace Plugin.Badge.iOS
{
    [Preserve]
    public class BadgedTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            for (var i = 0; i < TabBar.Items.Length; i++)
            {
                AddTabBadge(i);
            }


            Element.ChildAdded += OnTabAdded;
            Element.ChildRemoved += OnTabRemoved;
        }
        
        private void AddTabBadge(int tabIndex)
        {
            var element = Tabbed.Children[tabIndex];
            element.PropertyChanged += OnTabbedPagePropertyChanged;

            var tabBarItem = TabBar.Items[tabIndex];
            tabBarItem.BadgeValue = TabBadge.GetBadgeText(element);

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

            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                var tabIndex = Tabbed.Children.IndexOf(page);
                TabBar.Items[tabIndex].BadgeValue = TabBadge.GetBadgeText(page);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                var tabIndex = Tabbed.Children.IndexOf(page);
                TabBar.Items[tabIndex].BadgeColor = TabBadge.GetBadgeColor(page).ToUIColor();
            }
        }

        private void OnTabAdded(object sender, ElementEventArgs e)
        {
            var page = sender as Page;
            if (page == null)
                return;

            var tabIndex = Tabbed.Children.IndexOf(page);
            AddTabBadge(tabIndex);
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            var page = sender as Page;
            if (page == null)
                return;

            page.PropertyChanged -= OnTabbedPagePropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if (Tabbed != null)
            {
                foreach (var tab in Tabbed.Children)
                {
                    tab.PropertyChanged -= OnTabbedPagePropertyChanged;
                }
            }
            base.Dispose(disposing);
        }
    }
}
