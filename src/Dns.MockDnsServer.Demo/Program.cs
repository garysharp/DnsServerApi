using System;

namespace Dns.MockDnsServer.Demo
{
    using static System.Console;

    class Program
    {
        static void Main(string[] args)
        {

            // connect to DNS server
            using (var server = new MockDnsServer())
            {
                // create zone
                var zone = server.CreateZone("myzone.mock");

                // get zone by name
                var myZoneRef = server.GetZone("myzone.mock");

                // get records
                var myServerRecords = zone.GetRecords(DnsRecordTypes.A, "myserver.myzone.mock");

                // search records
                var allMyServerRecords = zone.SearchRecords("MyServer");
                var hostMyServerRecords = zone.SearchRecords(DnsRecordTypes.A, "MyServer");

                // update record
                var soa = zone.StartOfAuthority;
                soa.TimeToLive = TimeSpan.FromMinutes(30);
                soa.ResponsiblePerson = "responsible.hostmaster.";
                soa.Save();
                // or: zone.SaveRecord(soa);

                // create host record via template
                var hostTemplate = new DnsARecord("myhost.myzone.mock", TimeSpan.FromHours(1), "192.168.1.50");
                var hostRecord = zone.CreateRecord(hostTemplate);

                // add second host record (with template)
                hostTemplate.IpAddress = "192.168.1.51";
                var hostRecord2 = zone.CreateRecord(hostTemplate);

                // create alias record
                var cnameRecord = zone.CreateCNAMERecord("myhostalias.myzone.mock", TimeSpan.FromHours(1), "myhost.myzone.mock");

                // create service locator record
                var srvRecord = zone.CreateSRVRecord(
                    service: DnsSRVRecord.ServiceNames.LDAP,
                    protocol: DnsSRVRecord.ProtocolNames.TCP,
                    timeToLive: TimeSpan.FromHours(1),
                    priority: 0,
                    weight: 10,
                    port: DnsSRVRecord.ServicePorts.LDAP,
                    targetDomainName: "mycontroller.myzone.mock");

                // delete record
                hostRecord.Delete();
                // or: zone.DeleteRecord(aRecord);

                DumpDnsServer(server);

                // delete zone
                zone.Delete();
                // or: server.DeleteZone(zone);
                // or: server.DeleteZone("myzone.mock");
            }

        }

        #region Console Helpers
        static void DumpDnsServer(DnsServer server)
        {
            WriteLine(server);

            WriteLine("Zones:");
            foreach (var zone in server.GetZones())
            {
                DumpDnsZone(zone);
            }
        }

        static void DumpDnsZone(DnsZone zone)
        {
            WriteLine(zone);

            WriteLine("Records:");
            foreach (var record in zone.GetRecords())
            {
                WriteLine(record);
            }
        }
        #endregion
    }
}
