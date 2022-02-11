using Identity.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
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
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    config.Publish<AddUser>(p =>
                    {
                        p.ExchangeType = ExchangeType.Direct;
                    });

                    config.Publish<DeleteUser>(p =>
                    {
                        p.ExchangeType = ExchangeType.Direct;
                    });

                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}