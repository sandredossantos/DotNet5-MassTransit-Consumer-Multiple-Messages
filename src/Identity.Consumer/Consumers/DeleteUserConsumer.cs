using Identity.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Identity.Consumer.Consumers
{
    public class DeleteUserConsumer : IConsumer<DeleteUser>
    {
        private readonly ILogger<DeleteUserConsumer> _logger;

        public DeleteUserConsumer(ILogger<DeleteUserConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DeleteUser> context)
        {
            await Console.Out.WriteLineAsync(context.Message.Id.ToString());

            _logger.LogInformation($"Mensagem recebida: " + $"{context.Message.Id}");
        }
    }
}