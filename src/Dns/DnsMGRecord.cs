using System;

namespace Dns
{
    /// <summary>
    /// Mail Group (MG) Record
    /// </summary>
    /// <remarks>
    /// Adds domain mailboxes, each specified by a mailbox (MB) record in the current zone,
    /// as members of a domain mailing group that is identified by name in this record. (RFC 1035)
    /// </remarks>
    [Obsolete("Experimental; RFC 2505")]
    public class DnsMGRecord : DnsRecord
    {
        private string mgMailboxInitial;

        /// <summary>
        /// A Fully Qualified Domain Name which specifies a mailbox which is a member of the mail group specified by the record's owner name.
        /// </summary>
        public string MGMailbox { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMGRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mgMailbox">A Fully Qualified Domain Name which specifies a mailbox which is a member of the mail group specified by the record's owner name.</param>
        private DnsMGRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mgMailbox)
            : base(zone, providerState, name, DnsRecordTypes.MG, @class, timeToLive)
        {
            MGMailbox = mgMailbox;
            mgMailboxInitial = mgMailbox;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMGRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mgMailbox">A Fully Qualified Domain Name which specifies a mailbox which is a member of the mail group specified by the record's owner name.</param>
        public DnsMGRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mgMailbox)
            : this(zone, providerState: null, name, @class, timeToLive, mgMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMGRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mgMailbox">A Fully Qualified Domain Name which specifies a mailbox which is a member of the mail group specified by the record's owner name.</param>
        public DnsMGRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string mgMailbox)
            : this(zone: null, name, @class, timeToLive, mgMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMGRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mgMailbox">A Fully Qualified Domain Name which specifies a mailbox which is a member of the mail group specified by the record's owner name.</param>
        public DnsMGRecord(DnsZone zone, string name, TimeSpan timeToLive, string mgMailbox)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, mgMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMGRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mgMailbox">A Fully Qualified Domain Name which specifies a mailbox which is a member of the mail group specified by the record's owner name.</param>
        public DnsMGRecord(string name, TimeSpan timeToLive, string mgMailbox)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, mgMailbox)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(mgMailboxInitial, MGMailbox, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            mgMailboxInitial = MGMailbox;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsMGRecord(zone, providerState, Name, Class, TimeToLive, MGMailbox);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => MGMailbox;

    }
}
