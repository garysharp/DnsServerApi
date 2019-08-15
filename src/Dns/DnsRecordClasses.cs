using System;

namespace Dns
{
    /// <summary>
    /// DNS Record Classes
    /// </summary>
    /// <remarks>
    /// Based on IANA Assignments
    /// https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-2
    /// </remarks>
    public enum DnsRecordClasses : ushort
    {
        /// <summary>
        /// Internet (IN) [RFC1035]
        /// </summary>
        IN = 1,
        /// <summary>
        /// CSNET (Obsolete - used only for examples in some obsolete RFCs)
        /// </summary>
        [Obsolete]
        CS = 2,
        /// <summary>
        /// Chaos (CH) [D. Moon, "Chaosnet", A.I. Memo 628, Massachusetts Institute of Technology Artificial Intelligence Laboratory, June 1981.]
        /// </summary>
        CH = 3,
        /// <summary>
        /// Hesiod (HS) [Dyer, S., and F. Hsu, "Hesiod", Project Athena Technical Plan - Name Service, April 1987.]
        /// </summary>
        HS = 4,
        /// <summary>
        /// QCLASS NONE [RFC2136]
        /// </summary>
        QClassNone = 254,
        /// <summary>
        /// QCLASS * (ANY) [RFC1035]
        /// </summary>
        QClassAny = 255,
    }
}
