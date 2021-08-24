
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using transport_sim_app.Hubs.Clients;

namespace transport_sim_app.Hubs
{
    class SimulationHub : Hub<ISimulationClient>
    {
        public async Task SendStartSimulation(string simId)
        {
            await Clients.All.StartSimulationNotification(simId);
        }
        public ChannelReader<int> Counter(
            CancellationToken cancellationToken
        ){
            var channel = Channel.CreateUnbounded<int>();
            _ = WriteItemAsync(channel.Writer, cancellationToken);
            return channel.Reader;
        }

        async Task WriteItemAsync(
            ChannelWriter<int> writer,
            CancellationToken cancellationToken
        ){
            Exception localException = null;
            try
            {
                var i = 0;
                while (true)
                {
                    await writer.WriteAsync(i++, cancellationToken);
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                localException = ex;
            }
            finally
            {
                writer.Complete(localException);
            }
        }
    }
}