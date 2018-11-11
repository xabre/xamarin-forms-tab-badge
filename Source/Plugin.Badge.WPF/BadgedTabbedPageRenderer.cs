using System;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using DataTemplate = System.Windows.DataTemplate;
using Grid = System.Windows.Controls.Grid;

namespace Plugin.Badge.WPF
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
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

            this.Control.Loaded += ControlOnLoaded;
        }

        private void ControlOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (((this.Control.LightContentControl.Parent as Grid)?.Children[0] as Grid)?.Children[0] is ListBox listBox)
            {
                listBox.ItemTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(TabItemTemplate)) };
            }
            else
            {
                Console.WriteLine("BadgePlugin: Was not able to find tab bar. Badges not added.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Control != null)
            {
                this.Control.Loaded -= ControlOnLoaded;
            }

            base.Dispose(disposing);
        }
    }
}
