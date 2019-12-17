using System;
using System.IO;
using System.Reflection;
using static System.Console;

namespace _02_SingleFileExecutables
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Single File Executable on .NET Core 3.1");
            WriteLine("---------------------------------------");
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