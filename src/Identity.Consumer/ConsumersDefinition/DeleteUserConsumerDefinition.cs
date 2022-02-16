using Identity.Consumer.Consumers;
using Identity.Contracts.Messages;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;

namespace IIdentity.Consumer.ConsumersDefinition
{
    public class DeleteUserConsumerDefinition : ConsumerDefinition<DeleteUserConsumer>
    {
        public DeleteUserConsumerDefinition()
        {
            EndpointName = $"delete:user:consumer";
        }

        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<DeleteUserConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;


            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                rmq.ExchangeType = ExchangeType.Direct;
                rmq.PublishFaults = false;
                rmq.PrefetchCount = 10;
                rmq.BindQueue = false;

                rmq.ConfigureMessageTopology<DeleteUser>(false);
                rmq.DiscardFaultedMessages();
                rmq.DiscardSkippedMessages();
            }
        }
    }
}