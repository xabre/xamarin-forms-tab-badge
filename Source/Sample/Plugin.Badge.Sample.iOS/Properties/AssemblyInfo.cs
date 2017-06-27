using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Plugin.Badge.iOS;
using Xamarin.Forms;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Plugin.Badge.Sample.iOS")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Plugin.Badge.Sample.iOS")]
[assembly: AssemblyCopyright("Copyright © 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e29aa3f5-2f4d-4b8a-85a6-b6ca7c557278")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.2")]
[assembly: AssemblyFileVersion("1.0.2")]

//register renderer
[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
