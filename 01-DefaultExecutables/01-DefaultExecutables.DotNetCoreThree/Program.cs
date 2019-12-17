using _01_DefaultExecutables.Common;

namespace _01_DefaultExecutables.DotNetCoreThree
{
    class Program
    {
        static void Main(string[] args)
        {
            // .NET Core 3.0 and 3.1 will build and publish
            // both *.dll and *.exe for executable project
            // templates. Class libraries and web applications
            // will only produce *.dll executables.

            ConsoleHelper.Go("Default Executables on .NET Core 3.1");
        }
    }
}