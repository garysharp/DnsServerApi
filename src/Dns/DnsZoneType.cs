namespace Dns
{
    /// <summary>
    /// DNS Zone Types
    /// </summary>
    public enum DnsZoneType
    {
        /// <summary>
        /// This zone is used to store all cached DNS records received from remote DNS servers during normal query processing.
        /// </summary>
        Cache = 0,
        /// <summary>
        /// The DNS server is a primary DNS server for this zone.
        /// </summary>
        Primary = 1,
        /// <summary>
        /// The DNS server is acting as a secondary DNS server for this zone.
        /// </summary>
        Secondary = 2,
        /// <summary>
        /// Zone is a stub zone, that is, it contains only those resource records that are necessary to identify authoritative DNS servers for that zone.
        /// </summary>
        Stub = 3,
        /// <summary>
        /// The DNS server is a forwarder for this zone, that is, the server does not have authoritative information for resource records in this zone.
        /// </summary>
        Forwarder = 4,
        /// <summary>
        /// This zone is used to hold cached records for some implementation specific purpose.
        /// </summary>
        SecondaryCache = 5,
    }
}
