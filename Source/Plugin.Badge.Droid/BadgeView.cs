using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Graphics.Drawables.Shapes;

namespace Plugin.Badge.Droid
{
    public class BadgeView : TextView
    {
        public enum BadgePosition
        {
            PositionTopLeft = 1,
            PositionTopRight = 2,
            PositionBottomLeft = 3,
            PositionBottomRight = 4,
            PositionCenter = 5
        }


        private const int DefaultHmarginDip = -10;
        private const int DefaultVmarginDip = -5;
        private const int DefaultLrPaddingDip = 4;
        private const int DefaultCornerRadiusDip = 7;
        private const BadgePosition DefaultPosition = BadgePosition.PositionTopRight;
        private static readonly Color DefaultTextColor = Color.White;


        private static Animation _fadeIn;
        private static Animation _fadeOut;


        public View Target { get; private set; }
        private Context _context;


        public BadgePosition Postion { get; set; } = DefaultPosition;
        public int BadgeMarginH { get; set; }
        public int BadgeMarginV { get; set; }

        public static int TextSizeDip { get; set; } = 11;

        public Color BadgeColor { get; set; } = Color.ParseColor("#CCFF0000");

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
            Postion = DefaultPosition;
            BadgeMarginH = DipToPixels(DefaultHmarginDip);
            BadgeMarginV = DipToPixels(DefaultVmarginDip);

            Typeface = Typeface.DefaultBold;
            var paddingPixels = DipToPixels(DefaultLrPaddingDip);
            SetPadding(paddingPixels, 0, paddingPixels, 0);
            SetTextColor(DefaultTextColor);
            SetTextSize(ComplexUnitType.Dip, TextSizeDip);


            _fadeIn = new AlphaAnimation(0, 1)
            {
                Interpolator = new DecelerateInterpolator(),
                Duration = 200
            };

            _fadeOut = new AlphaAnimation(1, 0)
            {
                Interpolator = new AccelerateInterpolator(),
                Duration = 200
            };


            if (Target != null)
            {
                ApplyTo(Target);
            }
            else
            {
                Show();
            }
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
            Show(animate, _fadeIn);
        }



        public void Show(Animation anim)
        {
            Show(true, anim);
        }

        public void Hide()
        {
            Hide(false, null);
        }

        /**
         * Make the badge non-visible in the UI.
         *
         * @param animate flag to apply the default fade-out animation.
         */

        public void Hide(bool animate)
        {
            Hide(animate, _fadeOut);
        }
    
        public void Hide(Animation anim)
        {
            Hide(true, anim);
        }

        public void Toggle(bool animate = true)
        {
            Toggle(animate, animate ? _fadeIn : null, animate ? _fadeOut : null);
        }

        public void Toggle(Animation animIn, Animation animOut)
        {
            Toggle(true, animIn, animOut);
        }

        private void Show(bool animate, Animation anim)
        {

            Background = Background ?? DefaultBackground;

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


        private void Toggle(bool animate, Animation animIn, Animation animOut)
        {
            if (Visibility == ViewStates.Visible)
            {
                Hide(animate && (animOut != null), animOut);
            }
            else
            {
                Show(animate && (animIn != null), animIn);
            }
        }

        private ShapeDrawable DefaultBackground
        {
            get
            {
                var radius = DipToPixels(DefaultCornerRadiusDip);
                var outerR = new float[] { radius, radius, radius, radius, radius, radius, radius, radius };

                var drawable = new ShapeDrawable(new RoundRectShape(outerR, null, null));
                drawable.Paint.Color = BadgeColor;
                return drawable;
            }
        }

        private void ApplyLayoutParams()
        {
            var layoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            switch (Postion)
            {
                case BadgePosition.PositionTopLeft:
                    layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Top;
                    layoutParameters.SetMargins(BadgeMarginH, BadgeMarginV, 0, 0);
                    break;
                case BadgePosition.PositionTopRight:
                    layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Top;
                    layoutParameters.SetMargins(0, BadgeMarginV, BadgeMarginH, 0);
                    break;
                case BadgePosition.PositionBottomLeft:
                    layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Bottom;
                    layoutParameters.SetMargins(BadgeMarginH, 0, 0, BadgeMarginV);
                    break;
                case BadgePosition.PositionBottomRight:
                    layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Bottom;
                    layoutParameters.SetMargins(0, 0, BadgeMarginH, BadgeMarginV);
                    break;
                case BadgePosition.PositionCenter:
                    layoutParameters.Gravity = GravityFlags.Center;
                    layoutParameters.SetMargins(0, 0, 0, 0);
                    break;
            }

            LayoutParameters = layoutParameters;

        }

        private int DipToPixels(int dip)
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
