using System;

namespace Dns
{
    /// <summary>
    /// ATM Address (ATMA) Record
    /// </summary>
    /// <remarks>
    /// Maps a DNS domain name to an ATM address.
    /// </remarks>
    public class DnsATMARecord : DnsRecord
    {
        private ushort formatInitial;
        private string atmAddressInitial;

        /// <summary>
        /// The ATM address format.
        /// </summary>
        /// <remarks>
        /// Two possible values for FORMAT are: 0 indicating ATM End System Address (AESA) format and 1 indicating E.164 format.
        /// </remarks>
        public ushort Format { get; set; }
        /// <summary>
        /// The ATM address of the node/owner to which this RR pertains.
        /// </summary>
        public string ATMAddress { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsATMARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="format">The ATM address format.</param>
        /// <param name="aTMAddress">The ATM address of the node/owner to which this RR pertains.</param>
        private DnsATMARecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort format, string aTMAddress)
            : base(zone, providerState, name, DnsRecordTypes.ATMA, @class, timeToLive)
        {
            Format = format;
            formatInitial = format;

            ATMAddress = aTMAddress;
            atmAddressInitial = aTMAddress;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsATMARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="format">The ATM address format.</param>
        /// <param name="aTMAddress">The ATM address of the node/owner to which this RR pertains.</param>
        public DnsATMARecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort format, string aTMAddress)
            : this(zone, providerState: null, name, @class, timeToLive, format, aTMAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsATMARecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="format">The ATM address format.</param>
        /// <param name="aTMAddress">The ATM address of the node/owner to which this RR pertains.</param>
        public DnsATMARecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort format, string aTMAddress)
            : this(zone: null, name, @class, timeToLive, format, aTMAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsATMARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="format">The ATM address format.</param>
        /// <param name="aTMAddress">The ATM address of the node/owner to which this RR pertains.</param>
        public DnsATMARecord(DnsZone zone, string name, TimeSpan timeToLive, ushort format, string aTMAddress)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, format, aTMAddress)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsATMARecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="format">The ATM address format.</param>
        /// <param name="aTMAddress">The ATM address of the node/owner to which this RR pertains.</param>
        public DnsATMARecord(string name, TimeSpan timeToLive, ushort format, string aTMAddress)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, format, aTMAddress)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => formatInitial != Format ||
                !string.Equals(atmAddressInitial, ATMAddress, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            formatInitial = Format;
            atmAddressInitial = ATMAddress;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsATMARecord(zone, providerState, Name, Class, TimeToLive, Format, ATMAddress);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{ATMAddress} [{Format}]";

    }
}
