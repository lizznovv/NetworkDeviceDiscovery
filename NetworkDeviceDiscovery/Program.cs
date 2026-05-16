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

            List<DiscoveredDevice> discovered = new List<DiscoveredDevice>();
            int total = ipAddresses.Count;
            int current = 0;

            foreach (var host in ipAddresses)
            {
                current++;

                int percent = current * 100 / total;
                string bar = new string('#', percent / 5) + new string('-', 20 - percent / 5);

                Console.Write($"\r[{bar}] {percent}% ({current}/{total})");

                DiscoveredDevice device = ArpScanner.ScanHost(interfaceInfo.IpAddress, host);
                if (device != null)
                {
                    device.icmpAvailable = ICMPScanner.PingHost(host);
                    discovered.Add(device);
                    
                }
            }

            Console.WriteLine("\nDiscovered devices:");
            Console.WriteLine($"{"IP",-15} | {"MAC",-15} | STATUS");
            Console.WriteLine(new string('-', 45));

            foreach (var dev in discovered)
            {
                Console.WriteLine(
                    $"{dev.ip,-15} | " +
                    $"{dev.mac,-15} | " +
                    $"Ping: {dev.icmpAvailable}"
                );
            }
        }
    }
}
