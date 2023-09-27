using System.Diagnostics;
using System.Net.Sockets;

namespace Core
{
    public class Network
    {
        public ServerInfo ServerInfo { get; set; }

        public bool IsOnline { get; private set; }
        public bool CanSend { get; private set; }
        public bool CanReceive { get; private set; }

        private CancellationTokenSource cts;

        public Network() 
        { 
            ServerInfo = new ServerInfo();
            cts = new CancellationTokenSource();
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

        public async void Start()
        {
            if (ServerInfo != null)
            {
                ServerInfo.Socket.Listen(5);

                IsOnline = true;
                CanSend = true;
                CanReceive = true;

                cts = new CancellationTokenSource();
                await Listener();
            }
        }
        public async Task Close()
        {
            if (ServerInfo != null && cts != null)
            {
                cts.Cancel();

                try
                {
                    ServerInfo.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException ex)
                {
                    throw new Exception($"SocketException occured: {ex.Message}\n" +
                        $"Error code: {ex.ErrorCode}");
                }
                catch (ObjectDisposedException ex)
                {
                    throw new Exception($"ObjectDisposedException occured: {ex.Message}");
                }
                finally
                {
                    ServerInfo.Socket.Close();
                    ServerInfo.Socket.Disconnect(true);
                    ServerInfo.Socket.Dispose();

                    IsOnline = false;
                    CanSend = false;
                    CanReceive = false;

                    await Task.CompletedTask;
                }
            }
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

        private async Task Listener()
        {
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    Debug.WriteLine(ServerInfo.Socket.Connected);
                    Socket clientSocket = await Task.Factory.FromAsync(
                        new Func<AsyncCallback, object?, IAsyncResult>(ServerInfo.Socket.BeginAccept),
                        new Func<IAsyncResult, Socket>(ServerInfo.Socket.EndAccept),
                        null);
                    Debug.WriteLine(ServerInfo.Socket.Connected);
                    await DataHandler.HandleAsyncClient(clientSocket);
                }
                catch (SocketException)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
