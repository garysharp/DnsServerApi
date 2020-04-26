using System;
using System.Collections.Generic;
using System.Management;

namespace Dns.WindowsDnsServer
{
    /// <summary>
    /// Windows DNS Zone
    /// </summary>
    public class WindowsDnsZone : DnsZone
    {
        internal readonly WindowsDnsServer server;
        internal readonly string wmiPath;
        internal readonly ManagementBaseObject wmiZone;

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal WindowsDnsZone(WindowsDnsServer server, ManagementBaseObject wmiZone)
            : base(server, (string)wmiZone["ContainerName"], (DnsZoneType)(uint)wmiZone["ZoneType"], (bool)wmiZone["Reverse"])
        {
            this.server = server;
            wmiPath = (string)wmiZone["__PATH"];
            this.wmiZone = wmiZone;
        }

        /// <summary>
        /// Specifies the aging and scavenging behavior of the zone. Zero indicates scavenging is disabled.
        /// When scavenging is disabled, the timestamps of records in the zone are not refreshed, and the
        /// records are not subjected to scavenging. When set to one, records are subjected to scavenging and
        /// their timestamps are refreshed when the server receives the dynamic update request for the records.
        /// For Active Directory-integrated zones, this value is set to the DefaultAgingState property of the
        /// DNS Server where the zone is created. For standard primary zones, the default value is zero.
        /// </summary>
        public bool Aging => (bool)wmiZone[nameof(Aging)];

        /// <summary>
        /// Indicates whether the Zone accepts dynamic update requests.
        /// </summary>
        public AllowedDynamicUpdates AllowUpdate => (AllowedDynamicUpdates)wmiZone[nameof(AllowUpdate)];

        /// <summary>
        /// Indicates whether the Zone is autocreated.
        /// </summary>
        public bool AutoCreated => (bool)wmiZone[nameof(AutoCreated)];

        /// <summary>
        /// Specifies the time when the server may attempt scavenging the zone. Even if the zone is configured
        /// to enable scavenging the DNS server will not attempt scavenging this zone until after this moment.
        /// This value is set to the current time plus the Refresh Interval of the zone whenever the zone is loaded.
        /// This parameter is stored locally, and is not replicated to other copies of the zone.
        /// </summary>
        public TimeSpan AvailForScavengeTime => TimeSpan.FromHours((int)(uint)wmiZone.GetPropertyValue(nameof(AvailForScavengeTime)));

        /// <summary>
        /// Indicates the name of the zone file.
        /// </summary>
        public string DataFile => (string)wmiZone[nameof(DataFile)];

        /// <summary>
        /// Specifies whether the zone is DS integrated.
        /// </summary>
        public bool DsIntegrated => (bool)wmiZone[nameof(DsIntegrated)];

        /// <summary>
        /// Indicates whether the DNS uses recursion when resolving the names for the specified forward zone.
        /// Applicable to Conditional Forwarding zones only.
        /// </summary>
        public bool ForwarderSlave => (bool)wmiZone[nameof(ForwarderSlave)];

        /// <summary>
        /// Indicates the time, in seconds, a DNS server forwarding a query for the name under the forward zone waits for
        /// resolution from the forwarder before attempting to resolve the query itself. This parameter is applicable to the Forward zones only.
        /// </summary>
        public TimeSpan ForwarderTimeout => TimeSpan.FromSeconds((int)(uint)wmiZone.GetPropertyValue(nameof(ForwarderTimeout)));

        /// <summary>
        /// UTC time the SOA serial number for the zone was last checked.
        /// </summary>
        public DateTime LastSuccessfulSoaCheck => epoch.AddSeconds((int)(uint)wmiZone.GetPropertyValue(nameof(LastSuccessfulSoaCheck)));

        /// <summary>
        /// UTC time the zone was last transferred from a master server.
        /// </summary>
        public DateTime LastSuccessfulXfr => epoch.AddSeconds((int)(uint)wmiZone.GetPropertyValue(nameof(LastSuccessfulXfr)));

        /// <summary>
        /// Local IP addresses of the master DNS servers for this zone. If set, these masters over-ride the MasterServers found in Active Directory.
        /// </summary>
        public IEnumerable<string> LocalMasterServers => (string[])wmiZone.GetPropertyValue(nameof(LocalMasterServers));

        /// <summary>
        /// IP addresses of the master DNS servers for this zone.
        /// </summary>
        public IEnumerable<string> MasterServers => (string[])wmiZone.GetPropertyValue(nameof(MasterServers));

        /// <summary>
        /// Specifies the time interval between the last update of a record's timestamp and the earliest moment when the timestamp can be refreshed.
        /// </summary>
        public TimeSpan NoRefreshInterval => TimeSpan.FromHours((int)(uint)wmiZone.GetPropertyValue(nameof(NoRefreshInterval)));

