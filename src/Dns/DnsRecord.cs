using System;

namespace Dns
{
    /// <summary>
    /// A DNS resource record
    /// </summary>
    public abstract class DnsRecord
    {
        /// <summary>
        /// Associated zone
        /// </summary>
        public DnsZone Zone { get; }
        /// <summary>
        /// Provider-specific state storage
        /// </summary>
        public object ProviderState { get; }
        /// <summary>
        /// Owner name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Record type
        /// </summary>
        public DnsRecordTypes Type { get; }
        /// <summary>
        /// Record class
        /// </summary>
        public DnsRecordClasses Class { get; }
        /// <summary>
        /// Record time to live (TTL)
        /// </summary>
        public TimeSpan TimeToLive { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="type">Record type</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        public DnsRecord(DnsZone zone, object providerState, string name, DnsRecordTypes type, DnsRecordClasses @class, TimeSpan timeToLive)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (timeToLive.Ticks < 0)
                throw new ArgumentOutOfRangeException(nameof(timeToLive));

            Zone = zone;
            ProviderState = providerState;
            Name = name;
            Type = type;
            Class = @class;
            TimeToLive = timeToLive;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="type">Record type</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        public DnsRecord(DnsZone zone, string name, DnsRecordTypes type, DnsRecordClasses @class, TimeSpan timeToLive)
            : this (zone, providerState: null, name, type, @class, timeToLive)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsRecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="type">Record type</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        public DnsRecord(string name, DnsRecordTypes type, DnsRecordClasses @class, TimeSpan timeToLive)
            : this (zone: null, name, type, @class, timeToLive)
        {
        }

        /// <summary>
        /// Deletes this record from the DNS zone
        /// </summary>
        public void Delete()
        {
            if (Zone == null)
                throw new InvalidOperationException("A record must be associated with a zone to be deleted");

            Zone.DeleteRecord(this);
        }

        /// <summary>
        /// Saves this record to the DNS zone
        /// </summary>
        public void Save()
        {
            if (Zone == null)
                throw new InvalidOperationException("A record must be associated with a zone to be updated");

            Zone.SaveRecord(this);
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        public abstract DnsRecord Clone(DnsZone zone, object providerState);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected abstract string GetDataText();

        /// <summary>
        /// Returns a textual representation of the current instance
        /// </summary>
        /// <returns>A textual representation of the current instance</returns>
        public override string ToString()
            => $"{Type} Record [{Name} = {GetDataText()}]";
    }
}
