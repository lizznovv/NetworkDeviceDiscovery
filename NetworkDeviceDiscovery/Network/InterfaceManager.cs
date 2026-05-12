using NetworkDeviceDiscovery.Models;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetworkDeviceDiscovery.Network
{
    internal static class InterfaceManager
    {
        public static NetworkInterfaceInfo GetActiveInterfaceInfo()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var netInterface in interfaces)
            {
                if (netInterface.OperationalStatus != OperationalStatus.Up) 
                    continue;

                var properties = netInterface.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0) 
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return new NetworkInterfaceInfo
                        {
                            InterfaceName = netInterface.Name,
                            IpAddress = address.Address.ToString(),
                            SubnetMask = address.IPv4Mask.ToString(),
                            Gateway = properties.GatewayAddresses[0].Address.ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}
