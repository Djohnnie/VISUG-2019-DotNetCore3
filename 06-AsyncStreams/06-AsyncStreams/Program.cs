using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace _06_AsyncStreams
{
    class Program
    {
        static async Task Main(string[] args)
        {
            foreach (var dataPoint in FetchData())
            {
                WriteLine(dataPoint);
            }

            await foreach (var dataPoint in FetchDataAsync())
            {
                WriteLine(dataPoint);
            }

            ReadLine();
        }

        private static IEnumerable<int> FetchData()
        {
            for (int i = 1; i <= 10; i++)
            {
                Thread.Sleep(100);
                yield return i;
            }
        }

        private static async IAsyncEnumerable<int> FetchDataAsync()
        {
            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }
    }
}