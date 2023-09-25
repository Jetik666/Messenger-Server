using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

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

    public class NetworkServerHost : INetworkServerHost
    {
        private IPAddress? _ip;
        private ushort _port;

        public IPEndPoint EndPoint { get; set; }
        public Socket TcpSocket { get; set; }
        public bool IsOnline { get; private set; }
        public bool CanSend { get; private set; }
        public bool CanReceive { get; private set; }

        public NetworkServerHost()
        {
            _ip = IPAddress.Parse("127.0.0.1");
            _port = 50500;
            IsOnline = false;
            CanSend = false;
            CanReceive = false;

            EndPoint = new IPEndPoint(_ip, _port);
            TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public string IP
        {
            get 
            { 
                if (_ip == null)
                {
                    return "";
                }
                else
                {
                    return _ip.ToString();                
                }
            }
            set
            {
                if (IsOnline)
                {
                    if (IPAddress.TryParse(value, out _ip))
                    {
                        Debug.WriteLine("IP address was changed");

                    }
                    else
                    {
                        Debug.WriteLine("Invalid IP address");
                    }
                }
                else
                {
                    Debug.WriteLine("Server is offline. You must close it before changing IP or port.");
                }
            }
        }
        public ushort Port
        {
            get => _port;
            set
            {
                if (IsOnline)
                {
                    try
                    {
                        _port = value;
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Debug.WriteLine("Server is offline. You must close it before changing IP or port.");
                }
            }
        }

        public void Start()
        {
            try
            {
                if (_ip == null)
                {
                    _ip = IPAddress.Loopback;
                    _port = 50500;
                }
                Debug.WriteLine($"Current address is {_ip}:{_port}.");

                EndPoint = new IPEndPoint(_ip, _port);
                Debug.WriteLine("End point created.\nBinding socket.");

                TcpSocket.Bind(EndPoint);
                Debug.WriteLine("Socket was binded succesfully.");
                IsOnline = true;
                CanSend = true;
                CanReceive = true;

                TcpSocket.Listen(2);

                Listener();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsOnline = false;
                CanSend = false;
                CanReceive = false;
            }
        }
        public async void Close()
        {
            try
            {
                Debug.WriteLine("Closing server.");
                TcpSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                TcpSocket.Close();
                IsOnline = false;
                Debug.WriteLine("Server is offline.");
                await Task.CompletedTask;
            }
        }
        public async void ShutdownBoth()
        {
            try
            {
                Debug.WriteLine("Shutting down for sending and receiving data to server.");
                TcpSocket.Shutdown(SocketShutdown.Both);

                CanSend = false;
                CanReceive = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
                CanSend = true;
                CanReceive = true;
            }
            await Task.CompletedTask;
        }
        public async void ShutdownSendOnly()
        {
            try
            {
                Debug.WriteLine("Shutting down for sending data to server.");
                TcpSocket.Shutdown(SocketShutdown.Both);

                CanSend = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
                CanSend = true;
            }
            await Task.CompletedTask;
        }
        public async void ShutdownReceiveOnly()
        {
            try
            {
                Debug.WriteLine("Shutting down for receiving data to server.");
                TcpSocket.Shutdown(SocketShutdown.Both);

                CanReceive = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
                CanReceive = true;
            }
            await Task.CompletedTask;
        }

        private async void Listener()
        {
            while (true)
            {
                Debug.WriteLine("Ping");
                Socket clientSocket = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object?, IAsyncResult>(TcpSocket.BeginAccept),
                    new Func<IAsyncResult, Socket>(TcpSocket.EndAccept),
                    null);

                DataHandler.HandleAsyncClient(clientSocket);
            }
        }
    }
}
