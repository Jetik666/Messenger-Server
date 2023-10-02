using System.Net;
using System.Net.Sockets;

using Core.Validator;

namespace Core.Configuration
{
    /// <summary>
    /// Class Configuration is used for controlling server parameters
    /// </summary>
    public class NetworkConfiguration : NetworkStringValidator
    {
        private readonly Dictionary<Type, Action<string>> _defaultMethods;
        private readonly Dictionary<Type, Action<string>> _validationMethods;

        public NetworkConfiguration() 
        {
            _defaultMethods = new()
            {
                { typeof(IPAddress), (value) => IP = ChangeIPv4(value) },
                { typeof(ushort), (value) => Port = ChangePort(value) },
                { typeof(AddressFamily), (value) => AddressFamily = ChangeSocketParameter<AddressFamily>(value) },
                { typeof(SocketType), (value) => SocketType = ChangeSocketParameter<SocketType>(value) },
                { typeof(ProtocolType), (value) => ProtocolType = ChangeSocketParameter<ProtocolType>(value) },
            };
            _validationMethods = new()
            {
                { typeof(IPAddress), (value) => IP = NetworkValidator.SetIPv4(value) },
                { typeof(ushort), (value) => Port = NetworkValidator.SetPort(value) },
                { typeof(AddressFamily), (value) => AddressFamily = NetworkValidator.SetSocketParameter<AddressFamily>(value) },
                { typeof(SocketType), (value) => SocketType = NetworkValidator.SetSocketParameter<SocketType>(value) },
                { typeof(ProtocolType), (value) => ProtocolType = NetworkValidator.SetSocketParameter<ProtocolType>(value) },
            };
        }

        /// <summary>
        /// Method ChangeValue changes value of T type variable.
        /// </summary>
        /// <typeparam name="T">Type of a value you want to change.</typeparam>
        /// <param name="newValue">New value of a parameter.</param>
        /// <param name="setDefault">Use default values or not if there is an error.</param>
        /// <exception cref="ArgumentException"></exception>
        public void ChangeParameter<T>(string newValue, bool setDefault) 
        {
            if (AbleToEdit)
            { 
                if (setDefault) 
                { 
                    if (_defaultMethods.TryGetValue(typeof(T), out var method))
                    {
                        method(newValue);
                    }
                    else
                    {
                        throw new ArgumentException($"{typeof(T)} is unknown type.");
                    }
                }
                else
                {
                    if (_validationMethods.TryGetValue(typeof(T), out var method))
                    {
                        method(newValue);
                    }
                    else
                    {
                        throw new ArgumentException($"{typeof(T)} is unknown type.");
                    }
                }
            }
        }

        public void UpdateEndPoint() 
        {
            if (AbleToEdit)
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
        }
        public void UpdateSocket() 
        {
            if (AbleToEdit)
            {
                if (EndPoint != null)
                {
                    TurnOffSocket();
                    Socket = new(AddressFamily, SocketType, ProtocolType);
                    Socket.Bind(EndPoint);
                }
                else
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
