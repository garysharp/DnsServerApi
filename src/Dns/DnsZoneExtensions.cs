using System;

namespace Dns
{
    /// <summary>
    /// Helper methods for <see cref="DnsZone"/>
    /// </summary>
    public static class DnsZoneExtensions
    {
        /// <summary>
        /// Adds an IPv6 Host Address (AAAA) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipV6Address">IPv6 address of the Host (A) record</param>
        /// <returns>Resulting created record</returns>
        public static DnsAAAARecord CreateAAAARecord(this DnsZone zone, string name, TimeSpan timeToLive, string ipV6Address)
            => (DnsAAAARecord)zone.CreateRecord(new DnsAAAARecord(zone, name, timeToLive, ipV6Address));

        /// <summary>
        /// Adds an Andrew File System Database (AFSDB) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="subtype">Subtype of the host AFS server.</param>
        /// <param name="hostName">A Fully Qualified Domain Name which specifies a host that has a server for the AFS cell specified in owner name.</param>
        /// <returns>Resulting created record</returns>
        public static DnsAFSDBRecord CreateAFSDBRecord(this DnsZone zone, string name, TimeSpan timeToLive, ushort subtype, string hostName)
            => (DnsAFSDBRecord)zone.CreateRecord(new DnsAFSDBRecord(zone, name, timeToLive, subtype, hostName));

        /// <summary>
        /// Adds an IPv4 Host Address (A) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipAddress">IP address of the Host (A) record</param>
        /// <returns>Resulting created record</returns>
        public static DnsARecord CreateARecord(this DnsZone zone, string name, TimeSpan timeToLive, DnsIpAddress ipAddress)
            => (DnsARecord)zone.CreateRecord(new DnsARecord(zone, name, timeToLive, ipAddress));

        /// <summary>
        /// Adds an ATM Address (ATMA) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="format">The ATM address format.</param>
        /// <param name="aTMAddress">The ATM address of the node/owner to which this RR pertains.</param>
        /// <returns>Resulting created record</returns>
        public static DnsATMARecord CreateATMARecord(this DnsZone zone, string name, TimeSpan timeToLive, ushort format, string aTMAddress)
            => (DnsATMARecord)zone.CreateRecord(new DnsATMARecord(zone, name, timeToLive, format, aTMAddress));

        /// <summary>
        /// Adds an Alias/Canonical Name (CNAME) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        /// <returns>Resulting created record</returns>
        public static DnsCNAMERecord CreateCNAMERecord(this DnsZone zone, string name, TimeSpan timeToLive, string primaryName)
            => (DnsCNAMERecord)zone.CreateRecord(new DnsCNAMERecord(zone, name, timeToLive, primaryName));

        /// <summary>
        /// Adds a Host Information (HINFO) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="cpu">The CPU type of the owner of the record.</param>
        /// <param name="os">The operating system type of the owner.</param>
        /// <returns>Resulting created record</returns>
        public static DnsHINFORecord CreateHINFORecord(this DnsZone zone, string name, TimeSpan timeToLive, string cpu, string os)
            => (DnsHINFORecord)zone.CreateRecord(new DnsHINFORecord(zone, name, timeToLive, cpu, os));

        /// <summary>
        /// Adds an Integrated Services Digital Network (ISDN) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="isdnNumber">The ISDN number and DDI of the record's owner.</param>
        /// <param name="subAddress">The subaddress of the owner, if defined.</param>
        /// <returns>Resulting created record</returns>
        public static DnsISDNRecord CreateISDNRecord(this DnsZone zone, string name, TimeSpan timeToLive, string isdnNumber, string subAddress)
            => (DnsISDNRecord)zone.CreateRecord(new DnsHINFORecord(zone, name, timeToLive, isdnNumber, subAddress));

        /// <summary>
        /// Adds a Mail Exchange (MX) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">Preference given to this record</param>
        /// <param name="domainName">Mail Exchange Domain Name</param>
        /// <returns>Resulting created record</returns>
        public static DnsMXRecord CreateMXRecord(this DnsZone zone, string name, TimeSpan timeToLive, ushort preference, string domainName)
            => (DnsMXRecord)zone.CreateRecord(new DnsMXRecord(zone, name, timeToLive, preference, domainName));
        
        /// <summary>
        /// Adds a Name Server (NS) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="nameServer">A host which should be authoritative for the domain</param>
        /// <returns>Resulting created record</returns>
        public static DnsNSRecord CreateNSRecord(this DnsZone zone, string name, TimeSpan timeToLive, string nameServer)
            => (DnsNSRecord)zone.CreateRecord(new DnsNSRecord(zone, name, timeToLive, nameServer));

        /// <summary>
        /// Adds a Pointer (PTR) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        /// <returns>Resulting created record</returns>
        public static DnsPTRRecord CreatePTRRecord(this DnsZone zone, string name, TimeSpan timeToLive, string domainName)
            => (DnsPTRRecord)zone.CreateRecord(new DnsPTRRecord(zone, name, timeToLive, domainName));

        /// <summary>
        /// Adds a Pointer (PTR) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="address">Pointer address</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        /// <returns>Resulting created record</returns>
        public static DnsPTRRecord CreatePTRRecord(this DnsZone zone, DnsIpAddress address, TimeSpan timeToLive, string domainName)
            => (DnsPTRRecord)zone.CreateRecord(new DnsPTRRecord(zone, address, timeToLive, domainName));

