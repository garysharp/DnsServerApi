using System;

namespace Dns
{
    /// <summary>
    /// Start of Zone of Authority (SOA) Record
    /// </summary>
    public class DnsSOARecord : DnsRecord
    {
        private string primaryServerInitial;
        private string responsiblePersonInitial;
        private uint serialInitial;
        private TimeSpan refreshIntervalIntial;
        private TimeSpan retryDelayInitial;
        private TimeSpan expireLimitInitial;
        private TimeSpan minimumTimeToLiveInitial;

        /// <summary>
        /// Domain Name of the original or primary source of data for this zone
        /// </summary>
        public string PrimaryServer { get; set; }
        /// <summary>
        /// A domain name which specifies the mailbox of the person responsible for this zone
        /// </summary>
        public string ResponsiblePerson { get; set; }
        /// <summary>
        /// Version number of the zone
        /// </summary>
        public uint Serial { get; set; }
        /// <summary>
        /// Interval before the zone should be refreshed
        /// </summary>
        public TimeSpan RefreshInterval { get; set; }
        /// <summary>
        /// Time interval that should elapse before a failed refresh should be retried
        /// </summary>
        public TimeSpan RetryDelay { get; set; }
        /// <summary>
        /// The upper limit on the time interval that can elapse before the zone is no longer authoritative
        /// </summary>
        public TimeSpan ExpireLimit { get; set; }
        /// <summary>
        /// The minimum time-to-live (TTL) that should be exported with an record from this zone
        /// </summary>
        public TimeSpan MinimumTimeToLive { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSOARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryServer">Domain Name of the original or primary source of data for this zone</param>
        /// <param name="responsiblePerson">A domain name which specifies the mailbox of the person responsible for this zone</param>
        /// <param name="serial">Version number of the zone</param>
        /// <param name="refreshInterval">Interval before the zone should be refreshed</param>
        /// <param name="retryDelay">Time interval that should elapse before a failed refresh should be retried</param>
        /// <param name="expireLimit">The upper limit on the time interval that can elapse before the zone is no longer authoritative</param>
        /// <param name="minimumTimeToLive">The minimum time-to-live (TTL) that should be exported with an record from this zone</param>
        private DnsSOARecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive,
            string primaryServer, string responsiblePerson, uint serial, TimeSpan refreshInterval,
            TimeSpan retryDelay, TimeSpan expireLimit, TimeSpan minimumTimeToLive)
            : base(zone, providerState, name, DnsRecordTypes.SOA, @class, timeToLive)
        {
            if (string.IsNullOrWhiteSpace(primaryServer))
                throw new ArgumentNullException(nameof(primaryServer));
            if (string.IsNullOrWhiteSpace(responsiblePerson))
                throw new ArgumentNullException(nameof(responsiblePerson));
            if (refreshInterval.Ticks < 0)
                throw new ArgumentOutOfRangeException(nameof(refreshInterval));
            if (retryDelay.Ticks < 0)
                throw new ArgumentOutOfRangeException(nameof(retryDelay));
            if (expireLimit.Ticks < 0)
                throw new ArgumentOutOfRangeException(nameof(expireLimit));
            if (minimumTimeToLive.Ticks < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumTimeToLive));

            PrimaryServer = primaryServer;
            primaryServerInitial = primaryServer;

            ResponsiblePerson = responsiblePerson;
            responsiblePersonInitial = responsiblePerson;

            Serial = serial;
            serialInitial = serial;

            RefreshInterval = refreshInterval;
            refreshIntervalIntial = refreshInterval;

            RetryDelay = retryDelay;
            retryDelayInitial = retryDelay;

            ExpireLimit = expireLimit;
            expireLimitInitial = expireLimit;

