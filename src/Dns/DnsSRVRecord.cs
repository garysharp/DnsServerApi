using System;

namespace Dns
{
    /// <summary>
    /// Service Location (SRV) Record
    /// </summary>
    /// <remarks>
    /// Allows administrators to use several servers for a single DNS domain, to easily
    /// move a TCP/IP service from one host to another host with administration,
    /// and to designate some service provider hosts as primary servers for a service
    /// and other hosts as backups.
    /// DNS clients that use a SRV-type query ask for a specific TCP/IP service and protocol
    /// mapped to a specific DNS domain and receive the names of any available servers. (RFC 2052)
    /// </remarks>
    public class DnsSRVRecord : DnsRecord
    {
        private ushort priorityInitial;
        private ushort weightInitial;
        private ushort portInitial;
        private string targetDomainNameInitial;

        /// <summary>
        /// Priority of the this target host
        /// </summary>
        public ushort Priority { get; set; }
        /// <summary>
        /// Server selection mechanism
        /// </summary>
        public ushort Weight { get; set; }
        /// <summary>
        /// Port on this target host of this service. See <see cref="ServicePorts"/>.
        /// </summary>
        public ushort Port { get; set; }
        /// <summary>
        /// Domain name of the target host
        /// </summary>
        public string TargetDomainName { get; set; }

