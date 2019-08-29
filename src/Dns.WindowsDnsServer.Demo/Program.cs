using System;

namespace Dns.WindowsDnsServer.Demo
{
    using static System.Console;

    class Program
    {
        static void Main(string[] args)
        {

            // connect to DNS server
            using (var server = WindowsDnsServer.Connect(
                Environment.GetEnvironmentVariable("DnsServerAddress"),
                Environment.GetEnvironmentVariable("DnsServerAuthority"),
                Environment.GetEnvironmentVariable("DnsServerUsername"),
                Environment.GetEnvironmentVariable("DnsServerPassword")))
            {

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

            WriteLine("Records:");
            foreach (var record in zone.Records)
            {
                WriteLine(record);
            }
        }
        #endregion
    }
}
