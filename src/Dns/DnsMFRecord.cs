using System;

namespace Dns
{
    /// <summary>
    /// Mail Forwarder (MF) Record
    /// </summary>
    /// <remarks>
    /// Obsolete by RFC 973 - use Mail Exchange (MX) Record. Present for compatibility.
    /// </remarks>
    [Obsolete("Use DnsMXRecord; RFC 973")]
    public class DnsMFRecord : DnsRecord
    {
        private string mfHostInitial;

        /// <summary>
        /// A Fully Qualified Domain Name which specifies a host which has a mail agent which will accept mail for forwarding to the specified domain.
        /// </summary>
        public string MFHost { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMFRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mfHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which will accept mail for forwarding to the specified domain.</param>
        private DnsMFRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mfHost)
            : base(zone, providerState, name, DnsRecordTypes.MF, @class, timeToLive)
        {
            MFHost = mfHost;
            mfHostInitial = mfHost;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMFRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mfHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which will accept mail for forwarding to the specified domain.</param>
        public DnsMFRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string mfHost)
            : this(zone, providerState: null, name, @class, timeToLive, mfHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMFRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mfHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which will accept mail for forwarding to the specified domain.</param>
        public DnsMFRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string mfHost)
            : this(zone: null, name, @class, timeToLive, mfHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMFRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mfHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which will accept mail for forwarding to the specified domain.</param>
        public DnsMFRecord(DnsZone zone, string name, TimeSpan timeToLive, string mfHost)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, mfHost)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsMFRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="mfHost">A Fully Qualified Domain Name which specifies a host which has a mail agent which will accept mail for forwarding to the specified domain.</param>
        public DnsMFRecord(string name, TimeSpan timeToLive, string mfHost)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, mfHost)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(mfHostInitial, MFHost, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            mfHostInitial = MFHost;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsMFRecord(zone, providerState, Name, Class, TimeToLive, MFHost);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => MFHost;

    }
}
