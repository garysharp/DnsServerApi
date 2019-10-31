using System;
using System.Collections.Generic;
using System.Management;

namespace Dns.WindowsDnsServer
{
    public class WindowsDnsZone : DnsZone
    {
        internal readonly WindowsDnsServer server;
        internal readonly string wmiPath;

        /// <summary>
        /// Physical DNS zone file name
        /// </summary>
        public string DataFile { get; }
        /// <summary>
        /// Indicates if the zone data is stored in Active Directory
        /// </summary>
        public bool ActiveDirectoryIntegrated { get; }

        public override IEnumerable<DnsRecord> Records => GetRecords();

        internal WindowsDnsZone(WindowsDnsServer server, ManagementBaseObject wmiZone)
            : base(server, (string)wmiZone["ContainerName"], (DnsZoneType)(uint)wmiZone["ZoneType"], (bool)wmiZone["Reverse"])
        {
            this.server = server;
            wmiPath = (string)wmiZone["__PATH"];
            DataFile = (string)wmiZone["DataFile"];
            ActiveDirectoryIntegrated = (bool)wmiZone["DsIntegrated"];
        }

        /// <summary>
        /// Attaches state to a record allowing providers to keep track of records
        /// </summary>
        /// <param name="record"></param>
        /// <param name="providerState"></param>
        internal void SetRecordStateInternal(DnsRecord record, WindowsDnsRecordState providerState)
            => SetRecordState(record, providerState);

        /// <summary>
        /// Retrieves the provider state set on the record
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        internal WindowsDnsRecordState GetRecordStateInternal(DnsRecord record) => GetRecordState(record) as WindowsDnsRecordState;

        /// <summary>
        /// Clones the record associating it with the provider state
        /// </summary>
        /// <param name="record">Record to be cloned</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal DnsRecord CloneRecordInternal(DnsRecord record, WindowsDnsRecordState providerState)
            => CloneRecord(record, providerState);

        /// <summary>
        /// Clones the record, including the provider state
        /// </summary>
        /// <param name="record">Record to be cloned</param>
        /// <returns>A record clone</returns>
        internal DnsRecord CloneRecordInternal(DnsRecord record)
            => CloneRecord(record);

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

            this.DeleteRecordInternal(record);
        }

        public override void SaveRecord(DnsRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            this.ModifyRecordInternal(record);
        }

        public override IEnumerable<DnsRecord> GetRecords(DnsRecordTypes recordType)
        {
            return this.GetRecordsInternal(recordType);
        }
    }
}
