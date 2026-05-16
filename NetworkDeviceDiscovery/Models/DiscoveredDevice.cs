using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkDeviceDiscovery.Models
{
    internal class DiscoveredDevice
    {
        public string ip {  get; set; }
        public string mac { get; set; }
        public bool icmpAvailable { get; set; }

        public DiscoveredDevice(string ip, string mac, bool icmpAvailable)
        {
            this.ip = ip;
            this.mac = mac;
            this.icmpAvailable = icmpAvailable;
        }
    }
}
