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
		NSWindow _window;
		public AppDelegate()
		{
			var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

			var rect = new CoreGraphics.CGRect(200, 1000, 500, 768);
			_window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
			_window.Title = "Xamarin.Forms Badge Plugin on Mac!";
			_window.TitleVisibility = NSWindowTitleVisibility.Hidden;
		}

		public override NSWindow MainWindow
		{
			get { return _window; }
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			Forms.Init();
			LoadApplication(new App());
			base.DidFinishLaunching(notification);
		}
	}
}
