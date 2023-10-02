using System.Diagnostics;
using System.Net.Sockets;

using Core.Configuration;

namespace Core
{
    public class Network
    {
        public NetworkConfiguration Configuration { get; set; }

        public bool IsOnline { get; private set; }
        public bool CanSend { get; private set; }
        public bool CanReceive { get; private set; }

        private CancellationTokenSource cts;

        public Network() 
        {
            Configuration = new NetworkConfiguration();
            cts = new CancellationTokenSource();
        }

        public async void Start()
        {
            if (Configuration.Socket != null)
            {
                Configuration.Socket.Listen(5);

                IsOnline = true;
                CanSend = true;
                CanReceive = true;

                cts = new CancellationTokenSource();
                await Listener();
            }
        }
        public void Close()
        {
            if (Configuration.Socket != null && cts != null)
            {
                cts.Cancel();

                try
                {
                    Configuration.Socket.Shutdown(SocketShutdown.Both);
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
                    Configuration.Socket.Close();

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
            if (Configuration.Socket == null)
            {
                return;
            }

            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    Socket clientSocket = await Task.Factory.FromAsync(
                        new Func<AsyncCallback, object?, IAsyncResult>(Configuration.Socket.BeginAccept),
                        new Func<IAsyncResult, Socket>(Configuration.Socket.EndAccept),
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

            Configuration.Socket.Close();
        }
    }
}
