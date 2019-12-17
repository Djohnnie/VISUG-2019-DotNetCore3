using System.Diagnostics.CodeAnalysis;
using static System.Console;

namespace _07_NullableReferenceTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            string text1 = null;
            string? text2 = null;

            Test(text1);
            Test(text2);

            WriteLine(text1);
            WriteLine(text2);
        }


        static void Test([NotNull]string x)
        {
            _ = x.Length;
        }
    }
}