using System.Net;
using System.Net.Sockets;

using Core.ServerInfo;

namespace Core.Validator
{
    /// <summary>
    /// Class NetworkStringValidator is used for server configure validation for string values.
    /// </summary>
    public abstract class NetworkStringValidator : DefaultServerInfo
    {
        /// <summary>
        /// Method ChangeIPv4 changes new IP or sets it to default.
        /// </summary>
        /// <param name="newIP">New IP Address.</param>
        protected void ChangeIPv4(string newIP) 
        {
            try
            {
                NetworkValidator.IPv4(newIP);
                _ip =  IPAddress.Parse(newIP);
            }
            catch (ArgumentNullException argumentNull)
            {
                _ip = _defaultIP;
            }
            catch (ArgumentOutOfRangeException argumentOutOfRange)
            {
                _ip = _defaultIP;
            }
            catch (ArgumentException argument)
            {
                _ip = _defaultIP;
            }
        }

        /// <summary>
        /// Method ChangePort changes new Port or sets it to default.
        /// <para>Port has range from 0 to 65535.</para>
        /// </summary>
        /// <param name="newPort">New Port.</param>
        protected void ChangePort(string newPort) 
        {
            try
            {
                NetworkValidator.Port(newPort);
                _port = Convert.ToUInt16(newPort);
            }
            catch (ArgumentNullException argumentNull)
            {
                _port = _defaultPort;
            }
            catch (ArgumentOutOfRangeException argumentOutOfRange)
            {
                _port = _defaultPort;
            }
        }

        /// <summary>
        /// Method ChangeSocketParameter changes Socket parameter or sets it to default.
        /// </summary>
        /// <typeparam name="T">It`s type of a Socket parameter.</typeparam>
        /// <param name="newValue">New value of a Socket parameter as a string.</param>
        /// <returns>New value of a Socket parameter</returns>
        protected T ChangeSocketParameter<T>(string newValue) where T : Enum
        {
            try
            {
                NetworkValidator.SocketValue(newValue, typeof(T));
                return (T)Enum.Parse(typeof(T), newValue);
            }
            catch (ArgumentException argument)
            {
                return ReturnDefaultValue<T>();
            }
        }

        /// <summary>
        /// Method ReturnDefaultValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private T ReturnDefaultValue<T>() where T : Enum
        {
            if (typeof(T) == typeof(AddressFamily))
            {
                return (T)Enum.ToObject(typeof(T), _defaultAddressFamily);
            }
            else if (typeof(T) == typeof(SocketType))
            {
                return (T)Enum.ToObject(typeof(T), _defaultSocketType);
            }
            else if (typeof(T) == typeof(ProtocolType))
            {
                return (T)Enum.ToObject(typeof(T), _defaultProtocolType);
            }
            else
            {
                throw new InvalidOperationException($"{typeof(T)} is unknown type.");
            }
        }
    }
}
