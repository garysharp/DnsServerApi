using System;
using System.Linq;

namespace Dns.WindowsDnsServer.Demo
{
    using static System.Console;

    class Program
    {
        static void Main(string[] args)
        {

            // connect to DNS server
            Write($"Connecting to {Environment.GetEnvironmentVariable("DnsServerAddress")}...");
            using (var server = WindowsDnsServer.Connect(
                Environment.GetEnvironmentVariable("DnsServerAddress"),
                Environment.GetEnvironmentVariable("DnsServerAuthority"),
                Environment.GetEnvironmentVariable("DnsServerUsername"),
                Environment.GetEnvironmentVariable("DnsServerPassword")))
            {
                WriteLine(" [Done]");

                DumpDnsServer(server);

            }

        }

        #region Console Helpers
        static void DumpDnsServer(DnsServer server)
        {
            WriteLine(server);

            WriteLine("Zones:");
            foreach (var zone in server.Zones)
            {
                DumpDnsZone(zone);
            }
        }

        static void DumpDnsZone(DnsZone zone)
        {
            WriteLine(zone);

            var soa = zone.StartOfAuthority;
            WriteLine("Start Of Authority:");
            WriteLine($"      Primary Server: {soa.PrimaryServer}");
            WriteLine($"  Responsible Person: {soa.ResponsiblePerson}");
            WriteLine($"              Serial: {soa.Serial}");
            WriteLine($"    Refresh Interval: {soa.RefreshInterval}");
            WriteLine($"         Retry Delay: {soa.RetryDelay}");
            WriteLine($"        Expire Limit: {soa.ExpireLimit}");
            WriteLine($"         Minimum TTL: {soa.MinimumTimeToLive}");
            WriteLine();
            WriteLine("Records:");
            foreach (var record in zone.Records.Where(r => r.Type != DnsRecordTypes.SOA))
            {
                WriteLine(record);
            }
        }
        #endregion
    }
}
