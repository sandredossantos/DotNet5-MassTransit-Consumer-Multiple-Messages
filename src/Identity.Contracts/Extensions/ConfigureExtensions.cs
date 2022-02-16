using Identity.Contracts.Messages;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;

namespace Identity.Contracts.Extensions
{
    public static class ConfigureExtensions
    {
        public static void ConfigureMessageTopology(this IRabbitMqBusFactoryConfigurator config)
        {
            config.Message<AddUser>(x => x.SetEntityName("content.user"));
            config.Message<DeleteUser>(x => x.SetEntityName("content.user"));
            
            config.Publish<AddUser>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
                x.BindQueue("content.user", "add:user:consumer", x =>
                {
                    x.RoutingKey = "add:key";
                    x.ExchangeType = ExchangeType.Direct;
                });
            });

            config.Publish<DeleteUser>(x =>
            {
                x.ExchangeType = ExchangeType.Direct;
                x.BindQueue("content.user", "delete:user:consumer", x =>
                {
                    x.RoutingKey = "delete:key";
                    x.ExchangeType = ExchangeType.Direct;
                });
            });
        }
    }
}