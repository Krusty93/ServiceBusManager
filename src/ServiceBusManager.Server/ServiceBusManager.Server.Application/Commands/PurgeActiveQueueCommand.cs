using MediatR;

namespace ServiceBusManager.Server.Application.Commands
{
    public class PurgeActiveQueueCommand : IRequest
    {
        public PurgeActiveQueueCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
