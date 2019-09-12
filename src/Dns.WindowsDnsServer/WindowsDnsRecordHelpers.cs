using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Dns.WindowsDnsServer
{
    internal static class WindowsDnsRecordHelpers
    {
        private static readonly char[] recordDataSeperators = new char[] { ' ' };
        private static readonly Dictionary<string, DnsRecordClasses> classMap;
        private static readonly Dictionary<string, DnsRecordTypes> wmiClassMap;
        private static readonly Dictionary<DnsRecordTypes, string> wmiClassMapReverse;
        private static readonly Dictionary<DnsRecordTypes, Func<WindowsDnsZone, WindowsDnsRecordState, string, DnsRecordClasses, TimeSpan, string, DnsRecord>> recordParserMap;

        static WindowsDnsRecordHelpers()
        {
            classMap = Enum.GetValues(typeof(DnsRecordClasses))
                .Cast<DnsRecordClasses>()
                .ToDictionary(k => Enum.GetName(typeof(DnsRecordClasses), k), k => k);

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
            wmiClassMapReverse = wmiClassMap.ToDictionary(r => r.Value, r => r.Key);

            recordParserMap = new Dictionary<DnsRecordTypes, Func<WindowsDnsZone, WindowsDnsRecordState, string, DnsRecordClasses, TimeSpan, string, DnsRecord>>()
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

        public static DnsRecord ParseRecordInternal(this WindowsDnsZone zone, ManagementBaseObject managementObject)
        {
            var wmiClass = (string)managementObject["__CLASS"];
            var ownerName = (string)managementObject["OwnerName"];
            var recordClass = (ushort)managementObject["RecordClass"];
            var recordData = (string)managementObject["RecordData"];
            var ttl = (uint)managementObject["TTL"];

            var state = new WindowsDnsRecordState(managementObject);

            return ParseRecordInternal(zone, state, wmiClass, ownerName, recordClass, ttl, recordData);
        }

        public static DnsRecord ParseRecordInternal(this WindowsDnsZone zone, WindowsDnsRecordState state, string wmiClass, string ownerName, ushort recordClass, uint timeToLive, string data)
        {
            if (!wmiClassMap.TryGetValue(wmiClass, out var type))
                throw new ArgumentOutOfRangeException(nameof(wmiClass));
            var ttl = TimeSpan.FromSeconds(timeToLive);

            return ParseRecordInternal(zone, state, type, ownerName, (DnsRecordClasses)recordClass, ttl, data);
        }

        public static DnsRecord ParseRecordInternal(this WindowsDnsZone zone, WindowsDnsRecordState state, DnsRecordTypes type, string ownerName, DnsRecordClasses recordClass, TimeSpan timeToLive, string data)
        {
            if (!recordParserMap.TryGetValue(type, out var recordParser))
                throw new NotSupportedException($"The record type ({type}) is not supported");

            return recordParser(zone, state, ownerName, recordClass, timeToLive, data);
        }

        private static DnsARecord ParseARecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsARecord(zone, state, ownerName, timeToLive, DnsIpAddress.FromString(data));

        private static DnsCNAMERecord ParseCNAMERecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsCNAMERecord(zone, state, ownerName, @class, timeToLive, data);

        private static DnsMXRecord ParseMXRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var unknownIndex = data.IndexOf(' ');
            var priorityIndex = data.IndexOf(' ', unknownIndex + 1);
            var priority = ushort.Parse(data.Substring(unknownIndex + 1, priorityIndex - unknownIndex - 1));

            return new DnsMXRecord(zone, state, ownerName, @class, timeToLive, priority, data.Substring(priorityIndex + 1));
        }

        private static DnsNSRecord ParseNSRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsNSRecord(zone, state, ownerName, @class, timeToLive, data);

        private static DnsPTRRecord ParsePTRRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsPTRRecord(zone, state, ownerName, @class, timeToLive, data);

        private static DnsSOARecord ParseSOARecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
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

            return new DnsSOARecord(zone, state, ownerName, @class, timeToLive, primaryServer, responsiblePerson, serial, refreshInterval, retryDelay, expireLimit, minimumTtl);
        }

        private static DnsSRVRecord ParseSRVRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var arguments = data.Split(recordDataSeperators, StringSplitOptions.None);

            if (arguments.Length < 4)
                throw new ArgumentException("Unexpected data for SRV record", nameof(data));

            var priority = ushort.Parse(arguments[0]);
            var weight = ushort.Parse(arguments[1]);
            var port = ushort.Parse(arguments[2]);
            var target = arguments[3];

            return new DnsSRVRecord(zone, state, ownerName, timeToLive, priority, weight, port, target);
        }


        public static DnsRecord CreateRecordInternal(this WindowsDnsZone zone, DnsRecord templateRecord)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));
            if (templateRecord == null)
                throw new ArgumentNullException(nameof(templateRecord));

            if (!wmiClassMapReverse.TryGetValue(templateRecord.Type, out var wmiClassName))
                throw new NotSupportedException("This record type is not supported");

            using (var wmiClass = zone.server.WmiGetClass(wmiClassName))
            {
                using (var wmiParams = wmiClass.GetMethodParameters("CreateInstanceFromPropertyData"))
                {
                    wmiParams["ContainerName"] = zone.DomainName;
                    wmiParams["DnsServerName"] = zone.server.Name;
                    wmiParams["TTL"] = (uint)templateRecord.TimeToLive.TotalSeconds;
                    wmiParams["OwnerName"] = templateRecord.Name;
                    wmiParams["RecordClass"] = (uint)templateRecord.Class;

                    // populate record-specific parameters
                    switch (templateRecord)
                    {
                        case DnsARecord aRecord:
                            CreateARecordParamters(zone, aRecord, wmiParams);
                            break;
                        case DnsCNAMERecord cNAMERecord:
                            CreateCNameRecordParamters(zone, cNAMERecord, wmiParams);
                            break;
                        default:
                            throw new NotSupportedException("This record type is not supported");
                    }

                    var wmiResult = wmiClass.InvokeMethod("CreateInstanceFromPropertyData", wmiParams, null);
                    var wmiPath = (string)wmiResult["RR"];
                    var state = new WindowsDnsRecordState(wmiPath);

                    return templateRecord.Clone(zone, state);
                }
            }
        }

        private static void CreateARecordParamters(WindowsDnsZone zone, DnsARecord templateRecord, ManagementBaseObject wmiParams)
        {
            wmiParams["IPAddress"] = templateRecord.IpAddress.ToString();
        }

        private static void CreateCNameRecordParamters(WindowsDnsZone zone, DnsCNAMERecord templateRecord, ManagementBaseObject wmiParams)
        {
            wmiParams["PrimaryName"] = templateRecord.PrimaryName;
        }

    }
}
