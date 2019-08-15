# DNS Server API for .NET
 
A generic dotnet (C#, Visual Basic.NET, etc) library designed for managing DNS Servers.

A provider (implementation) is planned for Windows Server DNS Server (via the [WMI Provider](https://docs.microsoft.com/en-us/windows/win32/dns/dns-wmi-provider). Providers for other DNS Servers is possible and contributions are welcome.

A simple hierachy is present:
`DnsServer > DnsZone > DnsRecord`

Both `DnsServer` and `DnsZone` are abstract and must be implemented by the provider.

## Sample Usage

### Zones
```csharp
// connect to DNS server via provider
using (var server = new MockDnsServer())
{
    // create zone
    var myZone = server.CreateZone("myzone.mock");

    // get zone by name
    var myZoneRef = server.GetZone("myzone.mock");

    // enumerate zones
    foreach (var zone in server.Zones)
    {
        Console.WriteLine(zone.DomainName);
    }

    // delete zone
    myZone.Delete();
    // or: server.DeleteZone(zone);
    // or: server.DeleteZone("myzone.mock");
}
```

### Records
```csharp
// connect to DNS server
using (var server = new MockDnsServer())
{
    // create zone
    var zone = server.CreateZone("myzone.mock");

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

    // create host record
    var hostTemplate = new DnsARecord("myhost.myzone.mock", TimeSpan.FromHours(1), "192.168.1.50");
    var hostRecord = zone.CreateRecord(hostTemplate);

    // add second host record
    hostTemplate.IpAddress = "192.168.1.51";
    var hostRecord2 = zone.CreateRecord(hostTemplate);

    // create alias record
    var cnameTemplate = new DnsCNAMERecord("myhostalias.myzone.mock", TimeSpan.FromHours(1), "myhost.myzone.mock");
    var cnameRecord = zone.CreateRecord(cnameTemplate);

    // create service locator record
    var srvTemplate = new DnsSRVRecord(
            domainName: "myzone.mock",
            service: DnsSRVRecord.ServiceNames.LDAP,
            protocol: DnsSRVRecord.ProtocolNames.TCP,
            timeToLive: TimeSpan.FromHours(1),
            priority: 0,
            weight: 10,
            port: DnsSRVRecord.ServicePorts.LDAP,
            targetDomainName: "mycontroller.myzone.mock");
    var srvRecord = zone.CreateRecord(srvTemplate);

    // delete record
    hostRecord.Delete();
    // or: zone.DeleteRecord(aRecord);
}
```
