using System.Net;

namespace NetworkDeviceDiscovery.Network
{
    internal class NetworkCalculator
    {
        public static string CalculateNetworkIP(string IPaddr, string Mask)
        {
            byte[] ipBytes = IPAddress.Parse(IPaddr).GetAddressBytes();
            byte[] maskBytes = IPAddress.Parse(Mask).GetAddressBytes();
            byte[] networkBytes = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            return new IPAddress(networkBytes).ToString();

        }

        public static int CalculateCIDR(string mask)
        {
            byte[] maskBytes = IPAddress.Parse(mask).GetAddressBytes();
            int cidr = 0;

            foreach (byte b in maskBytes)
            {
                byte currentByte = b;

                while (currentByte != 0)
                {
                    cidr += (b & 1);
                    currentByte >>= 1;
                }
            }

            return cidr;
        }

        public static int[] BytesToBits(byte[] IPBytes)
        {
            int[] IPBits = new int[32];
            int bitIndex = 0;

            foreach (byte b in IPBytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    IPBits[bitIndex] = (b >> i) & 1;
                    bitIndex++;
                }
            }

            return IPBits;
        }

        public static List<string> GenerateHostIPs(string NetIP, int cidr)
        {
            List<string> hosts = new List<string>();
            int hostCount = (int)Math.Pow(2, 32 - cidr);

            byte[] NetIPBytes = IPAddress.Parse(NetIP).GetAddressBytes();
            int[] NetIPBits = BytesToBits(NetIPBytes);
            

            for (int count = 1; count < (hostCount - 1); count++)
            {
                int[] ipBits = new int[32];

                for (int i = 0; i < cidr; i++)
                    ipBits[i] = NetIPBits[i];

                int value = count;

                for (int i = 31; i >= cidr; i--)
                {
                    ipBits[i] = value & 1;
                    value >>= 1;
                }

                hosts.Add(string.Join("", ipBits));
            }

            return hosts;
        }

        public static string BitsToIPAddress(string bits)
        {
            string ip = "";

            for (int i = 0; i < 32; i += 8)
            {
                string octetBits = bits.Substring(i, 8);
                int octet = Convert.ToInt32(octetBits, 2);
                ip += octet.ToString();

                if (i < 24)
                {
                    ip += ".";
                }
            }

            return ip;
        }
    }
}
