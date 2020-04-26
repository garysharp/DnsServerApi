using System;

namespace Dns
{
    /// <summary>
    /// Pointer (PTR) Record
    /// </summary>
    /// <remarks>
    /// Points to a location in the domain name space. PTR records are typically used
    /// in special domains to perform reverse lookups of address-to-name mappings.
    /// Each record provides simple data that points to some other location in the
    /// domain name space (usually a forward lookup zone). Where PTR records are used,
    /// no additional section processing is implied or caused by their presence. (RFC 1035)
    /// </remarks>
    public class DnsPTRRecord : DnsRecord
    {
        private string domainNameInitial;

        /// <summary>
        /// Pointer Domain Name
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Pointer Address
        /// </summary>
        public DnsIpAddress Address
        {
            get => DnsIpAddress.FromPTRAddress(Name);
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        private DnsPTRRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string domainName)
            : base(zone, providerState, name, DnsRecordTypes.PTR, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            DomainName = domainName;
            domainNameInitial = domainName;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string domainName)
            : this(zone, providerState: null, name, @class, timeToLive, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="address">Pointer address</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(DnsZone zone, DnsIpAddress address, DnsRecordClasses @class, TimeSpan timeToLive, string domainName)
            : base(zone, $"{address.GetFlipped()}.in-addr.arpa", DnsRecordTypes.PTR, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            DomainName = domainName;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string domainName)
            : this(zone: null, name, @class, timeToLive, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="address">Pointer address</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(DnsIpAddress address, DnsRecordClasses @class, TimeSpan timeToLive, string domainName)
            : this(zone: null, $"{address.GetFlipped()}.in-addr.arpa", @class, timeToLive, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(DnsZone zone, string name, TimeSpan timeToLive, string domainName)
            : this(zone, name, DnsRecordClasses.IN, timeToLive, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="address">Pointer address</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(DnsZone zone, DnsIpAddress address, TimeSpan timeToLive, string domainName)
            : this(zone, address, DnsRecordClasses.IN, timeToLive, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(string name, TimeSpan timeToLive, string domainName)
            : this(zone: null, name, timeToLive, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="address">Pointer address</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(DnsIpAddress address, TimeSpan timeToLive, string domainName)
            : this(zone: null, $"{address.GetFlipped()}.in-addr.arpa", timeToLive, domainName)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(domainNameInitial, DomainName, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            domainNameInitial = DomainName;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsPTRRecord(zone, providerState, Name, Class, TimeToLive, DomainName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => DomainName;

    }
}
