namespace Core
{
    internal interface INetworkServerHost
    {
        Task Start();
        Task Close();
        Task ShutdownBoth();
        Task ShutdownSendOnly();
        Task ShutdownReceiveOnly();
    }
}
