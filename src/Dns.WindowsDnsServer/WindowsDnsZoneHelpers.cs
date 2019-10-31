using System;
using System.Management;

namespace Dns.WindowsDnsServer
{
    internal static class WindowsDnsZoneHelpers
    {

        public static WindowsDnsZone GetZoneInternal(this WindowsDnsServer server, string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            try
            {
                var result = server.WmiGetInstance("MicrosoftDNS_Zone", $"ContainerName=\"{domainName}\",DnsServerName=\"{server.Name}\",Name=\"{domainName}\"");
                return new WindowsDnsZone(server, result);
            }
            catch (ManagementException me) when (string.Equals(me.Message, "Generic failure ", StringComparison.Ordinal))
            {
                throw new ArgumentException("Zone not found", nameof(domainName));
            }
        }

        public static void DeleteZoneInternal(this WindowsDnsServer server, WindowsDnsZone zone)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            using (var instance = zone.server.WmiGetInstance(new ManagementPath(zone.wmiPath)))
            {
                instance.Delete();
            }
        }

    }
}
