using System;

namespace Dns
{
    /// <summary>
    /// Andrew File System Database (AFSDB) Record
    /// </summary>
    /// <remarks>
    /// Indicates the location of either of the following standard server subtypes: an AFS volume location (cell database)
    /// server or a Distributed Computing Environment (DCE) authenticated name server.
    /// Also, supports other user-defined server subtypes that use the AFSDB resource record format. (RFC 1183)
    /// </remarks>
    public class DnsAFSDBRecord : DnsRecord
    {
        private ushort subtypeInitial;
        private string hostNameInitial;

        /// <summary>
        /// Subtype of the host AFS server.
        /// </summary>
        /// <remarks>
        /// For subtype 1, the host has an AFS version 3.0 Volume Location Server for the named AFS cell.
        /// In the case of subtype 2, the host has an authenticated name server holding the cell-root directory node for the named DCE/NCA cell.
        /// </remarks>
        public ushort Subtype { get; set; }
        /// <summary>
        /// A Fully Qualified Domain Name which specifies a host that has a server for the AFS cell specified in owner name.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAFSDBRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="subtype">Subtype of the host AFS server.</param>
        /// <param name="hostName">A Fully Qualified Domain Name which specifies a host that has a server for the AFS cell specified in owner name.</param>
        private DnsAFSDBRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort subtype, string hostName)
            : base(zone, providerState, name, DnsRecordTypes.AFSDB, @class, timeToLive)
        {
            Subtype = subtype;
            subtypeInitial = subtype;
            
            HostName = hostName;
            hostNameInitial = hostName;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAFSDBRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="subtype">Subtype of the host AFS server.</param>
        /// <param name="hostName">A Fully Qualified Domain Name which specifies a host that has a server for the AFS cell specified in owner name.</param>
        public DnsAFSDBRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort subtype, string hostName)
            : this(zone, providerState: null, name, @class, timeToLive, subtype, hostName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAFSDBRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="hostName">A Fully Qualified Domain Name which specifies a host that has a server for the AFS cell specified in owner name.</param>
        /// <param name="subtype">Subtype of the host AFS server.</param>
        public DnsAFSDBRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort subtype, string hostName)
            : this(zone: null, name, @class, timeToLive, subtype, hostName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAFSDBRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="subtype">Subtype of the host AFS server.</param>
        /// <param name="hostName">A Fully Qualified Domain Name which specifies a host that has a server for the AFS cell specified in owner name.</param>
        public DnsAFSDBRecord(DnsZone zone, string name, TimeSpan timeToLive, ushort subtype, string hostName)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, subtype, hostName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsAFSDBRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="hostName">A Fully Qualified Domain Name which specifies a host that has a server for the AFS cell specified in owner name.</param>
        /// <param name="subtype">Subtype of the host AFS server.</param>
        public DnsAFSDBRecord(string name, TimeSpan timeToLive, ushort subtype, string hostName)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, subtype, hostName)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => subtypeInitial != Subtype ||
                !string.Equals(hostNameInitial, HostName, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            subtypeInitial = Subtype;
            hostNameInitial = HostName;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsAFSDBRecord(zone, providerState, Name, Class, TimeToLive, Subtype, HostName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"[{Subtype}] {HostName}";

    }
}
