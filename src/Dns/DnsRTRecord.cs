using System;

namespace Dns
{
    /// <summary>
    /// Route Through (RT) Record
    /// </summary>
    /// <remarks>
    /// Provides an intermediate-route-through binding for internal hosts that do not have their
    /// own direct wide area network (WAN) address. This record uses the same data format as the
    /// MX record type to indicate two required fields: a 16-bit integer that represents preference
    /// for each intermediate route and the DNS domain name for the route-through host as it appears
    /// elsewhere in an A, X25, or ISDN record for the zone. (RFC 1183)
    /// </remarks>
    public class DnsRTRecord : DnsRecord
    {
        private ushort preferenceInitial;
        private string intermediateHostInitial;

        /// <summary>
        /// The preference given to this RR among others at the same owner.  Lower values are preferred.
        /// </summary>
        public ushort Preference { get; set; }
        /// <summary>
        /// A Fully Qualified Domain Name which specifies a host which will serve as an intermediate in reaching the host specified by owner.
        /// </summary>
        public string IntermediateHost { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">The preference given to this RR among others at the same owner.  Lower values are preferred.</param>
        /// <param name="intermediateHost">A Fully Qualified Domain Name which specifies a host which will serve as an intermediate in reaching the host specified by owner.</param>
        private DnsRTRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort preference, string intermediateHost)
            : base(zone, providerState, name, DnsRecordTypes.RT, @class, timeToLive)
        {
            Preference = preference;
            preferenceInitial = preference;

            IntermediateHost = intermediateHost;
            intermediateHostInitial = intermediateHost;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">The preference given to this RR among others at the same owner.  Lower values are preferred.</param>
        /// <param name="intermediateHost">A Fully Qualified Domain Name which specifies a host which will serve as an intermediate in reaching the host specified by owner.</param>
        public DnsRTRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort preference, string intermediateHost)
            : this(zone, providerState: null, name, @class, timeToLive, preference, intermediateHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRTRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">The preference given to this RR among others at the same owner.  Lower values are preferred.</param>
        /// <param name="intermediateHost">A Fully Qualified Domain Name which specifies a host which will serve as an intermediate in reaching the host specified by owner.</param>
        public DnsRTRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort preference, string intermediateHost)
            : this(zone: null, name, @class, timeToLive, preference, intermediateHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">The preference given to this RR among others at the same owner.  Lower values are preferred.</param>
        /// <param name="intermediateHost">A Fully Qualified Domain Name which specifies a host which will serve as an intermediate in reaching the host specified by owner.</param>
        public DnsRTRecord(DnsZone zone, string name, TimeSpan timeToLive, ushort preference, string intermediateHost)
            : this(zone, name, DnsRecordClasses.IN, timeToLive, preference, intermediateHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRTRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">The preference given to this RR among others at the same owner.  Lower values are preferred.</param>
        /// <param name="intermediateHost">A Fully Qualified Domain Name which specifies a host which will serve as an intermediate in reaching the host specified by owner.</param>
        public DnsRTRecord(string name, TimeSpan timeToLive, ushort preference, string intermediateHost)
            : this(zone: null, name, timeToLive, preference, intermediateHost)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => preferenceInitial != Preference ||
                !string.Equals(intermediateHostInitial, IntermediateHost, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            preferenceInitial = Preference;
            intermediateHostInitial = IntermediateHost;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsRTRecord(zone, providerState, Name, Class, TimeToLive, Preference, IntermediateHost);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"[{Preference}] {IntermediateHost}";

    }
}
