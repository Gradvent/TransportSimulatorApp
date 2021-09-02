
using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using transport_sim_app.Hubs.Clients;
using transport_sim_app.Models.Repository;
using transport_sim_app.Models.Simulation;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Hubs
{
    class SimulationHub : Hub<ISimulationClient>
    {
        readonly ISimulationRepository _repository;
        public SimulationHub(ISimulationRepository repository)
        {
            _repository = repository;
        }

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
                    var sa = _repository.Simulation.SimulationEventArgs;
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
    }
}