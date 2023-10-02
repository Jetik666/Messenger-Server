using System.Net;
using System.Net.Sockets;

namespace Core.ServerInfo
{
    public abstract class DefaultServerInfo
    {
        protected IPAddress _ip;
        protected ushort _port;

        public AddressFamily AddressFamily { get; set; }
        public SocketType SocketType { get; set; }
        public ProtocolType ProtocolType { get; set; }

        public IPEndPoint? EndPoint { get; set; }
        public Socket? Socket { get; set; }

        protected readonly IPAddress _defaultIP;
        protected readonly ushort _defaultPort;

        protected readonly AddressFamily _defaultAddressFamily;
        protected readonly SocketType _defaultSocketType;
        protected readonly ProtocolType _defaultProtocolType;

        protected DefaultServerInfo()
        {
            _defaultIP = IPAddress.Parse("127.0.0.1");
            _defaultPort = 50500;

            _defaultAddressFamily = AddressFamily.InterNetwork;
            _defaultSocketType = SocketType.Stream;
            _defaultProtocolType = ProtocolType.Tcp;

            _ip = _defaultIP;
            _port = _defaultPort;

            AddressFamily = _defaultAddressFamily;
            SocketType = _defaultSocketType;
            ProtocolType = _defaultProtocolType;
        }
    }
}
