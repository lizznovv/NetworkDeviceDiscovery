using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;

namespace NetworkDeviceDiscovery.Network
{
    internal class ICMPScanner
    {
        public static bool PingHost(string ip)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply =
                    ping.Send(ip, 1000);

                return reply.Status
                    == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
