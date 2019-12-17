using System;
using System.IO;
using System.Reflection;
using static System.Console;

namespace _01_DefaultExecutables.Common
{
    public static class ConsoleHelper
    {
        public static void Go()
        {
            WriteLine("Default Executables on .NET Core 2.2");
            WriteLine("------------------------------------");
            WriteLine();

            string path = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);

            foreach (FileInfo fi in directoryInfo.GetFiles())
            {
                ForegroundColor = fi.Extension == ".exe" || fi.Extension == ".dll"
                    ? ConsoleColor.Yellow
                    : ConsoleColor.White;
                WriteLine(fi.Name);
            }

            ReadKey();
        }
    }
}