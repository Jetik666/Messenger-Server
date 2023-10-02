using System.Net;
using System.Net.Sockets;

using Core.Validator;

namespace Core.ServerInfo
{
    public class Server : NetworkStringValidator
    {
        public Server() { }

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
                    _ip = _defaultIP;
                    throw new ArgumentException($"Value is invalid. IP was changed to: {_defaultIP}.", nameof(value));
                }
            }
        }
        public string IPString
        {
            get { return _ip.ToString(); }
            set { ChangeIPv4(value); }
        }
        public ushort Port
        {
            get { return _port; }
            set { ChangePort(value.ToString()); }
        }

        public delegate void ServerValidation();


        public void UpdateEndPoint() 
        {
            try
            {
                EndPoint = new(IP, Port);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("IP or port is null.");
            }
        }
        public void UpdateSocket() 
        {
            if (EndPoint != null)
            {
                TurnOffSocket();
                Socket = new(AddressFamily, SocketType, ProtocolType);
                Socket.Bind(EndPoint);
            }
            else
            {
                try
                {
                    UpdateEndPoint();
                    UpdateSocket();
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException("EndPoint is null.", nameof(EndPoint));
                }
            }
        }

        private void TurnOffSocket() 
        {
            if (Socket != null)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket.Dispose();
            }
        }
    }
}
