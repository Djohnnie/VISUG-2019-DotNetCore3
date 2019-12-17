using System;
using System.Threading;
using System.Threading.Tasks;
using _10_gRPC.Proto;
using Grpc.Core;
using Grpc.Net.Client;
using static System.Console;

namespace _10_gRPC.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(5000);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Sauna.SaunaClient(channel);
            var request = new SaunaRequest { TemperatureUnit = "C" };
            var response = await client.FetchCurrentStateAsync(request);

            var timeStamp = DateTimeOffset.FromUnixTimeSeconds(response.TimeStamp);

            WriteLine($"[{timeStamp:T}] <{(response.IsInfraRed ? "I" : " ")}> <{(response.IsDrySauna ? "S" : " ")}> {response.Temperature}°C");



            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var streamingCall = client.FetchStateStream(request, cancellationToken: cts.Token);

            try
            {
                await foreach (var saunaMessage in streamingCall.ResponseStream.ReadAllAsync(cts.Token))
                {
                    timeStamp = DateTimeOffset.FromUnixTimeSeconds(saunaMessage.TimeStamp);
                    WriteLine(
                        $"[{timeStamp:T}] <{(saunaMessage.IsInfraRed ? "I" : " ")}> <{(saunaMessage.IsDrySauna ? "S" : " ")}> {saunaMessage.Temperature}°C");
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                WriteLine("Stream cancelled.");
            }
        }
    }
}