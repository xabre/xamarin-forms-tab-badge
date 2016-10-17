using Plugin.Badge.Abstractions;
using Plugin.Badge.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Internals;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
namespace Plugin.Badge.iOS
{
    [Preserve]
    public class BadgedTabbedPageRenderer : TabbedRenderer
    {
        public static void Init()
        {
            var r = new BadgedTabbedPageRenderer();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var tabbedPage = Tabbed;

            for (int i = 0; i < TabBar.Items.Length; i++)
            {
                tabbedPage.Children[i].PropertyChanged += TabbedPage_PropertyChanged;
              
                TabBar.Items[i].BadgeValue = TabBadge.GetBadgeText(tabbedPage.Children[i]);

                var tabColor = TabBadge.GetBadgeColor(tabbedPage.Children[i]);
                if(tabColor != default(Color))
                    TabBar.Items[i].BadgeColor = tabColor.ToUIColor();

            }
        }

        void TabbedPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                var tabIndex = Tabbed.Children.IndexOf(sender as Page);
                TabBar.Items[tabIndex].BadgeValue = TabBadge.GetBadgeText(sender as Page);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                var tabIndex = Tabbed.Children.IndexOf(sender as Page);
                TabBar.Items[tabIndex].BadgeColor = TabBadge.GetBadgeColor(sender as Page).ToUIColor();
                return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Tabbed != null)
            {
                foreach (var tab in Tabbed.Children)
                {
                    tab.PropertyChanged -= TabbedPage_PropertyChanged;
                }
            }
            base.Dispose(disposing);
        }
    }
}
