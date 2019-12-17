using _01_DefaultExecutables.Common;

namespace _01_DefaultExecutables.DotNetCoreTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            // .NET Core 1.0 up to 2.2 will only build and publish
            // *.dll executables that need to be executed using the
            // dotnet command.

            ConsoleHelper.Go();
        }
    }
}