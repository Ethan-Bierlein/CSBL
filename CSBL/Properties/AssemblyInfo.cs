using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("CSBL")]
[assembly: AssemblyDescription("A concatenative, stack-based language written in C#.")]
[assembly: AssemblyCompany("Ethan Bierlein")]
[assembly: AssemblyProduct("CSBL")]
[assembly: AssemblyCopyright("Copyright © 2017")]
[assembly: AssemblyTrademark("CSBL")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("7c6db243-6ab9-4b19-8936-6ab214bdcde0")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif