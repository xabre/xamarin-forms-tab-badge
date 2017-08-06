using System;
using AppKit;
using Xamarin.Forms.Platform.MacOS;

namespace Plugin.Badge.Mac
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        public BadgedTabbedPageRenderer()
        {
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            
            base.OnElementChanged(e);

            foreach(var item in this.TabViewItems){
                item.ToolTip = "bla";
            }




        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();


			//foreach (var item in this.TabViewItems)
			//{
			//	item.ToolTip = "blabla";
   //             item.View.Layer.BackgroundColor = new CoreGraphics.CGColor(0.1f, 0.5f, 0.7f);
               
   //             Console.WriteLine(item.View.GetType().Name);
   //             foreach(var view in item.View.Subviews){
   //                 Console.WriteLine(view.GetType().Name);
   //             }
			//}

			foreach (var view in this.View.Subviews)
			{
				Console.WriteLine(view.GetType().Name);
                if(view is NSSegmentedControl){
                    var segmentedControl = view as NSSegmentedControl;
                    segmentedControl.SegmentStyle = NSSegmentStyle.TexturedSquare;

                    foreach (var segment in segmentedControl.Subviews)
                    {
                        
                        var badge = new NSView(new CoreGraphics.CGRect(0, 0, 20, 20));
                        badge.WantsLayer = true;
                        badge.Layer.BackgroundColor = new CoreGraphics.CGColor(1f, 0f, 0f);
                        //badge.AddConstraints(new []{});

                        segment.AddSubview(badge);
						var constraint = NSLayoutConstraint.Create(badge, NSLayoutAttribute.Right, NSLayoutRelation.Equal);
                        badge.AddConstraint(constraint);


						Console.WriteLine(segment.GetType().Name);
                    }
                }
			}



        }
    }
}
