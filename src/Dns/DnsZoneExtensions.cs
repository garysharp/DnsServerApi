using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dns
{
    /// <summary>
    /// Helper methods for <see cref="DnsZone"/>
    /// </summary>
    public static class DnsZoneExtensions
    {

        /// <summary>
        /// Adds an A record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="ipAddress">IP address of the Host (A) record</param>
        /// <returns>Resulting created record</returns>
        public static DnsARecord CreateARecord(this DnsZone zone, string name, TimeSpan timeToLive, DnsIpAddress ipAddress)
            => (DnsARecord)zone.CreateRecord(new DnsARecord(zone, name, timeToLive, ipAddress));
        /// <summary>
        /// Adds a CNAME record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryName">Primary Name for <see cref="DnsRecord.Name"/></param>
        /// <returns>Resulting created record</returns>
        public static DnsCNAMERecord CreateCNAMERecord(this DnsZone zone, string name, TimeSpan timeToLive, string primaryName)
            => (DnsCNAMERecord)zone.CreateRecord(new DnsCNAMERecord(zone, name, timeToLive, primaryName));
        /// <summary>
        /// Adds a MX record to the zone
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
        /// Adds a NS record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="nameServer">A host which should be authoritative for the domain</param>
        /// <returns>Resulting created record</returns>
        public static DnsNSRecord CreateNSRecord(this DnsZone zone, string name, TimeSpan timeToLive, string nameServer)
            => (DnsNSRecord)zone.CreateRecord(new DnsNSRecord(zone, name, timeToLive, nameServer));

        /// <summary>
        /// Adds a PTR record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        /// <returns>Resulting created record</returns>
        public static DnsPTRRecord CreatePTRRecord(this DnsZone zone, string name, TimeSpan timeToLive, string domainName)
            => (DnsPTRRecord)zone.CreateRecord(new DnsPTRRecord(zone, name, timeToLive, domainName));

        /// <summary>
        /// Adds a PTR record to the zone
        /// </summary>
        /// <param name="zone">Zone to which the record should be added</param>
        /// <param name="address">Pointer address</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="domainName">Pointer Domain Name</param>
        /// <returns>Resulting created record</returns>
        public static DnsPTRRecord CreatePTRRecord(this DnsZone zone, DnsIpAddress address, TimeSpan timeToLive, string domainName)
            => (DnsPTRRecord)zone.CreateRecord(new DnsPTRRecord(zone, address, timeToLive, domainName));

        /// <summary>
        /// Adds a SOA record to the zone
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
        /// Adds a SRV record to the zone
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
        /// Adds a SRV record to the zone
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

    }
}
