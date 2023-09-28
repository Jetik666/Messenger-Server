using System.Net.Sockets;

namespace Core.ServerInfo
{
    public class ServerValidator : IValidate
    {
        public ServerValidator() { }

        public void ValidateIPv4(string newIP)
        {
            if (string.IsNullOrWhiteSpace(newIP))
            {
                throw new ArgumentNullException(nameof(newIP),
                    $"The value is null.");
            }

            string[] split = newIP.Split('.');
            if (split.Length != 4)
            {
                throw new ArgumentException($"The value is invalid.",
                    nameof(newIP));
            }

            if (!split.All(value => byte.TryParse(value, out byte tempForParsing)))
            {
                throw new ArgumentOutOfRangeException(nameof(newIP),
                    $"The provided IP is not a valid IPv4 address.");
            }
        }
        public void ValidatePort(string newPort)
        {
            if (string.IsNullOrWhiteSpace(newPort))
            {
                throw new ArgumentNullException(nameof(newPort), $"The provided port is null.");
            }

            try
            {
                ushort port = Convert.ToUInt16(newPort);
            }
            catch (ArgumentException)
            {
                throw new ArgumentOutOfRangeException(nameof(newPort),
                    "The provided port value is not valid.");
            }
        }
        public void ValidateAddressFamily(string newAddressFamily)
        {
            if (Enum.IsDefined(typeof(AddressFamily), newAddressFamily))
            {
                return;
            }
            else
            {
                throw new ArgumentException($"Unknown address family: {newAddressFamily}.",
                    nameof(newAddressFamily));
            }
        }
        public void ValidateSocketType(string newSocketType)
        {
            if (Enum.IsDefined(typeof(SocketType), newSocketType))
            {
                return;
            }
            else
            {
                throw new ArgumentException($"Unknown socket type: {newSocketType}.",
                    nameof(newSocketType));
            }
        }
        public void ValidateProtocolType(string newProtocolType)
        {
            if (Enum.IsDefined(typeof(ProtocolType), newProtocolType))
            {
                return;
            }
            else
            {
                throw new ArgumentException($"Unknown protocol type: {newProtocolType}.",
                    nameof(newProtocolType));
            }
        }
    }
}
