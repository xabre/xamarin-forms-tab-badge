using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;
using Plugin.Badge.Droid;
using Xamarin.Forms;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Plugin.Badge.Sample.Droid.Legacy")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Plugin.Badge.Sample.Droid.Legacy")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("2.1.0")]
[assembly: AssemblyVersion("2.1.0")]
[assembly: AssemblyFileVersion("2.1.0")]

//register renderer
[assembly: ExportRenderer(typeof(TabbedPage), typeof(LegacyBadgedTabbedRenderer))]
