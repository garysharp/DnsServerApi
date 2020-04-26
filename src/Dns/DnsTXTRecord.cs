using System;

namespace Dns
{
    /// <summary>
    /// Text (TXT) Record
    /// </summary>
    /// <remarks>
    /// Holds a string of characters that serves as descriptive text to be associated with
    /// a specific DNS domain name. The semantics of the actual descriptive text used as
    /// data with this record type depends on the DNS domain where these records are located. (RFC 1035)
    /// </remarks>
    public class DnsTXTRecord : DnsRecord
    {
        private string descriptiveTextInitial;

        /// <summary>
        /// Descriptive text whose semantics depend on the owner domain.
        /// </summary>
        public string DescriptiveText { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsTXTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="descriptiveText">Descriptive text whose semantics depend on the owner domain.</param>
        private DnsTXTRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string descriptiveText)
            : base(zone, providerState, name, DnsRecordTypes.TXT, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(descriptiveText))
                throw new ArgumentNullException(nameof(descriptiveText));

            DescriptiveText = descriptiveText;
            descriptiveTextInitial = descriptiveText;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsTXTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="descriptiveText">Descriptive text whose semantics depend on the owner domain.</param>
        public DnsTXTRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string descriptiveText)
            : this(zone, providerState: null, name, @class, timeToLive, descriptiveText)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsTXTRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="descriptiveText">Descriptive text whose semantics depend on the owner domain.</param>
        public DnsTXTRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string descriptiveText)
            : this(zone: null, name, @class, timeToLive, descriptiveText)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsTXTRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="descriptiveText">Descriptive text whose semantics depend on the owner domain.</param>
        public DnsTXTRecord(DnsZone zone, string name, TimeSpan timeToLive, string descriptiveText)
            : this(zone, name, DnsRecordClasses.IN, timeToLive, descriptiveText)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsTXTRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="descriptiveText">Descriptive text whose semantics depend on the owner domain.</param>
        public DnsTXTRecord(string name, TimeSpan timeToLive, string descriptiveText)
            : this(zone: null, name, timeToLive, descriptiveText)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(descriptiveTextInitial, DescriptiveText, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            descriptiveTextInitial = DescriptiveText;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsTXTRecord(zone, providerState, Name, Class, TimeToLive, DescriptiveText);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => DescriptiveText;

    }
}
