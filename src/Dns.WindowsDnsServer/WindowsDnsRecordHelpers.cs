using System;
using System.Collections.Generic;
using System.Linq;

namespace Dns.WindowsDnsServer
{
    internal static class WindowsDnsRecordHelpers
    {
        private static readonly char[] recordDataSeperators = new char[] { ' ' };
        private static readonly Dictionary<string, DnsRecordClasses> classMap;
        private static readonly Dictionary<string, DnsRecordTypes> wmiClassMap;
        private static readonly Dictionary<DnsRecordTypes, Func<WindowsDnsZone, string, DnsRecordClasses, TimeSpan, string, DnsRecord>> recordParserMap;

        static WindowsDnsRecordHelpers()
        {
            classMap = Enum.GetNames(typeof(DnsRecordClasses)).ToDictionary(k => k, k => (DnsRecordClasses)Enum.Parse(typeof(DnsRecordClasses), k));
            wmiClassMap = new Dictionary<string, DnsRecordTypes>()
            {
                { "MicrosoftDNS_AAAAType", DnsRecordTypes.AAAA },
                { "MicrosoftDNS_AFSDBType", DnsRecordTypes.AFSDB },
                { "MicrosoftDNS_ATMAType", DnsRecordTypes.ATMA },
                { "MicrosoftDNS_AType", DnsRecordTypes.A },
                { "MicrosoftDNS_CNAMEType", DnsRecordTypes.CNAME },
                { "MicrosoftDNS_HINFOType", DnsRecordTypes.HINFO },
                { "MicrosoftDNS_ISDNType", DnsRecordTypes.ISDN },
                { "MicrosoftDNS_KEYType", DnsRecordTypes.KEY },
                { "MicrosoftDNS_MBType", DnsRecordTypes.MB },
                { "MicrosoftDNS_MDType", DnsRecordTypes.MD },
                { "MicrosoftDNS_MFType", DnsRecordTypes.MF },
                { "MicrosoftDNS_MGType", DnsRecordTypes.MG },
                { "MicrosoftDNS_MINFOType", DnsRecordTypes.MINFO },
                { "MicrosoftDNS_MRType", DnsRecordTypes.MR },
                { "MicrosoftDNS_MXType", DnsRecordTypes.MX },
                { "MicrosoftDNS_NSType", DnsRecordTypes.NS },
                { "MicrosoftDNS_NXTType", DnsRecordTypes.NXT },
                { "MicrosoftDNS_PTRType", DnsRecordTypes.PTR },
                { "MicrosoftDNS_RPType", DnsRecordTypes.RP },
                { "MicrosoftDNS_RTType", DnsRecordTypes.RT },
                { "MicrosoftDNS_SIGType", DnsRecordTypes.SIG },
                { "MicrosoftDNS_SOAType", DnsRecordTypes.SOA },
                { "MicrosoftDNS_SRVType", DnsRecordTypes.SRV },
                { "MicrosoftDNS_TXTType", DnsRecordTypes.TXT },
                //{ "MicrosoftDNS_WINSRType", DnsRecordTypes.WINSR },
                //{ "MicrosoftDNS_WINSType", DnsRecordTypes.WINS },
                { "MicrosoftDNS_WKSType", DnsRecordTypes.WKS },
                { "MicrosoftDNS_X25Type", DnsRecordTypes.X25 },
            };

            recordParserMap = new Dictionary<DnsRecordTypes, Func<WindowsDnsZone, string, DnsRecordClasses, TimeSpan, string, DnsRecord>>()
            {
                { DnsRecordTypes.A, ParseARecord },
                { DnsRecordTypes.CNAME, ParseCNAMERecord },
                { DnsRecordTypes.MX, ParseMXRecord },
                { DnsRecordTypes.NS, ParseNSRecord },
                { DnsRecordTypes.PTR, ParsePTRRecord },
                { DnsRecordTypes.SOA, ParseSOARecord },
                { DnsRecordTypes.SRV, ParseSRVRecord },
            };
        }

