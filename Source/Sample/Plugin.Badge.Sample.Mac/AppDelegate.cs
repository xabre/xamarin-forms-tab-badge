using AppKit;
using Foundation;
using Xamarin.Forms.Platform.MacOS;
using Xamarin.Forms;

//register renderer
[assembly: ExportRenderer(typeof(TabbedPage), typeof(Plugin.Badge.Mac.BadgedTabbedPageRenderer))]

namespace Plugin.Badge.Sample.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 500, 768);
            MainWindow = new NSWindow(rect, style, NSBackingStore.Buffered, false)
            {
                Title = "Xamarin.Forms Badge Plugin on Mac!",
                TitleVisibility = NSWindowTitleVisibility.Hidden
            };
        }

        public override NSWindow MainWindow { get; }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
            LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }
    }
}
