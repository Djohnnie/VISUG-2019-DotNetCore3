using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Newtonsoft.Json;
using static System.Console;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace _09_BuiltInJsonSupport
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => WriteLine(5));

            WriteLine("Preparing data...");

            var data = Builder<DataPoint>.CreateListOfSize(10)
                .All()
                .With(p => p.Id = Guid.NewGuid())
                .With(p => p.Title = Faker.Identification.UKNationalInsuranceNumber())
                .With(p => p.Description = Faker.Lorem.Sentence(100))
                .With(p => p.LeftData = GenerateDataPoint(0))
                .With(p => p.RightData = GenerateDataPoint(0))
                .Build();

            for (int i = 0; i < 10; i++)
            {
                var sw1 = Stopwatch.StartNew();
                string newtonsoftJson = JsonConvert.SerializeObject(data);
                sw1.Stop();
                WriteLine($"Newtonsoft JSON serialization:   {sw1.ElapsedMilliseconds}ms");

                sw1.Restart();
                _ = JsonConvert.DeserializeObject<List<DataPoint>>(newtonsoftJson);
                sw1.Stop();
                WriteLine($"Newtonsoft JSON deserialization: {sw1.ElapsedMilliseconds}ms");

                var sw2 = Stopwatch.StartNew();
                string builtinJson = JsonSerializer.Serialize(data);
                sw2.Stop();
                WriteLine($"Built-in JSON serialization:     {sw2.ElapsedMilliseconds}ms");

                sw2.Restart();
                _ = JsonSerializer.Deserialize<List<DataPoint>>(builtinJson);
                sw2.Stop();
                WriteLine($"Built-in JSON deserialization:   {sw2.ElapsedMilliseconds}ms");

                WriteLine();
            }
        }

        static DataPoint GenerateDataPoint(int level)
        {
            if (level < 10)
            {
                return new DataPoint
                {
                    Id = Guid.NewGuid(),
                    Title = Faker.Identification.UKNationalInsuranceNumber(),
                    Description = Faker.Lorem.Sentence(100),
                    LeftData = GenerateDataPoint(level + 1),
                    RightData = GenerateDataPoint(level + 1)
                };
            }

            return null;
        }
    }
}