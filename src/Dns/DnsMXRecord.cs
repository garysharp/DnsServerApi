using System;

namespace Dns
{
    /// <summary>
    /// Mail Exchange (MX) Record
    /// </summary>
    /// <remarks>
    /// Provides message routing to a specified mail exchange host that is acting as a
    /// mail exchanger for a specified DNS domain name. MX records use a 16-bit integer to
    /// indicate host priority in message routing where multiple mail exchange hosts are specified.
    /// For each mail exchange host specified in this record type, a corresponding host
    /// address (A) type record is needed. (RFC 1035)
    /// </remarks>
    public class DnsMXRecord : DnsRecord
    {
        private ushort preferenceInitial;
        private string domainNameInitial;

        /// <summary>
        /// Preference given to this record
        /// </summary>
        public ushort Preference { get; set; }

        /// <summary>
        /// Mail Exchange Domain Name
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMXRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">Preference given to this record</param>
        /// <param name="domainName">Mail Exchange Domain Name</param>
        private DnsMXRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort preference, string domainName)
            : base(zone, providerState, name, DnsRecordTypes.MX, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(domainName);

            Preference = preference;
            preferenceInitial = preference;

            DomainName = domainName;
            domainNameInitial = domainName;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMXRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">Preference given to this record</param>
        /// <param name="domainName">Mail Exchange Domain Name</param>
        public DnsMXRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort preference, string domainName)
            : this(zone, providerState: null, name, @class, timeToLive, preference, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMXRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">Preference given to this record</param>
        /// <param name="domainName">Mail Exchange Domain Name</param>
        public DnsMXRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort preference, string domainName)
            : this(zone: null, name, @class, timeToLive, preference, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMXRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">Preference given to this record</param>
        /// <param name="domainName">Mail Exchange Domain Name</param>
        public DnsMXRecord(DnsZone zone, string name, TimeSpan timeToLive, ushort preference, string domainName)
            : this(zone, name, DnsRecordClasses.IN, timeToLive, preference, domainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMXRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">Preference given to this record</param>
        /// <param name="domainName">Mail Exchange Domain Name</param>
        public DnsMXRecord(string name, TimeSpan timeToLive, ushort preference, string domainName)
            : this(zone: null, name, timeToLive, preference, domainName)
        {
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsMXRecord(zone, providerState, Name, Class, TimeToLive, Preference, DomainName);

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => preferenceInitial != Preference ||
                !string.Equals(domainNameInitial, DomainName, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            preferenceInitial = Preference;
            domainNameInitial = DomainName;
        }

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"[{Preference}] {DomainName}";

    }
}
