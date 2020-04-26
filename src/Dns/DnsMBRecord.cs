using System;

namespace Dns
{
    /// <summary>
    /// Mailbox (MB) Record
    /// </summary>
    /// <remarks>
    /// Maps a specified domain mailbox name to a host that hosts this mailbox. (RFC 1035)
    /// </remarks>
    [Obsolete("Experimental; RFC 2505")]
    public class DnsMBRecord : DnsRecord
    {
        private string mbHostInitial;

        /// <summary>
        /// A Fully Qualified Domain Name which specifies a host of the mailbox specified in the record's Owner Name.
        /// </summary>
        public string MBHost { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMBRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mbHost">A Fully Qualified Domain Name which specifies a host of the mailbox specified in the record's Owner Name.</param>
        private DnsMBRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mbHost)
            : base(zone, providerState, name, DnsRecordTypes.MB, @class, timeToLive)
        {
            MBHost = mbHost;
            mbHostInitial = mbHost;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMBRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mbHost">A Fully Qualified Domain Name which specifies a host of the mailbox specified in the record's Owner Name.</param>
        public DnsMBRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mbHost)
            : this(zone, providerState: null, name, @class, timeToLive, mbHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMBRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mbHost">A Fully Qualified Domain Name which specifies a host of the mailbox specified in the record's Owner Name.</param>
        public DnsMBRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string mbHost)
            : this(zone: null, name, @class, timeToLive, mbHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMBRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mbHost">A Fully Qualified Domain Name which specifies a host of the mailbox specified in the record's Owner Name.</param>
        public DnsMBRecord(DnsZone zone, string name, TimeSpan timeToLive, string mbHost)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, mbHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMBRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mbHost">A Fully Qualified Domain Name which specifies a host of the mailbox specified in the record's Owner Name.</param>
        public DnsMBRecord(string name, TimeSpan timeToLive, string mbHost)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, mbHost)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(mbHostInitial, MBHost, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            mbHostInitial = MBHost;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsMBRecord(zone, providerState, Name, Class, TimeToLive, MBHost);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => MBHost;

    }
}
