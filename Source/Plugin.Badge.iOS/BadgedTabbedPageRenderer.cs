using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;

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

            if (TabBar.Items.Length > tabIndex)
            {
                var tabBarItem = TabBar.Items[tabIndex];
                tabBarItem.BadgeValue = TabBadge.GetBadgeText(element);

                var tabColor = TabBadge.GetBadgeColor(element);
                if (tabColor != Color.Default)
                {
                    tabBarItem.BadgeColor = tabColor.ToUIColor();
                }
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
                if(tabIndex < TabBar.Items.Length)
                    TabBar.Items[tabIndex].BadgeValue = TabBadge.GetBadgeText(page);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                var tabIndex = Tabbed.Children.IndexOf(page);
                if (tabIndex < TabBar.Items.Length)
                    TabBar.Items[tabIndex].BadgeColor = TabBadge.GetBadgeColor(page).ToUIColor();
            }
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            //workaround for XF, tabbar is not updated at this point and we have no way to know when its updated.
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
            if (Tabbed != null)
            {
                foreach (var tab in Tabbed.Children)
                {
                    tab.PropertyChanged -= OnTabbedPagePropertyChanged;
                }

                Tabbed.ChildAdded -= OnTabAdded;
                Tabbed.ChildRemoved -= OnTabRemoved;
            }



            base.Dispose(disposing);
        }
    }
}
