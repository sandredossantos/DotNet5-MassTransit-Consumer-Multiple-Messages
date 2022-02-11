using Identity.Consumer.Consumers;
using Identity.Contracts.Messages;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Identity.Consumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("queue:add:user", e =>
                    {
                        e.Consumer<AddUserConsumer>(context);

                        e.ExchangeType = ExchangeType.Direct;
                        e.ConfigureConsumeTopology = false;
                        e.PublishFaults = false;
                        e.PrefetchCount = 10;
                        e.Lazy = true;
                        
                        e.Bind<AddUser>(b =>
                        {
                            b.ExchangeType = ExchangeType.Direct;
                            b.RoutingKey = "A";
                        });

                        e.DiscardFaultedMessages();
                        e.DiscardSkippedMessages();
                    });

                    cfg.ReceiveEndpoint("queue:delete:user", e =>
                    {
                        e.Consumer<DeleteUserConsumer>(context);

                        e.ExchangeType = ExchangeType.Direct;
                        e.ConfigureConsumeTopology = false;
                        e.PublishFaults = false;
                        e.PrefetchCount = 10;
                        e.Lazy = true;

                        e.Bind<DeleteUser>(b =>
                        {
                            b.ExchangeType = ExchangeType.Direct;
                            b.RoutingKey = "B";
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