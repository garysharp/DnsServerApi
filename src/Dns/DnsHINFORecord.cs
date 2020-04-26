using System;

namespace Dns
{
    /// <summary>
    /// Host Information (HINFO) Record
    /// </summary>
    /// <remarks>
    /// Indicates RFC-1700 reserved character string values for CPU and operating system types for mapping
    /// to specific DNS host names. This information is used by application protocols such as FTP that can
    /// use special procedures when communicating between computers of the same CPU and OS type. (RFC 1035)
    /// </remarks>
    public class DnsHINFORecord : DnsRecord
    {
        private string cpuInitial;
        private string osInitial;

        /// <summary>
        /// The CPU type of the owner of the record.
        /// </summary>
        public string CPU { get; set; }
        /// <summary>
        /// The operating system type of the owner.
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsHINFORecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="cpu">The CPU type of the owner of the record.</param>
        /// <param name="os">The operating system type of the owner.</param>
        private DnsHINFORecord(DnsZone zone, object providerState, string name, DnsRecordClasses @class, TimeSpan timeToLive, string cpu, string os)
            : base(zone, providerState, name, DnsRecordTypes.HINFO, @class, timeToLive)
        {
            CPU = cpu;
            cpuInitial = cpu;

            OS = os;
            osInitial = os;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsHINFORecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="cpu">The CPU type of the owner of the record.</param>
        /// <param name="os">The operating system type of the owner.</param>
        public DnsHINFORecord(DnsZone zone, string name, DnsRecordClasses @class, TimeSpan timeToLive, string cpu, string os)
            : this(zone, providerState: null, name, @class, timeToLive, cpu, os)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsHINFORecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="class">Record class</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="cpu">The CPU type of the owner of the record.</param>
        /// <param name="os">The operating system type of the owner.</param>
        public DnsHINFORecord(string name, DnsRecordClasses @class, TimeSpan timeToLive, string cpu, string os)
            : this(zone: null, name, @class, timeToLive, cpu, os)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsHINFORecord"/>.
        /// </summary>
        /// <param name="zone">Associated zone</param>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="cpu">The CPU type of the owner of the record.</param>
        /// <param name="os">The operating system type of the owner.</param>
        public DnsHINFORecord(DnsZone zone, string name, TimeSpan timeToLive, string cpu, string os)
            : this(zone, providerState: null, name, DnsRecordClasses.IN, timeToLive, cpu, os)
        {
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsHINFORecord"/>.
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <param name="timeToLive">Record time to live (TTL)</param>
        /// <param name="cpu">The CPU type of the owner of the record.</param>
        /// <param name="os">The operating system type of the owner.</param>
        public DnsHINFORecord(string name, TimeSpan timeToLive, string cpu, string os)
            : this(zone: null, name, DnsRecordClasses.IN, timeToLive, cpu, os)
        {
        }

        /// <summary>
        /// Indicates whether the record changed
        /// </summary>
        /// <returns></returns>
        protected override bool HasDataChanges()
            => !string.Equals(cpuInitial, CPU, StringComparison.Ordinal) ||
                !string.Equals(osInitial, OS, StringComparison.Ordinal);

        /// <summary>
        /// Resets original value to current value
        /// </summary>
        protected override void ProviderDataSaved()
        {
            cpuInitial = CPU;
            osInitial = OS;
        }

        /// <summary>
        /// Clones the record associating it with the provided zone
        /// </summary>
        /// <param name="zone">Record associated zone</param>
        /// <param name="providerState">Provider-specific state storage</param>
        /// <returns>A record clone</returns>
        internal override DnsRecord Clone(DnsZone zone, object providerState)
            => new DnsHINFORecord(zone, providerState, Name, Class, TimeToLive, CPU, OS);

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        protected override string GetDataText()
            => $"{CPU}; {OS}";

    }
}
