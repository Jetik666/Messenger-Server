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
                { typeof(IPAddress), (value) => IP = SetIPv4(value) },
                { typeof(ushort), (value) => Port = SetPort(value) },
                { typeof(AddressFamily), (value) => AddressFamily = SetSocketParameter<AddressFamily>(value) },
                { typeof(SocketType), (value) => SocketType = SetSocketParameter<SocketType>(value) },
                { typeof(ProtocolType), (value) => ProtocolType = SetSocketParameter<ProtocolType>(value) },
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
        /// Method SetValue changes value of T type variable.
        /// </summary>
        /// <typeparam name="T">Type of a value you want to change.</typeparam>
        /// <param name="newValue">New value of a parameter.</param>
        /// <param name="setDefault">Use default values or not if there is an error.</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetParameter<T>(string newValue, bool setDefault) 
        {
            if (!AbleToEdit)
            {
                return;
            }

            if (setDefault)
            {
                if (!_defaultMethods.TryGetValue(typeof(T), out var method))
                {
                    throw new ArgumentException($"{typeof(T)} is unknown type.");
                }
                method(newValue);
            }
            else
            {
                if (!_validationMethods.TryGetValue(typeof(T), out var method))
                {
                    throw new ArgumentException($"{typeof(T)} is unknown type.");
                }
                method(newValue);
            }
        }

        /// <summary>
        /// Method SetEndPoint sets EndPoint.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void SetEndPoint() 
        {
            if (!AbleToEdit)
            {
                return;
            }

            try
            {
                EndPoint = new(IP, Port);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("IP or port is null.");
            }
        }
        /// <summary>
        /// Method SetSocket set Socket.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void SetSocket() 
        {
            if (!AbleToEdit)
            {
                return;
            }

            if (EndPoint != null)
            {
                TurnOffSocket();
                Socket = new(AddressFamily, SocketType, ProtocolType);
                Socket.Bind(EndPoint);
            }
            else
            {
                throw new ArgumentException("EndPoint is null or socket has unknown parameters.", nameof(EndPoint));
            }
        }

        /// <summary>
        /// Method TurnOffSocket turns off Socket bind and disposes it.
        /// </summary>
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
