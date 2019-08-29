using System;
using System.Collections.Generic;
using System.Management;
using System.Security;

namespace Dns.WindowsDnsServer
{
    public class WindowsDnsServer : DnsServer
    {
        private readonly ConnectionOptions connectionOptions;

        internal ManagementScope Scope { get; }
        public override IEnumerable<DnsZone> Zones => GetZones();

        private WindowsDnsServer(string domainName, ConnectionOptions connectionOptions)
            :base(domainName)
        {
            this.connectionOptions = connectionOptions;
            Scope = new ManagementScope($@"\\{domainName}\ROOT\MicrosoftDNS", connectionOptions);
            Scope.Connect();
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

        private IEnumerable<DnsZone> GetZones()
        {
            var query = new ObjectQuery("SELECT ContainerName FROM MicrosoftDNS_Zone");
            using (var searcher = new ManagementObjectSearcher(Scope, query))
            {
                using (var results = searcher.Get())
                {
                    foreach (var result in results)
                    {
                        yield return new WindowsDnsZone(this, (string)result["ContainerName"]);
                    }
                }
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
            throw new NotImplementedException();
        }

        public override void DeleteZone(string domainName)
        {
            throw new NotImplementedException();
        }

        public override DnsZone GetZone(string domainName)
        {
            throw new NotImplementedException();
        }
    }
}
