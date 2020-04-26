using System;

namespace Dns
{
    /// <summary>
    /// Cryptographic Signature (SIG) Record
    /// </summary>
    /// <remarks>
    /// Authenticates a resource record set of a particular type, class, and name and
    /// binds it to a time interval and the signer's DNS domain name. This authentication
    /// and binding is done using cryptographic techniques and the signer's private key.
    /// The signer is frequently the owner of the zone from which the resource record originated.
    /// </remarks>
    [Obsolete("RFC3755")]
    public class DnsSIGRecord : DnsRecord
    {
        private ushort typeCoveredInitial;
        private ushort algorithmInitial;
        private ushort labelsInitial;
        private uint originalTtlInitial;
        private uint signatureExpirationInitial;
        private uint signatureInceptionInitial;
        private ushort keyTagInitial;
        private string signerNameInitial;
        private string signatureInitial;

        /// <summary>
        /// Type of the RR covered by this SIG.
        /// </summary>
        public ushort TypeCovered { get; set; }
        /// <summary>
        /// Algorithm that can be used with the key specified in this record.
        /// The assigned values include 1-4 and map to the algorithms as follows.
        /// 1-RSA/MD5 [RFC 2537], 2-Diffie-Hellman [RFC 2539], 3-DSA [RFC 2536], 4- elliptic curve crypto 
        /// </summary>
        public ushort Algorithm { get; set; }
        /// <summary>
        /// The "labels" octet is an unsigned count of how many 
        /// labels there are in the original SIG RR owner name not counting the 
        /// null label for root and not counting any initial "*" for a wildcard. 
        /// </summary>
        public ushort Labels { get; set; }
        /// <summary>
        /// The TTL of the RRset signed by this SIG 
        /// </summary>
        public uint OriginalTTL { get; set; }
        /// <summary>
        /// SIG is valid until Signature Expiration. Signature Expiration 
        /// is specified in number of seconds since the start of 1 January 1970,
        /// GMT, ignoring leap seconds. 
        /// </summary>
        public uint SignatureExpiration { get; set; }
        /// <summary>
        /// SIG is valid after Signature Inception. Signature Inception is
        /// specified in number of seconds since the start of 1 January 1970,
        /// GMT, ignoring leap seconds. 
        /// </summary>
        public uint SignatureInception { get; set; }
        /// <summary>
        /// KeyTag provides an efficient away to choose a Key to verify SIG is more than
        /// one Keys are available. RFC 2535, Appendix C describes how to calculate a KeyTag.
        /// </summary>
        public ushort KeyTag { get; set; }
        /// <summary>
        /// The signer name is the domain name of the signer generating the SIG RR. 
        /// </summary>
        public string SignerName { get; set; }
        /// <summary>
        /// Signature is represented in base 64 as described in the RFC 2535, Appendix A. 
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSIGRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="typeCovered">Type of the RR covered by this SIG.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="labels">Label count</param>
        /// <param name="originalTTL">The TTL of the RRset signed by this SIG </param>
        /// <param name="signatureExpiration">SIG is valid until Signature Expiration.</param>
        /// <param name="signatureInception">SIG is valid after Signature Inception.</param>
        /// <param name="keyTag">KeyTag provides an efficient away to choose a Key to verify SIG is more than one Keys are available.</param>
        /// <param name="signerName">The signer name is the domain name of the signer generating the SIG RR.</param>
        /// <param name="signature">Signature is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        private DnsSIGRecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort typeCovered, ushort algorithm, ushort labels, uint originalTTL, uint signatureExpiration, uint signatureInception, ushort keyTag, string signerName, string signature)
            : base(zone, providerState, name, DnsRecordTypes.SIG, @class, timeToLive)
        {
            TypeCovered = typeCovered;
            typeCoveredInitial = typeCovered;

            Algorithm = algorithm;
            algorithmInitial = algorithm;

            Labels = labels;
            labelsInitial = labels;

            OriginalTTL = originalTTL;
            originalTtlInitial = originalTTL;

            SignatureExpiration = signatureExpiration;
            signatureExpirationInitial = signatureExpiration;

            SignatureInception = signatureInception;
            signatureInceptionInitial = signatureInception;

            KeyTag = keyTag;
            keyTagInitial = keyTag;

            SignerName = signerName;
            signerNameInitial = signerName;

            Signature = signature;
            signatureInitial = signature;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSIGRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="typeCovered">Type of the RR covered by this SIG.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="labels">Label count</param>
        /// <param name="originalTTL">The TTL of the RRset signed by this SIG </param>
        /// <param name="signatureExpiration">SIG is valid until Signature Expiration.</param>
        /// <param name="signatureInception">SIG is valid after Signature Inception.</param>
        /// <param name="keyTag">KeyTag provides an efficient away to choose a Key to verify SIG is more than one Keys are available.</param>
        /// <param name="signerName">The signer name is the domain name of the signer generating the SIG RR.</param>
        /// <param name="signature">Signature is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsSIGRecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort typeCovered, ushort algorithm, ushort labels, uint originalTTL, uint signatureExpiration, uint signatureInception, ushort keyTag, string signerName, string signature)
            : this(zone, providerState: null, name, @class, timeToLive, typeCovered, algorithm, labels, originalTTL, signatureExpiration, signatureInception, keyTag, signerName, signature)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSIGRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="typeCovered">Type of the RR covered by this SIG.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="labels">Label count</param>
        /// <param name="originalTTL">The TTL of the RRset signed by this SIG </param>
        /// <param name="signatureExpiration">SIG is valid until Signature Expiration.</param>
        /// <param name="signatureInception">SIG is valid after Signature Inception.</param>
        /// <param name="keyTag">KeyTag provides an efficient away to choose a Key to verify SIG is more than one Keys are available.</param>
        /// <param name="signerName">The signer name is the domain name of the signer generating the SIG RR.</param>
        /// <param name="signature">Signature is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsSIGRecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, ushort typeCovered, ushort algorithm, ushort labels, uint originalTTL, uint signatureExpiration, uint signatureInception, ushort keyTag, string signerName, string signature)
            : this(zone: null, name, @class, timeToLive, typeCovered, algorithm, labels, originalTTL, signatureExpiration, signatureInception, keyTag, signerName, signature)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSIGRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="typeCovered">Type of the RR covered by this SIG.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="labels">Label count</param>
        /// <param name="originalTTL">The TTL of the RRset signed by this SIG </param>
        /// <param name="signatureExpiration">SIG is valid until Signature Expiration.</param>
        /// <param name="signatureInception">SIG is valid after Signature Inception.</param>
        /// <param name="keyTag">KeyTag provides an efficient away to choose a Key to verify SIG is more than one Keys are available.</param>
        /// <param name="signerName">The signer name is the domain name of the signer generating the SIG RR.</param>
        /// <param name="signature">Signature is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsSIGRecord(DnsZone zone, string name, TimeSpan timeToLive, ushort typeCovered, ushort algorithm, ushort labels, uint originalTTL, uint signatureExpiration, uint signatureInception, ushort keyTag, string signerName, string signature)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, typeCovered, algorithm, labels, originalTTL, signatureExpiration, signatureInception, keyTag, signerName, signature)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSIGRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="typeCovered">Type of the RR covered by this SIG.</param>
        /// <param name="algorithm">Algorithm that can be used with the key specified in this record.</param>
        /// <param name="labels">Label count</param>
        /// <param name="originalTTL">The TTL of the RRset signed by this SIG </param>
        /// <param name="signatureExpiration">SIG is valid until Signature Expiration.</param>
        /// <param name="signatureInception">SIG is valid after Signature Inception.</param>
        /// <param name="keyTag">KeyTag provides an efficient away to choose a Key to verify SIG is more than one Keys are available.</param>
        /// <param name="signerName">The signer name is the domain name of the signer generating the SIG RR.</param>
        /// <param name="signature">Signature is represented in base 64 as described in the RFC 2535, Appendix A.</param>
        public DnsSIGRecord(string name, TimeSpan timeToLive, ushort typeCovered, ushort algorithm, ushort labels, uint originalTTL, uint signatureExpiration, uint signatureInception, ushort keyTag, string signerName, string signature)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, typeCovered, algorithm, labels, originalTTL, signatureExpiration, signatureInception, keyTag, signerName, signature)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
        {
            return
                typeCoveredInitial != TypeCovered ||
                algorithmInitial != Algorithm ||
                labelsInitial != Labels ||
                originalTtlInitial != OriginalTTL ||
                signatureExpirationInitial != SignatureExpiration ||
                signatureInceptionInitial != SignatureInception ||
                keyTagInitial != KeyTag ||
                !string.Equals(signerNameInitial, SignerName, StringComparison.Ordinal) ||
                !string.Equals(signatureInitial, Signature, StringComparison.Ordinal);
        }

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            typeCoveredInitial = TypeCovered;
            algorithmInitial = Algorithm;
            labelsInitial = Labels;
            originalTtlInitial = OriginalTTL;
            signatureExpirationInitial = SignatureExpiration;
            signatureInceptionInitial = SignatureInception;
            keyTagInitial = KeyTag;
            signerNameInitial = SignerName;
            signatureInitial = Signature;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsSIGRecord(zone, providerState, Name, Class, TimeToLive, TypeCovered, Algorithm, Labels, OriginalTTL, SignatureExpiration, SignatureInception, KeyTag, SignerName, Signature);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{TypeCovered}; {Algorithm}; {Labels}; {OriginalTTL}; {SignatureExpiration}; {SignatureInception}; {KeyTag}; {SignerName}; {Signature}";

    }
}
