using System;
using System.Collections.Generic;
using System.Linq;

namespace Dns.MockDnsServer
{
    /// <summary>
    /// Mock DNS Server Zone
    /// </summary>
    public class MockDnsZone : DnsZone
    {
        private readonly List<DnsRecord> records;

        /// <inheritdoc/>
        public MockDnsZone(MockDnsServer server, string domainName, DnsZoneType type, bool isReverseZone)
            : base(server, domainName, type, isReverseZone)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            var records = new List<DnsRecord>();

            // add mock SOA record
            var soaRecord = new DnsSOARecord(
                zone: this,
                name: domainName,
                @class: DnsRecordClasses.IN,
                timeToLive: TimeSpan.FromHours(1),
                primaryServer: server.DomainName,
                responsiblePerson: "hostmaster.",
                serial: 1,
                refreshInterval: TimeSpan.FromMinutes(15),
                retryDelay: TimeSpan.FromMinutes(10),
                expireLimit: TimeSpan.FromDays(1),
                minimumTimeToLive: TimeSpan.FromHours(1));
            records.Add(soaRecord);

            // add mock NS record
            var nsRecord = new DnsNSRecord(this, domainName, DnsRecordClasses.IN, TimeSpan.FromHours(1), server.DomainName);
            records.Add(nsRecord);

            this.records = records;
        }

        /// <inheritdoc/>
        public override IEnumerable<DnsRecord> GetRecords() => records;

        /// <inheritdoc/>
        public override DnsRecord CreateRecord(DnsRecord recordTemplate)
        {
            if (recordTemplate == null)
                throw new ArgumentNullException(nameof(recordTemplate));

            if (recordTemplate.Type == DnsRecordTypes.SOA)
                throw new ArgumentException("Cannot create SOA records. Edit the existing record instead.");

            var record = CloneRecord(recordTemplate, providerState: null);
            records.Add(record);

            return record;
        }

        /// <inheritdoc/>
        public override void SaveRecord(DnsRecord record)
        {
            // this mock implementation tracks by-reference records, so no action is required to save
            if (!ReferenceEquals(record.Zone, this) || !records.Any(r => ReferenceEquals(r, record)))
                throw new ArgumentException("Record is not part of this zone");
        }

        /// <inheritdoc/>
        public override void DeleteRecord(DnsRecord record)
        {
            var itemIndex = records.IndexOf(record);

            if (itemIndex < 0)
                throw new ArgumentException("Record is not part of this zone");

            records.RemoveAt(itemIndex);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                records.Clear();
        }
       
    }
}
