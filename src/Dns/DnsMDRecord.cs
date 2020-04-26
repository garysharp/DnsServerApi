using System;

namespace Dns
{
    /// <summary>
    /// Mail Destination (MD) Record
    /// </summary>
    /// <remarks>
    /// Obsolete by RFC 973 - use Mail Exchange (MX) Record. Present for compatibility.
    /// </remarks>
    [Obsolete("Use DnsMXRecord; RFC 973")]
    public class DnsMDRecord : DnsRecord
    {
        private string mdHostInitial;

        /// <summary>
        /// A Fully Qualified Domain Name which specifies a host which has a mail agent which should be able to deliver mail for the specified domain.
        /// </summary>
        public string MDHost { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMDRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mdHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which should be able to deliver mail for the specified domain.</param>
        private DnsMDRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mdHost)
            : base(zone, providerState, name, DnsRecordTypes.MD, @class, timeToLive)
        {
            MDHost = mdHost;
            mdHostInitial = mdHost;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMDRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mdHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which should be able to deliver mail for the specified domain.</param>
        public DnsMDRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mdHost)
            : this(zone, providerState: null, name, @class, timeToLive, mdHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMDRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mdHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which should be able to deliver mail for the specified domain.</param>
        public DnsMDRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string mdHost)
            : this(zone: null, name, @class, timeToLive, mdHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMDRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mdHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which should be able to deliver mail for the specified domain.</param>
        public DnsMDRecord(DnsZone zone, string name, TimeSpan timeToLive, string mdHost)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, mdHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMDRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mdHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which should be able to deliver mail for the specified domain.</param>
        public DnsMDRecord(string name, TimeSpan timeToLive, string mdHost)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, mdHost)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(mdHostInitial, MDHost, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            mdHostInitial = MDHost;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsMDRecord(zone, providerState, Name, Class, TimeToLive, MDHost);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => MDHost;

    }
}
