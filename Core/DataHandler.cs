using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Core
{
    internal class DataHandler : IDataHandler
    {
        public static void HandleAsyncClient(Socket clientSocket)
        {
            try
            {
                Debug.WriteLine(clientSocket.LocalEndPoint);
                Debug.WriteLine(clientSocket.RemoteEndPoint);

                byte[] receivedBuffer = new byte[1024];
                int bytesRead = clientSocket.Receive(receivedBuffer);
                string data = Encoding.ASCII.GetString(receivedBuffer, 0, bytesRead);

                Debug.WriteLine(data);
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }
}
