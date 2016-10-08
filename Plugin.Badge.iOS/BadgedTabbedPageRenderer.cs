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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var tabbedPage = Tabbed;

            for (int i = 0; i < TabBar.Items.Length; i++)
            {
                tabbedPage.Children[i].PropertyChanged += TabbedPage_PropertyChanged;
                var badgeText = BadgedTabbedPage.GetBadgeText(tabbedPage.Children[i]);

                TabBar.Items[i].BadgeValue = badgeText;
            }
        }

        void TabbedPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BadgedTabbedPage.BadgeTextProperty.PropertyName)
            {
                var tabIndex = Tabbed.Children.IndexOf(sender as Page);
                TabBar.Items[tabIndex].BadgeValue = BadgedTabbedPage.GetBadgeText(sender as Page);
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
