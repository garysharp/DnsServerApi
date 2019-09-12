using System.Management;

namespace Dns.WindowsDnsServer
{
    internal class WindowsDnsRecordState
    {
        public ManagementPath WmiPath { get; }

        public WindowsDnsRecordState(ManagementBaseObject managementObject)
        {
            var wmiPath = (string)managementObject["__PATH"];

            WmiPath = new ManagementPath(wmiPath);
        }

        public WindowsDnsRecordState(string wmiPath)
        {
            WmiPath = new ManagementPath(wmiPath);
        }
    }
}
