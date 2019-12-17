using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using _12_WindowsDesktop.gRPC.Contract;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace _12_WindowsDesktop.gRPC.Server
{
    public class StreamerService : Streamer.StreamerBase
    {
        private readonly ILogger<StreamerService> _logger;

        private static readonly ConcurrentDictionary<string, IServerStreamWriter<StreamResponse>> _subscribers = new ConcurrentDictionary<string, IServerStreamWriter<StreamResponse>>();

        public StreamerService(ILogger<StreamerService> logger)
        {
            _logger = logger;
        }

        public override async Task Do(
            IAsyncStreamReader<StreamRequest> requestStream,
            IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext())
                return;

            _subscribers.TryAdd(requestStream.Current.ClientId, responseStream);

            do
            {
                if (requestStream.Current == null)
                    continue;

                var response = new StreamResponse
                {
                    X = requestStream.Current.X,
                    Y = requestStream.Current.Y
                };

                foreach (var subscriber in _subscribers)
                {
                    await subscriber.Value.WriteAsync(response);
                }

            } while (await requestStream.MoveNext());
        }
    }
}