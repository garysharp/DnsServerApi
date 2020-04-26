using System;

namespace Dns
{
    /// <summary>
    /// Well Known Services (WKS) Record
    /// </summary>
    /// <remarks>
    /// Describes the well-known TCP/IP services supported by a particular protocol on a particular IP address.
    /// WKS records provide TCP and UDP availability information for TCP/IP servers. If a server supports both
    /// TCP and UDP for a well-known service or if the server has multiple IP addresses that support a service,
    /// multiple WKS records are used. (RFC 1035)
    /// </remarks>
    [Obsolete("RFC1123 RFC1127")]
    public class DnsWKSRecord : DnsRecord
    {
        private string ipProtocolInitial;
        private DnsIpAddress internetAddressInitial;
        private string servicesInitial;

        /// <summary>
        /// A string representing the IP protocol for this record. Values included 'udp' or 'tcp'.
        /// </summary>
        public string IpProtocol { get; set; }
        /// <summary>
        /// A IPv4 Internet address for the record's owner.
        /// </summary>
        public DnsIpAddress InternetAddress { get; set; }
        /// <summary>
        /// A string that contains all the services used by the Well Known Service (WKS) record.
        /// </summary>
        public string Services { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWKSRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipProtocol">A string representing the IP protocol for this record. Values included 'udp' or 'tcp'.</param>
        /// <param name="internetAddress">A IPv4 Internet address for the record's owner.</param>
        /// <param name="services">A string that contains all the services used by the Well Known Service (WKS) record.</param>
        private DnsWKSRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string ipProtocol, DnsIpAddress internetAddress, string services)
            : base(zone, providerState, name, DnsRecordTypes.WKS, @class, timeToLive)
        {
            InternetAddress = internetAddress;
            internetAddressInitial = internetAddress;

            IpProtocol = ipProtocol;
            ipProtocolInitial = ipProtocol;

            Services = services;
            servicesInitial = services;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWKSRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipProtocol">A string representing the IP protocol for this record. Values included 'udp' or 'tcp'.</param>
        /// <param name="internetAddress">A IPv4 Internet address for the record's owner.</param>
        /// <param name="services">A string that contains all the services used by the Well Known Service (WKS) record.</param>
        public DnsWKSRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string ipProtocol, DnsIpAddress internetAddress, string services)
            : this(zone, providerState: null, name, @class, timeToLive, ipProtocol, internetAddress, services)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWKSRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipProtocol">A string representing the IP protocol for this record. Values included 'udp' or 'tcp'.</param>
        /// <param name="internetAddress">A IPv4 Internet address for the record's owner.</param>
        /// <param name="services">A string that contains all the services used by the Well Known Service (WKS) record.</param>
        public DnsWKSRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string ipProtocol, DnsIpAddress internetAddress, string services)
            : this(zone: null, name, @class, timeToLive, ipProtocol, internetAddress, services)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWKSRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipProtocol">A string representing the IP protocol for this record. Values included 'udp' or 'tcp'.</param>
        /// <param name="internetAddress">A IPv4 Internet address for the record's owner.</param>
        /// <param name="services">A string that contains all the services used by the Well Known Service (WKS) record.</param>
        public DnsWKSRecord(DnsZone zone, string name, TimeSpan timeToLive, string ipProtocol, DnsIpAddress internetAddress, string services)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, ipProtocol, internetAddress, services)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsWKSRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipProtocol">A string representing the IP protocol for this record. Values included 'udp' or 'tcp'.</param>
        /// <param name="internetAddress">A IPv4 Internet address for the record's owner.</param>
        /// <param name="services">A string that contains all the services used by the Well Known Service (WKS) record.</param>
        public DnsWKSRecord(string name, TimeSpan timeToLive, string ipProtocol, DnsIpAddress internetAddress, string services)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, ipProtocol, internetAddress, services)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
        {
            return
                !string.Equals(ipProtocolInitial, IpProtocol, StringComparison.Ordinal) ||
                internetAddressInitial != InternetAddress ||
                !string.Equals(servicesInitial, Services, StringComparison.Ordinal);
        }

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            ipProtocolInitial = IpProtocol;
            internetAddressInitial = InternetAddress;
            servicesInitial = Services;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsWKSRecord(zone, providerState, Name, Class, TimeToLive, IpProtocol, InternetAddress, Services);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{InternetAddress}; {IpProtocol}; {Services}";

    }
}
