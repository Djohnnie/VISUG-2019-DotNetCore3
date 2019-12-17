using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static System.Console;

namespace _08_HardwareIntrinsics
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Getting a billion integers...");
            var source = new ReadOnlySpan<int>(Enumerable.Range(0, 1073741824).ToArray());

            for (int i = 0; i < 3; i++)
            {
                Write("Calculating a regular sum... ");
                var sw1 = Stopwatch.StartNew();
                _ = Sum(source);
                sw1.Stop();
                WriteLine($"{sw1.ElapsedMilliseconds}ms");

                Write("Calculating a sum with SIMD support... ");
                var sw2 = Stopwatch.StartNew();
                _ = SumVectorT(source);
                sw2.Stop();
                WriteLine($"{sw2.ElapsedMilliseconds}ms");

                Write("Calculating a sum with Hardware Intrinsics support... ");
                var sw3 = Stopwatch.StartNew();
                _ = SumVectorized(source);
                sw3.Stop();
                WriteLine($"{sw3.ElapsedMilliseconds}ms");
            }
        }

        static int Sum(ReadOnlySpan<int> source)
        {
            int result = 0;

            foreach (var number in source)
            {
                result += number;
            }

            return result;
        }

        static int SumVectorT(ReadOnlySpan<int> source)
        {
            int result = 0;

            Vector<int> vResult = Vector<int>.Zero;

            int i = 0;
            int lastBlockIndex = source.Length - source.Length % Vector<int>.Count;

            while (i < lastBlockIndex)
            {
                vResult += new Vector<int>(source.Slice(i));
                i += Vector<int>.Count;
            }

            for (int n = 0; n < Vector<int>.Count; n++)
            {
                result += vResult[n];
            }

            while (i < source.Length)
            {
                result += source[i];
                i += 1;
            }

            return result;
        }

        static int SumVectorized(ReadOnlySpan<int> source)
        {
            return Sse2.IsSupported ? SumVectorizedSse2(source) : SumVectorT(source);
        }

        static unsafe int SumVectorizedSse2(ReadOnlySpan<int> source)
        {
            int result;

            fixed (int* pSource = source)
            {
                Vector128<int> vResult = Vector128<int>.Zero;

                int i = 0;
                int lastBlockIndex = source.Length - source.Length % 4;

                while (i < lastBlockIndex)
                {
                    vResult = Sse2.Add(vResult, Sse2.LoadVector128(pSource + i));
                    i += 4;
                }

                if (Ssse3.IsSupported)
                {
                    vResult = Ssse3.HorizontalAdd(vResult, vResult);
                    vResult = Ssse3.HorizontalAdd(vResult, vResult);
                }
                else
                {
                    vResult = Sse2.Add(vResult, Sse2.Shuffle(vResult, 0x4E));
                    vResult = Sse2.Add(vResult, Sse2.Shuffle(vResult, 0xB1));
                }
                result = vResult.ToScalar();

                while (i < source.Length)
                {
                    result += pSource[i];
                    i += 1;
                }
            }

            return result;
        }
    }
}