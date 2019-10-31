namespace Dns
{
    /// <summary>
    /// DNS Zone Types
    /// </summary>
    public enum DnsZoneType
    {
        Cache,
        /// <summary>
        /// Stores a copy of the zone that can be updated directly
        /// </summary>
        Primary,
        /// <summary>
        /// Stores a copy of an existing zone.
        /// </summary>
        Secondary,
        /// <summary>
        /// Stores a copy of a zone containing only NS, SOA, and possibly glue A records.
        /// </summary>
        Stub,
        Forwarder,
    }
}
