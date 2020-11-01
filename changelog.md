# Changelog
## [2.3.1]
      - #98: Update Plugin.Badge.UWP.xr.xml location in nuspec
## [2.3.0]
      - #87: Updated to XF 4.5 and AndroidX. Target Android 10
### [2.3.0-pre.1/pre.2]
      - #87: Updated to XF 4.5 and AndroidX. Target Android 10
## [2.2.1]
      - #74: Fix NRE in UpdateBadgeProperties
## [2.2.0] 
      - #67: Update to XF 4.0.0.425677
      - #72: Fix Android Warnings
      - #64: Generic XF badge view that can be added to any XF layout
      - #30: Generic XF badge used with NavigationPage.TitleView
### [2.2.0-pre.1]
      - #67: Update to XF 4.0-pre. This is a pre release build design to workaround the issue described in #67. If your code is nota ffected by the issue please use the latest stable release instead.
## [2.1.2]
      - #65 iOS: Update badge properties on icon property changed
      - #66 Enusure cleanup also takes into account tabs wrapped by navigation pages
      - Android: Update target sdk to 28
## [2.1.1]
      - #62 Merged fix from PR #63
      - #61 UWP Nuget pacakge - Explicitly include .xr.xml and .xbf files
## [2.1.0]
      - #45 WPF Renderer and sample app
      - #58 Tabs as navigation pages. Consider Badge attached properties for wrapping navigation pages
### [2.1.0-pre.3]
      - #55 #48 Fix bottom layout and placement.
### [2.1.0-pre.2]
      - #49 Fix missing UWP dlls from nuget package
### [2.1.0-pre.1]
      - Update to Xamarin Forms 3.1 
      - #48 Android Bottom Placement Support
      - #46 Bugfix: consider tabpage children embedded in navigation pages
## [2.0.0]
      - #40 Added support for NON-AppCompat android activity (actionbar)
      - .NET Standard support with backwards compatibility to PCL
## [2.0.0-pre]
      - .NET Standard 1.4 support
## [1.3.0]
      - #33 Updated dependencies to Xamarin Forms 2.4.0.280 (first stable 2.4.x)
      - #37 #20 [Android][MacOS][UWP] Bindable badge postion and margin
### [1.3.0-beta]
      - #31 MacOS support
## [1.2.1]
      - #20 Support for more badge postions: TopCenter, BottomCenter, LeftCenter, RightCenter
## [1.2.0]
      - #8 #20 #36 Bindable badge position for Android and UWP
### [1.2.0-beta3]
      - UAP ensure missing *.xr.xml is also copied.
      - UAP XF dependency updated
      - #26 UWP support
## [1.1.3]
      - #25 enusure registered event handlers are cleaned up before new ones are registered
## [1.1.2]
      - #23 Use ViewCompat.SetBackground to prevent missing method crash on Android API 15
## [1.1.1]
      - #22 #24 Responds to selector check for iOS text attributes. Prevents crash for versions earlier than iOS 10.
## [1.1.0]
      - #19 Badge Text Color
      - #20 Badge font and font attributes
      - #15 final fix for missing method in android
## [1.0.2]
      - #15 fixed support for android support libs 25.3.1
      - tested sample app with latest XF and android support version
## [1.0.1]
      - #7 Responds to selector check for iOS badge color. Prevents crash for versions earlier than iOS 10.
## [1.0.0]
      - first stable release
