namespace Dns.WindowsDnsServer
{
    /// <summary>
    /// Zone Type
    /// </summary>
    internal enum CreateZoneType : uint
    {
        /// <summary>
        /// The DNS server is a primary DNS server for this zone.
        /// </summary>
        Primary = 0,
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
    }

    /// <summary>
    /// Allowed Dynamic Updates
    /// </summary>
    public enum AllowedDynamicUpdates : uint
    {
        /// <summary>
        /// No updates are allowed for the zone.
        /// </summary>
        None = 0,
        /// <summary>
        /// All updates (secure and unsecure) are allowed for the zone.
        /// </summary>
        All = 1,
        /// <summary>
        /// The zone only allows secure updates, that is, DNS packet MUST have a TSIG [RFC2845] present in the additional section.
        /// </summary>
        SecureOnly = 2
    }

    /// <summary>
    /// The types of security settings that are enforced by the master DNS server to honor zone transfer requests for a zone.
    /// </summary>
    public enum SecondarySecurity : uint
    {
        /// <summary>
        /// No security enforcement for secondaries, that is, any request will be honored.
        /// </summary>
        NoSecurity = 0,
        /// <summary>
        /// Zone transfer request will be honored from the remote servers, which are in the list of name servers for this zone.
        /// </summary>
        NameServersOnly = 1,
        /// <summary>
        /// Zone transfer request will be honored from the remote servers, which are explicitly configured by IP addresses in SecondaryServers
        /// </summary>
        ExplicitySecondariesOnly = 2,
        /// <summary>
        /// No zone transfer requests will be honored.
        /// </summary>
        NoTransfer = 3,
    }
}
