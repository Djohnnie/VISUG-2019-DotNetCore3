using System;
using static System.Console;

namespace _05_RangesAndIndices
{
    class Program
    {
        static void Main(string[] args)
        {
            // System.Index
            // Represents a type that can be used to index a collection either from the start or the end.

            // System.Range
            // Represents a range that has start and end indexes.

            // Creates an index at position 1 from the start.
            var index1 = new Index(1);
            // Creates an index at position 1 from the end.
            var index2 = new Index(1, fromEnd: true);
            // Creates a range from position 1 (inclusive) to position 5 (exclusive).
            var range = new Range(1, 5);

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            WriteLine(string.Join(",", numbers));
            WriteLine(string.Join(",", numbers[index1]));
            WriteLine(string.Join(",", numbers[index2]));
            WriteLine(string.Join(",", numbers[range]));
            WriteLine();
            WriteLine(string.Join(",", numbers));
            WriteLine(string.Join(",", numbers[1]));
            WriteLine(string.Join(",", numbers[^1]));
            WriteLine(string.Join(",", numbers[1..5]));
            WriteLine(string.Join(",", numbers[3..]));
            WriteLine(string.Join(",", numbers[..5]));
            WriteLine(string.Join(",", numbers[1..^1]));
        }
    }
}