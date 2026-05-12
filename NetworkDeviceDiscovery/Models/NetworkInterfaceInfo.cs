
namespace NetworkDeviceDiscovery.Models
{
    internal class NetworkInterfaceInfo
    {
        public string InterfaceName { get; set; }
        public string IpAddress { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }
        public override string ToString()
        {
            return $"Interface: {InterfaceName}\n" +
                   $"IP: {IpAddress}\n" +
                   $"Mask: {SubnetMask}\n" +
                   $"Gateway: {Gateway}";
        }
    }
}