using Identity.Consumer.Consumers;
using Identity.Contracts.Messages;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;

namespace Identity.Consumer.ConsumersDefinition
{
    public class AddUserConsumerDefinition : ConsumerDefinition<AddUserConsumer>
    {
        public AddUserConsumerDefinition()
        {
            EndpointName = $"add:user:consumer";
        }

        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<AddUserConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;

            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                rmq.ExchangeType = ExchangeType.Direct;
                rmq.PublishFaults = false;
                rmq.PrefetchCount = 10;
                rmq.BindQueue = false;

                rmq.ConfigureMessageTopology<AddUser>(false);
                rmq.DiscardFaultedMessages();
                rmq.DiscardSkippedMessages();
            }
        }
    }
}