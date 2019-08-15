using System;

namespace Dns
{
    /// <summary>
    /// Canonical Name (Alias) Record
    /// </summary>
    public class DnsCNAMERecord : DnsRecord
    {
        /// <summary>
        /// Primary Name for <see cref="DnsRecord.Name"/>
        /// </summary>
        public string PrimaryName { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsCNAMERecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        public DnsCNAMERecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string primaryName)
            : base(zone, name, DnsRecordTypes.CNAME, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(primaryName))
                throw new ArgumentNullException(nameof(primaryName));

            PrimaryName = primaryName;
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
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        public DnsCNAMERecord(string name, TimeSpan timeToLive, string primaryName)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, primaryName)
        {
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <returns>A record clone</returns>
        public override DnsRecord Clone(DnsZone zone)
            => new DnsCNAMERecord(zone, Name, Class, TimeToLive, PrimaryName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => PrimaryName;

    }
}
