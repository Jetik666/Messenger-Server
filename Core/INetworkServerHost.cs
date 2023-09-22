namespace Core
{
    internal interface INetworkServerHost
    {
        void Start();
        void Close();
        void ShutdownBoth();
        void ShutdownSendOnly();
        void ShutdownReceiveOnly();
    }
}
