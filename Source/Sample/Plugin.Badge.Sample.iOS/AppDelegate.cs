using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Plugin.Badge.iOS;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Plugin.Badge.Sample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
