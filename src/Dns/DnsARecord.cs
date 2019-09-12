using System;

namespace Dns
{
    /// <summary>
    /// Host Record
    /// </summary>
    public class DnsARecord : DnsRecord
    {
        /// <summary>
        /// IP address of the Host (A) record
        /// </summary>
        public DnsIpAddress IpAddress { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipAddress">IP address of the Host (A) record</param>
        public DnsARecord(DnsZone zone, object providerState, string name, TimeSpan timeToLive, DnsIpAddress ipAddress)
            : base(zone, providerState, name, DnsRecordTypes.A, DnsRecordClasses.IN, timeToLive)
        {
            IpAddress = ipAddress;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipAddress">IP address of the Host (A) record</param>
        public DnsARecord(DnsZone zone, string name, TimeSpan timeToLive, DnsIpAddress ipAddress)
            : this(zone, providerState: null, name, timeToLive, ipAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsARecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipAddress">IP address of the Host (A) record</param>
        public DnsARecord(string name, TimeSpan timeToLive, DnsIpAddress ipAddress)
            : this(zone: null, name, timeToLive, ipAddress)
        {
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        public override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsARecord(zone, providerState, Name, TimeToLive, IpAddress);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => IpAddress.ToString();

    }
}
