using Identity.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
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

        public Task Consume(ConsumeContext<DeleteUser> context)
        {
            _logger.LogInformation($"Mensagem recebida: " + $"{context.Message.Id}");

            return Task.CompletedTask;
        }
    }
}