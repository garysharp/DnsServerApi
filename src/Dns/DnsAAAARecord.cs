using System;

namespace Dns
{
    /// <summary>
    /// IPv6 Host Address (AAAA) Record
    /// </summary>
    /// <remarks>
    /// Maps a DNS domain name to a 128-bit IP version 6 address (RFC 1886). 
    /// </remarks>
    public class DnsAAAARecord : DnsRecord
    {
        private string ipV6AddressInitial;

        /// <summary>
        /// IP address of the Host (A) record
        /// </summary>
        public string IpV6Address { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAAAARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipV6Address">IPv6 address of the Host (A) record</param>
        private DnsAAAARecord(DnsZone zone, object providerState, string name, TimeSpan timeToLive, string ipV6Address)
            : base(zone, providerState, name, DnsRecordTypes.AAAA, DnsRecordClasses.IN, timeToLive)
        {
            IpV6Address = ipV6Address;
            ipV6AddressInitial = ipV6Address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAAAARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipV6Address">IPv6 address of the Host (A) record</param>
        public DnsAAAARecord(DnsZone zone, string name, TimeSpan timeToLive, string ipV6Address)
            : this(zone, providerState: null, name, timeToLive, ipV6Address)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAAAARecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipV6Address">IPv6 address of the Host (A) record</param>
        public DnsAAAARecord(string name, TimeSpan timeToLive, string ipV6Address)
            : this(zone: null, name, timeToLive, ipV6Address)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(ipV6AddressInitial, IpV6Address, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            ipV6AddressInitial = IpV6Address;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsAAAARecord(zone, providerState, Name, TimeToLive, IpV6Address);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => IpV6Address;

    }
}
