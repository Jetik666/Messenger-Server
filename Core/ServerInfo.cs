using System.Net;
using System.Net.Sockets;

namespace Core
{
    /* AddressFamily
    InterNetwork: адрес по протоколу IPv4
    InterNetworkV6: адрес по протоколу IPv6
    Ipx: адрес IPX или SPX
    NetBios: адрес NetBios
    */

    /*SocketType
    Dgram: сокет будет получать и отправлять дейтаграммы по протоколу Udp.
           Данный тип сокета работает в связке с типом протокола - Udp и значением AddressFamily.InterNetwork
    Raw: сокет имеет доступ к нижележащему протоколу транспортного уровня и может использовать для передачи сообщений такие протоколы, как ICMP и IGMP
    Rdm: сокет может взаимодействовать с удаленными хостами без установки постоянного подключения.
         В случае, если отправленные сокетом сообщения невозможно доставить, то сокет получит об этом уведомление
    Seqpacket: обеспечивает надежную двустороннюю передачу данных с установкой постоянного подключения
    Stream: обеспечивает надежную двустороннюю передачу данных с установкой постоянного подключения.Для связи используется протокол TCP,
            поэтому этот тип сокета используется в паре с типом протокола Tcp и значением AddressFamily.InterNetwork
    Unknown: адрес NetBios
    */

    /* ProtocolType
    Ggp
    Icmp
    IcmpV6
    Idp
    Igmp
    IP
    IPSecAuthenticationHeader (Заголовок IPv6 AH)
    IPSecEncapsulatingSecurityPayload (Заголовок IPv6 ESP)
    IPv4
    IPv6
    IPv6DestinationOptions (Заголовок IPv6 Destination Options)
    IPv6FragmentHeader (Заголовок IPv6 Fragment)
    IPv6HopByHopOptions (Заголовок IPv6 Hop by Hop Options)
    IPv6NoNextHeader (Заголовок IPv6 No next)
    IPv6RoutingHeader (Заголовок IPv6 Routing)
    Ipx
    ND
    Pup
    Raw
    Spx
    SpxII
    Tcp
    Udp
    Unknown (неизвестный протокол)
    Unspecified (неуказанный протокол)
    */

    public class ServerInfo : IDisposable
    {
        private IPAddress _ip; 
        private ushort _port;

        public AddressFamily AddressFamily { get; set; }
        public SocketType SocketType { get; set; }
        public ProtocolType ProtocolType { get; set; }

        public IPEndPoint EndPoint { get; set; }
        public Socket Socket { get; set; }

        private readonly IPAddress _additionalIP = IPAddress.Parse("127.0.0.1");

