using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Dns.WindowsDnsServer
{
#pragma warning disable CS0618 // Type or member is obsolete
    internal static class WindowsDnsRecordHelpers
    {
        private static readonly char[] recordDataSeperators = new char[] { ' ' };
        private static readonly Dictionary<string, DnsRecordClasses> classMap;
        private const string wmiClassBase = "MicrosoftDNS_ResourceRecord";
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
                // { "MicrosoftDNS_KEYType", DnsRecordTypes.KEY }, // unable to fetch via WMI; obsolete skipping; to do?
                { "MicrosoftDNS_MBType", DnsRecordTypes.MB },
                { "MicrosoftDNS_MDType", DnsRecordTypes.MD },
                { "MicrosoftDNS_MFType", DnsRecordTypes.MF },
                { "MicrosoftDNS_MGType", DnsRecordTypes.MG },
                { "MicrosoftDNS_MINFOType", DnsRecordTypes.MINFO },
                { "MicrosoftDNS_MRType", DnsRecordTypes.MR },
                { "MicrosoftDNS_MXType", DnsRecordTypes.MX },
                { "MicrosoftDNS_NSType", DnsRecordTypes.NS },
                // { "MicrosoftDNS_NXTType", DnsRecordTypes.NXT }, // unable to fetch via WMI; obsolete skipping; to do?
                { "MicrosoftDNS_PTRType", DnsRecordTypes.PTR },
                { "MicrosoftDNS_RPType", DnsRecordTypes.RP },
                { "MicrosoftDNS_RTType", DnsRecordTypes.RT },
                // { "MicrosoftDNS_SIGType", DnsRecordTypes.SIG }, // unable to fetch via WMI; obsolete skipping; to do?
                { "MicrosoftDNS_SOAType", DnsRecordTypes.SOA },
                { "MicrosoftDNS_SRVType", DnsRecordTypes.SRV },
                { "MicrosoftDNS_TXTType", DnsRecordTypes.TXT },
                // { "MicrosoftDNS_WINSRType", DnsRecordTypes.WINSR }, // unable to fetch via WMI; obsolete skipping; to do?
                // { "MicrosoftDNS_WINSType", DnsRecordTypes.WINS }, // unable to fetch via WMI; obsolete skipping; to do?
                { "MicrosoftDNS_WKSType", DnsRecordTypes.WKS },
                { "MicrosoftDNS_X25Type", DnsRecordTypes.X25 },
            };
            wmiClassMapReverse = wmiClassMap.ToDictionary(r => r.Value, r => r.Key);

            recordParserMap = new Dictionary<DnsRecordTypes, Func<WindowsDnsZone, WindowsDnsRecordState, string, DnsRecordClasses, TimeSpan, string, DnsRecord>>()
            {
                { DnsRecordTypes.AAAA, ParseAAAARecord },
                { DnsRecordTypes.AFSDB, ParseAFSDBRecord },
                { DnsRecordTypes.ATMA, ParseATMARecord },
                { DnsRecordTypes.A, ParseARecord },
                { DnsRecordTypes.CNAME, ParseCNAMERecord },
                { DnsRecordTypes.HINFO, ParseHINFORecord },
                { DnsRecordTypes.ISDN, ParseISDNRecord },
                // { DnsRecordTypes.KEY, ParseKEYRecord }, // unable to fetch via WMI; obsolete skipping; to do?
                { DnsRecordTypes.MB, ParseMBRecord },
                { DnsRecordTypes.MD, ParseMDRecord },
                { DnsRecordTypes.MF, ParseMFRecord },
                { DnsRecordTypes.MG, ParseMGRecord },
                { DnsRecordTypes.MINFO, ParseMINFORecord },
                { DnsRecordTypes.MR, ParseMRRecord },
                { DnsRecordTypes.MX, ParseMXRecord },
                { DnsRecordTypes.NS, ParseNSRecord },
                // { DnsRecordTypes.NXT, ParseNXTRecord }, // unable to fetch via WMI; obsolete skipping; to do?
                { DnsRecordTypes.PTR, ParsePTRRecord },
                { DnsRecordTypes.RP, ParseRPRecord },
                { DnsRecordTypes.RT, ParseRTRecord },
                //{ DnsRecordTypes.SIG, ParseSIGRecord }, // unable to fetch via WMI; obsolete skipping; to do?
                { DnsRecordTypes.SOA, ParseSOARecord },
                { DnsRecordTypes.SRV, ParseSRVRecord },
                { DnsRecordTypes.TXT, ParseTXTRecord },
                //{ DnsRecordTypes.WINSR, ParseWINSRRecord }, // unable to fetch via WMI; obsolete skipping; to do?
                //{ DnsRecordTypes.WINS, ParseWINSRecord }, // unable to fetch via WMI; obsolete skipping; to do?
                { DnsRecordTypes.WKS, ParseWKSRecord },
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
                throw new NotSupportedException($"The WMI Class ({wmiClass}) is not supported");
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

        private static DnsATMARecord ParseATMARecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var unknownIndex = data.IndexOf(' ');
            var formatIndex = data.IndexOf(' ', unknownIndex + 1);
            var format = ushort.Parse(data.Substring(unknownIndex + 1, formatIndex - unknownIndex - 1));

            return new DnsATMARecord(zone, ownerName, @class, timeToLive, format, data.Substring(formatIndex + 1));
        }

        private static DnsARecord ParseARecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsARecord(zone, ownerName, timeToLive, DnsIpAddress.FromString(data));

        private static DnsCNAMERecord ParseCNAMERecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsCNAMERecord(zone, ownerName, @class, timeToLive, data);

        private static DnsHINFORecord ParseHINFORecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var osIndex = data.IndexOf("\" \"", StringComparison.OrdinalIgnoreCase);
            var cpu = data.Substring(1, osIndex - 1);
            var os = data.Substring(osIndex + 3, data.Length - osIndex - 4);

            return new DnsHINFORecord(zone, ownerName, @class, timeToLive, cpu, os);
        }

        private static DnsISDNRecord ParseISDNRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var subAddressIndex = data.IndexOf("\" \"", StringComparison.OrdinalIgnoreCase);
            var isdnNumber = data.Substring(1, subAddressIndex - 1);
            var subAddress = data.Substring(subAddressIndex + 3, data.Length - subAddressIndex - 4);

            return new DnsISDNRecord(zone, ownerName, @class, timeToLive, isdnNumber, subAddress);
        }

        private static DnsMBRecord ParseMBRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsMBRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsMDRecord ParseMDRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsMDRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsMFRecord ParseMFRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsMFRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsMGRecord ParseMGRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsMGRecord(zone, ownerName, @class, timeToLive, data);

        private static DnsMINFORecord ParseMINFORecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var errorMailboxIndex = data.IndexOf(' ');
            var responsibleMailbox = data.Substring(0, errorMailboxIndex);
            var errorMailbox = data.Substring(errorMailboxIndex + 1);

            return new DnsMINFORecord(zone, ownerName, @class, timeToLive, responsibleMailbox, errorMailbox);
        }

        private static DnsMRRecord ParseMRRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsMRRecord(zone, ownerName, @class, timeToLive, data);

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

        private static DnsRPRecord ParseRPRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var txtDomainNameIndex = data.IndexOf(' ');
            var rpMailbox = data.Substring(0, txtDomainNameIndex);
            var txtDomainName = data.Substring(txtDomainNameIndex + 1);

            return new DnsRPRecord(zone, ownerName, @class, timeToLive, rpMailbox, txtDomainName);
        }

        private static DnsRTRecord ParseRTRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var unknownIndex = data.IndexOf(' ');
            var preferenceIndex = data.IndexOf(' ', unknownIndex + 1);
            var preference = ushort.Parse(data.Substring(unknownIndex + 1, preferenceIndex - unknownIndex - 1));

            return new DnsRTRecord(zone, ownerName, @class, timeToLive, preference, data.Substring(preferenceIndex + 1));
        }

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

        private static DnsTXTRecord ParseTXTRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsTXTRecord(zone, ownerName, @class, timeToLive, data.Substring(1, data.Length - 2));

        private static DnsX25Record ParseX25Record(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
            => new DnsX25Record(zone, ownerName, @class, timeToLive, data);

        private static DnsWKSRecord ParseWKSRecord(WindowsDnsZone zone, WindowsDnsRecordState state, string ownerName, DnsRecordClasses @class, TimeSpan timeToLive, string data)
        {
            var internetAddressIndex = data.IndexOf(' ');
            var servicesIndex = data.IndexOf(' ', internetAddressIndex + 1);
            var protocol = data.Substring(0, internetAddressIndex);
            var internetAddress = DnsIpAddress.FromString(data.Substring(internetAddressIndex + 1, servicesIndex - internetAddressIndex - 1));
            var services = data.Substring(servicesIndex + 2, data.Length - servicesIndex - 3);

            return new DnsWKSRecord(zone, ownerName, @class, timeToLive, protocol, internetAddress, services);
        }

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

            // only save if the record has changed
            if (!record.HasChanges())
                return;

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
                    
                    // let the record know it was saved so it can track changes appropriately
                    zone.SavedRecordInternal(record);
                }
            }
        }

        #endregion

        #region Record Create/Modify Parameters
        private static void PopulateRecordParameters(WindowsDnsZone zone, DnsRecord record, ManagementBaseObject wmiParams)
        {
            switch (record)
            {
                case DnsAAAARecord aaaaRecord:
                    wmiParams["IPv6Address"] = aaaaRecord.IpV6Address;
                    break;
                case DnsAFSDBRecord afsdbRecord:
                    wmiParams["ServerName"] = afsdbRecord.HostName;
                    wmiParams["Subtype"] = afsdbRecord.Subtype;
                    break;
                case DnsATMARecord atmaRecord:
                    wmiParams["Format"] = atmaRecord.Format;
                    wmiParams["ATMAddress"] = atmaRecord.ATMAddress;
                    break;
                case DnsARecord aRecord:
                    wmiParams["IPAddress"] = aRecord.IpAddress.ToString();
                    break;
                case DnsCNAMERecord cnameRecord:
                    wmiParams["PrimaryName"] = cnameRecord.PrimaryName;
                    break;
                case DnsHINFORecord hinfoRecord:
                    wmiParams["CPU"] = hinfoRecord.CPU;
                    wmiParams["OS"] = hinfoRecord.OS;
                    break;
                case DnsISDNRecord isdnRecord:
                    wmiParams["ISDNNumber"] = isdnRecord.ISDNNumber;
                    wmiParams["SubAddress"] = isdnRecord.SubAddress;
                    break;
                case DnsMBRecord mbRecord:
                    wmiParams["MBHost"] = mbRecord.MBHost;
                    break;
                case DnsMGRecord mgRecord:
                    wmiParams["MGMailbox"] = mgRecord.MGMailbox;
                    break;
                case DnsMINFORecord minfoRecord:
                    wmiParams["ResponsibleMailbox"] = minfoRecord.ResponsibleMailbox;
                    wmiParams["ErrorMailbox"] = minfoRecord.ErrorMailbox;
                    break;
                case DnsMRRecord mrRecord:
                    wmiParams["MRMailbox"] = mrRecord.MRMailbox;
                    break;
                case DnsMXRecord mxRecord:
                    wmiParams["MailExchange"] = mxRecord.DomainName;
                    wmiParams["Preference"] = mxRecord.Preference;
                    break;
                case DnsNSRecord nsRecord:
                    wmiParams["NSHost"] = nsRecord.NameServer;
                    break;
                case DnsPTRRecord ptrRecord:
                    wmiParams["PTRDomainName"] = ptrRecord.DomainName;
                    break;
                case DnsRPRecord rpRecord:
                    wmiParams["RPMailbox"] = rpRecord.RPMailbox;
                    wmiParams["TXTDomainName"] = rpRecord.TXTDomainName;
                    break;
                case DnsRTRecord rtRecord:
                    wmiParams["Preference"] = rtRecord.Preference;
                    wmiParams["IntermediateHost"] = rtRecord.IntermediateHost;
                    break;
                case DnsSOARecord soaRecord:
                    wmiParams["ExpireLimit"] = (uint)soaRecord.ExpireLimit.TotalSeconds;
                    wmiParams["MinimumTTL"] = (uint)soaRecord.MinimumTimeToLive.TotalSeconds;
                    wmiParams["PrimaryServer"] = soaRecord.PrimaryServer;
                    wmiParams["RefreshInterval"] = (uint)soaRecord.RefreshInterval.TotalSeconds;
                    wmiParams["ResponsibleParty"] = soaRecord.ResponsiblePerson;
                    wmiParams["RetryDelay"] = (uint)soaRecord.RetryDelay.TotalSeconds;
                    wmiParams["SerialNumber"] = soaRecord.Serial;
                    break;
                case DnsSRVRecord srvRecord:
                    wmiParams["Port"] = srvRecord.Port;
                    wmiParams["Priority"] = srvRecord.Priority;
                    wmiParams["SRVDomainName"] = srvRecord.TargetDomainName;
                    wmiParams["Weight"] = srvRecord.Weight;
                    break;
                case DnsTXTRecord txtRecord:
                    wmiParams["DescriptiveText"] = txtRecord.DescriptiveText;
                    break;
                case DnsWKSRecord wksRecord:
                    wmiParams["IPProtocol"] = wksRecord.IpProtocol;
                    wmiParams["InternetAddress"] = wksRecord.InternetAddress.ToString();
                    wmiParams["Services"] = $"\"{wksRecord.Services}\"";
                    break;
                case DnsX25Record x25Record:
                    wmiParams["PSDNAddress"] = x25Record.PsdnAddress;
                    break;
                default:
                    throw new NotSupportedException("This record type is not supported");
            }
        }
        #endregion

        public static IEnumerable<DnsRecord> GetRecordsInternal(this WindowsDnsZone zone)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            var wmiResults = zone.server.WmiQuery($"SELECT * FROM {wmiClassBase} WHERE ContainerName='{zone.DomainName}'");

            foreach (var wmiResult in wmiResults)
            {
                DnsRecord record;
                try
                {
                    record = ParseRecordInternal(zone, wmiResult);
                }
                catch (NotSupportedException)
                {
                    // skip records which are not supported by the library
                    continue;
                }
                yield return record;
            }
        }

        public static IEnumerable<DnsRecord> GetRecordsInternal(this WindowsDnsZone zone, DnsRecordTypes recordType, string name)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            if (!wmiClassMapReverse.TryGetValue(recordType, out var wmiClassName))
                throw new NotSupportedException($"The record type ({recordType}) is not supported");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (name.IndexOf('\'') >= 0)
                throw new ArgumentException("Invalid character present", nameof(name));

            var wmiResults = zone.server.WmiQuery($"SELECT * FROM {wmiClassName} WHERE ContainerName='{zone.DomainName}' AND OwnerName='{name}'");

            foreach (var wmiResult in wmiResults)
            {
                yield return ParseRecordInternal(zone, wmiResult);
            }
        }

        public static IEnumerable<DnsRecord> GetRecordsInternal(this WindowsDnsZone zone, string name)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));

            if (name.IndexOf('\'') >= 0)
                throw new ArgumentException("Invalid character present", nameof(name));

            var wmiResults = zone.server.WmiQuery($"SELECT * FROM {wmiClassBase} WHERE ContainerName='{zone.DomainName}' AND OwnerName='{name}'");

            foreach (var wmiResult in wmiResults)
            {
                DnsRecord record;
                try
                {
                    record = ParseRecordInternal(zone, wmiResult);
                }
                catch (NotSupportedException)
                {
                    // skip records which are not supported by the library
                    continue;
                }
                yield return record;
            }
        }

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
#pragma warning restore CS0618 // Type or member is obsolete
}
