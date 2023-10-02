using System.Net.Sockets;

namespace Core.Validator
{
    /// <summary>
    /// Static class NetworkValidator is used for server configure validation for string values.
    /// </summary>
    public static class NetworkValidator
    {
        /// <summary>
        /// Static method ChangeIPv4 validates new IP if there are no errors.
        /// </summary>
        /// <param name="newIP">New IP Address.</param>
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

        /// <summary>
        /// Static method SocketValue validates new value for Socket enums if there are no errors.
        /// </summary>
        /// <param name="newValue">New value for a Socket enum as a string type.</param>
        /// <param name="valueType">It take type of Socket enums.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void SocketValue(string newValue, Type valueType)
        {
            if (Enum.IsDefined(valueType, newValue))
            {
                return;
            }
            else
            {
                throw new ArgumentException($"Unknown address family: {newValue}.",
                    nameof(newValue));
            }
        }

        ///// <summary>
        ///// Static method ChangeST validates new Socket Type or sets Socket Type to default if there are any errors.
        ///// </summary>
        ///// <param name="newSocketType">New Socket Type as a string type.</param>
        ///// <exception cref="ArgumentException"></exception>
        //public static void SocketType(string newSocketType)
        //{
        //    if (Enum.IsDefined(typeof(SocketType), newSocketType))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        throw new ArgumentException($"Unknown socket type: {newSocketType}.",
        //            nameof(newSocketType));
        //    }
        //}

        ///// <summary>
        ///// Static method ChangePT validates new Protocol Type or sets Protocol Type to default if there are any errors.
        ///// </summary>
        ///// <param name="newProtocolType">New Protocol Type as a string type.</param>
        ///// <exception cref="ArgumentException"></exception>
        //public static void ProtocolType(string newProtocolType)
        //{
        //    if (Enum.IsDefined(typeof(ProtocolType), newProtocolType))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        throw new ArgumentException($"Unknown protocol type: {newProtocolType}.",
        //            nameof(newProtocolType));
        //    }
        //}
    }
}
