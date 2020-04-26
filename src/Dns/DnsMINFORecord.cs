using System;

namespace Dns
{
    /// <summary>
    /// Mailbox Information (MINFO) Record
    /// </summary>
    /// <remarks>
    ///  Specifies a domain mailbox name to contact. This contact maintains a mail list
    ///  or mailbox specified in this record. Also, specifies a mailbox for receiving error
    ///  messages related to the mailing list or mailbox specified in this record. (RFC 1035)
    /// </remarks>
    [Obsolete("Experimental; RFC 2505")]
    public class DnsMINFORecord : DnsRecord
    {
        private string responsibleMailboxInitial;
        private string errorMailboxInitial;

        /// <summary>
        /// A Fully Qualified Domain Name which specifies a mailbox which is responsible for the mailing list or mailbox specified in the record's Owner Name.
        /// </summary>
        public string ResponsibleMailbox { get; set; }
        /// <summary>
        /// A Fully Qualified Domain Name which specifies a mailbox which is to receive error messages related to the mailing list or mailbox specified by the owner name of the MINFO record.
        /// </summary>
        public string ErrorMailbox { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMINFORecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="responsibleMailbox">A Fully Qualified Domain Name which specifies a mailbox which is responsible for the mailing list or mailbox specified in the record's Owner Name.</param>
        /// <param name="errorMailbox">A Fully Qualified Domain Name which specifies a mailbox which is to receive error messages related to the mailing list or mailbox specified by the owner name of the MINFO record.</param>
        private DnsMINFORecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string responsibleMailbox, string errorMailbox)
            : base(zone, providerState, name, DnsRecordTypes.MINFO, @class, timeToLive)
        {
            ResponsibleMailbox = responsibleMailbox;
            responsibleMailboxInitial = responsibleMailbox;

            ErrorMailbox = errorMailbox;
            errorMailboxInitial = errorMailbox;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMINFORecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="responsibleMailbox">A Fully Qualified Domain Name which specifies a mailbox which is responsible for the mailing list or mailbox specified in the record's Owner Name.</param>
        /// <param name="errorMailbox">A Fully Qualified Domain Name which specifies a mailbox which is to receive error messages related to the mailing list or mailbox specified by the owner name of the MINFO record.</param>
        public DnsMINFORecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string responsibleMailbox, string errorMailbox)
            : this(zone, providerState: null, name, @class, timeToLive, responsibleMailbox, errorMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMINFORecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="responsibleMailbox">A Fully Qualified Domain Name which specifies a mailbox which is responsible for the mailing list or mailbox specified in the record's Owner Name.</param>
        /// <param name="errorMailbox">A Fully Qualified Domain Name which specifies a mailbox which is to receive error messages related to the mailing list or mailbox specified by the owner name of the MINFO record.</param>
        public DnsMINFORecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string responsibleMailbox, string errorMailbox)
            : this(zone: null, name, @class, timeToLive, responsibleMailbox, errorMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMINFORecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="responsibleMailbox">A Fully Qualified Domain Name which specifies a mailbox which is responsible for the mailing list or mailbox specified in the record's Owner Name.</param>
        /// <param name="errorMailbox">A Fully Qualified Domain Name which specifies a mailbox which is to receive error messages related to the mailing list or mailbox specified by the owner name of the MINFO record.</param>
        public DnsMINFORecord(DnsZone zone, string name, TimeSpan timeToLive, string responsibleMailbox, string errorMailbox)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, responsibleMailbox, errorMailbox)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMINFORecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="responsibleMailbox">A Fully Qualified Domain Name which specifies a mailbox which is responsible for the mailing list or mailbox specified in the record's Owner Name.</param>
        /// <param name="errorMailbox">A Fully Qualified Domain Name which specifies a mailbox which is to receive error messages related to the mailing list or mailbox specified by the owner name of the MINFO record.</param>
        public DnsMINFORecord(string name, TimeSpan timeToLive, string responsibleMailbox, string errorMailbox)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, responsibleMailbox, errorMailbox)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(responsibleMailboxInitial, ResponsibleMailbox, StringComparison.Ordinal) ||
                !string.Equals(errorMailboxInitial, ErrorMailbox, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            responsibleMailboxInitial = ResponsibleMailbox;
            errorMailboxInitial = ErrorMailbox;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsMINFORecord(zone, providerState, Name, Class, TimeToLive, ResponsibleMailbox, ErrorMailbox);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{ErrorMailbox} {ResponsibleMailbox}";

    }
}
