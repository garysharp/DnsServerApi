using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Security;

namespace Dns.WindowsDnsServer
{
    /// <summary>
    /// Windows DNS Server
    /// </summary>
    public class WindowsDnsServer : DnsServer
    {
        private readonly ConnectionOptions connectionOptions;
        internal readonly ManagementScope wmiScope;
        internal readonly ManagementObject wmiServer;

        private WindowsDnsServer(string domainName, ConnectionOptions connectionOptions)
            : base(domainName)
        {
            this.connectionOptions = connectionOptions;
            wmiScope = new ManagementScope($@"\\{domainName}\ROOT\MicrosoftDNS", connectionOptions);
            wmiScope.Connect();

            wmiServer = (ManagementObject)this.WmiGetInstances("MicrosoftDNS_Server").FirstOrDefault();

            if (wmiServer == null)
                throw new ArgumentException("DNS Server instance not found", nameof(domainName));

            Name = (string)wmiServer.Properties["Name"].Value;
        }

        /// <summary>
        /// Maximum number of host records returned in response to an address request. Values between 5 and 28 are valid.
        /// </summary>
        public int AddressAnswerLimit
        {
            get => (int)(uint)wmiServer.GetPropertyValue(nameof(AddressAnswerLimit));
            set
            {
                if (value < 5 && value > 28)
                    throw new ArgumentOutOfRangeException(nameof(value), "AddressAnswerLimit must be between 5 and 28");
                wmiServer.SetPropertyValue(nameof(AddressAnswerLimit), (uint)value);
            }
        }

        /// <summary>
        /// Specifies whether the DNS Server accepts dynamic update requests.
        /// </summary>
        public UpdateOptions AllowUpdate
        {
            get => (UpdateOptions)wmiServer.GetPropertyValue(nameof(AllowUpdate));
            set => wmiServer.SetPropertyValue(nameof(AllowUpdate), (uint)value);
        }

