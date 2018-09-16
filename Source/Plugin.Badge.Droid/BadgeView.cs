using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Graphics.Drawables.Shapes;
using Android.Support.V4.View;
using Plugin.Badge.Abstractions;

namespace Plugin.Badge.Droid
{
    public class BadgeView : TextView
    {
        private const int DefaultHmarginDip = -10;
        private const int DefaultVmarginDip = -5;
        private const int DefaultLrPaddingDip = 4;
        private const int DefaultCornerRadiusDip = 7;

        private static Animation _fadeInAnimation;
        private static Animation _fadeOutAnimation;

        private Context _context;
        private readonly Color _defaultBadgeColor = Color.ParseColor("#CCFF0000");
        private ShapeDrawable _backgroundShape;
        private BadgePosition _position;

        public View Target { get; private set; }
        private int _badgeMarginL;
        private int _badgeMarginR;
        private int _badgeMarginT;
        private int _badgeMarginB;

        public static int TextSizeDip { get; set; } = 11;

        public BadgePosition Postion
        {
            get => _position;

            set
            {
                if (_position == value)
                {
                    return;
                }
                
                _position = value;
                ApplyLayoutParams();
            }
        }

        public Color BadgeColor
        {
            get { return _backgroundShape.Paint.Color; }
            set
            {
                _backgroundShape.Paint.Color = value;

                Background.InvalidateSelf();
            }
        }

        public Color TextColor
        {
            get => new Color(CurrentTextColor);
            set => SetTextColor(value);
        }

        public void SetMargins(float left, float top, float right, float bottom)
        {
            _badgeMarginL = DipToPixels(left);
            _badgeMarginT = DipToPixels(top);
            _badgeMarginR = DipToPixels(right);
            _badgeMarginB = DipToPixels(bottom);

            ApplyLayoutParams();
        }

        public BadgeView(Context context, View target) : this(context, null, Android.Resource.Attribute.TextViewStyle, target)
        {
        }

        public BadgeView(Context context, IAttributeSet attrs, int defStyle, View target) : base(context, attrs, defStyle)
        {
            Init(context, target);
        }

        private void Init(Context context, View target)
        {
            _context = context;
            Target = target;

            // apply defaults
            _badgeMarginL = DipToPixels(DefaultHmarginDip);
            _badgeMarginT = DipToPixels(DefaultVmarginDip);
            _badgeMarginR = DipToPixels(DefaultHmarginDip);
            _badgeMarginB = DipToPixels(DefaultVmarginDip);
            
            Typeface = Typeface.DefaultBold;
            var paddingPixels = DipToPixels(DefaultLrPaddingDip);
            SetPadding(paddingPixels, 0, paddingPixels, 0);
            SetTextColor(Color.White);
            SetTextSize(ComplexUnitType.Dip, TextSizeDip);

            _fadeInAnimation = new AlphaAnimation(0, 1)
            {
                Interpolator = new DecelerateInterpolator(),
                Duration = 200
            };

            _fadeOutAnimation = new AlphaAnimation(1, 0)
            {
                Interpolator = new AccelerateInterpolator(),
                Duration = 200
            };

            _backgroundShape = CreateBackgroundShape();
            ViewCompat.SetBackground(this, _backgroundShape);
            BadgeColor = _defaultBadgeColor;

            if (Target != null)
            {
                ApplyTo(Target);
            }
            else
            {
                Show();
            }
        }

        private ShapeDrawable CreateBackgroundShape()
        {
            var radius = DipToPixels(DefaultCornerRadiusDip);
            var outerR = new float[] { radius, radius, radius, radius, radius, radius, radius, radius };

            return new ShapeDrawable(new RoundRectShape(outerR, null, null));
        }

        private void ApplyTo(View target)
        {
            var lp = target.LayoutParameters;
            var parent = target.Parent;

            var group = parent as ViewGroup;
            if (group == null)
            {
                Console.WriteLine("Badge target parent has to be a view group");
                return;
            }

            group.SetClipChildren(false);
            group.SetClipToPadding(false);

            var container = new FrameLayout(_context);
            var index = group.IndexOfChild(target);

            group.RemoveView(target);
            group.AddView(container, index, lp);

            container.AddView(target);
            group.Invalidate();

            Visibility = ViewStates.Gone;
            container.AddView(this);

        }

        public void Show()
        {
            Show(false, null);
        }

        public void Show(bool animate)
        {
            Show(animate, _fadeInAnimation);
        }


        public void Hide(bool animate)
        {
            Hide(animate, _fadeOutAnimation);
        }

        private void Show(bool animate, Animation anim)
        {
            ApplyLayoutParams();

            if (animate)
            {
                StartAnimation(anim);
            }

            Visibility = ViewStates.Visible;

        }

        private void Hide(bool animate, Animation anim)
        {
            Visibility = ViewStates.Gone;
            if (animate)
            {
                StartAnimation(anim);
            }
        }

        private void ApplyLayoutParams()
        {
            var layoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            switch (Postion)
            {
                case BadgePosition.PositionTopLeft:
                    layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Top;
                    layoutParameters.SetMargins(_badgeMarginL, _badgeMarginT, 0, 0);
                    break;
                case BadgePosition.PositionTopRight:
                    layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Top;
                    layoutParameters.SetMargins(0, _badgeMarginT, _badgeMarginR, 0);
                    break;
                case BadgePosition.PositionBottomLeft:
                    layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Bottom;
                    layoutParameters.SetMargins(_badgeMarginL, 0, 0, _badgeMarginB);
                    break;
                case BadgePosition.PositionBottomRight:
                    layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Bottom;
                    layoutParameters.SetMargins(0, 0, _badgeMarginR, _badgeMarginB);
                    break;
                case BadgePosition.PositionCenter:
                    layoutParameters.Gravity = GravityFlags.Center;
                    layoutParameters.SetMargins(0, 0, 0, 0);
                    break;
                case BadgePosition.PositionTopCenter:
                    layoutParameters.Gravity = GravityFlags.Center | GravityFlags.Top;
                    layoutParameters.SetMargins(0, _badgeMarginT, 0, 0);
                    break;
                case BadgePosition.PositionBottomCenter:
                    layoutParameters.Gravity = GravityFlags.Center | GravityFlags.Bottom;
                    layoutParameters.SetMargins(0, 0, 0, _badgeMarginB);
                    break;
                case BadgePosition.PositionLeftCenter:
                    layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Center;
                    layoutParameters.SetMargins(_badgeMarginL, 0, 0, 0);
                    break;
                case BadgePosition.PositionRightCenter:
                    layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Center;
                    layoutParameters.SetMargins(0, 0, _badgeMarginR, 0);
                    break;
            }

            LayoutParameters = layoutParameters;

        }

        private int DipToPixels(float dip)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, Resources.DisplayMetrics);
        }

        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;

                if (Visibility == ViewStates.Visible && string.IsNullOrEmpty(value))
                {
                    Hide(true);
                }
                else if (Visibility == ViewStates.Gone && !string.IsNullOrEmpty(value))
                {
                    Show(true);
                }
            }
        }

    }
}
