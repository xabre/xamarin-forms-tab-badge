# <img src="icon_small.png" width="80" height="80"/> xamarin-forms-tab-badge [![Build Status](https://www.bitrise.io/app/6fbb08a710d8f2aa.svg?token=SlEUDTzwNV54nK7HHhUdOQ&branch=master)](https://www.bitrise.io/app/6fbb08a710d8f2aa)
**Xamarin Forms** bindable tab badges for iOS and Android. The plugin creates a custom renderer (iOS & Android) and a custom attached property for adding tab bar badges which can be bound in XF shared code.

## Sample
<img src="Screencasts/xamarin.forms.android.gif" height="600"/><img src="Screencasts/xamarin.forms.ios.gif" height="600"/>

## Support & Limitations

| Feature / Availability  | Xamarin.Android | Xamarin.iOS |
| ------------- | ----------- | ----------- |
| Bindable Badge Text | ✓* | ✓ (iOS >=8.0) |
| Bindable Badge Color | ✓* | ✓ (iOS >= 10.0) |
| Bindable Badge Text Color | ✓* | ✓ (iOS >= 10.0) |
| Bindable Badge Font | ✓ | ✓ *\*(iOS >= 10.0) |
| Dynamic tab add/removal | ✓* | On overflow, 'More' / '...' menu don't have badges...yet. |

*Caution: For Xamarin.Forms Android make sure to use AppCompat. I.e.inherit from FormsAppCompatActivity

*\*Font restrictions: able to bind font attributes and family but not size

## Installation

- Make sure to install the package in your Xamarin.Forms PCL, iOS and Android project.

```
// stable
Install-Package Plugin.Badge
// or pre-release
Install-Package Plugin.Badge -Pre
```
[![NuGet](https://img.shields.io/nuget/v/Plugin.Badge.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Badge) [![NuGet Beta](https://img.shields.io/nuget/vpre/Plugin.Badge.svg?label=NuGet Beta)](https://www.nuget.org/packages/Plugin.Badge)


### iOS

You have to register the custom renderer. I usually put this in the AssemblyInfo.cs file of my iOS csproj.

```
[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
```

### Android

You have to register the custom renderer. I usually put this in the AssemblyInfo.cs file of my Android csproj.

```
[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
```

### Caution - Custom TabbedPage / Custom Renderers

If you are using a custom renderer for TabbedPage please change it to inherit from `BadgedTabbedPageRenderer` and you are all set. Of course dont forget to register your own renderer.

### Linker

As you already know when you enable linkling for your application the linker might remove some of the methods/types/properties/events used by third party libraries. This is why it's necessary to update your `LinkerPleaseInclude.cs`. Check this [blogpost](https://lostechies.com/jimmybogard/2014/11/11/dealing-with-the-linker-in-xamarin-apps/) for more linker info.

- For iOS add the following lines:
```
public void Include(UITabBarItem item)
{
    item.BadgeColor = UIColor.Red;
    item.BadgeValue = "badge";
}
```
- For Android no issues detected yet :). Feel free to report any issues here.

## Usage

### Badge Text

For each **child page** of the TabbedPage just bind the custom attached property 
[`TabBadge.BadgeText` (XAML) or `TabBadge.BadgeTextPropery` (CSharp)]

#### XAML

The important line here is binding the attached property:
```
  plugin:TabBadge.BadgeText="{Binding Count}"
```

Example tabbed page with badged children:
```
<TabbedPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:plugin="clr-namespace:Plugin.Badge.Abstractions;assembly=Plugin.Badge.Abstractions" 
             x:Class="Plugin.Badge.Sample.TabXaml">
  <TabbedPage.Children>
    <ContentPage Title="Tab1" 
                 Icon="icontab1.png"
                 plugin:TabBadge.BadgeText="{Binding Count}">
      <StackLayout>...</StackLayout>
    </ContentPage>
    ...
  </TabbedPage.Children>
</TabbedPage>
```
#### CSharp codebehind

```
var tab1 = new ContentPage //assume this is a child page of your Tab
{
    Title = "Tab1",
    Content = tab1Layout
};

tab1.SetBinding(TabBadge.BadgeTextProperty, new Binding("Count"));
```

### Badge Color

Very similar to `BadgeText` just use [`TabBadge.BadgeColor` (XAML) or `TabBadge.BadgeColorPropery` (CSharp)].

<img src="Screencasts/xamarin.forms.tabbadge.colors.png" height="100"/>

### Badge Text Color

Very similar to `BadgeColor` just use [`TabBadge.BadgeTextColor` (XAML) or `TabBadge.BadgeTextColorPropery` (CSharp)].

<img src="Screencasts/xamarin.forms.tabbadge.textcolors.PNG" height="100"/>

### Badge Font Attributes

Very similar to `BadgeColor` just use [`TabBadge.BadgeFont` (XAML) or `TabBadge.BadgeFontPropery` (CSharp)].

<img src="Screencasts/xamarin.forms.tabbadge.fontattributes.PNG" height="100"/>

ToDo:
- you can alo try to set font family, should wrok in theory
- font size is not supported yet on android ......

## Showning / Hiding the badge
If the value of the `BadgeText` is set to null or empty string the badge is hidden. To show it again set a non null or empty value


# Licence

[MIT](https://github.com/xabre/xamarin-forms-tab-badge/blob/master/LICENSE)