        /// <summary>
        /// Indicates whether the DNS Server attempts to update its cache entries using data from root servers.
        /// When a DNS Server boots, it needs a list of root server 'hints' NS and A records for the servers
        /// historically called the cache file. Microsoft DNS Servers have a feature that enables them to attempt
        /// to write back a new cache file based on the responses from root servers.
        /// </summary>
        public bool AutoCacheUpdate
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(AutoCacheUpdate));
            set => wmiServer.SetPropertyValue(nameof(AutoCacheUpdate), value);
        }

        /// <summary>
        /// Indicates which standard primary zones that are authoritative for the name of the DNS Server must be updated when the name server changes.
        /// </summary>
        public AutoConfigFileZones AutoConfigFileZones
        {
            get => (AutoConfigFileZones)wmiServer.GetPropertyValue(nameof(AutoConfigFileZones));
            set => wmiServer.SetPropertyValue(nameof(AutoConfigFileZones), (uint)value);
        }

        /// <summary>
        /// Determines the AXFR message format when sending to non-Microsoft DNS Server secondaries.
        /// When set to TRUE, the DNS Server sends transfers to non-Microsoft DNS Server secondaries in the uncompressed format.
        /// When FALSE, all transfers are sent in the fast format.
        /// </summary>
        public bool BindSecondaries
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(BindSecondaries));
            set => wmiServer.SetPropertyValue(nameof(BindSecondaries), value);
        }

        /// <summary>
        /// Initialization method for the DNS Server.
        /// </summary>
        public BootMethods BootMethod
        {
            get => (BootMethods)wmiServer.GetPropertyValue(nameof(BootMethod));
            set => wmiServer.SetPropertyValue(nameof(BootMethod), (uint)value);
        }

        /// <summary>
        /// Default ScavengingInterval value set for all Active Directory-integrated zones created on this DNS Server.
        /// The default value is zero, indicating scavenging is disabled.
        /// </summary>
        public bool DefaultAgingState
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(DefaultAgingState));
            set => wmiServer.SetPropertyValue(nameof(DefaultAgingState), value);
        }

        /// <summary>
        /// No-refresh interval, in hours, set for all Active Directory-integrated zones created on this DNS Server. The default value is 168 hours (seven days).
        /// </summary>
        public TimeSpan DefaultNoRefreshInterval
        {
            get => TimeSpan.FromHours((int)(uint)wmiServer.GetPropertyValue(nameof(DefaultNoRefreshInterval)));
            set => wmiServer.SetPropertyValue(nameof(DefaultNoRefreshInterval), (uint)value.TotalHours);
        }

        /// <summary>
        /// Refresh interval, in hours, set for all Active Directory-integrated zones created on this DNS Server. The default value is 168 hours (seven days).
        /// </summary>
        public TimeSpan DefaultRefreshInterval
        {
            get => TimeSpan.FromHours((int)(uint)wmiServer.GetPropertyValue(nameof(DefaultRefreshInterval)));
            set => wmiServer.SetPropertyValue(nameof(DefaultRefreshInterval), (uint)value.TotalHours);
        }

        /// <summary>
        /// Indicates whether the DNS Server automatically creates standard reverse look up zones.
        /// </summary>
        public bool DisableAutoReverseZones
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(DisableAutoReverseZones));
            set => wmiServer.SetPropertyValue(nameof(DisableAutoReverseZones), value);
        }

        /// <summary>
        /// Indicates whether the default port binding for a socket used to send queries to remote DNS Servers can be overridden.
        /// </summary>
        public bool DisjointNets
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(DisjointNets));
            set => wmiServer.SetPropertyValue(nameof(DisjointNets), value);
        }

        /// <summary>
        /// Indicates whether there is an available DS on the DNS Server.
        /// </summary>
        public bool DsAvailable
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(DsAvailable));
            set => wmiServer.SetPropertyValue(nameof(DsAvailable), value);
        }

        /// <summary>
        /// Interval, in seconds, to poll the DS-integrated zones.
        /// </summary>
        public TimeSpan DsPollingInterval
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(DsPollingInterval)));
            set => wmiServer.SetPropertyValue(nameof(DsPollingInterval), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Lifetime of tombstoned records in Directory Service integrated zones, expressed in seconds.
        /// </summary>
        public TimeSpan DsTombstoneInterval
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(DsTombstoneInterval)));
            set => wmiServer.SetPropertyValue(nameof(DsTombstoneInterval), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Lifetime, in seconds, of the cached information describing the EDNS version supported by other DNS Servers.
        /// </summary>
        public TimeSpan EDnsCacheTimeout
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(EDnsCacheTimeout)));
            set => wmiServer.SetPropertyValue(nameof(EDnsCacheTimeout), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Specifies whether support for application directory partitions is enabled on the DNS Server.
        /// </summary>
        public bool EnableDirectoryPartitions
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(EnableDirectoryPartitions));
            set => wmiServer.SetPropertyValue(nameof(EnableDirectoryPartitions), value);
        }

        /// <summary>
        /// Specifies whether the DNS Server includes DNSSEC-specific RRs, KEY, SIG, and NXT in a response.
        /// </summary>
        public DnsSecEnableModes EnableDnsSec
        {
            get => (DnsSecEnableModes)wmiServer.GetPropertyValue(nameof(EnableDnsSec));
            set => wmiServer.SetPropertyValue(nameof(EnableDnsSec), (uint)value);
        }

        /// <summary>
        /// Specifies the behavior of the DNS Server. When TRUE, the DNS Server always responds with OPT resource records
        /// according to RFC 2671, unless the remote server has indicated it does not support EDNS in a prior exchange.
        /// If FALSE, the DNS Server responds to queries with OPTs only if OPTs are sent in the original query.
        /// </summary>
        public bool EnableEDnsProbes
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(EnableEDnsProbes));
            set => wmiServer.SetPropertyValue(nameof(EnableEDnsProbes), value);
        }

        /// <summary>
        /// Indicates which events the DNS Server records in the Event Viewer system log.
        /// </summary>
        public EventLogLevels EventLogLevel
        {
            get => (EventLogLevels)wmiServer.GetPropertyValue(nameof(EventLogLevel));
            set => wmiServer.SetPropertyValue(nameof(EventLogLevel), (uint)value);
        }

        /// <summary>
        /// Specifies whether queries to delegated sub-zones are forwarded.
        /// </summary>
        public bool ForwardDelegations
        {
            get => (int)(uint)wmiServer.GetPropertyValue(nameof(ForwardDelegations)) != 0;
            set => wmiServer.SetPropertyValue(nameof(ForwardDelegations), value ? 1 : 0);
        }

        /// <summary>
        /// Enumerates the list of IP addresses of Forwarders to which the DNS Server forwards queries
        /// </summary>
        public IEnumerable<string> Forwarders
        {
            get => (IEnumerable<string>)wmiServer.GetPropertyValue(nameof(Forwarders));
            set => wmiServer.SetPropertyValue(nameof(Forwarders), value.ToArray());
        }

        /// <summary>
        /// Time, in seconds, a DNS Server forwarding a query will wait for resolution from the forwarder before attempting to resolve the query itself.
        /// This value is meaningless if the forwarding server is not set to use recursion.To determine this, check the IsSlave property.
        /// </summary>
        public TimeSpan ForwardingTimeout
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(ForwardingTimeout)));
            set => wmiServer.SetPropertyValue(nameof(ForwardingTimeout), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// TRUE if the DNS server does not use recursion when name-resolution through forwarders fails.
        /// </summary>
        public bool IsSlave
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(IsSlave));
            set => wmiServer.SetPropertyValue(nameof(IsSlave), value);
        }

        /// <summary>
        /// Enumerates the list of IP addresses on which the DNS Server can receive queries.
        /// </summary>
        public IEnumerable<string> ListenAddresses
        {
            get => (IEnumerable<string>)wmiServer.GetPropertyValue(nameof(ListenAddresses));
            set => wmiServer.SetPropertyValue(nameof(ListenAddresses), value.ToArray());
        }

        /// <summary>
        /// Indicates whether the DNS Server gives priority to the local net address when returning A records.
        /// </summary>
        public bool LocalNetPriority
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(LocalNetPriority));
            set => wmiServer.SetPropertyValue(nameof(LocalNetPriority), value);
        }

        /// <summary>
        /// Size of the DNS Server debug log, in bytes.
        /// </summary>
        public uint LogFileMaxSize
        {
            get => (uint)wmiServer.GetPropertyValue(nameof(LogFileMaxSize));
            set => wmiServer.SetPropertyValue(nameof(LogFileMaxSize), value);
        }

        /// <summary>
        /// File name and path for the DNS Server debug log. Default is %system32%\dns\dns.log.
        /// Relative paths are relative to %Systemroot%\System32. Absolute paths may be used, but UNC paths are not supported.
        /// </summary>
        public string LogFilePath
        {
            get => (string)wmiServer.GetPropertyValue(nameof(LogFilePath));
            set => wmiServer.SetPropertyValue(nameof(LogFilePath), value);
        }

        /// <summary>
        /// List of IP addresses used to filter DNS events written to the debug log.
        /// </summary>
        public IEnumerable<string> LogIPFilterList
        {
            get => (IEnumerable<string>)wmiServer.GetPropertyValue(nameof(LogIPFilterList));
            set => wmiServer.SetPropertyValue(nameof(LogIPFilterList), value.ToArray());
        }

        /// <summary>
        /// Indicates which policies are activated in the Event Viewer system log.
        /// </summary>
        public LogLevels LogLevel
        {
            get => (LogLevels)wmiServer.GetPropertyValue(nameof(LogLevel));
            set => wmiServer.SetPropertyValue(nameof(LogLevel), (uint)value);
        }

        /// <summary>
        /// Indicates whether the DNS Server performs loose wildcarding. If undefined or zero, the server follows
        /// the wildcarding behavior specified in the DNS RFC. In this case, an administrator is advised to include
        /// MX records for all hosts incapable of receiving mail. If nonzero, the server seeks the closest wildcard node;
        /// in this case, an administrator should put MX records at the zone root and in a wildcard node ('*') directly
        /// below the zone root. Also, administrators should put self-referencing MX records on hosts that receive their own mail.
        /// </summary>
        public bool LooseWildcarding
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(LooseWildcarding));
            set => wmiServer.SetPropertyValue(nameof(LooseWildcarding), value);
        }

        /// <summary>
        /// Maximum time, in seconds, the record of a recursive name query may remain in the DNS Server cache.
        /// The DNS Server deletes records from the cache when the value of this entry expires, even if the value of the TTL field in the record is greater.
        /// </summary>
        public TimeSpan MaxCacheTTL
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(MaxCacheTTL)));
            set => wmiServer.SetPropertyValue(nameof(MaxCacheTTL), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Maximum time, in seconds, a name error result from a recursive query may remain in the DNS Server cache.
        /// DNS deletes records from the cache when this timer expires, even if the TTL field is greater. Default value is 86,400 (one day).
        /// </summary>
        public TimeSpan MaxNegativeCacheTTL
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(MaxCacheTTL)));
            set => wmiServer.SetPropertyValue(nameof(MaxCacheTTL), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Server Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Indicates the set of eligible characters to be used in DNS names.
        /// </summary>
        public NameCheckFlags NameCheckFlag
        {
            get => (NameCheckFlags)wmiServer.GetPropertyValue(nameof(NameCheckFlag));
            set => wmiServer.SetPropertyValue(nameof(NameCheckFlag), (uint)value);
        }

        /// <summary>
        /// Indicates whether the DNS Server performs recursive look ups. TRUE indicates recursive look ups are not performed.
        /// </summary>
        public bool NoRecursion
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(NoRecursion));
            set => wmiServer.SetPropertyValue(nameof(NoRecursion), value);
        }

        /// <summary>
        /// Elapsed seconds before retrying a recursive look up. If the property is undefined or zero, retries are made after three seconds.
        /// Users are discouraged from altering this property. There are certain situations when the property should be changed;
        /// one example is when the DNS Server contacts remote servers over a slow link, and the DNS Server is retrying before receiving
        /// response from the remote DNS. In this case, raising the time out to be slightly longer than the observed response time from
        /// the remote DNS would be reasonable.
        /// </summary>
        public TimeSpan RecursionRetry
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(RecursionRetry)));
            set => wmiServer.SetPropertyValue(nameof(RecursionRetry), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Elapsed seconds before the DNS Server gives up recursive query. If the property is undefined or zero, the DNS Server gives up after 15 seconds.
        /// In general, the 15-second time out is sufficient to allow any outstanding response to get back to the DNS Server.
        /// 
        /// Users are discouraged from altering this property.One scenario where the property should be changed is when the DNS Server contacts remote servers
        /// over a slow link, and the DNS Server is observed rejecting queries (with SERVER_FAILURE) before responses are received.
        /// 
        /// Client resolvers also retry queries, so careful investigation is required to determine whether remote responses are actually associated with the
        /// query that timed out. In this case, raising the time out value to be slightly longer than the observed response time from the remote DNS would be reasonable.
        /// </summary>
        public TimeSpan RecursionTimeout
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(RecursionTimeout)));
            set => wmiServer.SetPropertyValue(nameof(RecursionTimeout), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Indicates whether the DNS Server round robins multiple A records.
        /// </summary>
        public bool RoundRobin
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(RoundRobin));
            set => wmiServer.SetPropertyValue(nameof(RoundRobin), value);
        }

        /// <summary>
        /// RPC protocol or protocols over which administrative RPC runs.
        /// </summary>
        public RpcProtocols RpcProtocol
        {
            get => (RpcProtocols)wmiServer.GetPropertyValue(nameof(RpcProtocol));
            set => wmiServer.SetPropertyValue(nameof(RpcProtocol), (uint)value);
        }

        /// <summary>
        /// Interval, in hours, between two consecutive scavenging operations performed by the DNS Server.
        /// Zero indicates scavenging is disabled. The default value is 168 hours (seven days).
        /// </summary>
        public TimeSpan ScavengingInterval
        {
            get => TimeSpan.FromHours((int)(uint)wmiServer.GetPropertyValue(nameof(ScavengingInterval)));
            set => wmiServer.SetPropertyValue(nameof(ScavengingInterval), (uint)value.TotalHours);
        }

        /// <summary>
        /// Indicates whether the DNS Server exclusively saves records of names in the same subtree as the server that provided them.
        /// </summary>
        public bool SecureResponses
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(SecureResponses));
            set => wmiServer.SetPropertyValue(nameof(SecureResponses), value);
        }

        /// <summary>
        /// Port on which the DNS Server sends UDP queries to other servers. By default, the DNS Server sends queries on a socket bound to the DNS port.
        /// 
        /// Under certain situations, this is not the best configuration.One obvious case is when an administrator blocks the DNS port with a firewall
        /// to prevent outside access to the DNS Server, but still wants the server to be able to contact Internet DNS Servers to provide name resolution
        /// for internal clients.Another case is when the DNS Server is supporting disjoint nets(the property DisjointNets set to TRUE identifies this scenario).
        /// In these cases, setting the SendOnNonDnsPort property to a nonzero value directs the DNS Server to bind to an arbitrary port for sending to remote DNS Servers.
        /// 
        /// If the SendOnNonDnsPort value is greater than 1024, the DNS Server binds explicitly to the port value given.
        /// This configuration option is useful when an administrator needs to fix the DNS query port for firewall purposes.
        /// </summary>
        public int SendPort
        {
            get => (int)(uint)wmiServer.GetPropertyValue(nameof(SendPort));
            set => wmiServer.SetPropertyValue(nameof(SendPort), (uint)value);
        }

        /// <summary>
        /// Enumerates the list of IP addresses for the DNS Server.
        /// </summary>
        public IEnumerable<string> ServerAddresses => (IEnumerable<string>)wmiServer.GetPropertyValue(nameof(ServerAddresses));

        /// <summary>
        /// Indicates whether the DNS Server parses zone files strictly. If undefined or zero, the server will log and ignore bad data in the
        /// zone file and continue to load. If nonzero, the server will log and fail on zone file errors.
        /// </summary>
        public bool StrictFileParsing
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(StrictFileParsing));
            set => wmiServer.SetPropertyValue(nameof(StrictFileParsing), value);
        }

        /// <summary>
        /// Restricts the type of records that can be dynamically updated on the server, used in addition to the AllowUpdate settings on Server and Zone objects.
        /// </summary>
        public UpdateOptions UpdateOptions => (UpdateOptions)wmiServer.GetPropertyValue(nameof(UpdateOptions));

        /// <summary>
        /// Version of the DNS Server.
        /// </summary>
        public int Version => (int)(uint)wmiServer.GetPropertyValue(nameof(Version));

        /// <summary>
        /// Specifies whether the DNS Server writes NS and SOA records to the authority section on successful response.
        /// </summary>
        public bool WriteAuthorityNS
        {
            get => (bool)wmiServer.GetPropertyValue(nameof(WriteAuthorityNS));
            set => wmiServer.SetPropertyValue(nameof(WriteAuthorityNS), value);
        }

        /// <summary>
        /// Time, in seconds, the DNS Server waits for a successful TCP connection to a remote server when attempting a zone transfer.
        /// </summary>
        public TimeSpan XfrConnectTimeout
        {
            get => TimeSpan.FromSeconds((int)(uint)wmiServer.GetPropertyValue(nameof(XfrConnectTimeout)));
            set => wmiServer.SetPropertyValue(nameof(XfrConnectTimeout), (uint)value.TotalSeconds);
        }

        /// <summary>
        /// Retrieves DNS distinguished name for the zone.
        /// </summary>
        /// <returns></returns>
        public string GetDistinguishedName() => (string)wmiServer.InvokeMethod(nameof(GetDistinguishedName), new object[] { });

        /// <summary>
        /// Starts scavenging stale records in the zones subjected to scavenging.
        /// </summary>
        public void StartScavenging()
        {
            var resultCode = (int)wmiServer.InvokeMethod(nameof(StartScavenging), new object[] { });

            if (resultCode != 0)
                throw new Win32Exception(resultCode);
        }

        /// <summary>
        /// Starts the DNS Server.
        /// </summary>
        public void StartService()
        {
            var resultCode = (int)wmiServer.InvokeMethod(nameof(StartService), new object[] { });

            if (resultCode != 0)
                throw new Win32Exception(resultCode);
        }

        /// <summary>
        /// Stops the DNS Server.
        /// </summary>
        public void StopService()
        {
            var resultCode = (int)wmiServer.InvokeMethod(nameof(StopService), new object[] { });

            if (resultCode != 0)
                throw new Win32Exception(resultCode);
        }

        /// <summary>
        /// Connects to the Windows DNS Server
        /// </summary>
        /// <param name="domainName">Domain name of the DNS server</param>
        /// <returns><see cref="WindowsDnsServer"/> instance</returns>
        public static WindowsDnsServer Connect(string domainName)
        {
            // default connection options (impersonate callee)
            var options = new ConnectionOptions();
            return new WindowsDnsServer(domainName, options);
        }

        /// <summary>
        /// Connects to the Windows DNS Server
        /// </summary>
        /// <param name="domainName">Domain name of the DNS server</param>
        /// <param name="username">Username used in the connection</param>
        /// <param name="password">Password used in the connection</param>
        /// <returns><see cref="WindowsDnsServer"/> instance</returns>
        public static WindowsDnsServer Connect(string domainName, string username, string password)
            => Connect(domainName, authority: null, username, password);

        /// <summary>
        /// Connects to the Windows DNS Server
        /// </summary>
        /// <param name="domainName">Domain name of the DNS server</param>
        /// <param name="authority">Specifies the authority to use to authenticate the WMI connection. You can specify standard NTLM or Kerberos authentication. To use NTLM, set the authority setting to ntlmdomain:{DomainName}, where {DomainName} identifies a valid NTLM domain name. To use Kerberos, specify kerberos:{DomainName}\{ServerName}. You cannot include the authority setting when you connect to the local computer.</param>
        /// <param name="username">Username used in the connection</param>
        /// <param name="password">Password used in the connection</param>
        /// <returns><see cref="WindowsDnsServer"/> instance</returns>
        public static WindowsDnsServer Connect(string domainName, string authority, string username, string password)
        {
            var options = new ConnectionOptions()
            {
                Username = username,
                Password = password,
                Authority = authority,
            };
            return new WindowsDnsServer(domainName, options);
        }

        /// <summary>
        /// Connects to the Windows DNS Server
        /// </summary>
        /// <param name="domainName">Domain name of the DNS server</param>
        /// <param name="username">Username used in the connection</param>
        /// <param name="securePassword">Password used in the connection</param>
        /// <returns><see cref="WindowsDnsServer"/> instance</returns>
        public static WindowsDnsServer Connect(string domainName, string username, SecureString securePassword)
            => Connect(domainName, authority: null, username, securePassword);

        /// <summary>
        /// Connects to the Windows DNS Server
        /// </summary>
        /// <param name="domainName">Domain name of the DNS server</param>
        /// <param name="authority">Specifies the authority to use to authenticate the WMI connection. You can specify standard NTLM or Kerberos authentication. To use NTLM, set the authority setting to ntlmdomain:{DomainName}, where {DomainName} identifies a valid NTLM domain name. To use Kerberos, specify kerberos:{DomainName}\{ServerName}. You cannot include the authority setting when you connect to the local computer.</param>
        /// <param name="username">Username used in the connection</param>
        /// <param name="securePassword">Password used in the connection</param>
        /// <returns><see cref="WindowsDnsServer"/> instance</returns>
        public static WindowsDnsServer Connect(string domainName, string authority, string username, SecureString securePassword)
        {
            var options = new ConnectionOptions()
            {
                Username = username,
                SecurePassword = securePassword,
                Authority = authority,
            };
            return new WindowsDnsServer(domainName, options);
        }

        /// <summary>
        /// Retrieves all DNS zones
        /// </summary>
        /// <returns>List of DNS zones</returns>
        public override IEnumerable<DnsZone> GetZones() => this.GetZonesInternal();
        /// <summary>
        /// Retrieve DNS Zone
        /// </summary>
        /// <param name="domainName">Domain name of the zone</param>
        /// <returns></returns>
        public override DnsZone GetZone(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            return this.GetZoneInternal(domainName);
        }

        /// <summary>
        /// Creates a Zone on the DNS Server
        /// </summary>
        /// <param name="domainName">Name of the Zone</param>
        /// <param name="type">Type of the Zone</param>
        /// <param name="dsIntegrated">True if the Zone should be integrated with directory services</param>
        /// <param name="dataFileName">Name of the data file associated with the zone</param>
        /// <param name="masterDnsServerAddresses">Master DNS Server addresses for this zone (Secondary, Stub, Forwarder zone types only)</param>
        /// <param name="adminEmailName">Email address of the administrator responsible for the zone</param>
        /// <returns>Newly created zone</returns>
        public WindowsDnsZone CreateZone(string domainName, DnsZoneType type, bool dsIntegrated, string dataFileName, IEnumerable<DnsIpAddress> masterDnsServerAddresses, string adminEmailName)
        {
            CreateZoneType createType;
            switch (type)
            {
                case DnsZoneType.Primary:
                    createType = CreateZoneType.Primary;
                    break;
                case DnsZoneType.Secondary:
                case DnsZoneType.Stub:
                case DnsZoneType.Forwarder:
                    createType = (CreateZoneType)type;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), $"Unable to create a zone of this type ({type})");
            }

            return this.CreateZoneInternal(domainName, createType, dsIntegrated, dataFileName, masterDnsServerAddresses, adminEmailName);
        }

        /// <summary>
        /// Creates a Zone on the DNS Server
        /// </summary>
        /// <param name="domainName">Name of the Zone</param>
        /// <param name="type">Type of the Zone</param>
        /// <param name="dsIntegrated">True if the Zone should be integrated with directory services</param>
        /// <param name="masterDnsServerAddresses">Master DNS Server addresses for this zone (Secondary, Stub, Forwarder zone types only)</param>
        /// <param name="adminEmailName">Email address of the administrator responsible for the zone</param>
        /// <returns>Newly created zone</returns>
        public WindowsDnsZone CreateZone(string domainName, DnsZoneType type, bool dsIntegrated, IEnumerable<DnsIpAddress> masterDnsServerAddresses, string adminEmailName)
            => CreateZone(domainName, type, dsIntegrated, dataFileName: null, masterDnsServerAddresses, adminEmailName);

        /// <summary>
        /// Creates a Zone on the DNS Server
        /// </summary>
        /// <param name="domainName">Name of the Zone</param>
        /// <param name="type">Type of the Zone</param>
        /// <param name="dsIntegrated">True if the Zone should be integrated with directory services</param>
        /// <param name="adminEmailName">Email address of the administrator responsible for the zone</param>
        /// <returns>Newly created zone</returns>
        public WindowsDnsZone CreateZone(string domainName, DnsZoneType type, bool dsIntegrated, string adminEmailName)
            => CreateZone(domainName, type, dsIntegrated, dataFileName: null, masterDnsServerAddresses: null, adminEmailName);

        /// <summary>
        /// Creates a Zone on the DNS Server
        /// </summary>
        /// <param name="domainName">Name of the Zone</param>
        /// <param name="type">Type of the Zone</param>
        /// <param name="dsIntegrated">True if the Zone should be integrated with directory services</param>
        /// <returns>Newly created zone</returns>
        public WindowsDnsZone CreateZone(string domainName, DnsZoneType type, bool dsIntegrated)
            => CreateZone(domainName, type, dsIntegrated, dataFileName: null, masterDnsServerAddresses: null, adminEmailName: null);

        /// <summary>
        /// Adds a DNS Zone to this server
        /// </summary>
        /// <param name="zoneTemplate">Template of zone to be added</param>
        /// <param name="dsIntegrated">True if the Zone should be integrated with directory services</param>
        /// <returns>Newly created zone</returns>
        public WindowsDnsZone CreateZone(DnsZone zoneTemplate, bool dsIntegrated)
            => CreateZone(zoneTemplate.DomainName, zoneTemplate.Type, dsIntegrated);

        /// <summary>
        /// Adds a DNS Zone to this server
        /// </summary>
        /// <param name="zoneTemplate">Template of zone to be added</param>
        /// <returns>Newly created zone</returns>
        public override DnsZone CreateZone(DnsZone zoneTemplate)
            => CreateZone(zoneTemplate, dsIntegrated: false);

        /// <summary>
        /// Adds a DNS Zone to this server using default values
        /// </summary>
        /// <param name="domainName">Domain Name of the zone to be added</param>
        /// <returns>Newly created zone</returns>
        public override DnsZone CreateZone(string domainName)
            => CreateZone(domainName, DnsZoneType.Primary, dsIntegrated: false);

        /// <summary>
        /// Remove a DNS Zone
        /// </summary>
        /// <param name="zone">Zone to be removed</param>
        public override void DeleteZone(DnsZone zone)
        {
            var windowsZone = zone as WindowsDnsZone;

            if (zone == null)
                throw new ArgumentException("Zone must be created by the provider");

            this.DeleteZoneInternal(windowsZone);
        }
        /// <summary>
        /// Remove a DNS Zone
        /// </summary>
        /// <param name="domainName">Domain Name of the zone to be removed</param>
        public override void DeleteZone(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(nameof(domainName));

            var zone = this.GetZoneInternal(domainName);
            this.DeleteZoneInternal(zone);
        }

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            wmiServer.Dispose();
        }
    }
}
