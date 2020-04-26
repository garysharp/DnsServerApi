using System;

namespace Dns
{
    /// <summary>
    /// Windows Internet Name Service (WINS) Record
    /// </summary>
    /// <remarks>
    /// Obsolete - Present for compatibility.
    /// See documentation: https://docs.microsoft.com/en-us/windows/win32/api/windns/ns-windns-dns_wins_data
    /// </remarks>
    [Obsolete]
    public class DnsWINSRecord : DnsRecord
    {
        private uint mappingFlagInitial;
        private uint lookupTimeoutInitial;
        private uint cacheTimeoutInitial;
        private string winsServersInitial;

        /// <summary>
        /// A WINS mapping flag that specifies whether the record must be included into the zone replication.
        /// It may have only two values: 0x80000000 and 0x00010000 corresponding to the replication and no-replication (local record) flags, respectively.
        /// </summary>
        public uint MappingFlag { get; set; }
        /// <summary>
        /// How long (in seconds) a DNS server, using WINS Lookup, waits before giving up.
        /// </summary>
        public uint LookupTimeout { get; set; }
        /// <summary>
        /// How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.
        /// </summary>
        public uint CacheTimeout { get; set; }
        /// <summary>
        /// A comma separated list of IP addresses of WINS servers to be addressed in a WINS Lookups.
        /// </summary>
        public string WinsServers { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINS mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="winsServers">A comma separated list of IP addresses of WINS servers to be addressed in a WINS Lookups.</param>
        private DnsWINSRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string winsServers)
            : base(zone, providerState, name, DnsRecordTypes.WINS, @class, timeToLive)
        {
            MappingFlag = mappingFlag;
            mappingFlagInitial = mappingFlag;

            LookupTimeout = lookupTimeout;
            lookupTimeoutInitial = lookupTimeout;

            CacheTimeout = cacheTimeout;
            cacheTimeoutInitial = cacheTimeout;

            WinsServers = winsServers;
            winsServersInitial = winsServers;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINS mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="winsServers">A comma separated list of IP addresses of WINS servers to be addressed in a WINS Lookups.</param>
        public DnsWINSRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string winsServers)
            : this(zone, providerState: null, name, @class, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, winsServers)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINS mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="winsServers">A comma separated list of IP addresses of WINS servers to be addressed in a WINS Lookups.</param>
        public DnsWINSRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string winsServers)
            : this(zone: null, name, @class, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, winsServers)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINS mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="winsServers">A comma separated list of IP addresses of WINS servers to be addressed in a WINS Lookups.</param>
        public DnsWINSRecord(DnsZone zone, string name, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string winsServers)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, winsServers)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINS mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="winsServers">A comma separated list of IP addresses of WINS servers to be addressed in a WINS Lookups.</param>
        public DnsWINSRecord(string name, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string winsServers)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, winsServers)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
        {
            return
                mappingFlagInitial != MappingFlag ||
                lookupTimeoutInitial != LookupTimeout ||
                cacheTimeoutInitial != CacheTimeout ||
                !string.Equals(winsServersInitial, WinsServers, StringComparison.Ordinal);
        }

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            mappingFlagInitial = MappingFlag;
            lookupTimeoutInitial = LookupTimeout;
            cacheTimeoutInitial = CacheTimeout;
            winsServersInitial = WinsServers;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsWINSRecord(zone, providerState, Name, Class, TimeToLive, MappingFlag, LookupTimeout, CacheTimeout, WinsServers);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{WinsServers} [{MappingFlag}; {LookupTimeout}; {CacheTimeout}]";

    }
}
