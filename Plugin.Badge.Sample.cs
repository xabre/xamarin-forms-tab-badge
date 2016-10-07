using System;

using Xamarin.Forms;
using Plugin.Badge.Sample.ViewModels;

namespace Plugin.Badge.Sample
{
    public class App : Application
    {
        public App()
        {
            var tab1Layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                                new Label {
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    Text = "Welcome to Xamarin Forms Tab1!"
                                },

                            }
            };

            var buttonIncrement = new Button() { Text = "Increment" };
            buttonIncrement.SetBinding(Button.CommandProperty, "IncrementCommand");
            tab1Layout.Children.Add(buttonIncrement);

            var buttonDecrement = new Button() { Text = "Decrement" };
            buttonDecrement.SetBinding(Button.CommandProperty, "DecrementCommand");
            tab1Layout.Children.Add(buttonDecrement);

            var tab1 = new ContentPage
            {
                Title = "Tab1",
                Content = tab1Layout
            };

            tab1.SetBinding(BadgedTabbedPage.BadgeTextProperty, new Binding("Count"));

            tab1.BindingContext = new Tab1ViewModel();


            var tab2 = new ContentPage
            {
                Title = "Tab2",
                Content = new StackLayout
                {

                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                                new Label {
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    Text = "Welcome to Xamarin Forms Tab2!"
                                }
                            }
                }
            };

            BadgedTabbedPage.SetBadgeText(tab2, "y");

            var tab3 = new ContentPage
            {
                Title = "Tab3",
                Icon = "icon.png",
                Content = new StackLayout
                {

                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                                new Label {
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    Text = "Welcome to Xamarin Forms Tab3!"
                                }
                            }
                }
            };

            BadgedTabbedPage.SetBadgeText(tab3, "z");

            // The root page of your application
            var content = new TabbedPage
            {
                Title = "Tab badge sample",
                Children ={
                    tab1,
                    tab2,
                    tab3
                }
            };

            MainPage = new NavigationPage(content);
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
