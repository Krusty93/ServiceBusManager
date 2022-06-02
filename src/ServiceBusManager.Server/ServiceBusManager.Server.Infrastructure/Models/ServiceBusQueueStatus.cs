namespace ServiceBusManager.Server.Infrastructure
{
    public enum ServiceBusQueueStatus
    {
        InvalidState = 0,

        Active,
        Disabled,
        SendDisabled,
        ReceiveDisabled,
    }
}
