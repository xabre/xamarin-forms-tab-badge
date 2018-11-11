using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using DataTemplate = Windows.UI.Xaml.DataTemplate;

namespace Plugin.Badge.UWP
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        private readonly HeaderTemplate _header = new HeaderTemplate();
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            if (Element == null)
            {
                return;
            }

            Control.HeaderTemplate = _header.Resources[nameof(HeaderTemplate)] as DataTemplate;

            for (var tabIndex = 0; tabIndex < Element.Children.Count; tabIndex++)
            {
                //if the child page is a navigation page get its root page
                var tab = Element.GetChildPageWithBadge(tabIndex);

                tab.PropertyChanged -= Tab_PropertyChanged;
                tab.PropertyChanged += Tab_PropertyChanged;
            }
        }

        private void Tab_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("Badge"))
            {
                //ToDo find a better way to refresh the bindings, this causes flickering for the tab icon
                Control.HeaderTemplate = null;
                Control.HeaderTemplate = _header.Resources[nameof(HeaderTemplate)] as DataTemplate;
            }
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var tab in Element.Children)
            {
                tab.PropertyChanged -= Tab_PropertyChanged;
            }

            base.Dispose(disposing);
        }
    }
}
