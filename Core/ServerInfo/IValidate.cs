using System.Net;

namespace Core.ServerInfo
{
    internal interface IValidate
    {
        void ValidateIPv4(string newIP);
        void ValidatePort(string newPort);
        void ValidateAddressFamily(string newAddressFamily);
        void ValidateSocketType(string newSocketType);
        void ValidateProtocolType(string newProtocolType);
    }
}
