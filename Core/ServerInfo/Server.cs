using System.Net;
using System.Net.Sockets;

namespace Core.ServerInfo
{
    public class Server : DefaultServerInfo, IValidate
    {
        private IPAddress _ip;
        private ushort _port;

        public AddressFamily AddressFamily { get; set; }
        public SocketType SocketType { get; set; }
        public ProtocolType ProtocolType { get; set; }

        public IPEndPoint? EndPoint { get; set; }
        public Socket? Socket { get; set; }

        public Server()
        {
            _ip = _defaultIP;
            _port = _defaultPort;

            AddressFamily = _defaultAddressFamily;
            SocketType = _defaultSocketType;
            ProtocolType = _defaultProtocolType;
        }

        public IPAddress IP
        {
            get { return _ip; }
            set
            {
                try
                {
                    _ip = value;
                }
                catch
                {
                    _ip = _defaultIP;
                    throw new ArgumentException($"Value is invalid. IP was changed to: {_defaultIP}.", nameof(value));
                }
            }
        }
        public string IPString
        {
            get { return _ip.ToString(); }
            set { ValidateIPv4(value); }
        }
        public ushort Port
        {
            get { return _port; }
            set { ValidatePort(value.ToString()); }
        }

        public void ValidateIPv4(string newIP)
        {
            if (string.IsNullOrWhiteSpace(newIP))
            {
                _ip = _defaultIP;
                throw new ArgumentNullException(nameof(newIP),
                    $"The value is null. IP was changed to: {_defaultIP}.");
            }

            string[] split = newIP.Split('.');
            if (split.Length != 4)
            {
                _ip = _defaultIP;
                throw new ArgumentException($"The value is invalid. IP was changed to: {_defaultIP}.",
                    nameof(newIP));
            }

            if (!split.All(value => byte.TryParse(value, out byte tempForParsing)))
            {
                _ip = _defaultIP;
                throw new ArgumentOutOfRangeException(nameof(newIP),
                    $"The provided IP is not a valid IPv4 address. IP was changed to: {_defaultIP}.");
            }

            _ip = IPAddress.Parse(newIP);
        }
        public void ValidatePort(string newPort)
        {
            if (string.IsNullOrWhiteSpace(newPort))
            {
                _port = _defaultPort;
                throw new ArgumentNullException(nameof(newPort), $"The provided port is null.\nPort was set to default: {_defaultPort}.");
            }

            try
            {
                _port = Convert.ToUInt16(newPort);
            }
            catch (ArgumentException)
            {
                _port = _defaultPort;
                throw new ArgumentOutOfRangeException(nameof(newPort),
                    "The provided port value is not valid.");
            }
        }
        public void ValidateAddressFamily(string newAddressFamily)
        {
            try
            {
                AddressFamily = (AddressFamily)Enum.Parse(typeof(AddressFamily), newAddressFamily, true);
            }
            catch (ArgumentException)
            {
                AddressFamily = _defaultAddressFamily;
                throw new ArgumentException($"Unknown address family: {newAddressFamily}.\n" +
                    $"Address family was set to default: {_defaultAddressFamily}.", 
                    nameof(newAddressFamily));
            }
        }
        public void ValidateSocketType(string newSocketType)
        {
            try
            {
                SocketType = (SocketType)Enum.Parse(typeof(SocketType), newSocketType, true);
            }
            catch (ArgumentException)
            {
                SocketType = _defaultSocketType;
                throw new ArgumentException($"Unknown socket type: {newSocketType}.\n" +
                    $"Socket type was set to default: {_defaultAddressFamily}.",
                    nameof(newSocketType));
            }
        }
        public void ValidateProtocolType(string newProtocolType)
        {
            try
            {
                ProtocolType = (ProtocolType)Enum.Parse(typeof(ProtocolType), newProtocolType, true);
            }
            catch (ArgumentException)
            {
                ProtocolType = _defaultProtocolType;
                throw new ArgumentException($"Unknown protocol type: {newProtocolType}.\n" +
                    $"Protocol type was set to default: {_defaultProtocolType}.",
                    nameof(newProtocolType));
            }
        }

        public void UpdateEndPoint()
        {
            try
            {
                EndPoint = new(IP, Port);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("IP or port is null.");
            }
        }
        public void UpdateSocket()
        {
            if (EndPoint != null)
            {
                TurnOffSocket();
                Socket = new(AddressFamily, SocketType, ProtocolType);
                Socket.Bind(EndPoint);
            }
            else
            {
                try
                {
                    UpdateEndPoint();
                    UpdateSocket();
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException("EndPoint is null.", nameof(EndPoint));
                }
            }
        }

        private void TurnOffSocket()
        {
            if (Socket != null)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket.Dispose();
            }
        }
    }
}
