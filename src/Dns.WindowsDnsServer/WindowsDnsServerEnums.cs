using System;

namespace Dns.WindowsDnsServer
{
    /// <summary>
    /// Auto Config File Zones
    /// </summary>
    [Flags]
    public enum AutoConfigFileZones : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Only servers that allow dynamic updates
        /// </summary>
        WithDynamicUpdates = 1,
        /// <summary>
        /// Only servers that do not allow dynamic updates
        /// </summary>
        WithoutDynamicUpdates = 2,
        /// <summary>
        /// All servers
        /// </summary>
        All = 4,
    }

    /// <summary>
    /// Boot Methods
    /// </summary>
    [Flags]
    public enum BootMethods : uint
    {
        /// <summary>
        /// Uninitialized
        /// </summary>
        Uninitialized = 0,
        /// <summary>
        /// Boot from file
        /// </summary>
        FromFile = 1,
        /// <summary>
        /// Boot from registry
        /// </summary>
        FromRegistry = 2,
        /// <summary>
        /// Boot from directory and registry
        /// </summary>
        FromDirectoryAndRegistry = 3,
    }

    /// <summary>
    /// DNSSEC Enable Modes
    /// </summary>
    public enum DnsSecEnableModes : uint
    {
        /// <summary>
        /// No DNSSEC records are included in the response unless the query requested a resource record set of the DNSSEC record type.
        /// </summary>
        None = 0,
        /// <summary>
        /// DNSSEC records are included in the response according to RFC 2535.
        /// </summary>
        RFC2535 = 1,
        /// <summary>
        /// DNSSEC records are included in a response only if the original client query contained the OPT resource record according to RFC 2671
        /// </summary>
        RFC2671 = 2,
    }

    /// <summary>
    /// Event Log Levels
    /// </summary>
    [Flags]
    public enum EventLogLevels : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Log only errors
        /// </summary>
        Errors = 1,
        /// <summary>
        /// Log only warnings and errors
        /// </summary>
        ErrorsAndWarnings = 2,
        /// <summary>
        /// Log all events
        /// </summary>
        All = 4
    }

    /// <summary>
    /// Log Levels
    /// </summary>
    [Flags]
    public enum LogLevels : uint
    {
        /// <summary>
        /// Query
        /// </summary>
        Query = 1,
        /// <summary>
        /// Notify
        /// </summary>
        Notify = 16,
        /// <summary>
        /// Update
        /// </summary>
        Update = 32,
        /// <summary>
        /// Nonquery Transactions
        /// </summary>
        NonqueryTransactions = 254,
        /// <summary>
        /// Questions
        /// </summary>
        Questions = 256,
        /// <summary>
        /// Answers
        /// </summary>
        Answers = 512,
        /// <summary>
        /// Send
        /// </summary>
        Send = 4096,
        /// <summary>
        /// Receive
        /// </summary>
        Receive = 8192,
        /// <summary>
        /// UDP
        /// </summary>
        UDP = 16384,
        /// <summary>
        /// TCP
        /// </summary>
        TCP = 32768,
        /// <summary>
        /// All Packets
        /// </summary>
        AllPackets = 65535,
        /// <summary>
        /// NT Directory Service write transaction
        /// </summary>
        NtDirectoryServiceWriteTransaction = 65536,
        /// <summary>
        /// NT Directory Service update transaction
        /// </summary>
        NtDirectoryServiceUpdateTransaction = 131072,
        /// <summary>
        /// Full Packets
        /// </summary>
        FullPackets = 16777216,
        /// <summary>
        /// Write Through
        /// </summary>
        WriteThrough = 2147483648,
    }

    /// <summary>
    /// Name Check Flags
    /// </summary>
    public enum NameCheckFlags : uint
    {
        /// <summary>
        /// Strict RFC (ANSI)
        /// </summary>
        StrictRFC = 0,
        /// <summary>
        /// Non RFC (ANSI)
        /// </summary>
        NonRFC = 1,
        /// <summary>
        /// Multibyte (UTF8)
        /// </summary>
        Multibyte = 3,
    }

    /// <summary>
    /// RPC Protocols
    /// </summary>
    [Flags]
    public enum RpcProtocols : int
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// TCP
        /// </summary>
        TCP = 1,
        /// <summary>
        /// Named Pipes
        /// </summary>
        NamedPipes = 2,
        /// <summary>
        /// LPC
        /// </summary>
        LPC = 4
    }

    /// <summary>
    /// Dynamic Update Restrictions
    /// </summary>
    [Flags]
    public enum UpdateOptions : uint
    {
        /// <summary>
        /// No Restrictions
        /// </summary>
        None = 0,
        /// <summary>
        /// Does not allow dynamic updates of SOA records
        /// </summary>
        RestrictSOA = 1,
        /// <summary>
        /// Does not allow dynamic updates of NS records at the zone root
        /// </summary>
        RestrictNS = 2,
        /// <summary>
        /// Does not allow dynamic updates of NS records not at the zone root (delegation NS records)
        /// </summary>
        RestrictRootNS = 4,
    }

}
