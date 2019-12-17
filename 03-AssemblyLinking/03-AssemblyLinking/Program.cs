using System;
using System.IO;
using System.Reflection;
using static System.Console;

namespace _03_AssemblyLinking
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Default Executables on .NET Core 2.2");
            WriteLine("------------------------------------");
            WriteLine();

            string path = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);

            WriteLine(directory);
            WriteLine();

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