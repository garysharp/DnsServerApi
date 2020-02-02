using System;

namespace Dns
{
    /// <summary>
    /// Integrated Services Digital Network (ISDN) Record
    /// </summary>
    /// <remarks>
    /// Maps a DNS domain name to an ISDN telephone number. ISDN telephone numbers used
    /// with this record meet CCITT E.163/E.164 international telephone numbering standards. (RFC 1183)
    /// </remarks>
    public class DnsISDNRecord : DnsRecord
    {
        /// <summary>
        /// The ISDN number and DDI of the record's owner.
        /// </summary>
        public string ISDNNumber { get; set; }
        /// <summary>
        /// The subaddress of the owner, if defined.
        /// </summary>
        public string SubAddress { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsISDNRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="isdnNumber">The ISDN number and DDI of the record's owner.</param>
        /// <param name="subAddress">The subaddress of the owner, if defined.</param>
        private DnsISDNRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string isdnNumber, string subAddress)
            : base(zone, providerState, name, DnsRecordTypes.ISDN, @class, timeToLive)
        {
            ISDNNumber = isdnNumber;
            SubAddress = subAddress;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsISDNRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="isdnNumber">The ISDN number and DDI of the record's owner.</param>
        /// <param name="subAddress">The subaddress of the owner, if defined.</param>
        public DnsISDNRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string isdnNumber, string subAddress)
            : this(zone, providerState: null, name, @class, timeToLive, isdnNumber, subAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsISDNRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="isdnNumber">The ISDN number and DDI of the record's owner.</param>
        /// <param name="subAddress">The subaddress of the owner, if defined.</param>
        public DnsISDNRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string isdnNumber, string subAddress)
            : this(zone: null, name, @class, timeToLive, isdnNumber, subAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsISDNRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="isdnNumber">The ISDN number and DDI of the record's owner.</param>
        /// <param name="subAddress">The subaddress of the owner, if defined.</param>
        public DnsISDNRecord(DnsZone zone, string name, TimeSpan timeToLive, string isdnNumber, string subAddress)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, isdnNumber, subAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsISDNRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="isdnNumber">The ISDN number and DDI of the record's owner.</param>
        /// <param name="subAddress">The subaddress of the owner, if defined.</param>
        public DnsISDNRecord(string name, TimeSpan timeToLive, string isdnNumber, string subAddress)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, isdnNumber, subAddress)
        {
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsISDNRecord(zone, providerState, Name, Class, TimeToLive, ISDNNumber, SubAddress);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{ISDNNumber} {SubAddress}";

    }
}
