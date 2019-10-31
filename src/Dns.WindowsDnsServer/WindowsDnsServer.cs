using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security;

namespace Dns.WindowsDnsServer
{
    public class WindowsDnsServer : DnsServer
    {
        private readonly ConnectionOptions connectionOptions;

        public string Name { get; }
        internal ManagementPath Path { get; }
        internal ManagementScope Scope { get; }

        public override IEnumerable<DnsZone> Zones => GetZones();
        public IEnumerable<WindowsDnsZone> WindowsZones => GetZones();

        private WindowsDnsServer(string domainName, ConnectionOptions connectionOptions)
            : base(domainName)
        {
            this.connectionOptions = connectionOptions;
            Scope = new ManagementScope($@"\\{domainName}\ROOT\MicrosoftDNS", connectionOptions);
            Scope.Connect();

            var server = this.WmiGetInstances("MicrosoftDNS_Server").FirstOrDefault();

            if (server == null)
                throw new ArgumentException("DNS Server instance not found", nameof(domainName));

            Path = new ManagementPath((string)server["__PATH"]);
            Name = (string)server.Properties["Name"].Value;
        }

        public static WindowsDnsServer Connect(string domainName)
        {
            // default connection options (impersonate callee)
            var options = new ConnectionOptions();
            return new WindowsDnsServer(domainName, options);
        }

        public static WindowsDnsServer Connect(string domainName, string username, string password)
            => Connect(domainName, authority: null, username, password);

        public static WindowsDnsServer Connect(string domainName, string authority, string username, string password)
        {
            var options = new ConnectionOptions()
            {
                Username = username,
                Password = password,
                Authority = authority,
            };
            return new WindowsDnsServer(domainName, options);
        }

        public static WindowsDnsServer Connect(string domainName, string username, SecureString securePassword)
            => Connect(domainName, authority: null, username, securePassword);

        public static WindowsDnsServer Connect(string domainName, string authority, string username, SecureString securePassword)
        {
            var options = new ConnectionOptions()
            {
                Username = username,
                SecurePassword = securePassword,
                Authority = authority,
            };
            return new WindowsDnsServer(domainName, options);
        }

        private IEnumerable<WindowsDnsZone> GetZones()
        {
            foreach (var result in this.WmiGetInstances("MicrosoftDNS_Zone"))
            {
                yield return new WindowsDnsZone(this, result);
            }
        }

        public override DnsZone CreateZone(DnsZone zoneTemplate)
        {
            throw new NotImplementedException();
        }

        public override DnsZone CreateZone(string domainName)
        {
            throw new NotImplementedException();
        }

        public override void DeleteZone(DnsZone zone)
        {
            var windowsZone = zone as WindowsDnsZone;

            if (zone == null)
                throw new ArgumentException("Zone must be created by the provider");

            this.DeleteZoneInternal(windowsZone);
        }

        public override void DeleteZone(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            var zone = this.GetZoneInternal(domainName);
            this.DeleteZoneInternal(zone);
        }

        public override DnsZone GetZone(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            return this.GetZoneInternal(domainName);
        }

    }
}
