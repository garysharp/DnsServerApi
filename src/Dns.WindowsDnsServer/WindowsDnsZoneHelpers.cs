using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Dns.WindowsDnsServer
{
    internal static class WindowsDnsZoneHelpers
    {

        public static IEnumerable<WindowsDnsZone> GetZonesInternal(this WindowsDnsServer server)
        {
            foreach (var result in server.WmiGetInstances("MicrosoftDNS_Zone"))
            {
                yield return new WindowsDnsZone(server, result);
            }
        }

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

        public static WindowsDnsZone CreateZoneInternal(this WindowsDnsServer server, string domainName, CreateZoneType type, bool directoryServicesIntegrated, string dataFileName, IEnumerable<DnsIpAddress> masterDnsServerAddresses, string adminEmailName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));
            if (string.IsNullOrWhiteSpace(dataFileName))
                dataFileName = null;
            var serverAddresses = default(string[]);
            if (masterDnsServerAddresses != null)
            {
                serverAddresses = masterDnsServerAddresses.Select(a => a.ToString()).ToArray();
                if (serverAddresses.Length == 0)
                    serverAddresses = null;
                else
                {
                    if (type != CreateZoneType.Secondary && type != CreateZoneType.Stub && type != CreateZoneType.Forwarder)
                        throw new ArgumentException("Master DNS Server Addresses can only be used in Secondary, Stub and Forwarder zones.", nameof(masterDnsServerAddresses));
                }
            }
            if (string.IsNullOrWhiteSpace(adminEmailName))
                adminEmailName = null;

            using (var wmiZoneClass = server.WmiGetClass("MicrosoftDNS_Zone"))
            {
                using (var createZoneParameters = wmiZoneClass.GetMethodParameters("CreateZone"))
                {
                    createZoneParameters.SetPropertyValue("ZoneName", domainName);
                    createZoneParameters.SetPropertyValue("ZoneType", (uint)type);
                    createZoneParameters.SetPropertyValue("DsIntegrated", directoryServicesIntegrated);
                    if (dataFileName != null)
                        createZoneParameters.SetPropertyValue("DataFileName", dataFileName);
                    if (serverAddresses != null)
                        createZoneParameters.SetPropertyValue("IpAddr", serverAddresses);
                    if (adminEmailName != null)
                        createZoneParameters.SetPropertyValue("AdminEmailName", adminEmailName);

                    using (var wmiZoneReference = wmiZoneClass.InvokeMethod("CreateZone", createZoneParameters, null))
                    {
                        var wmiZonePath = $"{server.wmiScope.Path.NamespacePath}:{(string)wmiZoneReference.GetPropertyValue("RR")}";
                        var wmiZone = server.WmiGetInstance(wmiZonePath);
                        return new WindowsDnsZone(server, wmiZone);
                    }
                }
            }
            
        }

    }
}
