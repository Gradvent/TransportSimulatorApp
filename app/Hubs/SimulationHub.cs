
using System;
using System.Linq;
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
            // _repository.ScopeUpdateEvent += SimulationScopeUpdated; 
        }

        async void SimulationScopeUpdated(object sender, SimulationEventArgs e)
        {
            await Clients.All.ScopeUpdatedNotification(new ScopeUpdatedNotificationArgs
            {
                Transports = e.Transports.ToArray<ITransport>(),
                TrackDistance = e.TrackDistance,
                AllFinished = e.Transports.All((t)=>t.Finished)
            });
        }

        // public async Task SendStartSimulation(string simId)
        // {
        //     await Clients.All.StartSimulationNotification(simId);
        // }

        public ChannelReader<SimulationEventArgs<object>> SimulationUpdate(
            CancellationToken cancellationToken
        ){
            var channel = Channel.CreateUnbounded<SimulationEventArgs<object>>();
            _ = UpdateAsync(channel.Writer, cancellationToken);
            return channel.Reader;
        }

        async Task UpdateAsync(
            ChannelWriter<SimulationEventArgs<object>> writer,
            CancellationToken cancellationToken
        ){
            Exception localException = null;
            try
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested) break;
                    var sa = _repository.SimulationArgs;
                    await writer.WriteAsync(
                        new SimulationEventArgs<object>{
                            Message = sa.Message,
                            Status = sa.Status,
                            TrackDistance = sa.TrackDistance,
                            Transports = sa.Transports.ToArray<object>()
                        }, 
                        cancellationToken);
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