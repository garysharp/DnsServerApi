using System;

namespace Dns
{
    /// <summary>
    /// X.25 (X25) Record
    /// </summary>
    /// <remarks>
    /// Maps a DNS domain name to a Public Switched Data Network (PSDN) address,
    /// such as X.121 addresses, which are typically used to identify
    /// each point of service located on a public X.25 network. (RFC 1183)
    /// </remarks>
    public class DnsX25Record : DnsRecord
    {
        private string psdnAddressInitial;

        /// <summary>
        /// PSDN Address
        /// </summary>
        public string PsdnAddress { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsX25Record"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="psdnAddress">PSDN Address</param>
        private DnsX25Record(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string psdnAddress)
            : base(zone, providerState, name, DnsRecordTypes.X25, @class, timeToLive)
        {
            PsdnAddress = psdnAddress;
            psdnAddressInitial = psdnAddress;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsX25Record"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="psdnAddress">PSDN Address</param>
        public DnsX25Record(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string psdnAddress)
            : this(zone, providerState: null, name, @class, timeToLive, psdnAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsX25Record"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="psdnAddress">PSDN Address</param>
        public DnsX25Record(string name, DnsRecordClasses @class, TimeSpan timeToLive, string psdnAddress)
            : this(zone: null, name, @class, timeToLive, psdnAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsX25Record"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="psdnAddress">PSDN Address</param>
        public DnsX25Record(DnsZone zone, string name, TimeSpan timeToLive, string psdnAddress)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, psdnAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsX25Record"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="psdnAddress">PSDN Address</param>
        public DnsX25Record(string name, TimeSpan timeToLive, string psdnAddress)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, psdnAddress)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(psdnAddressInitial, PsdnAddress, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            psdnAddressInitial = PsdnAddress;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsX25Record(zone, providerState, Name, Class, TimeToLive, PsdnAddress);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => PsdnAddress;

    }
}
