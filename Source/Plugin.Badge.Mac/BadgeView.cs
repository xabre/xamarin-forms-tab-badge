using System.Linq;
using AppKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Plugin.Badge.Mac
{
    /// <summary>
    /// A badge view with ediatble properties that can be added to any view.
    /// </summary>
    public class BadgeView : NSTextField
    {
        public string Text
        {
            get => StringValue;
            set
            {
                StringValue = value;
                Hidden = string.IsNullOrEmpty(value);
                SizeToFit();
            }
        }

        public Color Color
        {
            set => BackgroundColor = value == Color.Default ? Color.Red.ToNSColor() : value.ToNSColor();
        }

        public new Color TextColor
        {
            set => base.TextColor = value == Color.Default ? NSColor.White : value.ToNSColor();
        }

        public new Font Font
        {
            set => base.Font = value == Font.Default ? NSFont.SystemFontOfSize(NSFont.SmallSystemFontSize) : value.ToNSFont();
        }

        /// <summary>
        /// Creates and adds a badge to the target view.
        /// </summary>
        /// <param name="target">Target view to add badge to.</param>
        /// <param name="alignRelativeToContent">Tries to align the badge relative to the first contecnt of the target</param>
        public BadgeView(NSView target, bool alignRelativeToContent = false)
        {
            Alignment = NSTextAlignment.Center;
            Editable = false;
            Text = string.Empty;
            TranslatesAutoresizingMaskIntoConstraints = false;
            Font = Font.Default;
            Bordered = false;
            LineBreakMode = NSLineBreakMode.TruncatingTail;
            UsesSingleLineMode = true;
            WantsLayer = true;
            Layer.CornerRadius = 7.5f;
            target.AddSubview(this);

            WidthAnchor.ConstraintLessThanOrEqualToConstant(35).Active = true;

         
            if (alignRelativeToContent && target.Subviews.Any())
            {
                LeftAnchor.ConstraintEqualToAnchor(target.Subviews.First().RightAnchor, 0).Active = true;
                CenterYAnchor.ConstraintEqualToAnchor(target.Subviews.First().CenterYAnchor).Active = true;
            }
            else
            {
				TopAnchor.ConstraintEqualToAnchor(target.TopAnchor, 0).Active = true;
                RightAnchor.ConstraintEqualToAnchor(target.RightAnchor, 0).Active = true;
			}
        }
    }
}
