using System.Diagnostics;

namespace Core
{
    public class NetworkServerHost : INetworkServerHost
    {
        public ServerInfo? ServerInfo { get; set; }

        public bool IsOnline { get; private set; }
        public bool CanSend { get; private set; }
        public bool CanReceive { get; private set; }

        public NetworkServerHost()
        {
            
        }

        //public string IP
        //{
        //    get 
        //    { 
        //        if (_ip == null)
        //        {
        //            return "";
        //        }
        //        else
        //        {
        //            return _ip.ToString();                
        //        }
        //    }
        //    set
        //    {
        //        if (IsOnline)
        //        {
        //            if (IPAddress.TryParse(value, out _ip))
        //            {
        //                Debug.WriteLine("IP address was changed");

        //            }
        //            else
        //            {
        //                Debug.WriteLine("Invalid IP address");
        //            }
        //        }
        //        else
        //        {
        //            Debug.WriteLine("Server is offline. You must close it before changing IP or port.");
        //        }
        //    }
        //}
        //public ushort Port
        //{
        //    get => _port;
        //    set
        //    {
        //        if (IsOnline)
        //        {
        //            try
        //            {
        //                _port = value;
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                Debug.WriteLine(ex.Message);
        //            }
        //        }
        //        else
        //        {
        //            Debug.WriteLine("Server is offline. You must close it before changing IP or port.");
        //        }
        //    }
        //}

        public void Start()
        {
            //try
            //{
            //    if (_ip == null)
            //    {
            //        _ip = IPAddress.Loopback;
            //        _port = 50500;
            //    }
            //    Debug.WriteLine($"Current address is {_ip}:{_port}.");

            //    EndPoint = new IPEndPoint(_ip, _port);
            //    Debug.WriteLine("End point created.\nBinding socket.");

            //    TcpSocket.Bind(EndPoint);
            //    Debug.WriteLine("Socket was binded succesfully.");
            //    IsOnline = true;
            //    CanSend = true;
            //    CanReceive = true;

            //    TcpSocket.Listen(2);

            //    Listener();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    IsOnline = false;
            //    CanSend = false;
            //    CanReceive = false;
            //}
        }
        public async void Close()
        {
            //try
            //{
            //    Debug.WriteLine("Closing server.");
            //    TcpSocket.Shutdown(SocketShutdown.Both);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    TcpSocket.Close();
            //    IsOnline = false;
            //    Debug.WriteLine("Server is offline.");
            //    await Task.CompletedTask;
            //}
        }
        public async void ShutdownBoth()
        {
            try
            {
                Debug.WriteLine("Shutting down for sending and receiving data to server.");
                //TcpSocket.Shutdown(SocketShutdown.Both);

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
                //TcpSocket.Shutdown(SocketShutdown.Both);

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
                //TcpSocket.Shutdown(SocketShutdown.Both);

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
                //Socket clientSocket = await Task.Factory.FromAsync(
                //    new Func<AsyncCallback, object?, IAsyncResult>(TcpSocket.BeginAccept),
                //    new Func<IAsyncResult, Socket>(TcpSocket.EndAccept),
                //    null);

                //DataHandler.HandleAsyncClient(clientSocket);
            }
        }
    }
}
