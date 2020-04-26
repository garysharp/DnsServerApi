using System;

namespace Dns
{
    /// <summary>
    /// Alias/Canonical Name (CNAME) Record
    /// </summary>
    /// <remarks>
    ///  Indicates an alternate or alias DNS domain name for a name already specified in
    ///  other resource record types used in this zone. (RFC 1035)
    /// </remarks>
    public class DnsCNAMERecord : DnsRecord
    {
        private string primaryNameInitial;

        /// <summary>
        /// Primary Name for <see cref="DnsRecord.Name"/>
        /// </summary>
        public string PrimaryName { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsCNAMERecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        private DnsCNAMERecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string primaryName)
            : base(zone, providerState, name, DnsRecordTypes.CNAME, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(primaryName))
                throw new ArgumentNullException(nameof(primaryName));

            PrimaryName = primaryName;
            primaryNameInitial = primaryName;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsCNAMERecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        public DnsCNAMERecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string primaryName)
            : this(zone, providerState: null, name, @class, timeToLive, primaryName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsCNAMERecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        public DnsCNAMERecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string primaryName)
            : this(zone: null, name, @class, timeToLive, primaryName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsCNAMERecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        public DnsCNAMERecord(DnsZone zone, string name, TimeSpan timeToLive, string primaryName)
            : this(zone, name, DnsRecordClasses.IN, timeToLive, primaryName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsCNAMERecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        public DnsCNAMERecord(string name, TimeSpan timeToLive, string primaryName)
            : this(zone: null, name, timeToLive, primaryName)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(primaryNameInitial, PrimaryName, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            primaryNameInitial = PrimaryName;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsCNAMERecord(zone, providerState, Name, Class, TimeToLive, PrimaryName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => PrimaryName;

    }
}
