using Xamarin.Forms;

namespace Plugin.Badge.Abstractions
{
    /// <summary>
    /// Generic xamarin forms view representing a badge 
    /// </summary>
    public class Badge : Frame
    {
        public static BindableProperty BadgeTextProperty = BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(Badge), default(string), propertyChanged: BadgePropertyChanged);

        public string BadgeText
        {
            get => (string)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }

        public static BindableProperty BadgeTextColorProperty = BindableProperty.Create(nameof(BadgeTextColor), typeof(Color), typeof(Badge), Color.Default, propertyChanged: BadgePropertyChanged);

        public Color BadgeTextColor
        {
            get => (Color)GetValue(BadgeTextColorProperty);
            set => SetValue(BadgeTextColorProperty, value);
        }

        public static BindableProperty BadgeFontAttributesProperty = BindableProperty.Create(nameof(BadgeFontAttributes), typeof(FontAttributes), typeof(Badge), FontAttributes.Bold, propertyChanged: BadgePropertyChanged);

        public FontAttributes BadgeFontAttributes
        {
            get => (FontAttributes)GetValue(BadgeFontAttributesProperty);
            set => SetValue(BadgeFontAttributesProperty, value);
        }

        public static BindableProperty BadgeFontFamilyProperty = BindableProperty.Create(nameof(BadgeFontFamily), typeof(string), typeof(Badge), Font.Default.FontFamily, propertyChanged: BadgePropertyChanged);

        public string BadgeFontFamily
        {
            get => (string)GetValue(BadgeFontFamilyProperty);
            set => SetValue(BadgeFontFamilyProperty, value);
        }

        public static BindableProperty BadgeFontSizeProperty = BindableProperty.Create(nameof(BadgeFontSizeProperty), typeof(double), typeof(Badge), 11.0, propertyChanged: BadgePropertyChanged);

        public double BadgeFontSize
        {
            get => (double)GetValue(BadgeFontSizeProperty);
            set => SetValue(BadgeFontSizeProperty, value);
        }

        private new Label Content => (Label)base.Content;

        public Badge()
        {
            base.Content = new Label {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            this.Padding = new Thickness(5, 3);
            this.VerticalOptions = LayoutOptions.Start;
            this.HorizontalOptions = LayoutOptions.End;
            this.BackgroundColor = Color.Red;

            this.UpdateBadgeProperties();
        }

        private void UpdateBadgeProperties()
        {
            this.Content.FontAttributes = this.BadgeFontAttributes;

            if (!string.IsNullOrWhiteSpace(this.BadgeFontFamily))
            {
                this.Content.FontFamily = this.BadgeFontFamily;
            }

            this.Content.FontSize = this.BadgeFontSize > 0 ? this.BadgeFontSize : this.Content.FontSize;

            this.Content.TextColor = this.BadgeTextColor;

            this.CornerRadius = this.Height > 0f ? (float)this.Height / 2f : (float)this.Content.FontSize * 1.25f / 2f;

            this.IsVisible = !string.IsNullOrWhiteSpace(this.BadgeText);
            this.Content.Text = this.BadgeText;
        }

        private static void BadgePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Badge)?.UpdateBadgeProperties();
        }
    }
}
