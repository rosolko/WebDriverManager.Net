using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("WebDriverManager.Net")]
[assembly: AssemblyDescription("Automatic Selenium WebDriver binaries management for .Net")]
#if DEBUG

[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Alexander Rosolko")]
[assembly: AssemblyProduct("WebDriverManager.Net")]
[assembly: AssemblyCopyright("Copyright © Alexander Rosolko 2016-2017")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("DDF73921-D0CD-4B7F-BAFB-021CEAC5FF73")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion("2.2.4")]
[assembly: AssemblyFileVersion("2.2.4")]