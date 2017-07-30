using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

namespace Plugin.Badge.UWP
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        HeaderTemplate header = new HeaderTemplate();
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var template = Control.HeaderTemplate;
            //Control.ToolbarBackground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));

            //var assembly = this.GetType().GetTypeInfo().Assembly;
            //var all = assembly.GetManifestResourceNames();

            //using (Stream stream = assembly.GetManifestResourceStream(all.First(s => s.Contains("Resources.xaml"))))
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    string result = reader.ReadToEnd();
            //    var resources = (ResourceDictionary)XamlReader.Load(result);
            //    Control.HeaderTemplate = resources["HeaderTemplate"] as DataTemplate;
            //}

           
            Control.HeaderTemplate = header.Resources["HeaderTemplate"] as DataTemplate;

            foreach(var tab in Element.Children)
            {
                tab.PropertyChanged += Tab_PropertyChanged;
            }
        }

        private void Tab_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("Badge"))
            {
                Control.HeaderTemplate = null;
                Control.HeaderTemplate = header.Resources["HeaderTemplate"] as DataTemplate;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
