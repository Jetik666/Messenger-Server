using Core.Configuration;
using System.Net;
using System.Net.Sockets;

// TODO: Add loger to all exceptions

namespace Core.Validator
{
    /// <summary>
    /// Class NetworkStringValidator is used for server configure validation for string values.
    /// </summary>
    public abstract class NetworkStringValidator : DefaultConfiguration
    {
        /// <summary>
        /// Method ChangeIPv4 changes new IPv4 or sets it to default.
        /// </summary>
        /// <param name="newIP">New IPv4 Address.</param>
        /// <returns>New or default IPv4 Address.</returns>
        protected IPAddress SetIPv4(string newIP) 
        {
            try
            {
                return NetworkValidator.SetIPv4(newIP);
            }
            catch (ArgumentNullException argumentNull)
            {
                return _defaultIP;
            }
            catch (ArgumentOutOfRangeException argumentOutOfRange)
            {
                return _defaultIP;
            }
            catch (ArgumentException argument)
            {
                return _defaultIP;
            }
        }

        /// <summary>
        /// Method ChangePort changes new Port or sets it to default.
        /// <para>Port has range from 0 to 65535.</para>
        /// </summary>
        /// <param name="newPort">New Port.</param>
        /// <returns>New or default Port.</returns>
        protected ushort SetPort(string newPort) 
        {
            try
            {
                return NetworkValidator.SetPort(newPort);
            }
            catch (ArgumentNullException argumentNull)
            {
                return _defaultPort;
            }
            catch (ArgumentOutOfRangeException argumentOutOfRange)
            {
                return _defaultPort;
            }
        }

        /// <summary>
        /// Method ChangeSocketParameter changes Socket parameter or sets it to default.
        /// </summary>
        /// <typeparam name="T">It`s type of a Socket parameter.</typeparam>
        /// <param name="newValue">New value of a Socket parameter as a string.</param>
        /// <returns>New or default value of a Socket parameter</returns>
        protected T SetSocketParameter<T>(string newValue) where T : Enum
        {
            try
            {
                return NetworkValidator.SetSocketParameter<T>(newValue);
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
