using System;
using System.Net;

namespace Dns
{
    /// <summary>
    /// IP Address
    /// </summary>
    [Serializable]
    public struct DnsIpAddress : IEquatable<DnsIpAddress>, IEquatable<IPAddress>
    {
#pragma warning disable IDE0032 // Use auto property
        /// <summary>
        /// IP Address stored in big-endian (network) order
        /// </summary>
        private readonly uint address;
#pragma warning restore IDE0032 // Use auto property

        /// <summary>
        /// Constructs a new instance of <see cref="DnsIpAddress"/>.
        /// </summary>
        /// <param name="nativeAddress">Native 32-bit IP Address</param>
        public DnsIpAddress(uint nativeAddress)
        {
            address = nativeAddress;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsIpAddress"/>.
        /// </summary>
        /// <param name="address">Text representation of the IP Address</param>
        public DnsIpAddress(string address)
        {
            this.address = BitHelper.StringToIpAddress(address);
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsIpAddress"/>.
        /// </summary>
        /// <param name="address">Text representation of the IP Address</param>
        /// <param name="index">Character index to being parsing the IP Address</param>
        /// <param name="length">Number of characters in the IP Address</param>
        public DnsIpAddress(string address, int index, int length)
        {
            this.address = BitHelper.StringToIpAddress(address, index, length);
        }

        /// <summary>
        /// Constructs a new instance of <see cref="DnsIpAddress"/>.
        /// </summary>
        /// <param name="address">Framework representation of the IP Address</param>
        public DnsIpAddress(IPAddress address)
        {
            if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                throw new ArgumentOutOfRangeException(nameof(address), "Only IPv4 addresses are supported");
            }

#pragma warning disable CS0618 // Type or member is obsolete
            var ip = (uint)address.Address;
#pragma warning restore CS0618 // Type or member is obsolete

            // DhcpServerIpAddress always stores in network order
            // IPAddress stores in host order
            this.address = BitHelper.HostToNetworkOrder(ip);
        }

        /// <summary>
        /// IP Address in network order
        /// </summary>
        public uint Native => address;

        /// <summary>
        /// Returns the IP Address as a byte array
        /// </summary>
        /// <returns>The IP Address as a byte array</returns>
        public byte[] GetBytes()
        {
            var buffer = new byte[4];
            BitHelper.Write(buffer, 0, address);
            return buffer;
        }

        /// <summary>
        /// Returns the specified octet of the IP Address
        /// </summary>
        /// <param name="index">Octet to retrieve (0-3)</param>
        /// <returns>The specified octet of the IP Address</returns>
        public byte GetByte(int index)
        {
            if (index < 0 || index > 3)
                throw new ArgumentOutOfRangeException(nameof(index));

            return (byte)(address >> ((3 - index) * 8));
        }

        /// <summary>
        /// Flips the address so that the first, second, third and forth octet become the forth, third, second and first.
        /// </summary>
        /// <returns>A flipped IP Address</returns>
        public DnsIpAddress GetFlipped()
        {
            // rotate octets
            var flippedAddress =
                ((address & 0x000000FF) << 24 ) |
                ((address & 0x0000FF00) << 8 ) |
                ((address & 0x00FF0000) >> 8 ) |
                ((address & 0xFF000000) >> 24);

            return new DnsIpAddress(flippedAddress);
        }

        /// <summary>
        /// Constructs an empty IP Address (0.0.0.0)
        /// </summary>
        public static DnsIpAddress Empty => new DnsIpAddress(0);
        /// <summary>
        /// Constructs an IP Address from a native 32-bit address
        /// </summary>
        /// <param name="nativeAddress">Native 32-bit address</param>
        /// <returns>An IP Address</returns>
        public static DnsIpAddress FromNative(uint nativeAddress) => new DnsIpAddress(nativeAddress);
        /// <summary>
        /// Constructs an IP Address from a native 32-bit address
        /// </summary>
        /// <param name="nativeAddress">Native 32-bit address</param>
        /// <returns>An IP Address</returns>
        public static DnsIpAddress FromNative(int nativeAddress) => new DnsIpAddress((uint)nativeAddress);
        /// <summary>
        /// Constructs an IP Address from a text representation
        /// </summary>
        /// <param name="address">Text representation of the IP Address</param>
        /// <returns>An IP Address</returns>
        public static DnsIpAddress FromString(string address) => new DnsIpAddress(address);
        /// <summary>
        /// Constructs an IP Address from a PTR text representation
        /// </summary>
        /// <param name="ptrAddress">PTR text representation of the IP Address</param>
        /// <returns>An IP Address</returns>
        public static DnsIpAddress FromPTRAddress(string ptrAddress)
        {
            var index = 0;
            for (int i = 0; i < 4; i++)
                index = ptrAddress.IndexOf('.', index + 1);

            var address = new DnsIpAddress(ptrAddress, 0, index);
            return address.GetFlipped();
        }

        /// <summary>
        /// Returns a textual representation of the current instance data
        /// </summary>
        /// <returns>A textual representation of the current instance data</returns>
        public override string ToString() => BitHelper.IpAddressToString(address);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is DnsIpAddress sia)
                return Equals(sia);
            else if (obj is IPAddress ia)
                return Equals(ia);

            return false;
        }

        public override int GetHashCode() => (int)address;

        public bool Equals(DnsIpAddress other) => address == other.address;

        public bool Equals(IPAddress other)
        {
            if (other == null || other.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                return false;

#pragma warning disable CS0618 // Type or member is obsolete
            var otherIp = (uint)other.Address;
#pragma warning restore CS0618 // Type or member is obsolete

            return address == BitHelper.HostToNetworkOrder(otherIp);
        }

        public static bool operator ==(DnsIpAddress lhs, DnsIpAddress rhs) => lhs.Equals(rhs);

        public static bool operator !=(DnsIpAddress lhs, DnsIpAddress rhs) => !lhs.Equals(rhs);

        public static bool operator ==(DnsIpAddress lhs, IPAddress rhs) => lhs.Equals(rhs);

        public static bool operator !=(DnsIpAddress lhs, IPAddress rhs) => !lhs.Equals(rhs);

        public static explicit operator uint(DnsIpAddress address) => address.address;
        public static explicit operator DnsIpAddress(uint address) => new DnsIpAddress(address);
        public static explicit operator int(DnsIpAddress address) => (int)address.address;
        public static explicit operator DnsIpAddress(int address) => new DnsIpAddress((uint)address);

        public static implicit operator DnsIpAddress(IPAddress address) => new DnsIpAddress(address);
        public static implicit operator IPAddress(DnsIpAddress address)
            // IPAddress stores in host order; DhcpServerIpAddress always stores in network order
            => new IPAddress(BitHelper.NetworkToHostOrder(address.address));

        public static implicit operator string(DnsIpAddress address) => address.ToString();
        public static implicit operator DnsIpAddress(string address) => FromString(address);

        public static bool operator >(DnsIpAddress a, DnsIpAddress b) => a.address > b.address;
        public static bool operator >=(DnsIpAddress a, DnsIpAddress b) => a.address >= b.address;
        public static bool operator <(DnsIpAddress a, DnsIpAddress b) => a.address < b.address;
        public static bool operator <=(DnsIpAddress a, DnsIpAddress b) => a.address <= b.address;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
