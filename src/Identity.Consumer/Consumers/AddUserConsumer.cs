using Identity.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Identity.Consumer.Consumers
{
    public class AddUserConsumer : IConsumer<AddUser>
    {
        private readonly ILogger<AddUserConsumer> _logger;

        public AddUserConsumer(ILogger<AddUserConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<AddUser> context)
        {
            _logger.LogInformation($"Mensagem recebida: " + $"{context.Message.Id}");

            return Task.CompletedTask;
        }
    }
}