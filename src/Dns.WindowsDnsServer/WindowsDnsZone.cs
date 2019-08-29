using System;
using System.Collections.Generic;
using System.Management;

namespace Dns.WindowsDnsServer
{
    public class WindowsDnsZone : DnsZone
    {
        public override IEnumerable<DnsRecord> Records => GetRecords();

        internal WindowsDnsZone(WindowsDnsServer server, string domainName)
            : base(server, domainName)
        {
        }

        private IEnumerable<DnsRecord> GetRecords()
        {
            var query = new ObjectQuery($"SELECT OwnerName, RecordClass, RecordData, TTL FROM MicrosoftDNS_ResourceRecord WHERE DomainName='{DomainName}'");
            using (var searcher = new ManagementObjectSearcher(((WindowsDnsServer)Server).Scope, query))
            {
                using (var results = searcher.Get())
                {
                    foreach (var result in results)
                    {
                        var wmiClass = (string)result["__CLASS"];
                        var ownerName = (string)result["OwnerName"];
                        var recordClass = (ushort)result["RecordClass"];
                        var recordData = (string)result["RecordData"];
                        var ttl = (uint)result["TTL"];
                        var record = default(DnsRecord);

                        try
                        {
                            record = WindowsDnsRecordHelpers.ParseRecord(this, wmiClass, ownerName, recordClass, ttl, recordData);
                        }
                        catch (NotSupportedException)
                        {
                            // skip records which are not supported by the library
                            continue;
                        }

                        yield return record;
                    }
                }
            }
        }

        public override DnsRecord CreateRecord(DnsRecord recordTemplate)
        {
            throw new NotImplementedException();
        }

        public override void DeleteRecord(DnsRecord record)
        {
            throw new NotImplementedException();
        }

        public override void SaveRecord(DnsRecord record)
        {
            throw new NotImplementedException();
        }
    }
}
