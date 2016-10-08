using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Content.Res;
using Android.Graphics.Drawables.Shapes;

namespace Plugin.Badge.Droid
{
    /**
 * A simple text label view that can be applied as a "badge" to any given {@link android.view.View}. 
 * This class is intended to be instantiated at runtime rather than included in XML layouts.
 * 
 * @author Jeff Gilfelt
 */
    public class BadgeView : TextView
    {

        public const int POSITION_TOP_LEFT = 1;
        public const int POSITION_TOP_RIGHT = 2;
        public const int POSITION_BOTTOM_LEFT = 3;
        public const int POSITION_BOTTOM_RIGHT = 4;
        public const int POSITION_CENTER = 5;


        private const int DEFAULT_HMARGIN_DIP = -10;
        private const int DEFAULT_VMARGIN_DIP = -5;
        private const int DEFAULT_LR_PADDING_DIP = 4;
        private const int DEFAULT_CORNER_RADIUS_DIP = 7;
        private const int DEFAULT_POSITION = POSITION_TOP_RIGHT;
        private static readonly Color DEFAULT_BADGE_COLOR = Color.ParseColor("#CCFF0000"); //Color.RED;
        private static readonly Color DEFAULT_TEXT_COLOR = Color.White;


        private static Animation fadeIn;
        private static Animation fadeOut;


        private Context context;
        private View target;


        private int badgePosition;
        private int badgeMarginH;
        private int badgeMarginV;
        private Color badgeColor;


        private bool isShown;


        private ShapeDrawable badgeBg;


        private int targetTabIndex;


        public BadgeView(Context context) : this(context, (IAttributeSet)null, Android.Resource.Attribute.TextViewStyle)
        {

        }


        public BadgeView(Context context, IAttributeSet attrs) : this(context, attrs, Android.Resource.Attribute.TextViewStyle)
        {

        }

        /**
         * Constructor -
         * 
         * create a new BadgeView instance attached to a target {@link android.view.View}.
         *
         * @param context context for this view.
         * @param target the View to attach the badge to.
         */

        public BadgeView(Context context, View target) : this(context, null,Android.Resource.Attribute.TextViewStyle, target, 0)
        {

        }

        /**
         * Constructor -
         * 
         * create a new BadgeView instance attached to a target {@link android.widget.TabWidget}
         * tab at a given index.
         *
         * @param context context for this view.
         * @param target the TabWidget to attach the badge to.
         * @param index the position of the tab within the target.
         */

        public BadgeView(Context context, TabWidget target, int index) : this(context, null, Android.Resource.Attribute.TextViewStyle, target, index)
        {

        }


        public BadgeView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, null, 0)
        {

        }


        public BadgeView(Context context, IAttributeSet attrs, int defStyle, View target, int tabIndex) : base(context, attrs, defStyle)
        {
            init(context, target, tabIndex);
        }

        private void init(Context context, View target, int tabIndex)
        {


            this.context = context;
            this.target = target;
            this.targetTabIndex = tabIndex;

            // apply defaults

            badgePosition = DEFAULT_POSITION;
            badgeMarginH = DipToPixels(DEFAULT_HMARGIN_DIP);
            badgeMarginV = DipToPixels(DEFAULT_VMARGIN_DIP);;
            badgeColor = DEFAULT_BADGE_COLOR;


            Typeface = Typeface.DefaultBold;
            int paddingPixels = DipToPixels(DEFAULT_LR_PADDING_DIP);
            SetPadding(paddingPixels, 0, paddingPixels, 0);
            SetTextColor(DEFAULT_TEXT_COLOR);
            SetTextSize(ComplexUnitType.Dip, 11);


            fadeIn = new AlphaAnimation(0, 1);
            fadeIn.Interpolator = new DecelerateInterpolator();
            fadeIn.Duration = 200;

            fadeOut = new AlphaAnimation(1, 0);
            fadeOut.Interpolator = new AccelerateInterpolator();
            fadeOut.Duration = 200;


            isShown = false;


            if (this.target != null)
            {
                applyTo(this.target);
            }
            else {
                show();
            }


        }