            MinimumTimeToLive = minimumTimeToLive;
            minimumTimeToLiveInitial = minimumTimeToLive;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSOARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryServer">Domain Name of the original or primary source of data for this zone</param>
        /// <param name="responsiblePerson">A domain name which specifies the mailbox of the person responsible for this zone</param>
        /// <param name="serial">Version number of the zone</param>
        /// <param name="refreshInterval">Interval before the zone should be refreshed</param>
        /// <param name="retryDelay">Time interval that should elapse before a failed refresh should be retried</param>
        /// <param name="expireLimit">The upper limit on the time interval that can elapse before the zone is no longer authoritative</param>
        /// <param name="minimumTimeToLive">The minimum time-to-live (TTL) that should be exported with an record from this zone</param>
        public DnsSOARecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive,
            string primaryServer, string responsiblePerson, uint serial, TimeSpan refreshInterval,
            TimeSpan retryDelay, TimeSpan expireLimit, TimeSpan minimumTimeToLive)
            : this(zone, providerState: null, name, @class, timeToLive, primaryServer,
                  responsiblePerson, serial, refreshInterval, retryDelay, expireLimit, minimumTimeToLive)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSOARecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryServer">Domain Name of the original or primary source of data for this zone</param>
        /// <param name="responsiblePerson">A domain name which specifies the mailbox of the person responsible for this zone</param>
        /// <param name="serial">Version number of the zone</param>
        /// <param name="refreshInterval">Interval before the zone should be refreshed</param>
        /// <param name="retryDelay">Time interval that should elapse before a failed refresh should be retried</param>
        /// <param name="expireLimit">The upper limit on the time interval that can elapse before the zone is no longer authoritative</param>
        /// <param name="minimumTimeToLive">The minimum time-to-live (TTL) that should be exported with an record from this zone</param>
        public DnsSOARecord(string name, DnsRecordClasses @class, TimeSpan timeToLive,
            string primaryServer, string responsiblePerson, uint serial, TimeSpan refreshInterval,
            TimeSpan retryDelay, TimeSpan expireLimit, TimeSpan minimumTimeToLive)
            : this(zone: null, name, @class, timeToLive, primaryServer, responsiblePerson,
                  serial, refreshInterval, retryDelay, expireLimit, minimumTimeToLive)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSOARecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryServer">Domain Name of the original or primary source of data for this zone</param>
        /// <param name="responsiblePerson">A domain name which specifies the mailbox of the person responsible for this zone</param>
        /// <param name="serial">Version number of the zone</param>
        /// <param name="refreshInterval">Interval before the zone should be refreshed</param>
        /// <param name="retryDelay">Time interval that should elapse before a failed refresh should be retried</param>
        /// <param name="expireLimit">The upper limit on the time interval that can elapse before the zone is no longer authoritative</param>
        /// <param name="minimumTimeToLive">The minimum time-to-live (TTL) that should be exported with an record from this zone</param>
        public DnsSOARecord(DnsZone zone, string name, TimeSpan timeToLive,
            string primaryServer, string responsiblePerson, uint serial, TimeSpan refreshInterval,
            TimeSpan retryDelay, TimeSpan expireLimit, TimeSpan minimumTimeToLive)
            : this(zone, name, DnsRecordClasses.IN, timeToLive, primaryServer, responsiblePerson,
                  serial, refreshInterval, retryDelay, expireLimit, minimumTimeToLive)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsSOARecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="primaryServer">Domain Name of the original or primary source of data for this zone</param>
        /// <param name="responsiblePerson">A domain name which specifies the mailbox of the person responsible for this zone</param>
        /// <param name="serial">Version number of the zone</param>
        /// <param name="refreshInterval">Interval before the zone should be refreshed</param>
        /// <param name="retryDelay">Time interval that should elapse before a failed refresh should be retried</param>
        /// <param name="expireLimit">The upper limit on the time interval that can elapse before the zone is no longer authoritative</param>
        /// <param name="minimumTimeToLive">The minimum time-to-live (TTL) that should be exported with an record from this zone</param>
        public DnsSOARecord(string name, TimeSpan timeToLive,
            string primaryServer, string responsiblePerson, uint serial, TimeSpan refreshInterval,
            TimeSpan retryDelay, TimeSpan expireLimit, TimeSpan minimumTimeToLive)
            : this(zone: null, name, timeToLive, primaryServer, responsiblePerson,
                  serial, refreshInterval, retryDelay, expireLimit, minimumTimeToLive)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
        {
            return
                !string.Equals(primaryServerInitial, PrimaryServer, StringComparison.Ordinal) ||
                !string.Equals(responsiblePersonInitial, ResponsiblePerson, StringComparison.Ordinal) ||
                serialInitial != Serial ||
                refreshIntervalIntial != RefreshInterval ||
                retryDelayInitial != RetryDelay ||
                expireLimitInitial != ExpireLimit ||
                minimumTimeToLiveInitial != MinimumTimeToLive;
        }

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            primaryServerInitial = PrimaryServer;
            responsiblePersonInitial = ResponsiblePerson;
            serialInitial = Serial;
            refreshIntervalIntial = RefreshInterval;
            retryDelayInitial = RetryDelay;
            expireLimitInitial = ExpireLimit;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsSOARecord(zone, providerState, Name, Class, TimeToLive, PrimaryServer, ResponsiblePerson, Serial, RefreshInterval, RetryDelay, ExpireLimit, MinimumTimeToLive);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"[{Serial}], {PrimaryServer}, {ResponsiblePerson}";

    }
}
