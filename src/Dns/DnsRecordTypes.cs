using System;

namespace Dns
{
    /// <summary>
    /// DNS Record Types
    /// </summary>
    /// <remarks>
    /// Based on IANA Assignments (with some modifications)
    /// https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-4
    /// </remarks>
    public enum DnsRecordTypes : ushort
    {
        /// <summary>
        /// a host address. [RFC1035]
        /// </summary>
        A = 1,
        /// <summary>
        /// an authoritative name server. [RFC1035]
        /// </summary>
        NS = 2,
        /// <summary>
        /// a mail destination (OBSOLETE - use MX). [RFC1035]
        /// </summary>
        [Obsolete("Use MX; RFC973")]
        MD = 3,
        /// <summary>
        /// a mail forwarder (OBSOLETE - use MX). [RFC1035]
        /// </summary>
        [Obsolete("Use MX; RFC973")]
        MF = 4,
        /// <summary>
        /// the canonical name for an alias. [RFC1035]
        /// </summary>
        CNAME = 5,
        /// <summary>
        /// marks the start of a zone of authority. [RFC1035]
        /// </summary>
        SOA = 6,
        /// <summary>
        /// a mailbox domain name (EXPERIMENTAL). [RFC1035]
        /// </summary>
        [Obsolete("Experimental; RFC2505")]
        MB = 7,
        /// <summary>
        /// a mail group member (EXPERIMENTAL). [RFC1035]
        /// </summary>
        [Obsolete("Experimental; RFC2505")]
        MG = 8,
        /// <summary>
        /// a mail rename domain name (EXPERIMENTAL). [RFC1035]
        /// </summary>
        [Obsolete("Experimental; RFC2505")]
        MR = 9,
        /// <summary>
        /// a null RR (EXPERIMENTAL). [RFC1035]
        /// </summary>
        [Obsolete("RFC1035")]
        NULL = 10,
        /// <summary>
        /// a well known service description. [RFC1035]
        /// </summary>
        [Obsolete("RFC1123 RFC1127")]
        WKS = 11,
        /// <summary>
        /// a domain name pointer. [RFC1035]
        /// </summary>
        PTR = 12,
        /// <summary>
        /// host information. [RFC1035]
        /// </summary>
        HINFO = 13,
        /// <summary>
        /// mailbox or mail list information. [RFC1035]
        /// </summary>
        [Obsolete("Experimental; RFC2505")]
        MINFO = 14,
        /// <summary>
        /// mail exchange. [RFC1035]
        /// </summary>
        MX = 15,
        /// <summary>
        /// text strings. [RFC1035]
        /// </summary>
        TXT = 16,
        /// <summary>
        /// for Responsible Person. [RFC1183]
        /// </summary>
        RP = 17,
        /// <summary>
        /// for AFS Data Base location. [RFC1183][RFC5864]
        /// </summary>
        AFSDB = 18,
        /// <summary>
        /// for X.25 PSDN address. [RFC1183]
        /// </summary>
        X25 = 19,
        /// <summary>
        /// for ISDN address. [RFC1183]
        /// </summary>
        ISDN = 20,
        /// <summary>
        /// for Route Through. [RFC1183]
        /// </summary>
        RT = 21,
        /// <summary>
        /// for NSAP address, NSAP style A record. [RFC1706]
        /// </summary>
        NSAP = 22,
        /// <summary>
        /// for domain name pointer, NSAP style. [RFC1348][RFC1637][RFC1706]
        /// </summary>
        NSAP_PTR = 23,
        /// <summary>
        /// for security signature. [RFC4034][RFC3755][RFC2535][RFC2536][RFC2537][RFC2931][RFC3110][RFC3008]
        /// </summary>
        [Obsolete("RFC3755")]
        SIG = 24,
        /// <summary>
        /// for security key. [RFC4034][RFC3755][RFC2535][RFC2536][RFC2537][RFC2539][RFC3008][RFC3110]
        /// </summary>
        [Obsolete("RFC3755")]
        KEY = 25,
        /// <summary>
        /// X.400 mail mapping information. [RFC2163]
        /// </summary>
        PX = 26,
        /// <summary>
        /// Geographical Position. [RFC1712]
        /// </summary>
        GPOS = 27,
        /// <summary>
        /// IP6 Address. [RFC3596]
        /// </summary>
        AAAA = 28,
        /// <summary>
        /// Location Information. [RFC1876]
        /// </summary>
        LOC = 29,
        /// <summary>
        /// Next Domain (OBSOLETE). [RFC3755][RFC2535]
        /// </summary>
        [Obsolete("RFC3755")]
        NXT = 30,
        /// <summary>
        /// Endpoint Identifier. [Michael_Patton][http://ana-3.lcs.mit.edu/~jnc/nimrod/dns.txt]
        /// </summary>
        EID = 31,
        /// <summary>
        /// Nimrod Locator. [1][Michael_Patton][http://ana-3.lcs.mit.edu/~jnc/nimrod/dns.txt]
        /// </summary>
        NIMLOC = 32,
        /// <summary>
        /// Server Selection. [1][RFC2782]
        /// </summary>
        SRV = 33,
        /// <summary>
        /// ATM Address. [ATM Forum Technical Committee, ""ATM Name System, V2.0"", Doc ID: AF-DANS-0152.000, July 2000. Available from and held in escrow by IANA.]
        /// </summary>
        ATMA = 34,
        /// <summary>
        /// Naming Authority Pointer. [RFC2915][RFC2168][RFC3403]
        /// </summary>
        NAPTR = 35,
        /// <summary>
        /// Key Exchanger. [RFC2230]
        /// </summary>
        KX = 36,
        /// <summary>
        /// CERT. [RFC4398]
        /// </summary>
        CERT = 37,
        /// <summary>
        /// A6 (OBSOLETE - use AAAA). [RFC3226][RFC2874][RFC6563]
        /// </summary>
        [Obsolete("Use AAAA; RFC6563")]
        A6 = 38,
        /// <summary>
        /// DNAME. [RFC6672]
        /// </summary>
        DNAME = 39,
        /// <summary>
        /// SINK. [Donald_E_Eastlake][http://tools.ietf.org/html/draft-eastlake-kitchen-sink]
        /// </summary>
        SINK = 40,
        /// <summary>
        /// OPT. [RFC6891][RFC3225]
        /// </summary>
        OPT = 41,
        /// <summary>
        /// APL. [RFC3123]
        /// </summary>
        APL = 42,
        /// <summary>
        /// Delegation Signer. [RFC4034][RFC3658]
        /// </summary>
        DS = 43,
        /// <summary>
        /// SSH Key Fingerprint. [RFC4255]
        /// </summary>
        SSHFP = 44,
        /// <summary>
        /// IPSECKEY. [RFC4025]
        /// </summary>
        IPSECKEY = 45,
        /// <summary>
        /// RRSIG. [RFC4034][RFC3755]
        /// </summary>
        RRSIG = 46,
        /// <summary>
        /// NSEC. [RFC4034][RFC3755]
        /// </summary>
        NSEC = 47,
        /// <summary>
        /// DNSKEY. [RFC4034][RFC3755]
        /// </summary>
        DNSKEY = 48,
        /// <summary>
        /// DHCID. [RFC4701]
        /// </summary>
        DHCID = 49,
        /// <summary>
        /// NSEC3. [RFC5155]
        /// </summary>
        NSEC3 = 50,
        /// <summary>
        /// NSEC3PARAM. [RFC5155]
        /// </summary>
        NSEC3PARAM = 51,
        /// <summary>
        /// TLSA. [RFC6698]
        /// </summary>
        TLSA = 52,
        /// <summary>
        /// S/MIME cert association. [RFC8162]
        /// </summary>
        SMIMEA = 53,
        /// <summary>
        /// Host Identity Protocol. [RFC8005]
        /// </summary>
        HIP = 55,
        /// <summary>
        /// NINFO. [Jim_Reid]
        /// </summary>
        NINFO = 56,
        /// <summary>
        /// RKEY. [Jim_Reid]
        /// </summary>
        RKEY = 57,
        /// <summary>
        /// Trust Anchor LINK. [Wouter_Wijngaards]
        /// </summary>
        TALINK = 58,
        /// <summary>
        /// Child DS. [RFC7344]
        /// </summary>
        CDS = 59,
        /// <summary>
        /// DNSKEY(s) the Child wants reflected in DS. [RFC7344]
        /// </summary>
        CDNSKEY = 60,
        /// <summary>
        /// OpenPGP Key. [RFC7929]
        /// </summary>
        OPENPGPKEY = 61,
        /// <summary>
        /// Child-To-Parent Synchronization. [RFC7477]
        /// </summary>
        CSYNC = 62,
        /// <summary>
        /// message digest for DNS zone. [draft-wessels-dns-zone-digest]
        /// </summary>
        ZONEMD = 63,
        /// <summary>
        /// [RFC7208]
        /// </summary>
        [Obsolete("RFC4408")]
        SPF = 99,
        /// <summary>
        /// [IANA-Reserved]
        /// </summary>
        UINFO = 100,
        /// <summary>
        /// [IANA-Reserved]
        /// </summary>
        UID = 101,
        /// <summary>
        /// [IANA-Reserved]
        /// </summary>
        GID = 102,
        /// <summary>
        /// [IANA-Reserved]
        /// </summary>
        UNSPEC = 103,
        /// <summary>
        /// [RFC6742]
        /// </summary>
        NID = 104,
        /// <summary>
        /// [RFC6742]
        /// </summary>
        L32 = 105,
        /// <summary>
        /// [RFC6742]
        /// </summary>
        L64 = 106,
        /// <summary>
        /// [RFC6742]
        /// </summary>
        LP = 107,
        /// <summary>
        /// an EUI-48 address. [RFC7043]
        /// </summary>
        EUI48 = 108,
        /// <summary>
        /// an EUI-64 address. [RFC7043]
        /// </summary>
        EUI64 = 109,
        /// <summary>
        /// Transaction Key. [RFC2930]
        /// </summary>
        TKEY = 249,
        /// <summary>
        /// Transaction Signature. [RFC2845]
        /// </summary>
        TSIG = 250,
        /// <summary>
        /// incremental transfer. [RFC1995]
        /// </summary>
        IXFR = 251,
        /// <summary>
        /// transfer of an entire zone. [RFC1035][RFC5936]
        /// </summary>
        AXFR = 252,
        /// <summary>
        /// mailbox-related RRs (MB, MG or MR). [RFC1035]
        /// </summary>
        [Obsolete("RFC2505")]
        MAILB = 253,
        /// <summary>
        /// mail agent RRs (OBSOLETE - see MX). [RFC1035]
        /// </summary>
        [Obsolete("Use MX; RFC973")]
        MAILA = 254,
        /// <summary>
        /// A request for some or all records the server has available. [RFC1035][RFC6895][RFC8482]
        /// </summary>
        Any = 255,
        /// <summary>
        /// URI. [RFC7553]
        /// </summary>
        URI = 256,
        /// <summary>
        /// Certification Authority Restriction. [RFC-ietf-lamps-rfc6844bis-07]
        /// </summary>
        CAA = 257,
        /// <summary>
        /// Application Visibility and Control. [Wolfgang_Riedel]
        /// </summary>
        AVC = 258,
        /// <summary>
        /// Digital Object Architecture. [draft-durand-doa-over-dns]
        /// </summary>
        DOA = 259,
        /// <summary>
        /// Automatic Multicast Tunneling Relay. [draft-ietf-mboned-driad-amt-discovery]
        /// </summary>
        AMTRELAY = 260,
        /// <summary>
        /// DNSSEC Trust Authorities. [Sam_Weiler][http://cameo.library.cmu.edu/][Deploying DNSSEC Without a Signed Root.  Technical Report 1999-19, Information Networking Institute, Carnegie Mellon University, April 2004.]
        /// </summary>
        TA = 32768,
        /// <summary>
        /// DNSSEC Lookaside Validation. [RFC4431]
        /// </summary>
        DLV = 32769,
        /// <summary>
        /// Windows Internet Name Service [MS-WINSRA]
        /// </summary>
        [Obsolete]
        WINS = 65281,
        /// <summary>
        /// Windows Internet Name Service reverse-lookup [MS-WINSRA]
        /// </summary>
        [Obsolete]
        WINSR = 65282,
    }
}
