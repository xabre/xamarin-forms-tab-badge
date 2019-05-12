using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Plugin.Badge.Abstractions;
using Plugin.Badge.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Color = Windows.UI.Color;

[assembly: ExportRenderer(typeof(Badge), typeof(BadgeRenderer))]
namespace Plugin.Badge.UWP
{
    public class BadgeRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            /*
            if (this.Control != null)
            {
                this.Control.Background = new SolidColorBrush(Xamarin.Forms.Color.Red.ToWindowsColor());
                e.NewElement.BackgroundColor = Xamarin.Forms.Color.Transparent;
            }*/
        }
    }
}
