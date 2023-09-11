using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace Core
{
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

        public async Task Start()
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsOnline = false;
                CanSend = false;
                CanReceive = false;
            }
        }
        public async Task Close()
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
        public async Task ShutdownBoth()
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
        public async Task ShutdownSendOnly()
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
        public async Task ShutdownReceiveOnly()
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
    }
}
