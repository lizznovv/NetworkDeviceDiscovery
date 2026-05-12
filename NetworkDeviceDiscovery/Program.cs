using NetworkDeviceDiscovery.Models;
using NetworkDeviceDiscovery.Network;

namespace NetworkDeviceDiscovery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var interfaceInfo = InterfaceManager.GetActiveInterfaceInfo();
            Console.WriteLine(interfaceInfo);

            var ip = interfaceInfo.IpAddress;
            var mask = interfaceInfo.SubnetMask;
            var NetIP = NetworkCalculator.CalculateNetworkIP(ip, mask);
            var cidr = NetworkCalculator.CalculateCIDR(mask);

            Console.WriteLine($"\nNetwork: {NetIP}/{cidr}");

            var HostsList = NetworkCalculator.GenerateHostIPs(NetIP, cidr);
            List<string> ipAddresses = new List<string>();

            foreach (var bits in HostsList)
            {
                ipAddresses.Add(NetworkCalculator.BitsToIPAddress(bits));
            }

            Console.WriteLine($"\nList of IPs: ");
            foreach (var host in ipAddresses)
            {
                Console.WriteLine(host);
            }

            

        }
    }
}
