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
                { DnsRecordTypes.AAAA, ParseAAAARecord },
                { DnsRecordTypes.AFSDB, ParseAFSDBRecord },
                { DnsRecordTypes.A, ParseARecord },
                { DnsRecordTypes.CNAME, ParseCNAMERecord },
                { DnsRecordTypes.MX, ParseMXRecord },
                { DnsRecordTypes.NS, ParseNSRecord },
                { DnsRecordTypes.PTR, ParsePTRRecord },
                { DnsRecordTypes.SOA, ParseSOARecord },
                { DnsRecordTypes.SRV, ParseSRVRecord },
                { DnsRecordTypes.X25, ParseX25Record },
            };
        }

        #region Record Parsing

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

            var record = recordParser(zone, state, ownerName, recordClass, timeToLive, data);

            zone.SetRecordStateInternal(record, state);

            return record;
        }

        private static DnsAAAARecord ParseAAAARecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsAAAARecord(zone, ownerName, timeToLive, DnsIpAddress.FromString(data));

        private static DnsAFSDBRecord ParseAFSDBRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var unknownIndex = data.IndexOf(' ');
            var subtypeIndex = data.IndexOf(' ', unknownIndex + 1);
            var subtype = ushort.Parse(data.Substring(unknownIndex + 1, subtypeIndex - unknownIndex - 1));

            return new DnsAFSDBRecord(zone, ownerName, @class, timeToLive, subtype, data.Substring(subtypeIndex + 1));
        }

        private static DnsARecord ParseARecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsARecord(zone, ownerName, timeToLive, DnsIpAddress.FromString(data));

        private static DnsCNAMERecord ParseCNAMERecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsCNAMERecord(zone, ownerName, @class, timeToLive, data);

        private static DnsMXRecord ParseMXRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var unknownIndex = data.IndexOf(' ');
            var priorityIndex = data.IndexOf(' ', unknownIndex + 1);
            var priority = ushort.Parse(data.Substring(unknownIndex + 1, priorityIndex - unknownIndex - 1));

            return new DnsMXRecord(zone, ownerName, @class, timeToLive, priority, data.Substring(priorityIndex + 1));
        }

        private static DnsNSRecord ParseNSRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsNSRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsPTRRecord ParsePTRRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsPTRRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsSOARecord ParseSOARecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var arguments = data.Split(recordDataSeperators, StringSplitOptions.None);

            if (arguments.Length < 8)
                throw new ArgumentException("Unexpected data for SOA record", nameof(data));

            var primaryServer = arguments[0];
            var responsiblePerson = arguments[1];
            var serial = uint.Parse(arguments[3]);
            var refreshInterval = TimeSpan.FromSeconds(uint.Parse(arguments[4]));
            var retryDelay = TimeSpan.FromSeconds(uint.Parse(arguments[5]));
            var expireLimit = TimeSpan.FromSeconds(uint.Parse(arguments[6]));
            var minimumTtl = TimeSpan.FromSeconds(uint.Parse(arguments[7]));

            return new DnsSOARecord(zone, ownerName, @class, timeToLive, primaryServer, responsiblePerson, serial, refreshInterval, retryDelay, expireLimit, minimumTtl);
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

            return new DnsSRVRecord(zone, ownerName, timeToLive, priority, weight, port, target);
        }

        private static DnsX25Record ParseX25Record(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsX25Record(zone, ownerName, @class, timeToLive, data);

        #endregion

        #region Record Deletion

        public static void DeleteRecordInternal(this WindowsDnsZone zone, DnsRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            var state = zone.GetRecordStateInternal(record);

            if (state == null)
                throw new ArgumentException("Record must be created by the provider");

            using (var instance = zone.server.WmiGetInstance(state.WmiPath))
            {
                instance.Delete();
            }
        }

        #endregion

        #region Record Creation

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
                    PopulateRecordParameters(zone, templateRecord, wmiParams);

                    var wmiResult = wmiClass.InvokeMethod("CreateInstanceFromPropertyData", wmiParams, null);
                    var wmiPath = (string)wmiResult["RR"];
                    var state = new WindowsDnsRecordState(wmiPath);

                    return zone.CloneRecordInternal(templateRecord, state);
                }
            }
        }

        #endregion

        #region Record Modify

        public static void ModifyRecordInternal(this WindowsDnsZone zone, DnsRecord record)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            var state = zone.GetRecordStateInternal(record);

            if (state == null)
                throw new ArgumentException("Record must be created by the provider");

            using (var instance = zone.server.WmiGetInstance(state.WmiPath))
            {
                using (var wmiParams = instance.GetMethodParameters("Modify"))
                {
                    wmiParams["TTL"] = (uint)record.TimeToLive.TotalSeconds;

                    // populate record-specific parameters
                    PopulateRecordParameters(zone, record, wmiParams);

                    var wmiResult = instance.InvokeMethod("Modify", wmiParams, null);
                    var wmiPath = (string)wmiResult["RR"];
                    var newState = new WindowsDnsRecordState(wmiPath);

                    zone.SetRecordStateInternal(record, newState);
                }
            }
        }

        #endregion

        #region Record Create/Modify Parameters

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsRecord record, ManagementBaseObject wmiParams)
        {
            switch (record)
            {
                case DnsAAAARecord aAAARecord:
                    PopulateRecordParameters(zone, aAAARecord, wmiParams);
                    break;
                case DnsAFSDBRecord aFSDBRecord:
                    PopulateRecordParameters(zone, aFSDBRecord, wmiParams);
                    break;
                case DnsARecord aRecord:
                    PopulateRecordParameters(zone, aRecord, wmiParams);
                    break;
                case DnsCNAMERecord cNAMERecord:
                    PopulateRecordParameters(zone, cNAMERecord, wmiParams);
                    break;
                case DnsMXRecord mXRecord:
                    PopulateRecordParameters(zone, mXRecord, wmiParams);
                    break;
                case DnsNSRecord nSRecord:
                    PopulateRecordParameters(zone, nSRecord, wmiParams);
                    break;
                case DnsPTRRecord pTRRecord:
                    PopulateRecordParameters(zone, pTRRecord, wmiParams);
                    break;
                case DnsSOARecord sOARecord:
                    PopulateRecordParameters(zone, sOARecord, wmiParams);
                    break;
                case DnsSRVRecord sRVRecord:
                    PopulateRecordParameters(zone, sRVRecord, wmiParams);
                    break;
                case DnsX25Record x25Record:
                    PopulateRecordParameters(zone, x25Record, wmiParams);
                    break;
                default:
                    throw new NotSupportedException("This record type is not supported");
            }
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsAAAARecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["IPv6Address"] = record.IpV6Address;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsAFSDBRecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["ServerName"] = record.HostName;
            wmiParams["Subtype"] = record.Subtype;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsARecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["IPAddress"] = record.IpAddress.ToString();
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsCNAMERecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["PrimaryName"] = record.PrimaryName;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsMXRecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["MailExchange"] = record.DomainName;
            wmiParams["Preference"] = record.Preference;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsNSRecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["NSHost"] = record.NameServer;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsPTRRecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["PTRDomainName"] = record.DomainName;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsSOARecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["ExpireLimit"] = (uint)record.ExpireLimit.TotalSeconds;
            wmiParams["MinimumTTL"] = (uint)record.MinimumTimeToLive.TotalSeconds;
            wmiParams["PrimaryServer"] = record.PrimaryServer;
            wmiParams["RefreshInterval"] = (uint)record.RefreshInterval.TotalSeconds;
            wmiParams["ResponsibleParty"] = record.ResponsiblePerson;
            wmiParams["RetryDelay"] = (uint)record.RetryDelay.TotalSeconds;
            wmiParams["SerialNumber"] = record.Serial;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsSRVRecord record, ManagementBaseObject wmiParams)
        {
            wmiParams["Port"] = record.Port;
            wmiParams["Priority"] = record.Priority;
            wmiParams["SRVDomainName"] = record.TargetDomainName;
            wmiParams["Weight"] = record.Weight;
        }

        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsX25Record record, ManagementBaseObject wmiParams)
        {
            wmiParams["PSDNAddress"] = record.PsdnAddress;
        }

        #endregion

        public static IEnumerable<DnsRecord> GetRecordsInternal(this WindowsDnsZone zone, DnsRecordTypes recordType)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            if (!wmiClassMapReverse.TryGetValue(recordType, out var wmiClassName))
                throw new NotSupportedException($"The record type ({recordType}) is not supported");

            var wmiResults = zone.server.WmiQuery($"SELECT * FROM {wmiClassName} WHERE ContainerName='{zone.DomainName}'");

            foreach (var wmiResult in wmiResults)
            {
                yield return ParseRecordInternal(zone, wmiResult);
            }
        }

    }
}
