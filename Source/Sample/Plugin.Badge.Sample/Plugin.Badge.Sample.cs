using System;

using Xamarin.Forms;
using Plugin.Badge.Sample.ViewModels;
using Plugin.Badge.Abstractions;

namespace Plugin.Badge.Sample
{
    public class App : Application
    {
        private TabbedPage _tabbedPage;

        public App()
        {
            MainPage = new NavigationPage(new ContentPage
            {
                Title = "XF Tab Badges",
                Content = new StackLayout
                {
                    Children =
                    {
                        new Button
                        {
                            Text = "Tabbed page as root",
                            Command = new Command(CreateTabedPageAsRoot),
                            VerticalOptions = LayoutOptions.Center
                        },

                        new Button
                        {
                            Text = "Tabbed page inside navigation page",
                            Command = new Command(CreateTabedPageInsideNavigationPage),
                            VerticalOptions = LayoutOptions.Center
                        },

                        new Button
                        {
                            Text = "Tabbed page root with children as navigation pages",
                            Command = new Command(CreateTabedPageWithNavigationPageChildren),
                            VerticalOptions = LayoutOptions.Center
                        },
                    }
                }
            });
        }

        private void CreateTabedPageAsRoot()
        {
            var tab1 = CreateTab1();

            var tab2 = CreateTab2();

            var tab3 = CreateTab3();

            // The root page of your application
            _tabbedPage = new TabbedPage
            {
                Title = "Tab badge sample",
                Children =
                {
                    tab1,
                    tab2,
                    tab3
                }
            };

            _tabbedPage.ToolbarItems.Add(new ToolbarItem("Item1", "tabicon.png", () => { }, ToolbarItemOrder.Primary));
            MainPage = new NavigationPage(_tabbedPage);
        }

        private void CreateTabedPageWithNavigationPageChildren()
        {
            var tab1 = new NavigationPage(CreateTab1());

            var tab2 = new NavigationPage(CreateTab2());

            var tab3 = new NavigationPage(CreateTab3());

            // The root page of your application
            _tabbedPage = new TabbedPage
            {
                Title = "Tab badge sample",
                Children =
                {
                    tab1,
                    tab2,
                    tab3
                }
            };

            MainPage = _tabbedPage;
        }

        private void CreateTabedPageInsideNavigationPage()
        {
            var tab1 = CreateTab1();

            var tab2 = CreateTab2();

            var tab3 = CreateTab3();

            // The root page of your application
            _tabbedPage = new TabbedPage
            {
                Title = "Tab badge sample",
                Children =
                {
                    tab1,
                    tab2,
                    tab3
                }
            };

            _tabbedPage.ToolbarItems.Add(new ToolbarItem("Item1", "tabicon.png", () => { }, ToolbarItemOrder.Primary));
            (MainPage as NavigationPage)?.PushAsync(_tabbedPage);
        }

        private ContentPage CreateTab2()
        {
            var tab2 = CreateTab1();
            tab2.Icon = "tabicon.png";
            (tab2.BindingContext as Tab1ViewModel).CountValue = 1;
            return tab2;
        }

        private ContentPage CreateTab3()
        {
            var tab3 = new ContentPage
            {
                Title = "Tab3",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.AliceBlue,
                    Children =
                    {
                        new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms Tab3!"
                        },
                        new Image
                        {

                            Margin = new Thickness(0, 40),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.Center,
                            Source = "tabicon.png"
                        },
                        new Button
                        {
                            Text = "Navigate to other page",
                            Command =  new Command(() =>
                            {
                                (this.MainPage as NavigationPage)?.PushAsync(CreateNavigationPage());
                            })
                        }
                    }
                }
            };

