
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using transport_sim_app.Hubs.Clients;
using transport_sim_app.Models;

namespace transport_sim_app.Hubs
{
    class SimulationHub : Hub<ISimulationClient>
    {
        readonly ISimulationRepository _repository;
        public SimulationHub(ISimulationRepository repository)
        {
            _repository = repository;
        }

        // public async Task SendStartSimulation(string simId)
        // {
        //     await Clients.All.StartSimulationNotification(simId);
        // }

        public ChannelReader<SimulationEventArgs> SimulationUpdate(
            CancellationToken cancellationToken
        ){
            var channel = Channel.CreateUnbounded<SimulationEventArgs>();
            EventHandler<SimulationEventArgs> updateHandler = async (sender, e) => {
                try
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    await channel.Writer.WriteAsync(e, cancellationToken);
                }
                catch (System.Exception ex)
                {
                    channel.Writer.Complete(ex);
                }
            };
            _repository.UpdateEvent += updateHandler;
            _repository.StopEvent += async (sender, e) => {
                _repository.UpdateEvent -= updateHandler;
                try {
                    await channel.Writer.WriteAsync(e, cancellationToken);
                    channel.Writer.Complete();
                }
                catch(Exception ex) {
                    channel.Writer.Complete(ex);
                }
                
            };
            return channel.Reader;
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