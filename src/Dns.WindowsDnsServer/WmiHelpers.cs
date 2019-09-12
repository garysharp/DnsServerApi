using System.Collections.Generic;
using System.Management;

namespace Dns.WindowsDnsServer
{
    internal static class WmiHelpers
    {
        private static ObjectGetOptions wmiDefaultGetOptions = new ObjectGetOptions();

        public static List<ManagementBaseObject> WmiQuery(this WindowsDnsServer server, string wqlQuery)
        {
            var wmiQuery = new ObjectQuery(wqlQuery);
            using (var wmiSearcher = new ManagementObjectSearcher(server.Scope, wmiQuery))
            {
                using (var wmiSearchResults = wmiSearcher.Get())
                {
                    var wmiResults = new List<ManagementBaseObject>(wmiSearchResults.Count);
                    foreach (var wmiResult in wmiSearchResults)
                        wmiResults.Add(wmiResult);
                    return wmiResults;
                }
            }
        }

        public static List<ManagementBaseObject> WmiGetInstances(this WindowsDnsServer server, string className)
        {
            using (var wmiClass = WmiGetClass(server, className))
            {
                using (var wmiInstances = wmiClass.GetInstances())
                {
                    var wmiResults = new List<ManagementBaseObject>(wmiInstances.Count);
                    foreach (var wmiInstance in wmiInstances)
                        wmiResults.Add(wmiInstance);
                    return wmiResults;
                }
            }
        }

        public static ManagementObject WmiGetInstance(this WindowsDnsServer server, string wmiClass, string wmiKeys)
        {
            var wmiPath = new ManagementPath($"{server.Path.NamespacePath}:{wmiClass}.{wmiKeys}");

            var wmiInstance = new ManagementObject(server.Scope, wmiPath, wmiDefaultGetOptions);

            // bind
            wmiInstance.Get();

            return wmiInstance;
        }

        public static ManagementObject WmiGetInstance(this WindowsDnsServer server, ManagementPath wmiPath)
        {
            var wmiInstance = new ManagementObject(server.Scope, wmiPath, wmiDefaultGetOptions);

            // bind
            wmiInstance.Get();

            return wmiInstance;
        }

        public static ManagementClass WmiGetClass(this WindowsDnsServer server, string className)
        {
            var wmiPath = new ManagementPath(className);
            var wmiClass = new ManagementClass(server.Scope, wmiPath, wmiDefaultGetOptions);

            // bind
            wmiClass.Get();

            return wmiClass;
        }

    }
}
