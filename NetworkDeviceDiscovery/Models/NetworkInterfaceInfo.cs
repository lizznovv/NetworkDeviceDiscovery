
namespace NetworkDeviceDiscovery.Models
{
    internal class NetworkInterfaceInfo
    {
        public string InterfaceName { get; set; }
        public string IpAddress { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }

        public NetworkInterfaceInfo(string InterfaceName, string IpAddress, string SubnetMask, string Gateway) 
        { 
            this.InterfaceName = InterfaceName;
            this.IpAddress = IpAddress;
            this.SubnetMask = SubnetMask;
            this.Gateway = Gateway;
        }
        public override string ToString()
        {
            return $"Interface: {InterfaceName}\n" +
                   $"IP: {IpAddress}\n" +
                   $"Mask: {SubnetMask}\n" +
                   $"Gateway: {Gateway}";
        }
    }
}