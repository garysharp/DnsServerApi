using System;

namespace Dns
{
    /// <summary>
    /// A DNS resource record
    /// </summary>
    public abstract class DnsRecord
    {
        private TimeSpan timeToLiveInitial;

        /// <summary>
        /// Associated zone
        /// </summary>
        public DnsZone Zone { get; }
        /// <summary>
        /// Provider-specific state storage
        /// </summary>
        internal object ProviderState { get; set; }
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
        internal DnsRecord(DnsZone zone, object providerState, string name, DnsRecordTypes type, DnsRecordClasses @class, TimeSpan timeToLive)
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
            timeToLiveInitial = timeToLive;
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
            : this(zone, providerState: null, name, type, @class, timeToLive)
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
            : this(zone: null, providerState: null, name, type, @class, timeToLive)
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
        /// Indicates whether the record has been changed and requires saving
        /// </summary>
        /// <returns>True if the record has changed</returns>
        public bool HasChanges()
        {
            // check base record state
            if (TimeToLive != timeToLiveInitial)
                return true;

            // check inherited state
            return HasDataChanges();
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
        /// Called when the provider indicates changes have been saved
        /// </summary>
        internal void ProviderSaved()
        {
            // reset original value to current value
            timeToLiveInitial = TimeToLive;

            // let inheriting records know changes are persisted
            ProviderDataSaved();
        }

        /// <summary>
        /// Allows inheriting records to inform a provider whether the record has changed and requires saving
        /// </summary>
        /// <returns></returns>
        protected abstract bool HasDataChanges();

        /// <summary>
        /// Called when the provider indicates changes have been saved
        /// </summary>
        protected abstract void ProviderDataSaved();

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal abstract DnsRecord Clone(DnsZone zone, object providerState);

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
            => $"{Type} Record [{Name} = {GetDataText()}]{(HasChanges() ? "*" : null)}";
    }
}