        public ServerInfo()
        {
            _ip = _additionalIP;
            _port = 50500;

            AddressFamily = AddressFamily.InterNetwork;
            SocketType = SocketType.Stream;
            ProtocolType = ProtocolType.Tcp;

            EndPoint = new IPEndPoint(_ip, _port);
            Socket = new Socket(AddressFamily, SocketType, ProtocolType);

            Socket.Bind(EndPoint);
        }
        public ServerInfo(string ip, ushort port, 
            string addressFamily, string socketType, string protocolType) 
        {
            try
            {
                ValidateIPv4(ip);
                _ip = IPAddress.Parse(ip);
                Port = port;

                SetAddressFamilyFromString(addressFamily);
                SetSocketTypeFromString(socketType);
                SetProtocolTypeFromString(protocolType);
            }
            catch
            {
                _ip = _additionalIP;
                Port = 50500;

                AddressFamily = AddressFamily.InterNetwork;
                SocketType = SocketType.Stream;
                ProtocolType = ProtocolType.Tcp;
            }
            finally
            {
                if (_ip != null)
                {
                    EndPoint = new(_ip, Port);
                }
                else
                {
                    EndPoint = new(_additionalIP, Port);
                }
                Socket = new(AddressFamily, SocketType, ProtocolType);

                Socket.Bind(EndPoint);
            }
        }
        public ServerInfo(string ip, ushort port, 
            AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            try
            {
                ValidateIPv4(ip);
                _ip = IPAddress.Parse(ip);
                Port = port;

                AddressFamily = addressFamily;
                SocketType = socketType;
                ProtocolType = protocolType;
            }
            catch
            {
                _ip = _additionalIP;
                Port = port;

                AddressFamily = addressFamily;
                SocketType = socketType;
                ProtocolType = protocolType;
            }
            finally
            {
                if (_ip != null)
                {
                    EndPoint = new(_ip, Port);
                }
                else
                {
                    EndPoint = new(_additionalIP, Port);
                }
                Socket = new(AddressFamily, SocketType, ProtocolType);

                Socket.Bind(EndPoint);
            }
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
                    _ip = _additionalIP;
                    throw new ArgumentException($"Value is invalid. IP was changed to: {_additionalIP}.", nameof(value));
                }
            }
        }
        public string IPString
        {
            get { return _ip.ToString(); }
            set
            {
                try
                {
                    ValidateIPv4(value);
                    _ip = IPAddress.Parse(value);
                }
                catch
                {
                    _ip = _additionalIP;
                }
            }
        }
        public ushort Port
        {
            get { return _port; }
            set
            {
                if (value < ushort.MaxValue)
                {
                    _port = value;
                }
                else
                {
                    _port = 50500;
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "The provided port value is not valid.");
                }
            }
        }
        
        private void ValidateIPv4(string newIP)
        {
            if (string.IsNullOrWhiteSpace(newIP))
            {
                throw new ArgumentNullException(nameof(newIP),
                    $"The value is null. IP was changed to: {_additionalIP}.");
            }

            string[] split = newIP.Split('.');
            if (split.Length != 4)
            {
                throw new ArgumentException($"The value is invalid. IP was changed to: {_additionalIP}.", 
                    nameof(newIP));
            }

            if (!split.All(value => byte.TryParse(value, out byte tempForParsing)))
            {
                throw new ArgumentOutOfRangeException(nameof(newIP),
                    $"The provided IP is not a valid IPv4 address. IP was changed to: {_additionalIP}.");
            }
        }

        public void SetAddressFamilyFromString(string addressFamily)
        {
            try
            {
                AddressFamily = GetAddressFamilyFromString(addressFamily);
            }
            catch
            {
                AddressFamily = AddressFamily.InterNetwork;
            }
        }
        private static AddressFamily GetAddressFamilyFromString(string addressFamily)
        {
            try
            {
                return (AddressFamily)Enum.Parse(typeof(AddressFamily), addressFamily);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Unknown argument value.",
                    nameof(addressFamily));
            }
        }

        public void SetSocketTypeFromString(string socketType)
        {
            try
            {
                SocketType = GetSocketTypeFromString(socketType);
            }
            catch
            {
                SocketType = SocketType.Stream;
            }
        }
        private static SocketType GetSocketTypeFromString(string socketType)
        {
            try
            {
                return (SocketType)Enum.Parse(typeof(SocketType), socketType);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Unknown argument value.",
                    nameof(socketType));
            }
        }

        public void SetProtocolTypeFromString(string protocolType)
        {
            try
            {
                ProtocolType = GetProtocolTypeFromString(protocolType);
            }
            catch
            {
                ProtocolType = ProtocolType.Tcp;
            }
        }
        private static ProtocolType GetProtocolTypeFromString(string protocolType)
        {
            try
            {
                return (ProtocolType)Enum.Parse(typeof(ProtocolType), protocolType, true);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Unknown argument value.", 
                    nameof(protocolType));
            }
        }

        // TODO: Fix update functions if there are any errors.
        public void UpdateEndPoint()
        {
            EndPoint = new(IP, Port);
        }
        public void UpdateSocket()
        {
            Socket?.Dispose();
            Socket = new(AddressFamily, SocketType, ProtocolType);

            Socket.Bind(EndPoint);
        }

        // Disposing
        bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _ip = null!;
                    _port = ushort.MaxValue;

                    if (EndPoint != null)
                    {
                        Socket.Close();
                        EndPoint = null!;
                    }
                    if (Socket != null)
                    {
                        Socket.Close();
                        Socket = null!;
                    }
                }
                _disposed = true;
            }
        }
    }
}
