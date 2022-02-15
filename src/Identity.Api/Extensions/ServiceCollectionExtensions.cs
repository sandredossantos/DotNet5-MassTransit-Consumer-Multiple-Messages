using Identity.Contracts.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.ConfigureMessageTopology();

                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}