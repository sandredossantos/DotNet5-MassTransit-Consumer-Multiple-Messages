using Identity.Contracts.Extensions;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Consumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                config.AddConsumers(entryAssembly);

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureMessageTopology();

                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}