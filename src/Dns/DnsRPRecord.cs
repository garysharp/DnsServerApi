using System;

namespace Dns
{
    /// <summary>
    /// Responsible Person (RP) Record
    /// </summary>
    /// <remarks>
    /// Specifies the domain mailbox name for a responsible person and maps this name to a domain name for
    /// which text (TXT) resource records exist. Where RP records are used in DNS queries, subsequent queries
    /// can be needed to retrieve the text (TXT) record information mapped using the RP record type. (RFC 1183)
    /// </remarks>
    public class DnsRPRecord : DnsRecord
    {
        private string rpMailboxInitial;
        private string txtDomainNameInitial;

        /// <summary>
        /// A Fully Qualified Domain Name that specifies the mailbox for the responsible person.
        /// </summary>
        public string RPMailbox { get; set; }
        /// <summary>
        /// A Fully Qualified Domain Name for which TXT RR's exist.
        /// </summary>
        public string TXTDomainName { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRPRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="rpMailbox">A Fully Qualified Domain Name that specifies the mailbox for the responsible person.</param>
        /// <param name="txtDomainName">A Fully Qualified Domain Name for which TXT RR's exist.</param>
        private DnsRPRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string rpMailbox, string txtDomainName)
            : base(zone, providerState, name, DnsRecordTypes.RP, @class, timeToLive)
        {
            RPMailbox = rpMailbox;
            rpMailboxInitial = rpMailbox;

            TXTDomainName = txtDomainName;
            txtDomainNameInitial = txtDomainName;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRPRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="rpMailbox">A Fully Qualified Domain Name that specifies the mailbox for the responsible person.</param>
        /// <param name="txtDomainName">A Fully Qualified Domain Name for which TXT RR's exist.</param>
        public DnsRPRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string rpMailbox, string txtDomainName)
            : this(zone, providerState: null, name, @class, timeToLive, rpMailbox, txtDomainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRPRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="rpMailbox">A Fully Qualified Domain Name that specifies the mailbox for the responsible person.</param>
        /// <param name="txtDomainName">A Fully Qualified Domain Name for which TXT RR's exist.</param>
        public DnsRPRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string rpMailbox, string txtDomainName)
            : this(zone: null, name, @class, timeToLive, rpMailbox, txtDomainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRPRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="rpMailbox">A Fully Qualified Domain Name that specifies the mailbox for the responsible person.</param>
        /// <param name="txtDomainName">A Fully Qualified Domain Name for which TXT RR's exist.</param>
        public DnsRPRecord(DnsZone zone, string name, TimeSpan timeToLive, string rpMailbox, string txtDomainName)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, rpMailbox, txtDomainName)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRPRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="rpMailbox">A Fully Qualified Domain Name that specifies the mailbox for the responsible person.</param>
        /// <param name="txtDomainName">A Fully Qualified Domain Name for which TXT RR's exist.</param>
        public DnsRPRecord(string name, TimeSpan timeToLive, string rpMailbox, string txtDomainName)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, rpMailbox, txtDomainName)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(rpMailboxInitial, RPMailbox, StringComparison.Ordinal) ||
                !string.Equals(txtDomainNameInitial, TXTDomainName, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            rpMailboxInitial = RPMailbox;
            txtDomainNameInitial = TXTDomainName;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsRPRecord(zone, providerState, Name, Class, TimeToLive, RPMailbox, TXTDomainName);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{RPMailbox} {TXTDomainName}";

    }
}