        public static DnsRecord ParseRecord(WindowsDnsZone zone, string recordTextRepresentation, uint timeToLive)
        {
            if (string.IsNullOrWhiteSpace(recordTextRepresentation))
                throw new ArgumentNullException(nameof(recordTextRepresentation));

            int ownerIndex = recordTextRepresentation.IndexOf(' ');
            int classIndex = recordTextRepresentation.IndexOf(' ', ownerIndex + 1);
            int typeIndex = recordTextRepresentation.IndexOf(' ', classIndex + 1);

            var ownerName = recordTextRepresentation.Substring(0, ownerIndex);
            var classString = recordTextRepresentation.Substring(ownerIndex + 1, classIndex - ownerIndex - 1);
            if (!classMap.TryGetValue(classString, out var recordClass))
                throw new ArgumentException("Invalid DNS Record Class", nameof(recordTextRepresentation));
            var typeString = recordTextRepresentation.Substring(classIndex + 1, typeIndex - classIndex - 1);
            if (!Enum.TryParse<DnsRecordTypes>(typeString, out var type))
                throw new ArgumentException("Invalid DNS Record Type", nameof(recordTextRepresentation));

            var ttl = TimeSpan.FromSeconds((int)timeToLive);

            return ParseRecord(zone, type, ownerName, recordClass, ttl, recordTextRepresentation.Substring(typeIndex + 1));
        }

        public static DnsRecord ParseRecord(WindowsDnsZone zone, string wmiClass, string ownerName, ushort recordClass, uint timeToLive, string data)
        {
            if (!wmiClassMap.TryGetValue(wmiClass, out var type))
                throw new ArgumentOutOfRangeException(nameof(wmiClass));
            var ttl = TimeSpan.FromSeconds(timeToLive);

            return ParseRecord(zone, type, ownerName, (DnsRecordClasses)recordClass, ttl, data);
        }

        public static DnsRecord ParseRecord(WindowsDnsZone zone, DnsRecordTypes type, string ownerName, DnsRecordClasses recordClass, TimeSpan timeToLive, string data)
        {
            if (!recordParserMap.TryGetValue(type, out var recordParser))
                throw new NotSupportedException($"The record type ({type}) is not supported");

            return recordParser(zone, ownerName, recordClass, timeToLive, data);
        }

        private static DnsARecord ParseARecord(WindowsDnsZone zone, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsARecord(zone, ownerName, timeToLive, DnsIpAddress.FromString(data));

        private static DnsCNAMERecord ParseCNAMERecord(WindowsDnsZone zone, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsCNAMERecord(zone, ownerName, timeToLive, data);

        private static DnsMXRecord ParseMXRecord(WindowsDnsZone zone, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var unknownIndex = data.IndexOf(' ');
            var priorityIndex = data.IndexOf(' ', unknownIndex + 1);
            var priority = ushort.Parse(data.Substring(unknownIndex + 1, priorityIndex - unknownIndex - 1));

            return new DnsMXRecord(zone, ownerName, timeToLive, priority, data.Substring(priorityIndex + 1));
        }

        private static DnsNSRecord ParseNSRecord(WindowsDnsZone zone, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsNSRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsPTRRecord ParsePTRRecord(WindowsDnsZone zone, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsPTRRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsSOARecord ParseSOARecord(WindowsDnsZone zone, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var arguments = data.Split(recordDataSeperators, StringSplitOptions.None);

            if (arguments.Length < 8)
                throw new ArgumentException("Unexpected data for SOA record", nameof(data));

            var primaryServer = arguments[0];
            var responsiblePerson = arguments[1];
            var serial = uint.Parse(arguments[3]);
            var refreshInterval = TimeSpan.FromSeconds(int.Parse(arguments[4]));
            var retryDelay = TimeSpan.FromSeconds(int.Parse(arguments[5]));
            var expireLimit = TimeSpan.FromSeconds(int.Parse(arguments[6]));
            var minimumTtl = TimeSpan.FromSeconds(int.Parse(arguments[7]));

            return new DnsSOARecord(zone, ownerName, @class, timeToLive, primaryServer, responsiblePerson, serial, refreshInterval, retryDelay, expireLimit, minimumTtl);
        }

        public static DnsSRVRecord ParseSRVRecord(WindowsDnsZone zone, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var arguments = data.Split(recordDataSeperators, StringSplitOptions.None);

            if (arguments.Length < 4)
                throw new ArgumentException("Unexpected data for SRV record", nameof(data));

            var priority = ushort.Parse(arguments[0]);
            var weight = ushort.Parse(arguments[1]);
            var port = ushort.Parse(arguments[2]);
            var target = arguments[3];

            return new DnsSRVRecord(zone, ownerName, timeToLive, priority, weight, port, target);
        }

    }
}
