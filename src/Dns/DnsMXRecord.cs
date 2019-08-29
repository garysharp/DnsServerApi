using System;

namespace Dns
{
    /// <summary>
    /// Mail Exchange Record
    /// </summary>
    public class DnsMXRecord : DnsRecord
    {
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
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">Preference given to this record</param>
        /// <param name="domainName">Mail Exchange Domain Name</param>
        public DnsMXRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort preference, string domainName)
            : base(zone, name, DnsRecordTypes.MX, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(domainName);

            Preference = preference;
            DomainName = domainName;
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
        /// <returns>A record clone</returns>
        public override DnsRecord Clone(DnsZone zone)
            => new DnsMXRecord(zone, Name, Class, TimeToLive, Preference, DomainName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"[{Preference}] {DomainName}";

    }
}
