using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Graphics.Drawables.Shapes;
using AndroidX.Core.View;
using Plugin.Badge.Abstractions;

namespace Plugin.Badge.Droid
{
    public class BadgeView : TextView
    {
        private const int DefaultLrPaddingDip = 4;
        private const int DefaultCornerRadiusDip = 7;

        private static Animation _fadeInAnimation;
        private static Animation _fadeOutAnimation;

        private Context _context;
        private readonly Color _defaultBadgeColor = Color.ParseColor("#CCFF0000");
        private ShapeDrawable _backgroundShape;
        private BadgePosition _position;


        private int _badgeMarginL;
        private int _badgeMarginR;
        private int _badgeMarginT;
        private int _badgeMarginB;

        private bool _hasWrappedLayout;

        public static int TextSizeDip { get; set; } = 11;

        public View Target { get; private set; }

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
            get => _backgroundShape.Paint.Color;
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

        /// <summary>
        /// Creates a badge view for a given view by wrapping both views in a new layout.
        /// </summary>
        /// <returns>The view.</returns>
        /// <param name="context">Context.</param>
        /// <param name="target">Target.</param>
        public static BadgeView ForTarget(Context context, View target)
        {
            var badgeView = new BadgeView(context, null, Android.Resource.Attribute.TextViewStyle);
            badgeView.WrapTargetWithLayout(target);
            return badgeView;
        }

        /// <summary>
        /// Creates a bage view and adds it to the specified layout without adding any additionaly wrapping layouts.
        /// 
        /// </summary>
        /// <returns>The layout.</returns>
        /// <param name="context">Context.</param>
        /// <param name="target">Target</param>
        public static BadgeView ForTargetLayout(Context context, View target)
        {
            var badgeView = new BadgeView(context, null, Android.Resource.Attribute.TextViewStyle);
            badgeView.AddToTargetLayout(target);
            return badgeView;
        }

        private BadgeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            _context = context;

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

            Visibility = ViewStates.Gone;
        }

        private ShapeDrawable CreateBackgroundShape()
        {
            var radius = DipToPixels(DefaultCornerRadiusDip);
            var outerR = new float[] { radius, radius, radius, radius, radius, radius, radius, radius };

            return new ShapeDrawable(new RoundRectShape(outerR, null, null));
        }

        private void AddToTargetLayout(View target)
        {
            var layout = target.Parent as ViewGroup;
            if (layout == null)
            {
                Console.WriteLine("Badge target parent has to be a view group");
                return;
            }

            layout.SetClipChildren(false);
            layout.SetClipToPadding(false);

            layout.AddView(this);

            Target = target;
        }

        private void WrapTargetWithLayout(View target)
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

            container.AddView(this);

            Target = target;
            _hasWrappedLayout = true;
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

            if (!_hasWrappedLayout)
            {
                var targetParams = ((FrameLayout.LayoutParams)Target.LayoutParameters);
                var w = targetParams.Width / 2;
                var h = targetParams.Height / 2;

                layoutParameters.Gravity = GravityFlags.Center;
                switch (Postion)
                {
                    case BadgePosition.PositionTopLeft:
                        layoutParameters.SetMargins(_badgeMarginL - w, _badgeMarginT - h, 0, 0);
                        break;
                    case BadgePosition.PositionTopRight:
                        layoutParameters.SetMargins(0, _badgeMarginT - h, _badgeMarginR - w, 0);
                        break;
                    case BadgePosition.PositionBottomLeft:
                        layoutParameters.SetMargins(_badgeMarginL - w, 0, 0, 0 + _badgeMarginB - h);
                        break;
                    case BadgePosition.PositionBottomRight:
                        layoutParameters.SetMargins(0, 0, _badgeMarginR - w, 0 + _badgeMarginB - h);
                        break;
                    case BadgePosition.PositionCenter:
                        layoutParameters.SetMargins(_badgeMarginL, _badgeMarginT, _badgeMarginR, _badgeMarginB);
                        break;
                    case BadgePosition.PositionTopCenter:
                        layoutParameters.SetMargins(0, 0 + _badgeMarginT - h, 0, 0);
                        break;
                    case BadgePosition.PositionBottomCenter:
                        layoutParameters.SetMargins(0, 0, 0, 0 + _badgeMarginB - h);
                        break;
                    case BadgePosition.PositionLeftCenter:
                        layoutParameters.SetMargins(_badgeMarginL - w, 0, 0, 0);
                        break;
                    case BadgePosition.PositionRightCenter:
                        layoutParameters.SetMargins(0, 0, _badgeMarginR - w, 0);
                        break;
                }
            }
            else
            {
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
            }

            LayoutParameters = layoutParameters;
        }

        private int DipToPixels(float dip)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, Resources.DisplayMetrics);
        }

        public new string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;

                switch (Visibility)
                {
                    case ViewStates.Visible when string.IsNullOrEmpty(value):
                        Hide(true);
                        break;
                    case ViewStates.Gone when !string.IsNullOrEmpty(value):
                        Show(true);
                        break;
                }
            }
        }

    }
}
