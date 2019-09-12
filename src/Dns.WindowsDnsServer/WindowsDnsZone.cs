using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Dns.WindowsDnsServer
{
    public class WindowsDnsZone : DnsZone
    {
        internal readonly WindowsDnsServer server;

        public override IEnumerable<DnsRecord> Records => GetRecords();

        internal WindowsDnsZone(WindowsDnsServer server, ManagementBaseObject wmiZone)
            : base(server, (string)wmiZone["ContainerName"])
        {
            this.server = server;
        }

        private IEnumerable<DnsRecord> GetRecords()
        {
            foreach (var result in server.WmiQuery($"SELECT * FROM MicrosoftDNS_ResourceRecord WHERE ContainerName='{DomainName}'"))
            {
                var record = default(DnsRecord);

                try
                {
                    record = this.ParseRecordInternal(result);
                }
                catch (NotSupportedException)
                {
                    // TODO: Support all Microsoft DNS record types

                    // skip records which are not supported by the library
                    continue;
                }

                yield return record;
            }
        }

        public override DnsSOARecord StartOfAuthority
        {
            get
            {
                var wmiResult = server.WmiQuery($"SELECT * FROM MicrosoftDNS_SOAType WHERE ContainerName='{DomainName}'").FirstOrDefault();

                if (wmiResult == null)
                    throw new Exception("Record not found");

                return (DnsSOARecord)this.ParseRecordInternal(wmiResult);
            }
        }

        public override DnsRecord CreateRecord(DnsRecord recordTemplate)
        {
            if (recordTemplate == null)
                throw new ArgumentNullException(nameof(recordTemplate));

            return this.CreateRecordInternal(recordTemplate);
        }

        public override void DeleteRecord(DnsRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            if (record.ProviderState == null)
                throw new ArgumentException("Record must be created by the provider");

            if (!(record.ProviderState is WindowsDnsRecordState state))
                throw new ArgumentException("Record must be created by the provider");

            var instance = server.WmiGetInstance(state.WmiPath);

            instance.Delete();
        }

        public override void SaveRecord(DnsRecord record)
        {
            throw new NotImplementedException();
        }
    }
}