        /// <summary>
        /// Symbolic name of the desired service. See <see cref="ServiceNames"/>.
        /// </summary>
        public string Service => Name.Substring(0, Name.IndexOf('.'));
        /// <summary>
        /// Symbolic name of the desired protocol. See <see cref="ProtocolNames"/>.
        /// </summary>
        public string Protocol
        {
            get
            {
                var protocolStartIndex = Name.IndexOf('.') + 1;
                var protocolEndIndex =  Name.IndexOf('.', Name.IndexOf(".") +1 );
                var length = protocolEndIndex - protocolStartIndex;
                return Name.Substring(protocolStartIndex, length);
            }
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSRVRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="priority">Priority of the this target host</param>
        /// <param name="weight">Server selection mechanism</param>
        /// <param name="port">Port on this target host of this service. See <see cref="ServicePorts"/>.</param>
        /// <param name="targetDomainName">Domain name of the target host</param>
        private DnsSRVRecord(DnsZone zone, object providerState, string name, TimeSpan timeToLive,
            ushort priority, ushort weight, ushort port, string targetDomainName)
            : base(zone, providerState, name, DnsRecordTypes.SRV, DnsRecordClasses.IN, timeToLive)
        {
            var protocolStartIndex = name.IndexOf('.');
            if (protocolStartIndex < 1 || name.IndexOf('.', protocolStartIndex + 1) < 0)
                throw new ArgumentException("The name does not meet RFC2782 requirements for a SRV record", nameof(name));

            if (string.IsNullOrWhiteSpace(targetDomainName))
                throw new ArgumentNullException(nameof(targetDomainName));

            Priority = priority;
            priorityInitial = priority;

            Weight = weight;
            weightInitial = weight;

            Port = port;
            portInitial = port;

            TargetDomainName = targetDomainName;
            targetDomainNameInitial = targetDomainName;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSRVRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="priority">Priority of the this target host</param>
        /// <param name="weight">Server selection mechanism</param>
        /// <param name="port">Port on this target host of this service. See <see cref="ServicePorts"/>.</param>
        /// <param name="targetDomainName">Domain name of the target host</param>
        public DnsSRVRecord(DnsZone zone, string name, TimeSpan timeToLive,
            ushort priority, ushort weight, ushort port, string targetDomainName)
            : this(zone, providerState: null, name, timeToLive, priority, weight, port, targetDomainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSRVRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="service">Symbolic name of the desired service. See <see cref="ServiceNames"/>.</param>
        /// <param name="protocol">Symbolic name of the desired protocol. See <see cref="ProtocolNames"/>.</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="priority">Priority of the this target host</param>
        /// <param name="weight">Server selection mechanism</param>
        /// <param name="port">Port on this target host of this service. See <see cref="ServicePorts"/>.</param>
        /// <param name="targetDomainName">Domain name of the target host</param>
        public DnsSRVRecord(DnsZone zone, string service, string protocol, TimeSpan timeToLive,
            ushort priority, ushort weight, ushort port, string targetDomainName)
            : this(zone, $"{service}.{protocol}.{zone.DomainName}", timeToLive, priority, weight, port, targetDomainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSRVRecord"/>.
        /// </summary>
        /// <param name="domainName">Zone domain name</param>
        /// <param name="service">Symbolic name of the desired service. See <see cref="ServiceNames"/>.</param>
        /// <param name="protocol">Symbolic name of the desired protocol. See <see cref="ProtocolNames"/>.</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="priority">Priority of the this target host</param>
        /// <param name="weight">Server selection mechanism</param>
        /// <param name="port">Port on this target host of this service. See <see cref="ServicePorts"/>.</param>
        /// <param name="targetDomainName">Domain name of the target host</param>
        public DnsSRVRecord(string domainName, string service, string protocol, TimeSpan timeToLive,
            ushort priority, ushort weight, ushort port, string targetDomainName)
            : this(zone: null, $"{service}.{protocol}.{domainName}", timeToLive, priority, weight, port, targetDomainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSRVRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="priority">Priority of the this target host</param>
        /// <param name="weight">Server selection mechanism</param>
        /// <param name="port">Port on this target host of this service. See <see cref="ServicePorts"/>.</param>
        /// <param name="targetDomainName">Domain name of the target host</param>
        public DnsSRVRecord(string name, TimeSpan timeToLive,
            ushort priority, ushort weight, ushort port, string targetDomainName)
            : this(zone: null, name, timeToLive, priority, weight, port, targetDomainName)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
        {
            return
                priorityInitial != Priority ||
                weightInitial != Weight ||
                portInitial != Port ||
                !string.Equals(targetDomainNameInitial, TargetDomainName, StringComparison.Ordinal);
        }

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            priorityInitial = Priority;
            weightInitial = Weight;
            portInitial = Port;
            targetDomainNameInitial = TargetDomainName;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsSRVRecord(zone, providerState, Name, TimeToLive, Priority, Weight, Port, TargetDomainName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"[{Priority}][{Weight}][{Port}] {TargetDomainName}";

        /// <summary>
        /// Service Names
        /// </summary>
        public static class ServiceNames
        {
            /// <summary>
            /// Finger Service Name
            /// </summary>
            public const string Finger = "_finger";
            /// <summary>
            /// FTP Service Name
            /// </summary>
            public const string FTP = "_ftp";
            /// <summary>
            /// HTTP Service Name
            /// </summary>
            public const string HTTP = "_http";
            /// <summary>
            /// Kerberos Service Name
            /// </summary>
            public const string Kerberos = "_kerberos";
            /// <summary>
            /// LDAP Service Name
            /// </summary>
            public const string LDAP = "_ldap";
            /// <summary>
            /// MSDCS Service Name
            /// </summary>
            public const string MSDCS = "_msdcs";
            /// <summary>
            /// NNTP Service Name
            /// </summary>
            public const string NNTP = "_nntp";
            /// <summary>
            /// Telnet Service Name
            /// </summary>
            public const string Telnet = "_telnet";
            /// <summary>
            /// WhoIs Service Name
            /// </summary>
            public const string WhoIs = "_whois";
        }
        /// <summary>
        /// Service Ports
        /// </summary>
        public static class ServicePorts
        {
            /// <summary>
            /// Finger Port
            /// </summary>
            public const ushort Finger = 79;
            /// <summary>
            /// FTP Port
            /// </summary>
            public const ushort FTP = 21;
            /// <summary>
            /// HTTP Port
            /// </summary>
            public const ushort HTTP = 80;
            /// <summary>
            /// Kerberos Port
            /// </summary>
            public const ushort Kerberos = 88;
            /// <summary>
            /// LDAP Port
            /// </summary>
            public const ushort LDAP = 389;
            /// <summary>
            /// MSDCS Port
            /// </summary>
            public const ushort MSDCS = 389;
            /// <summary>
            /// NNTP Port
            /// </summary>
            public const ushort NNTP = 119;
            /// <summary>
            /// Telnet Port
            /// </summary>
            public const ushort Telnet = 23;
            /// <summary>
            /// WhoIs Port
            /// </summary>
            public const ushort WhoIs = 43;
        }
        /// <summary>
        /// Protocol Names
        /// </summary>
        public static class ProtocolNames
        {
            /// <summary>
            /// TCP Protocol
            /// </summary>
            public const string TCP = "_tcp";
            /// <summary>
            /// UDP Protocol
            /// </summary>
            public const string UDP = "_udp";
        }

    }
}
