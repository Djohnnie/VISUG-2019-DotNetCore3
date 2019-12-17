using static System.Console;

namespace _07_NullableReferenceTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            string text1 = null;
            string? text2 = null;

#nullable disable
            string text3 = null;
#nullable enable

            Test(text1);
            Test(text2);
            Test(text3);

            WriteLine(text1);
            WriteLine(text2);
            WriteLine(text3);
        }


        static void Test(string x)
        {
            _ = x.Length;
        }
    }
}