        /// <summary>
        /// Array of strings enumerating IP addresses of DNS servers to be notified of changes in this zone.
        /// </summary>
        public IEnumerable<string> NotifyServers => (string[])wmiZone.GetPropertyValue(nameof(NotifyServers));

        /// <summary>
        /// Indicates whether the Zone is paused.
        /// </summary>
        public bool Paused => (bool)wmiZone[nameof(Paused)];

        /// <summary>
        /// Specifies the refresh interval, during which the records with nonzero timestamp are expected to be refreshed to remain in the zone.
        /// Records that have not been refreshed by expiration of the Refresh interval could be removed by the next scavenging performed by a server.
        /// This value should never be less than the longest refresh period of the records registered in the zone. Values that are too small may lead
        /// to removal of valid DNS records. values that are too large prolong the lifetime of stale records. This value is set to the DefaultRefreshInterval
        /// property of the DNS server where the zone is created.
        /// </summary>
        public TimeSpan RefreshInterval => TimeSpan.FromHours((int)(uint)wmiZone.GetPropertyValue(nameof(RefreshInterval)));

        /// <summary>
        /// Array of strings that enumerates the list of IP addresses of DNS servers that are allowed to perform scavenging of stale records of this zone.
        /// If the list is not specified, any primary DNS server authoritative for the zone is allowed to scavenge the zone when other prerequisites are met.
        /// </summary>
        public IEnumerable<string> ScavengeServers => (string[])wmiZone.GetPropertyValue(nameof(ScavengeServers));

        /// <summary>
        /// Array of strings enumerating IP addresses of DNS servers allowed to receive this zone through zone replication.
        /// </summary>
        public IEnumerable<string> SecondaryServers => (string[])wmiZone.GetPropertyValue(nameof(SecondaryServers));

        /// <summary>
        /// Indicates whether zone transfer is allowed to designated secondaries only.
        /// Designated secondaries are DNS Servers whose IP addresses are listed in SecondariesIPAddressesArray.
        /// </summary>
        public SecondarySecurity SecureSecondaries => (SecondarySecurity)wmiZone[nameof(SecureSecondaries)];

        /// <summary>
        /// Indicates whether the copy of the Zone is expired. If TRUE, the Zone is expired, or shut down.
        /// </summary>
        public bool Shutdown => (bool)wmiZone[nameof(Shutdown)];

        /// <summary>
        /// Indicates whether the Zone uses NBStat reverse lookup.
        /// </summary>
        public bool UseNBStat => (bool)wmiZone[nameof(UseNBStat)];

        /// <summary>
        /// Indicates whether the Zone uses WINS look up.
        /// </summary>
        public bool UseWins => (bool)wmiZone[nameof(UseWins)];

        /// <summary>
        /// Retrieves all DNS records
        /// </summary>
        /// <returns>List of DNS records</returns>
        public override IEnumerable<DnsRecord> GetRecords()
            => this.GetRecordsInternal();

        /// <summary>
        /// Retrieves DNS records by record type
        /// </summary>
        /// <param name="recordType">Type of records to be retrieved</param>
        /// <returns>List of DNS records</returns>
        public override IEnumerable<DnsRecord> GetRecords(DnsRecordTypes recordType)
            => this.GetRecordsInternal(recordType);

        /// <summary>
        /// Retrieves DNS records by record type and name
        /// </summary>
        /// <param name="recordType">Type of records to be retrieved</param>
        /// <param name="name">Owner name</param>
        /// <returns>List of DNS records</returns>
        public override IEnumerable<DnsRecord> GetRecords(DnsRecordTypes recordType, string name)
            => this.GetRecordsInternal(recordType, name);

        /// <summary>
        /// Retrieves DNS records by name
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <returns>List of DNS records</returns>
        public override IEnumerable<DnsRecord> GetRecords(string name)
            => this.GetRecordsInternal(name);

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
        /// To be called by the provider when the changes to a record are saved
        /// </summary>
        /// <param name="record">Record that was saved</param>
        internal void SavedRecordInternal(DnsRecord record)
            => SavedRecord(record);

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

        /// <summary>
        /// Adds a DNS record to the zone
        /// </summary>
        /// <param name="recordTemplate">Template record to be added</param>
        /// <returns>Resulting created record</returns>
        public override DnsRecord CreateRecord(DnsRecord recordTemplate)
        {
            if (recordTemplate == null)
                throw new ArgumentNullException(nameof(recordTemplate));

            return this.CreateRecordInternal(recordTemplate);
        }

        /// <summary>
        /// Removes a DNS record from the zone
        /// </summary>
        /// <param name="record">Record to be removed</param>
        public override void DeleteRecord(DnsRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            this.DeleteRecordInternal(record);
        }

        /// <summary>
        /// Updates a DNS record
        /// </summary>
        /// <param name="record">True if the record was updated, otherwise false</param>
        public override void SaveRecord(DnsRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            this.ModifyRecordInternal(record);
        }

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            wmiZone.Dispose();
        }
    }
}
