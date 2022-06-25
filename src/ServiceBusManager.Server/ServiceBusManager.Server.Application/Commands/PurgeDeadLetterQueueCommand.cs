using MediatR;

namespace ServiceBusManager.Server.Application.Commands
{
    public class PurgeDeadLetterQueueCommand : IRequest
    {
        public PurgeDeadLetterQueueCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
