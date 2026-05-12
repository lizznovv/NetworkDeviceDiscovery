using SharpPcap;
using PacketDotNet;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkDeviceDiscovery.Network
{
    internal class ArpScanner
    {
        public static void ScanHost(string targetIP)
        {
            var devices = CaptureDeviceList.Instance;

            if (devices.Count < 1)
            {
                Console.WriteLine("No devices found");
                return;
            }

            var device = devices[0];


            device.Open();

            var interfaceInfo = InterfaceManager.GetActiveInterfaceInfo();

            PhysicalAddress broadcast = PhysicalAddress.Parse("FFFFFFFFFFFF");
            PhysicalAddress sourceMac = device.MacAddress;
            IPAddress sourceIP = IPAddress.Parse(interfaceInfo.IpAddress);
            IPAddress destinationIP = IPAddress.Parse(targetIP);
        }
    }
}
