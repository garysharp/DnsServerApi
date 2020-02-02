using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dns
{
    /// <summary>
    /// DNS Server
    /// </summary>
    public abstract class DnsServer : IDisposable
    {
        /// <summary>
        /// DNS Server Domain Name
        /// </summary>
        public string DomainName { get; }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsServer"/>.
        /// </summary>
        /// <param name="domainName">DNS server domain name</param>
        public DnsServer(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentNullException(domainName);

            DomainName = domainName;
        }

        /// <summary>
        /// Retrieves all DNS zones
        /// </summary>
        /// <returns>List of DNS zones</returns>
        public abstract IEnumerable<DnsZone> GetZones();

        /// <summary>
        /// Retrieves a DNS Zone
        /// </summary>
        /// <param name="domainName">Domain name of the zone to retrieve</param>
        /// <param name="zone">If successful, the retrieved zone</param>
        /// <returns>True if the zone was retrieved, otherwise false</returns>
        public virtual bool TryGetZone(string domainName, out DnsZone zone)
        {
            if (string.IsNullOrWhiteSpace(domainName))
            {
                zone = null;
                return false;
            }

            try
            {
                zone = GetZone(domainName);
                return true;
            }
            catch (Exception)
            {
                zone = null;
                return false;
            }
        }
        /// <summary>
        /// Retrieve DNS Zone
        /// </summary>
        /// <param name="domainName">Domain name of the zone</param>
        /// <returns></returns>
        public abstract DnsZone GetZone(string domainName);

        /// <summary>
        /// Adds a DNS Zone to this server
        /// </summary>
        /// <param name="zoneTemplate">Template of zone to be added</param>
        /// <param name="zone">Created zone</param>
        /// <returns>True if the zone is created, otherwise false</returns>
        public virtual bool TryCreateZone(DnsZone zoneTemplate, out DnsZone zone)
        {
            if (zoneTemplate == null)
            {
                zone = null;
                return false;
            }

            try
            {
                zone = CreateZone(zoneTemplate);
                return true;
            }
            catch (Exception)
            {
                zone = null;
                return false;
            }
        }
        /// <summary>
        /// Adds a DNS Zone to this server
        /// </summary>
        /// <param name="zoneTemplate">Template of zone to be added</param>
        /// <returns></returns>
        public abstract DnsZone CreateZone(DnsZone zoneTemplate);
        /// <summary>
        /// Adds a DNS Zone to this server using default values (primary, not reverse)
        /// </summary>
        /// <param name="domainName">Domain Name of the zone to be added</param>
        /// <param name="zone">Created zone</param>
        /// <returns>True if the zone is created, otherwise false</returns>
        public virtual bool TryCreateZone(string domainName, out DnsZone zone)
        {
            if (string.IsNullOrWhiteSpace(domainName))
            {
                zone = null;
                return false;
            }

            try
            {
                zone = CreateZone(domainName);
                return true;
            }
            catch (Exception)
            {
                zone = null;
                return false;
            }
        }
        /// <summary>
        /// Adds a DNS Zone to this server using default values (primary, not reverse)
        /// </summary>
        /// <param name="domainName">Domain Name of the zone to be added</param>
        /// <returns></returns>
        public abstract DnsZone CreateZone(string domainName);

        /// <summary>
        /// Removes a DNS Zone
        /// </summary>
        /// <param name="zone">Zone to be removed</param>
        /// <returns>True if the zone was removed, otherwise false</returns>
        public virtual bool TryDeleteZone(DnsZone zone)
        {
            if (zone == null)
                return false;

            try
            {
                DeleteZone(zone);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Remove a DNS Zone
        /// </summary>
        /// <param name="zone">Zone to be removed</param>
        public abstract void DeleteZone(DnsZone zone);

        /// <summary>
        /// Removes a DNS Zone
        /// </summary>
        /// <param name="domainName">Domain Name of the zone to be removed</param>
        /// <returns>True if the zone was removed, otherwise false</returns>
        public virtual bool TryDeleteZone(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                return false;

            try
            {
                DeleteZone(domainName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Remove a DNS Zone
        /// </summary>
        /// <param name="domainName">Domain Name of the zone to be removed</param>
        public abstract void DeleteZone(string domainName);

        /// <summary>
        /// Returns a textual representation of the current instance
        /// </summary>
        /// <returns>A textual representation of the current instance</returns>
        public override string ToString()
            => $"DNS Server [{DomainName}]";

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        /// <param name="disposing">True if disposing, false if finalising</param>
        protected virtual void Dispose(bool disposing)
        {
        }
        /// <summary>
        /// Finalise
        /// </summary>
        ~DnsServer()
        {
            Dispose(false);
        }
    }
}
