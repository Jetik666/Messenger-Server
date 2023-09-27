using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Core
{
    internal class DataHandler : IDataHandler
    {
        public static async Task HandleAsyncClient(Socket clientSocket)
        {
            try
            {
                using (NetworkStream networkStream = new(clientSocket)) 
                {
                    byte[] receivedBuffer = new byte[1024];
                    int bytesRead = await networkStream.ReadAsync(receivedBuffer.AsMemory(0, receivedBuffer.Length));
                    string data = Encoding.ASCII.GetString(receivedBuffer, 0, bytesRead);



                    byte[] sendBuffer = Encoding.ASCII.GetBytes("Your response data");
                    await networkStream.WriteAsync(sendBuffer.AsMemory(0, sendBuffer.Length));
                }
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
