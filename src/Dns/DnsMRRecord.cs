using System;

namespace Dns
{
    /// <summary>
    /// Renamed Mailbox (MR) Record
    /// </summary>
    /// <remarks>
    /// Specifies a domain mailbox name, which is the proper rename of an existing specified mailbox
    /// (specified in an existing mailbox or an MB-type record in the zone). The main use for an MR record
    /// is as a forwarding entry for a user who has moved to a different mailbox.
    /// If used, MR records do not cause additional section processing. (RFC 1035)
    /// </remarks>
    [Obsolete("Experimental; RFC 2505")]
    public class DnsMRRecord : DnsRecord
    {
        private string mrMailboxInitial;

        /// <summary>
        /// A Fully Qualified Domain Name which specifies a mailbox which is the proper rename of the mailbox specified in the record's Owner Name.
        /// </summary>
        public string MRMailbox { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mrMailbox">A Fully Qualified Domain Name which specifies a mailbox which is the proper rename of the mailbox specified in the record's Owner Name.</param>
        private DnsMRRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mrMailbox)
            : base(zone, providerState, name, DnsRecordTypes.MR, @class, timeToLive)
        {
            MRMailbox = mrMailbox;
            mrMailboxInitial = mrMailbox;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mrMailbox">A Fully Qualified Domain Name which specifies a mailbox which is the proper rename of the mailbox specified in the record's Owner Name.</param>
        public DnsMRRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mrMailbox)
            : this(zone, providerState: null, name, @class, timeToLive, mrMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMRRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mrMailbox">A Fully Qualified Domain Name which specifies a mailbox which is the proper rename of the mailbox specified in the record's Owner Name.</param>
        public DnsMRRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string mrMailbox)
            : this(zone: null, name, @class, timeToLive, mrMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMRRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mrMailbox">A Fully Qualified Domain Name which specifies a mailbox which is the proper rename of the mailbox specified in the record's Owner Name.</param>
        public DnsMRRecord(DnsZone zone, string name, TimeSpan timeToLive, string mrMailbox)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, mrMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMRRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mrMailbox">A Fully Qualified Domain Name which specifies a mailbox which is the proper rename of the mailbox specified in the record's Owner Name.</param>
        public DnsMRRecord(string name, TimeSpan timeToLive, string mrMailbox)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, mrMailbox)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(mrMailboxInitial, MRMailbox, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            mrMailboxInitial = MRMailbox;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsMRRecord(zone, providerState, Name, Class, TimeToLive, MRMailbox);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => MRMailbox;

    }
}
