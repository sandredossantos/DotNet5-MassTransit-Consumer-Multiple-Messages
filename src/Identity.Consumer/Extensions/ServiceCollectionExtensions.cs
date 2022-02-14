using GreenPipes;
using Identity.Consumer.Consumers;
using Identity.Contracts.Messages;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace Identity.Consumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<AddUserConsumer>();
                config.AddConsumer<DeleteUserConsumer>();

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("queue:add:user", e =>
                    {
                        e.ConfigureConsumer<AddUserConsumer>(context, c =>
                        {
                            c.UseMessageRetry(retry =>
                            {
                                retry.Interval(1, TimeSpan.FromSeconds(10));
                            });

                            c.UseConcurrentMessageLimit(10);
                        });

                        e.ExchangeType = ExchangeType.Direct;
                        e.ConfigureConsumeTopology = false;
                        e.PublishFaults = false;
                        e.PrefetchCount = 10;
                        e.Lazy = true;
                        
                        e.Bind<AddUser>(b =>
                        {
                            b.ExchangeType = ExchangeType.Direct;
                        });

                        e.DiscardFaultedMessages();
                        e.DiscardSkippedMessages();
                    });

                    cfg.ReceiveEndpoint("queue:delete:user", e =>
                    {
                        e.ConfigureConsumer<DeleteUserConsumer>(context, c =>
                        {
                            c.UseMessageRetry(retry =>
                            {
                                retry.Interval(1, TimeSpan.FromSeconds(10));
                            });

                            c.UseConcurrentMessageLimit(10);
                        });

                        e.ExchangeType = ExchangeType.Direct;
                        e.ConfigureConsumeTopology = false;
                        e.PublishFaults = false;
                        e.PrefetchCount = 10;
                        e.Lazy = true;

                        e.Bind<DeleteUser>(b =>
                        {
                            b.ExchangeType = ExchangeType.Direct;
                        });

                        e.DiscardFaultedMessages();
                        e.DiscardSkippedMessages();
                    });

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