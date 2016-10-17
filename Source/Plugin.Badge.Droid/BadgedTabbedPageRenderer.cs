using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Plugin.Badge.Droid;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Plugin.Badge.Abstractions;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
namespace Plugin.Badge.Droid
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        protected BadgeView[] BadgeViews;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            var tabLayout = FindChildOfType<TabLayout>(ViewGroup);
            if (tabLayout == null)
                return;

            var tabStrip = FindChildOfType<TabLayout.SlidingTabStrip>(tabLayout);
            BadgeViews = new BadgeView[tabLayout.TabCount];
            for (var i = 0; i < tabLayout.TabCount; i++)
            {
                var tab = tabLayout.GetTabAt(i);
                var view = tab.CustomView ?? tabStrip?.GetChildAt(i);
                var imageView = FindChildOfType<ImageView>(view as ViewGroup);

                var badgeTarget = imageView.Drawable != null ? (Android.Views.View)imageView : FindChildOfType<TextView>(view as ViewGroup);

                //get text
                var badgeText = TabBadge.GetBadgeText(Element.Children[i]);

                //create bage for tab
                BadgeViews[i] = new BadgeView(Context, badgeTarget) { Text = badgeText };

                // set color if not default
                var tabColor = TabBadge.GetBadgeColor(Element.Children[i]);
                if (tabColor != default(Color))
                    BadgeViews[i].BadgeColor = tabColor.ToAndroid();


                Element.Children[i].PropertyChanged += OnTabPagePropertyChanged;

            }
        }

        protected virtual void OnTabPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var page = sender as Page;
            if (page == null)
                return;

            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                var tabIndex = Element.Children.IndexOf(page);
                BadgeViews[tabIndex].Text = TabBadge.GetBadgeText(page);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                var tabIndex = Element.Children.IndexOf(page);
                BadgeViews[tabIndex].BadgeColor = TabBadge.GetBadgeColor(page).ToAndroid();
                return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Element != null)
            {
                foreach (var tab in Element.Children)
                {
                    tab.PropertyChanged -= OnTabPagePropertyChanged;
                }

                BadgeViews = null;
            }

            base.Dispose(disposing);
        }

        private static T FindChildOfType<T>(ViewGroup parent) where T : Android.Views.View
        {
            if (parent == null)
                return null;

            if (parent.ChildCount == 0)
                return null;

            for (var i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChildAt(i);


                var typedChild = child as T;
                if (typedChild != null)
                {
                    return typedChild;
                }

                if (!(child is ViewGroup))
                    continue;


                var result = FindChildOfType<T>(child as ViewGroup);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
