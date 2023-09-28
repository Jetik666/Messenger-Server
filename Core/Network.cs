using System.Diagnostics;
using System.Net.Sockets;

using Core.ServerInfo;

namespace Core
{
    public class Network
    {
        public Server Server { get; set; }

        public bool IsOnline { get; private set; }
        public bool CanSend { get; private set; }
        public bool CanReceive { get; private set; }

        private CancellationTokenSource cts;

        public Network() 
        { 
            Server = new Server();
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
            if (Server.Socket != null)
            {
                Server.Socket.Listen(5);

                IsOnline = true;
                CanSend = true;
                CanReceive = true;

                cts = new CancellationTokenSource();
                await Listener();
            }
        }
        public void Close()
        {
            if (Server.Socket != null && cts != null)
            {
                cts.Cancel();

                try
                {
                    Server.Socket.Shutdown(SocketShutdown.Both);
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
                    Server.Socket.Close();

                    IsOnline = false;
                    CanSend = false;
                    CanReceive = false;
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
            if (Server.Socket != null)
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        Socket clientSocket = await Task.Factory.FromAsync(
                            new Func<AsyncCallback, object?, IAsyncResult>(Server.Socket.BeginAccept),
                            new Func<IAsyncResult, Socket>(Server.Socket.EndAccept),
                            null);
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

                Server.Socket.Close();
            }
        }
    }
}
