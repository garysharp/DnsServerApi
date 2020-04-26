using System;

namespace Dns
{
    /// <summary>
    /// Public Key (KEY) Record
    /// </summary>
    /// <remarks>
    /// Stores a public key that is related to a DNS domain name.
    /// This public key can be of a zone, a user, or a host or other end entity.
    /// A KEY resource record is authenticated by a SIG resource record.
    /// A zone level key must sign KEYs.
    /// </remarks>
    [Obsolete("RFC3755")]
    public class DnsKEYRecord : DnsRecord
    {
        private ushort flagsInitial;
        private byte protocolInitial;
        private byte algorithmInitial;
        private string publicKeyInitial;

        /// <summary>
        /// Set of flags described in RFC 2535.
        /// </summary>
        public ushort Flags { get; set; }
        /// <summary>
        /// Protocol for which the key specified in this record can be used.
        /// The assigned values include 1-5 and map to the protocols as follows. 1-TLS, 2-email, 3-dnssec, 4-IPSEC, 255-All 
        /// </summary>
        public byte Protocol { get; set; }
        /// <summary>
        /// Algorithm that can be used with the key specified in this record.
        /// The assigned values include 1-4 and map to the algorithms as follows.
        /// 1-RSA/MD5 [RFC 2537], 2-Diffie-Hellman [RFC 2539], 3-DSA [RFC 2536], 4- elliptic curve crypto 
        /// </summary>
        public byte Algorithm { get; set; }
        /// <summary>
        /// Public key is represented in base 64 as described in the RFC 2535, Appendix A. 
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsKEYRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="flags">Set of flags described in RFC 2535.</param>
        /// <param name="protocol">Protocol for which the key specified in this record can be used.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="publicKey">Public key is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        private DnsKEYRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort flags, byte protocol, byte algorithm, string publicKey)
            : base(zone, providerState, name, DnsRecordTypes.KEY, @class, timeToLive)
        {
            Flags = flags;
            flagsInitial = flags;

            Protocol = protocol;
            protocolInitial = protocol;

            Algorithm = algorithm;
            algorithmInitial = algorithm;

            PublicKey = publicKey;
            publicKeyInitial = publicKey;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsKEYRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="flags">Set of flags described in RFC 2535.</param>
        /// <param name="protocol">Protocol for which the key specified in this record can be used.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="publicKey">Public key is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsKEYRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort flags, byte protocol, byte algorithm, string publicKey)
            : this(zone, providerState: null, name, @class, timeToLive, flags, protocol, algorithm, publicKey)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsKEYRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="flags">Set of flags described in RFC 2535.</param>
        /// <param name="protocol">Protocol for which the key specified in this record can be used.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="publicKey">Public key is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsKEYRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort flags, byte protocol, byte algorithm, string publicKey)
            : this(zone: null, name, @class, timeToLive, flags, protocol, algorithm, publicKey)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsKEYRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="flags">Set of flags described in RFC 2535.</param>
        /// <param name="protocol">Protocol for which the key specified in this record can be used.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="publicKey">Public key is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsKEYRecord(DnsZone zone, string name, TimeSpan timeToLive, ushort flags, byte protocol, byte algorithm, string publicKey)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, flags, protocol, algorithm, publicKey)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsKEYRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="flags">Set of flags described in RFC 2535.</param>
        /// <param name="protocol">Protocol for which the key specified in this record can be used.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="publicKey">Public key is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsKEYRecord(string name, TimeSpan timeToLive, ushort flags, byte protocol, byte algorithm, string publicKey)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, flags, protocol, algorithm, publicKey)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
        {
            return
                flagsInitial != Flags ||
                protocolInitial != Protocol ||
                algorithmInitial != Algorithm ||
                !string.Equals(publicKeyInitial, PublicKey, StringComparison.Ordinal);
        }

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            flagsInitial = Flags;
            protocolInitial = Protocol;
            algorithmInitial = Algorithm;
            publicKeyInitial = PublicKey;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsKEYRecord(zone, providerState, Name, Class, TimeToLive, Flags, Protocol, Algorithm, PublicKey);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"[{Flags:X2}; {Protocol}; {Algorithm}] {PublicKey}";

    }
}
