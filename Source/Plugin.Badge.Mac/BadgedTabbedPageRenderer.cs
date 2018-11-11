using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Plugin.Badge.Mac
{
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        protected readonly Dictionary<Element, BadgeView> BadgeViews = new Dictionary<Element, BadgeView>();
        private NSSegmentedControl _segmentedControl;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            Cleanup(Tabbed);

            Tabbed.ChildAdded += OnTabAdded;
            Tabbed.ChildRemoved += OnTabRemoved;

            _segmentedControl = this.View.Subviews.FirstOrDefault(s => s is NSSegmentedControl) as NSSegmentedControl;
            if (_segmentedControl == null)
            {
                Console.WriteLine("[TabBadge] No SegmentedControl found. Not adding tabs.");
            }

            _segmentedControl.SegmentStyle = NSSegmentStyle.TexturedSquare;

            var tabWidth = this.View.Frame.Width / _segmentedControl.SegmentCount;

            for (var i = 0; i < Tabbed.Children.Count; i++)
            {
                AddTabBadge(i);
            }
        }

        protected virtual void AddTabBadge(int tabIndex)
        {
            var segment = _segmentedControl.Subviews[tabIndex];

            var element = Tabbed.GetChildPageWithBadge(tabIndex);
            element.PropertyChanged += OnTabbedPagePropertyChanged;

            var badge = new BadgeView(segment, false)
            {
                Color = TabBadge.GetBadgeColor(element),
                TextColor = TabBadge.GetBadgeTextColor(element),
                Text = TabBadge.GetBadgeText(element)
            };

            BadgeViews.Add(element, badge);
        }

        protected virtual void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var element = sender as Element;
            if (element == null)
                return;

            if (!BadgeViews.TryGetValue(element, out BadgeView badgeView))
            {
                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                badgeView.Text = TabBadge.GetBadgeText(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                badgeView.Color = TabBadge.GetBadgeColor(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName)
            {
                badgeView.TextColor = TabBadge.GetBadgeTextColor(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
            {
                badgeView.Font = TabBadge.GetBadgeFont(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgePositionProperty.PropertyName)
            {
                badgeView.Position = TabBadge.GetBadgePosition(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeMarginProperty.PropertyName)
            {
                badgeView.Margin = TabBadge.GetBadgeMargin(element);
                return;
            }
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= OnTabbedPagePropertyChanged;
            BadgeViews.Remove(e.Element);
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            //workaround for XF, tabbar is not updated at this point and we have no way of knowing for sure when it will be updated. so we have to wait ... 
            await Task.Delay(50);

            var page = e.Element as Page;
            if (page == null)
                return;

            var tabIndex = Tabbed.Children.IndexOf(page);
            AddTabBadge(tabIndex);
        }

        protected override void Dispose(bool disposing)
        {
            Cleanup(Tabbed);

            base.Dispose(disposing);
        }

        private void Cleanup(TabbedPage page)
        {
            if (page == null)
            {
                return;
            }

            foreach (var tab in page.Children)
            {
                tab.PropertyChanged -= OnTabbedPagePropertyChanged;
            }

            page.ChildRemoved -= OnTabRemoved;
            page.ChildAdded -= OnTabAdded;

            BadgeViews.Clear();
        }
    }
}