        private void applyTo(View target)
        {
            var lp = target.LayoutParameters;
            var parent = target.Parent;
            var container = new FrameLayout(context);// { Orientation = Android.Widget.Orientation.Horizontal };
          
            if (target is TabWidget) {

                // set target to the relevant tab child container

                target = ((TabWidget)target).GetChildTabViewAt(targetTabIndex);
                this.target = target;


                ((ViewGroup)target).AddView(container, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));

            }
            else
                {

                // TODO verify that parent is indeed a ViewGroup

                ViewGroup group = (ViewGroup)parent;
                group.SetClipChildren(false);
                group.SetClipToPadding(false);

                int index = group.IndexOfChild(target);


                group.RemoveView(target);
                group.AddView(container, index, lp);


                container.AddView(target);
                group.Invalidate();

            }

            Visibility = ViewStates.Gone;
            container.AddView(this);

        }

        /**
         * Make the badge visible in the UI.
         * 
         */

        public void show()
        {
            show(false, null);
        }

        /**
         * Make the badge visible in the UI.
         *
         * @param animate flag to apply the default fade-in animation.
         */

        public void show(bool animate)
        {
            show(animate, fadeIn);
        }

        /**
         * Make the badge visible in the UI.
         *
         * @param anim Animation to apply to the view when made visible.
         */

        public void show(Animation anim)
        {
            show(true, anim);
        }

        /**
         * Make the badge non-visible in the UI.
         * 
         */

        public void hide()
        {
            hide(false, null);
        }

        /**
         * Make the badge non-visible in the UI.
         *
         * @param animate flag to apply the default fade-out animation.
         */

        public void hide(bool animate)
        {
            hide(animate, fadeOut);
        }

        /**
         * Make the badge non-visible in the UI.
         *
         * @param anim Animation to apply to the view when made non-visible.
         */

        public void hide(Animation anim)
        {
            hide(true, anim);
        }

        /**
         * Toggle the badge visibility in the UI.
         * 
         */

        public void toggle()
        {
            toggle(false, null, null);
        }

        /**
         * Toggle the badge visibility in the UI.
         * 
         * @param animate flag to apply the default fade-in/out animation.
         */

        public void toggle(bool animate)
        {
            toggle(animate, fadeIn, fadeOut);
        }

        /**
         * Toggle the badge visibility in the UI.
         *
         * @param animIn Animation to apply to the view when made visible.
         * @param animOut Animation to apply to the view when made non-visible.
         */

        public void toggle(Animation animIn, Animation animOut)
        {
            toggle(true, animIn, animOut);
        }


        private void show(bool animate, Animation anim)
        {
            if (Background == null)
            {
                if (badgeBg == null)
                {
                    badgeBg = getDefaultBackground();
                }

                SetBackgroundDrawable(badgeBg);
            }
            applyLayoutParams();


            if (animate)
            {
                StartAnimation(anim);
            }
            Visibility = ViewStates.Visible;
            isShown = true;
        }


        private void hide(bool animate, Animation anim)
        {
            Visibility = ViewStates.Gone;
            if (animate)
            {
                this.StartAnimation(anim);
            }
            isShown = false;
        }


        private void toggle(bool animate, Animation animIn, Animation animOut)
        {
            if (isShown)
            {
                hide(animate && (animOut != null), animOut);
            }
            else {
                show(animate && (animIn != null), animIn);
            }
        }

        /**
         * Increment the numeric badge label. If the current badge label cannot be converted to
         * an integer value, its label will be set to "0".
         * 
         * @param offset the increment offset.
         */

       

        /**
         * Decrement the numeric badge label. If the current badge label cannot be converted to
         * an integer value, its label will be set to "0".
         * 
         * @param offset the decrement offset.
         */

       

        private ShapeDrawable getDefaultBackground()
        {


            int r = DipToPixels(DEFAULT_CORNER_RADIUS_DIP);
            float[] outerR = new float[] { r, r, r, r, r, r, r, r };


            RoundRectShape rr = new RoundRectShape(outerR, null, null);
            ShapeDrawable drawable = new ShapeDrawable(rr);
            drawable.Paint.Color = badgeColor;

            return drawable;

        }


        private void applyLayoutParams()
        {


            var lp = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            switch (badgePosition)
            {
                case POSITION_TOP_LEFT:
                    lp.Gravity = GravityFlags.Left | GravityFlags.Top;
                    lp.SetMargins(badgeMarginH, badgeMarginV, 0, 0);
                    break;
                case POSITION_TOP_RIGHT:
                    lp.Gravity = GravityFlags.Right | GravityFlags.Top;
                    lp.SetMargins(0, badgeMarginV, badgeMarginH, 0);
                    break;
                case POSITION_BOTTOM_LEFT:
                    lp.Gravity = GravityFlags.Left | GravityFlags.Bottom;
                    lp.SetMargins(badgeMarginH, 0, 0, badgeMarginV);
                    break;
                case POSITION_BOTTOM_RIGHT:
                    lp.Gravity = GravityFlags.Right | GravityFlags.Bottom;
                    lp.SetMargins(0, 0, badgeMarginH, badgeMarginV);
                    break;
                case POSITION_CENTER:
                    lp.Gravity = GravityFlags.Center;
                    lp.SetMargins(0, 0, 0, 0);
                    break;
                default:
                    break;
            }


            LayoutParameters = lp;


        }

        /**
         * Returns the target View this badge has been attached to.
         * 
         */
        public View getTarget()
        {
            return target;
        }

        /**
         * Is this badge currently visible in the UI?
         * 
         */
 
        public override bool IsShown => isShown;


        /**
         * Returns the positioning of this badge.
         * 
         * one of POSITION_TOP_LEFT, POSITION_TOP_RIGHT, POSITION_BOTTOM_LEFT, POSITION_BOTTOM_RIGHT, POSTION_CENTER.
         * 
         */
        public int getBadgePosition()
        {
            return badgePosition;
        }

        /**
         * Set the positioning of this badge.
         * 
         * @param layoutPosition one of POSITION_TOP_LEFT, POSITION_TOP_RIGHT, POSITION_BOTTOM_LEFT, POSITION_BOTTOM_RIGHT, POSTION_CENTER.
         * 
         */
        public void setBadgePosition(int layoutPosition)
        {
            this.badgePosition = layoutPosition;
        }

        /**
         * Returns the horizontal margin from the target View that is applied to this badge.
         * 
         */
        public int getHorizontalBadgeMargin()
        {
            return badgeMarginH;
        }

        /**
         * Returns the vertical margin from the target View that is applied to this badge.
         * 
         */

        public int getVerticalBadgeMargin()
        {
            return badgeMarginV;
        }

        /**
         * Set the horizontal/vertical margin from the target View that is applied to this badge.
         * 
         * @param badgeMargin the margin in pixels.
         */
        public void setBadgeMargin(int badgeMargin)
        {
            this.badgeMarginH = badgeMargin;
            this.badgeMarginV = badgeMargin;
        }

        /**
         * Set the horizontal/vertical margin from the target View that is applied to this badge.
         * 
         * @param horizontal margin in pixels.
         * @param vertical margin in pixels.
         */

        public void setBadgeMargin(int horizontal, int vertical)
        {
            this.badgeMarginH = horizontal;
            this.badgeMarginV = vertical;
        }

        /**
         * Returns the color value of the badge background.
         * 
         */

        public int getBadgeBackgroundColor()
        {
            return badgeColor;
        }

        /**
         * Set the color value of the badge background.
         * 
         * @param badgeColor the badge background color.
         */
        public void setBadgeBackgroundColor(Color badgeColor)
        {
            this.badgeColor = badgeColor;
            badgeBg = getDefaultBackground();
        }


        private int DipToPixels(int dip)
        { 
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, Resources.DisplayMetrics);
        }

    }
}
