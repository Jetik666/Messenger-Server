using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal class ServerInfo : IDisposable
    {
        private IPAddress? _ip; 
        private ushort _port;

        private AddressFamily _addressFamily;
        private SocketType _socketType;
        private ProtocolType _protocolType;

        public IPEndPoint EndPoint { get; set; }
        public Socket Socket { get; set; }

        public ServerInfo() 
        {
            _ip = IPAddress.Parse("127.0.0.1");
            _port = 50500;

            _addressFamily = AddressFamily.InterNetwork;
            _socketType = SocketType.Stream;
            _protocolType = ProtocolType.Tcp;

            EndPoint = new IPEndPoint(_ip, _port);
            Socket = new Socket(_addressFamily, _socketType, _protocolType);
        }


        public string AddressFamily
        { 
            get { return _addressFamily; } 
        }








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
                    _ip = null;
                    _port = ushort.MaxValue;

                    if (EndPoint != null)
                    {
                        Socket.Close();
                        EndPoint = null;
                    }
                    if (Socket != null)
                    {
                        Socket.Close();
                        Socket = null;
                    }
                }
                _disposed = true;
            }
        }
    }
}
