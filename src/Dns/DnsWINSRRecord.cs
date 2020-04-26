using System;

namespace Dns
{
    /// <summary>
    /// Windows Internet Name Service reverse-lookup (WINSR) Record
    /// </summary>
    /// <remarks>
    /// Obsolete - Present for compatibility.
    /// See documentation: https://docs.microsoft.com/en-us/windows/win32/api/windns/ns-windns-dns_winsr_dataw
    /// </remarks>
    [Obsolete]
    public class DnsWINSRRecord : DnsRecord
    {
        private uint mappingFlagInitial;
        private uint lookupTimeoutInitial;
        private uint cacheTimeoutInitial;
        private string resultDomainInitial;

        /// <summary>
        /// A WINSR mapping flag that specifies whether the record must be included into the zone replication.
        /// It may have only two values: 0x80000000 and 0x00010000 corresponding to the replication and no-replication (local record) flags, respectively.
        /// </summary>
        public uint MappingFlag { get; set; }
        /// <summary>
        /// How long (in seconds) a DNS server, using WINS Reverse Lookup, waits before giving up.
        /// </summary>
        public uint LookupTimeout { get; set; }
        /// <summary>
        /// How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.
        /// </summary>
        public uint CacheTimeout { get; set; }
        /// <summary>
        /// A domain name to append to returned NetBIOS names.
        /// </summary>
        public string ResultDomain { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINSR mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Reverse Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="resultDomain">A domain name to append to returned NetBIOS names.</param>
        private DnsWINSRRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string resultDomain)
            : base(zone, providerState, name, DnsRecordTypes.WINSR, @class, timeToLive)
        {
            MappingFlag = mappingFlag;
            mappingFlagInitial = mappingFlag;

            LookupTimeout = lookupTimeout;
            lookupTimeoutInitial = lookupTimeout;

            CacheTimeout = cacheTimeout;
            cacheTimeoutInitial = cacheTimeout;

            ResultDomain = resultDomain;
            resultDomainInitial = resultDomain;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINSR mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Reverse Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="resultDomain">A domain name to append to returned NetBIOS names.</param>
        public DnsWINSRRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string resultDomain)
            : this(zone, providerState: null, name, @class, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, resultDomain)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINSR mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Reverse Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="resultDomain">A domain name to append to returned NetBIOS names.</param>
        public DnsWINSRRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string resultDomain)
            : this(zone: null, name, @class, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, resultDomain)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINSR mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Reverse Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="resultDomain">A domain name to append to returned NetBIOS names.</param>
        public DnsWINSRRecord(DnsZone zone, string name, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string resultDomain)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, resultDomain)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWINSRRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mappingFlag">A WINSR mapping flag that specifies whether the record must be included into the zone replication.</param>
        /// <param name="lookupTimeout">How long (in seconds) a DNS server, using WINS Reverse Lookup, waits before giving up.</param>
        /// <param name="cacheTimeout">How long (in seconds) a DNS server, using WINS Lookup, may cache the WINS server's response.</param>
        /// <param name="resultDomain">A domain name to append to returned NetBIOS names.</param>
        public DnsWINSRRecord(string name, TimeSpan timeToLive, uint mappingFlag, uint lookupTimeout, uint cacheTimeout, string resultDomain)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, mappingFlag, lookupTimeout, cacheTimeout, resultDomain)
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
                !string.Equals(resultDomainInitial, ResultDomain, StringComparison.Ordinal);
        }

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            mappingFlagInitial = MappingFlag;
            lookupTimeoutInitial = LookupTimeout;
            cacheTimeoutInitial = CacheTimeout;
            resultDomainInitial = ResultDomain;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsWINSRRecord(zone, providerState, Name, Class, TimeToLive, MappingFlag, LookupTimeout, CacheTimeout, ResultDomain);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{ResultDomain} [{MappingFlag}; {LookupTimeout}; {CacheTimeout}]";

    }
}
