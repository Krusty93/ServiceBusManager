using MediatR;

namespace ServiceBusManager.Server.Application.Commands
{
    public class DeleteQueueCommand : IRequest
    {
        public DeleteQueueCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
