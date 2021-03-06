﻿using System;
using System.Collections.Generic;

namespace Dns.MockDnsServer
{
    /// <summary>
    /// Mock DNS Server Provider
    /// </summary>
    public class MockDnsServer : DnsServer
    {
        private readonly List<MockDnsZone> zones = new List<MockDnsZone>();

        /// <inheritdoc/>
        public MockDnsServer()
            : base("in-memory.server.mock.")
        {
        }

        /// <inheritdoc/>
        public override IEnumerable<DnsZone> GetZones() => zones;

        /// <inheritdoc/>
        public override DnsZone CreateZone(DnsZone zoneTemplate)
        {
            if (zoneTemplate == null)
                throw new ArgumentNullException(nameof(zoneTemplate));

            if (zones.Exists(z => string.Equals(zoneTemplate.DomainName, z.DomainName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("A zone with that name already exists", nameof(zoneTemplate.DomainName));

            var zone = new MockDnsZone(this, zoneTemplate.DomainName, zoneTemplate.Type, zoneTemplate.IsReverseZone);

            zones.Add(zone);

            return zone;
        }

        /// <inheritdoc/> 
        public override DnsZone CreateZone(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            var zoneTemplate = new MockDnsZone(this, domainName, DnsZoneType.Primary, false);

            return CreateZone(zoneTemplate);
        }

        /// <inheritdoc/>
        public override void DeleteZone(DnsZone zone)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            DeleteZone(zone.DomainName);
        }

        /// <inheritdoc/>
        public override void DeleteZone(string domainName)
        {
            if (domainName == null)
                throw new ArgumentNullException(nameof(domainName));

            var zoneIndex = zones.FindIndex(z => string.Equals(z.DomainName, domainName, StringComparison.OrdinalIgnoreCase));

            if (zoneIndex < 0)
                throw new ArgumentException("The zone was not found");

            zones.RemoveAt(zoneIndex);
        }

        /// <inheritdoc/>
        public override DnsZone GetZone(string domainName)
        {
            var zone = zones.Find(z => string.Equals(domainName, z.DomainName, StringComparison.OrdinalIgnoreCase));

            if (zone == null)
                throw new ArgumentException("No zone with that name exists", nameof(domainName));

            return zone;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var zone in zones)
                    zone.Dispose();

                zones.Clear();
            }
        }

    }
}
