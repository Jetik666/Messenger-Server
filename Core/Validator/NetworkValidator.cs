using System.Net;

namespace Core.Validator
{
    /// <summary>
    /// Static class NetworkValidator is used for server configure validation for string values.
    /// </summary>
    public static class NetworkValidator
    {
        /// <summary>
        /// Static method ChangeIPv4 validates new IPv4 if there are no errors.
        /// </summary>
        /// <param name="newIP"></param>
        /// <returns>New IPv4 value.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IPAddress SetIPv4(string newIP)
        {
            if (string.IsNullOrWhiteSpace(newIP))
            {
                throw new ArgumentNullException(nameof(newIP),
                    $"The value is null.");
            }

            if (IPAddress.TryParse(newIP, out IPAddress? ipAddress))
            {
                return ipAddress;
            }
            else
            {
                throw new ArgumentException($"The value is invalid",
                    nameof(newIP));
            }
        }

        /// <summary>
        /// Static method ChangePort validates new Port if there are no errors.
        /// <para>Port has range from 0 to 65535.</para>
        /// </summary>
        /// <param name="newPort">New Port.</param>
        /// <returns>New Port value.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ushort SetPort(string newPort)
        {
            if (string.IsNullOrWhiteSpace(newPort))
            {
                throw new ArgumentNullException(nameof(newPort), $"The provided port is null.");
            }

            if (Convert.ToUInt16(newPort) > ushort.MaxValue || Convert.ToUInt16(newPort) < ushort.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(newPort), $"The provided port is out of range");
            }

            return ushort.Parse(newPort);
        }

        /// <summary>
        /// Static method SocketValue validates new value for Socket enums if there are no errors.
        /// </summary>
        /// <typeparam name="T">Type of a Socket Parameter.</typeparam>
        /// <param name="newValue">Value of a Socket Parameter</param>
        /// <returns>New Value of a Socket Parameter</returns>
        /// <exception cref="ArgumentException"></exception>
        public static T SetSocketParameter<T>(string newValue)
        {
            if (Enum.IsDefined(typeof(T), newValue))
            {
                return (T)Enum.Parse(typeof(T), newValue);
            }
            else
            {
                throw new ArgumentException($"Unknown address family: {newValue}.",
                    nameof(newValue));
            }
        }

        /// <summary>
        /// Static method ChangeIPv4 validates new IPv4 if there are no errors.
        /// </summary>
        /// <param name="newIP"></param>
        /// <returns>True if IPv4 is valid.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IPv4(string newIP)
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

        /// <summary>
        /// Static method ChangePort validates new Port if there are no errors.
        /// <para>Port has range from 0 to 65535.</para>
        /// </summary>
        /// <param name="newPort">New Port.</param>
        /// <returns>True if Port is valid.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void Port(string newPort)
        {
            if (string.IsNullOrWhiteSpace(newPort))
            {
                throw new ArgumentNullException(nameof(newPort), $"The provided port is null.");
            }

            if (Convert.ToUInt16(newPort) > ushort.MaxValue || Convert.ToUInt16(newPort) < ushort.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(newPort), $"The provided port is out of range");
            }
        }

        /// <summary>
        /// Static method SocketValue validates new value for Socket enums if there are no errors.
        /// </summary>
        /// <typeparam name="T">Type of a Socket Parameter.</typeparam>
        /// <param name="newValue">Value of a Socket Parameter</param>
        /// <returns>True if Socket parameter is valid.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static void SocketParameter<T>(string newValue)
        {
            if (!Enum.IsDefined(typeof(T), newValue))
            {
                throw new ArgumentException($"Unknown address family: {newValue}.",
                    nameof(newValue));
            }
        }
    }
}
