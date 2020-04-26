using System;

namespace Dns
{
    /// <summary>
    /// Next (NXT) Record
    /// </summary>
    /// <remarks>
    /// NXT resource records indicate the nonexistence of a name in a zone by creating
    /// a chain of all of the literal owner names in that zone.
    /// They also indicate what resource record types are present for an existing name.
    /// </remarks>
    [Obsolete("Considered obsolete by Internet Assigned Numbers Authority (IANA)")]
    public class DnsNXTRecord : DnsRecord
    {
        private string nextDomainNameInitial;
        private string typesInitial;

        /// <summary>
        /// Next Domain Name.
        /// </summary>
        public string NextDomainName { get; set; }
        /// <summary>
        /// Types is the space separated list of the RR types mnemonics that exist for the owner name of the NXT RR. 
        /// </summary>
        public string Types { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsNXTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="nextDomainName">Next Domain Name.</param>
        /// <param name="types">Types is the space separated list of the RR types mnemonics that exist for the owner name of the NXT RR. </param>
        private DnsNXTRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string nextDomainName, string types)
            : base(zone, providerState, name, DnsRecordTypes.NXT, @class, timeToLive)
        {
            NextDomainName = nextDomainName;
            nextDomainNameInitial = nextDomainName;

            Types = types;
            typesInitial = types;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsNXTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="nextDomainName">Next Domain Name.</param>
        /// <param name="types">Types is the space separated list of the RR types mnemonics that exist for the owner name of the NXT RR. </param>
        public DnsNXTRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string nextDomainName, string types)
            : this(zone, providerState: null, name, @class, timeToLive, nextDomainName, types)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsNXTRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="nextDomainName">Next Domain Name.</param>
        /// <param name="types">Types is the space separated list of the RR types mnemonics that exist for the owner name of the NXT RR. </param>
        public DnsNXTRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string nextDomainName, string types)
            : this(zone: null, name, @class, timeToLive, nextDomainName, types)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsNXTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="nextDomainName">Next Domain Name.</param>
        /// <param name="types">Types is the space separated list of the RR types mnemonics that exist for the owner name of the NXT RR. </param>
        public DnsNXTRecord(DnsZone zone, string name, TimeSpan timeToLive, string nextDomainName, string types)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, nextDomainName, types)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsNXTRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="nextDomainName">Next Domain Name.</param>
        /// <param name="types">Types is the space separated list of the RR types mnemonics that exist for the owner name of the NXT RR. </param>
        public DnsNXTRecord(string name, TimeSpan timeToLive, string nextDomainName, string types)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, nextDomainName, types)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(nextDomainNameInitial, NextDomainName, StringComparison.Ordinal) ||
                !string.Equals(typesInitial, Types, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            nextDomainNameInitial = NextDomainName;
            typesInitial = Types;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsNXTRecord(zone, providerState, Name, Class, TimeToLive, NextDomainName, Types);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{NextDomainName} {Types}";

    }
}
