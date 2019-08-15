using System;

namespace Dns
{
    /// <summary>
    /// Pointer Record
    /// </summary>
    public class DnsPTRRecord : DnsRecord
    {
        /// <summary>
        /// Pointer Domain Name
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsPTRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string domainName)
            : base(zone, name, DnsRecordTypes.PTR, @class, timeToLive)
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
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        public DnsPTRRecord(string name, TimeSpan timeToLive, string domainName)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, domainName)
        {
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <returns>A record clone</returns>
        public override DnsRecord Clone(DnsZone zone)
            => new DnsPTRRecord(zone, Name, Class, TimeToLive, DomainName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => DomainName;

    }
}