        /// <summary>
        /// Adds a Responsible Person (RP) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="rpMailbox">A Fully Qualified Domain Name that specifies the mailbox for the responsible person.</param>
        /// <param name="txtDomainName">A Fully Qualified Domain Name for which TXT RR's exist.</param>
        /// <returns>Resulting created record</returns>
        public static DnsRPRecord CreateRPRecord(this DnsZone zone, string name, TimeSpan timeToLive, string rpMailbox, string txtDomainName)
            => (DnsRPRecord)zone.CreateRecord(new DnsRPRecord(zone, name, timeToLive, rpMailbox, txtDomainName));

        /// <summary>
        /// Adds a Route Through (RT) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="preference">The preference given to this RR among others at the same owner.  Lower values are preferred.</param>
        /// <param name="intermediateHost">A Fully Qualified Domain Name which specifies a host which will serve as an intermediate in reaching the host specified by owner.</param>
        /// <returns>Resulting created record</returns>
        public static DnsRTRecord CreateRPRecord(this DnsZone zone, string name, TimeSpan timeToLive, ushort preference, string intermediateHost)
            => (DnsRTRecord)zone.CreateRecord(new DnsRTRecord(zone, name, timeToLive, preference, intermediateHost));

        /// <summary>
        /// Adds a Start of Zone of Authority (SOA) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryServer">Domain Name of the original or primary source of data for this zone</param>
        /// <param name="responsiblePerson">A domain name which specifies the mailbox of the person responsible for this zone</param>
        /// <param name="serial">Version number of the zone</param>
        /// <param name="refreshInterval">Interval before the zone should be refreshed</param>
        /// <param name="retryDelay">Time interval that should elapse before a failed refresh should be retried</param>
        /// <param name="expireLimit">The upper limit on the time interval that can elapse before the zone is no longer authoritative</param>
        /// <param name="minimumTimeToLive">The minimum time-to-live (TTL) that should be exported with an record from this zone</param>
        /// <returns>Resulting created record</returns>
        public static DnsSOARecord CreateSOARecord(this DnsZone zone, string name, TimeSpan timeToLive,
            string primaryServer, string responsiblePerson, uint serial, TimeSpan refreshInterval,
            TimeSpan retryDelay, TimeSpan expireLimit, TimeSpan minimumTimeToLive)
            => (DnsSOARecord)zone.CreateRecord(new DnsSOARecord(zone, name, timeToLive, primaryServer,
                responsiblePerson, serial, refreshInterval, retryDelay, expireLimit, minimumTimeToLive));

        /// <summary>
        /// Adds a Service Location (SRV) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="priority">Priority of the this target host</param>
        /// <param name="weight">Server selection mechanism</param>
        /// <param name="port">Port on this target host of this service. See <see cref="DnsSRVRecord.ServicePorts"/>.</param>
        /// <param name="targetDomainName">Domain name of the target host</param>
        /// <returns>Resulting created record</returns>
        public static DnsSRVRecord CreateSRVRecord(this DnsZone zone, string name, TimeSpan timeToLive,
            ushort priority, ushort weight, ushort port, string targetDomainName)
            => (DnsSRVRecord)zone.CreateRecord(new DnsSRVRecord(zone, name, timeToLive, priority, weight, port, targetDomainName));

        /// <summary>
        /// Adds a Service Location (SRV) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="service">Symbolic name of the desired service. See <see cref="DnsSRVRecord.ServiceNames"/>.</param>
        /// <param name="protocol">Symbolic name of the desired protocol. See <see cref="DnsSRVRecord.ProtocolNames"/>.</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="priority">Priority of the this target host</param>
        /// <param name="weight">Server selection mechanism</param>
        /// <param name="port">Port on this target host of this service. See <see cref="DnsSRVRecord.ServicePorts"/>.</param>
        /// <param name="targetDomainName">Domain name of the target host</param>
        /// <returns>Resulting created record</returns>
        public static DnsSRVRecord CreateSRVRecord(this DnsZone zone, string service, string protocol, TimeSpan timeToLive,
            ushort priority, ushort weight, ushort port, string targetDomainName)
            => (DnsSRVRecord)zone.CreateRecord(new DnsSRVRecord(zone, service, protocol, timeToLive, priority, weight, port, targetDomainName));

        /// <summary>
        /// Adds a Text (TXT) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="address">Pointer address</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="descriptiveText">Descriptive text whose semantics depend on the owner domain.</param>
        /// <returns>Resulting created record</returns>
        public static DnsTXTRecord CreateTXTRecord(this DnsZone zone, DnsIpAddress address, TimeSpan timeToLive, string descriptiveText)
            => (DnsTXTRecord)zone.CreateRecord(new DnsTXTRecord(zone, address, timeToLive, descriptiveText));

        /// <summary>
        /// Adds a X.25 (X25) record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="address">Pointer address</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="psdnAddress">PSDN Address</param>
        /// <returns>Resulting created record</returns>
        public static DnsX25Record CreateX25Record(this DnsZone zone, DnsIpAddress address, TimeSpan timeToLive, string psdnAddress)
            => (DnsX25Record)zone.CreateRecord(new DnsX25Record(zone, address, timeToLive, psdnAddress));
    }
}
