using System;
using System.Linq;
using AppKit;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Plugin.Badge.Mac
{
    /// <summary>
    /// A badge view with ediatble properties that can be added to any view.
    /// </summary>
    public class BadgeView : NSTextField
    {
        private NSLayoutConstraint _topConstraint;
        private NSLayoutConstraint _bottomConstraint;
        private NSLayoutConstraint _leftConstraint;
        private NSLayoutConstraint _rightConstraint;
        private NSLayoutConstraint _horizontalCenterConstraint;
        private NSLayoutConstraint _verticalCenterConstraint;
        private BadgePosition _position = BadgePosition.PositionTopRight;
        private Thickness _margin;

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

        public BadgePosition Position
        {
            get => _position;
            set
            {
                _position = value;
                ApplyConstraints();
            }
        }

        public Thickness Margin
        {
            get => _margin;
            set {
                _margin = value;
                _leftConstraint.Constant = (nfloat)value.Left;
                _topConstraint.Constant = (nfloat)value.Top;
                _rightConstraint.Constant = (nfloat)value.Right;
                _bottomConstraint.Constant = (nfloat)value.Bottom;
            }
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
            Layer.ZPosition = 1;

            WidthAnchor.ConstraintLessThanOrEqualToConstant(35).Active = true;

            if (alignRelativeToContent && target.Subviews.Any())
            {
                target = target.Subviews.First();
            }

            _topConstraint = TopAnchor.ConstraintEqualToAnchor(target.TopAnchor, 0);
            _bottomConstraint = BottomAnchor.ConstraintEqualToAnchor(target.BottomAnchor, 0);
            _leftConstraint = LeftAnchor.ConstraintEqualToAnchor(target.LeftAnchor, 0);
            _rightConstraint = RightAnchor.ConstraintEqualToAnchor(target.RightAnchor, 0);
            _horizontalCenterConstraint = CenterXAnchor.ConstraintEqualToAnchor(target.CenterXAnchor, 0);
            _verticalCenterConstraint = CenterYAnchor.ConstraintEqualToAnchor(target.CenterYAnchor, 0);

            ApplyConstraints();
        }

        private void ApplyConstraints()
        {
            switch (_position)
            {
                case BadgePosition.PositionTopLeft:
                    _topConstraint.Active = true;
                    _leftConstraint.Active = true;
                    _rightConstraint.Active = false;
                    _bottomConstraint.Active = false;
                    _horizontalCenterConstraint.Active = false;
                    _verticalCenterConstraint.Active = false;
                    break;
                case BadgePosition.PositionTopRight:
                    _topConstraint.Active = true;
                    _leftConstraint.Active = false;
                    _rightConstraint.Active = true;
                    _bottomConstraint.Active = false;
                    _horizontalCenterConstraint.Active = false;
                    _verticalCenterConstraint.Active = false;
                    break;
                case BadgePosition.PositionBottomRight:
                    _topConstraint.Active = false;
                    _leftConstraint.Active = false;
                    _rightConstraint.Active = true;
                    _bottomConstraint.Active = true;
                    _horizontalCenterConstraint.Active = false;
                    _verticalCenterConstraint.Active = false;
                    break;
                case BadgePosition.PositionBottomLeft:
                    _topConstraint.Active = false;
                    _leftConstraint.Active = true;
                    _rightConstraint.Active = false;
                    _bottomConstraint.Active = true;
                    _horizontalCenterConstraint.Active = false;
                    _verticalCenterConstraint.Active = false;
                    break;
                case BadgePosition.PositionTopCenter:
                    _topConstraint.Active = true;
                    _leftConstraint.Active = false;
                    _rightConstraint.Active = false;
                    _bottomConstraint.Active = false;
                    _horizontalCenterConstraint.Active = true;
                    _verticalCenterConstraint.Active = false;
                    break;
                case BadgePosition.PositionRightCenter:
                    _topConstraint.Active = false;
                    _leftConstraint.Active = false;
                    _rightConstraint.Active = true;
                    _bottomConstraint.Active = false;
                    _horizontalCenterConstraint.Active = false;
                    _verticalCenterConstraint.Active = true;
                    break;
                case BadgePosition.PositionBottomCenter:
                    _topConstraint.Active = false;
                    _leftConstraint.Active = false;
                    _rightConstraint.Active = false;
                    _bottomConstraint.Active = true;
                    _horizontalCenterConstraint.Active = true;
                    _verticalCenterConstraint.Active = false;
                    break;
                case BadgePosition.PositionLeftCenter:
                    _topConstraint.Active = false;
                    _leftConstraint.Active = true;
                    _rightConstraint.Active = false;
                    _bottomConstraint.Active = false;
                    _horizontalCenterConstraint.Active = false;
                    _verticalCenterConstraint.Active = true;
                    break;
                case BadgePosition.PositionCenter:
                    _topConstraint.Active = false;
                    _leftConstraint.Active = false;
                    _rightConstraint.Active = false;
                    _bottomConstraint.Active = false;
                    _horizontalCenterConstraint.Active = true;
                    _verticalCenterConstraint.Active = true;
                    break;
            }
        }
    }
}
