using NetworkDeviceDiscovery.Models;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkDeviceDiscovery.Network
{
    internal class ArpScanner
    {
        public static LibPcapLiveDevice deviceSearcher(string deviceIP, LibPcapLiveDeviceList devices)
        {
            LibPcapLiveDevice device = null;

            foreach (var dev in devices)
            {
                foreach (var addr in dev.Addresses)
                {
                    if (addr.Addr != null && addr.Addr.ipAddress != null)
                    {
                        if (addr.Addr.ipAddress.ToString() == deviceIP)
                        {
                            device = dev;
                            break;
                        }
                    }
                }

                if (device != null)
                {
                    break;
                }
            }

            return device;
        }

        public static DiscoveredDevice ScanHost(string deviceIP, string targetIP)
        {
            var devices = LibPcapLiveDeviceList.Instance;

            if (devices.Count < 1)
            {
                Console.WriteLine("No devices found");
                return null;
            }

            LibPcapLiveDevice device = deviceSearcher(deviceIP, devices);
            if (device == null)
            {
                Console.WriteLine("Device not found");
                return null;
            }

            device.Open();

            PhysicalAddress broadcast = PhysicalAddress.Parse("FFFFFFFFFFFF");
            PhysicalAddress sourceMac = device.MacAddress;
            IPAddress sourceIP = IPAddress.Parse(deviceIP);
            IPAddress destinationIP = IPAddress.Parse(targetIP);

            //создаем arp message
            var arpPacket = new ArpPacket(ArpOperation.Request,
                PhysicalAddress.Parse("000000000000"),
                destinationIP,
                sourceMac,
                sourceIP
            );

            //ethernet оболочка
            var ethernet = new EthernetPacket(
                sourceMac,
                broadcast,
                EthernetType.Arp
            );

            ethernet.PayloadPacket = arpPacket;
            device.SendPacket(ethernet);

            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalSeconds < 2)
            {
                //ожидаем след сетевого пакета
                var status = device.GetNextPacket(out var rawPacket);

                //если пакет не был успешно прочитан —  ждём дальше
                if (status != GetPacketStatus.PacketRead)
{
                    continue;
                }

                //парсим ethernet frame
                var packet = Packet.ParsePacket(
                    rawPacket.GetPacket().LinkLayerType,
                    rawPacket.GetPacket().Data
                );

                //пытаемся достать arp пакет
                var arp = packet.Extract<ArpPacket>();

                if (arp != null)
                {
                    //reply это или request
                    if (arp.Operation == ArpOperation.Response)
                    {
                        //Console.WriteLine($"ARP reply from {arp.SenderProtocolAddress}");

                        //от нужного ли ip reply
                        if (arp.SenderProtocolAddress.ToString() == targetIP)
                        {
                            string ip = targetIP;
                            string mac = arp.SenderHardwareAddress.ToString();
                            device.Close();
                            return new DiscoveredDevice(ip, mac, false);
                        }
                    }    
                }
            }
            device.Close();
            return null;
        }
    }
}
