using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;

namespace Plugin.Badge.UWP
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        HeaderTemplate header = new HeaderTemplate();
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            Control.HeaderTemplate = header.Resources[nameof(HeaderTemplate)] as DataTemplate;

            foreach (var tab in Element.Children)
            {
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
                Control.HeaderTemplate = header.Resources[nameof(HeaderTemplate)] as DataTemplate;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            foreach (var tab in Element.Children)
            {
                tab.PropertyChanged -= Tab_PropertyChanged;
            }
        }
    }
}
