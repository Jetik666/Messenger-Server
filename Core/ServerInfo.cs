using System.Net;
using System.Net.Sockets;

namespace Core
{
    internal class ServerInfo : IDisposable
    {
        private IPAddress? _ip; 
        private ushort _port;

        public AddressFamily AddressFamily { get; set; }
        public SocketType SocketType { get; set; }
        public ProtocolType ProtocolType { get; set; }

        public IPEndPoint EndPoint { get; set; }
        public Socket Socket { get; set; }

        public ServerInfo() 
        {
            _ip = IPAddress.Parse("127.0.0.1");
            _port = 50500;

            AddressFamily = AddressFamily.InterNetwork;
            SocketType = SocketType.Stream;
            ProtocolType = ProtocolType.Tcp;

            EndPoint = new IPEndPoint(_ip, _port);
            Socket = new Socket(AddressFamily, SocketType, ProtocolType);
        }

        public void SetAddressFamilyFromString(string addressFamily)
        {
            AddressFamily = GetAddressFamilyFromString(addressFamily);
        }
        private static AddressFamily GetAddressFamilyFromString(string addressFamily)
        {
            /*switch(addressFamily)
            {
                case "InterNetwork":
                    return AddressFamily.InterNetwork;
                case "InterNetworkV6":
                    return AddressFamily.InterNetworkV6;
                case "Ipx":
                    return AddressFamily.Ipx;
                case "NetBios":
                    return AddressFamily.NetBios;
                default:
                    throw new NotImplementedException();
            }*/

            // C# 8.0 or greater
            return addressFamily switch
            {
                "InterNetwork" => AddressFamily.InterNetwork,
                "InterNetworkV6" => AddressFamily.InterNetworkV6,
                "Ipx" => AddressFamily.Ipx,
                "NetBios" => AddressFamily.NetBios,
                _ => throw new NotImplementedException(),
            };
        }

        public void SetSocketTypeFromString(string socketType)
        {
            SocketType = GetSocketTypeFromString(socketType);
        }
        private static SocketType GetSocketTypeFromString(string socketType)
        {
            return socketType switch
            {
                "Dgram" => SocketType.Dgram,
                "Raw" => SocketType.Raw,
                "Rdm" => SocketType.Rdm,
                "Seqpacket" => SocketType.Seqpacket,
                "Stream" => SocketType.Stream,
                "Unknown" => SocketType.Unknown,
                _ => throw new NotImplementedException()
            };
        }

        public void SetProtocolTypeFromString(string protocolType)
        {
            ProtocolType = GetProtocolTypeFromString(protocolType);
        }
        private static ProtocolType GetProtocolTypeFromString(string protocolType)
        {
            return protocolType switch
            {
                "Ggp" => ProtocolType.Ggp,
                "Icmp" => ProtocolType.Icmp,
                "IcmpV6" => ProtocolType.IcmpV6,
                "Idp" => ProtocolType.Idp,
                "Igmp" => ProtocolType.Igmp,
                "IP" => ProtocolType.IP,
                "IPSecAuthenticationHeader" => ProtocolType.IPSecAuthenticationHeader,
                "IPSecEncapsulatingSecurityPayload" => ProtocolType.IPSecEncapsulatingSecurityPayload,
                "IPv4" => ProtocolType.IPv4,
                "IPv6" => ProtocolType.IPv6,
                "IPv6DestinationOptions" => ProtocolType.IPv6DestinationOptions,
                "IPv6FragmentHeader" => ProtocolType.IPv6FragmentHeader,
                "IPv6HopByHopOptions" => ProtocolType.IPv6HopByHopOptions,
                "IPv6NoNextHeader" => ProtocolType.IPv6NoNextHeader,
                "IPv6RoutingHeader" => ProtocolType.IPv6RoutingHeader,
                "Ipx" => ProtocolType.Ipx,
                "ND" => ProtocolType.ND,
                "Pup" => ProtocolType.Pup,
                "Raw" => ProtocolType.Raw,
                "Spx" => ProtocolType.Spx,
                "SpxII" => ProtocolType.SpxII,
                "Tcp" => ProtocolType.Tcp,
                "Udp" => ProtocolType.Udp,
                "Unknown" => ProtocolType.Unknown,
                "Unspecified" => ProtocolType.Unspecified,
                _ => throw new NotImplementedException()
            };
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