            TabBadge.SetBadgeText(tab3, "1+");
            TabBadge.SetBadgeColor(tab3, Color.FromHex("#A0FFA500"));
            return tab3;
        }

        private ContentPage CreateTab1()
        {
            var tab1Layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "Xamarin Forms Tab Badge Sample",
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold
                    },
                }
            };

            var stepper = new Stepper
            {
                Increment = 1,
                Maximum = 111,
                Minimum = 0,
                VerticalOptions = LayoutOptions.Center
            };

            stepper.SetBinding(Stepper.ValueProperty, nameof(Tab1ViewModel.CountValue), BindingMode.TwoWay);

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(new Label { Text = "Increment / Decrement Value: ", HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center }, 0, 0);
            grid.Children.Add(stepper, 1, 0);
            tab1Layout.Children.Add(grid);

            var buttonChangeColor = new Button { Text = "Change Color" };
            buttonChangeColor.SetBinding(Button.CommandProperty, nameof(Tab1ViewModel.ChangeColorCommand));
            tab1Layout.Children.Add(buttonChangeColor);

            var buttonChangeTextColor = new Button { Text = "Change Text Color" };
            buttonChangeTextColor.SetBinding(Button.CommandProperty, nameof(Tab1ViewModel.ChangeTextColorCommand));
            tab1Layout.Children.Add(buttonChangeTextColor);

            var buttonChangeFontAttributes = new Button { Text = "Change Font Attributes" };
            buttonChangeFontAttributes.SetBinding(Button.CommandProperty, nameof(Tab1ViewModel.ChangeFontAttributesCommand));
            tab1Layout.Children.Add(buttonChangeFontAttributes);

            var buttonChangePosition = new Button { Text = "Change Position" };
            buttonChangePosition.SetBinding(Button.CommandProperty, nameof(Tab1ViewModel.ChangePositionCommand));
            tab1Layout.Children.Add(buttonChangePosition);

            var buttonAddTab = new Button() { Text = "Add tab" };
            buttonAddTab.Clicked += ButtonAddTab_Clicked;
            var buttonRemoveTab = new Button() { Text = "Remove tab" };
            buttonRemoveTab.Clicked += ButtonRemoveTab_Clicked;

            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(buttonAddTab, 0, 0);
            grid.Children.Add(buttonRemoveTab, 1, 0);
            tab1Layout.Children.Add(grid);

            grid = new Grid { RowSpacing = 0 };
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            var leftStepper = new Stepper { Increment = 1, Minimum = -50, Maximum = 50 };
            leftStepper.SetBinding(Stepper.ValueProperty, nameof(Tab1ViewModel.MarginLeft), BindingMode.TwoWay);
            var leftMarginLabel = new Label();
            leftMarginLabel.SetBinding(Label.TextProperty, nameof(Tab1ViewModel.MarginLeft), stringFormat: "Left: {0}");
            grid.Children.Add(leftMarginLabel, 0, 0);
            grid.Children.Add(leftStepper, 0, 1);

            var topStepper = new Stepper { Increment = 1, Minimum = -50, Maximum = 50 };
            topStepper.SetBinding(Stepper.ValueProperty, nameof(Tab1ViewModel.MarginTop), BindingMode.TwoWay);
            var topMarginLabel = new Label();
            topMarginLabel.SetBinding(Label.TextProperty, nameof(Tab1ViewModel.MarginTop), stringFormat: "Top: {0}");
            grid.Children.Add(topMarginLabel, 1, 0);
            grid.Children.Add(topStepper, 1, 1);

            var rightStepper = new Stepper { Increment = 1, Minimum = -50, Maximum = 50 };
            rightStepper.SetBinding(Stepper.ValueProperty, nameof(Tab1ViewModel.MarginRight), BindingMode.TwoWay);
            var rightMarginLabel = new Label();
            rightMarginLabel.SetBinding(Label.TextProperty, nameof(Tab1ViewModel.MarginRight), stringFormat: "Right: {0}");
            grid.Children.Add(rightMarginLabel, 0, 2);
            grid.Children.Add(rightStepper, 0, 3);

            var bottomStepper = new Stepper { Increment = 1, Minimum = -50, Maximum = 50 };
            bottomStepper.SetBinding(Stepper.ValueProperty, nameof(Tab1ViewModel.MarginBottom), BindingMode.TwoWay);
            var bottomMarginLabel = new Label();
            bottomMarginLabel.SetBinding(Label.TextProperty, nameof(Tab1ViewModel.MarginBottom), stringFormat: "Bottom: {0}");
            grid.Children.Add(bottomMarginLabel, 1, 2);
            grid.Children.Add(bottomStepper, 1, 3);

            tab1Layout.Children.Add(grid);

            var tab1 = new ContentPage
            {
                Title = "Tab1",
                Content = new ScrollView() { Content = tab1Layout }
            };

            tab1.SetBinding(TabBadge.BadgeTextProperty, nameof(Tab1ViewModel.Count));
            tab1.SetBinding(TabBadge.BadgeColorProperty, nameof(Tab1ViewModel.BadgeColor));
            tab1.SetBinding(TabBadge.BadgeTextColorProperty, nameof(Tab1ViewModel.BadgeTextColor));
            tab1.SetBinding(TabBadge.BadgeFontProperty, nameof(Tab1ViewModel.BadgeFont));
            tab1.SetBinding(TabBadge.BadgePositionProperty, nameof(Tab1ViewModel.Position));
            tab1.SetBinding(TabBadge.BadgeMarginProperty, nameof(Tab1ViewModel.Margin));

            tab1.BindingContext = new Tab1ViewModel();
            return tab1;
        }

        private ContentPage CreateNavigationPage()
        {
            return new ContentPage()
            {
                Title = "Demo navigation",
                Content = new StackLayout()
                {
                    Children =
                    {
                        new Button()
                        {
                            Text = "Use navigation page for tabs",
                            Command = new Command(() =>
                            {
                                (this.MainPage as NavigationPage)?.PushAsync(new TabbedPage
                                {
                                    Title = "Tabed page & content pages navigation page",
                                    Children =
                                    {
                                        new NavigationPage(CreateTab1()),
                                        new NavigationPage(CreateTab2())
                                    }
                                });
                            })
                        }
                    }
                }
            };
        }

        private void ButtonRemoveTab_Clicked(object sender, EventArgs e)
        {
            if (_tabbedPage.Children.Count > 1)
            {
                _tabbedPage.Children.RemoveAt(_tabbedPage.Children.Count - 1);
            }
        }

        private void ButtonAddTab_Clicked(object sender, EventArgs e)
        {
            var newTab = new ContentPage
            {
                Title = "Tab sample",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms Tab sample!"
                        },
                    }
                }
            };

            _tabbedPage.Children.Add(newTab);

            TabBadge.SetBadgeText(newTab, "#1");
            TabBadge.SetBadgeColor(newTab, Color.Black);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        
    }
}

