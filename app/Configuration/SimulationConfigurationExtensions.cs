
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using transport_sim_app.Configuration;
using transport_sim_app.Hubs;
using transport_sim_app.Models.Factories;
using transport_sim_app.Models.Transports;
using transport_sim_app.Models.Simulation;
using transport_sim_app.Models.Repository;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimulationConfigurationExtensions
    {
        public static IServiceCollection AddTransportSimulation(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<TransportOptions>(config.GetSection(TransportOptions.Transport));
            services.Configure<SimulationOptions>(config.GetSection(SimulationOptions.Simulation));
            services.Configure<TransportFactoryOptions>(config.GetSection(TransportFactoryOptions.Factory));
            
            services.AddSingleton<ITransportCollection, TransportCollection>(provider => {
                var factoryOptions = provider.GetRequiredService<IOptions<TransportFactoryOptions>>().Value;
                var transportOptions = provider.GetRequiredService<IOptions<TransportOptions>>();
                var transports = new TransportCollection();
                if (factoryOptions.RandTransport) 
                {
                    var factory = new RandomTransportFactory(transportOptions, factoryOptions);
                    factory.CreateAndAddRange(transports, factoryOptions.Counts);
                }
                return transports;
            });
            services.AddSingleton<TransportCollection>(provider =>
                provider.GetRequiredService<ITransportCollection>()
                    as TransportCollection);

            services.AddScoped<ITransportRepository, TransportRepository>();
            services.AddScoped<ISimulationRepository, SimulationRepository>();
            
            services.AddSingleton<ISimulation, Simulator>();
            // services.AddHostedService<SimulationWorker>();
            return services;
        }

        public static IApplicationBuilder UseTransportSimulation(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapHub<SimulationHub>("/hubs/sim");
            });
            return app;
        }
    }
}