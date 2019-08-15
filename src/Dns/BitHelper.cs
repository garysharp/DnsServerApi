using System;

namespace Dns
{
    internal static class BitHelper
    {

        public static void Write(byte[] buffer, int index, uint value)
        {
            buffer[index++] = (byte)(value >> 24);
            buffer[index++] = (byte)(value >> 16);
            buffer[index++] = (byte)(value >> 8);
            buffer[index] = (byte)(value);
        }

        public static void Write(byte[] buffer, int index, int value) => Write(buffer, index, (uint)value);

        #region Byte Reordering
        public static uint NetworkToHostOrder(uint bits)
        {
            if (BitConverter.IsLittleEndian)
            {
                // Swap
                return ((bits >> 24) & 0x0000_00FF) |
                       ((bits >> 08) & 0x0000_FF00) |
                       ((bits << 08) & 0x00FF_0000) |
                       ((bits << 24) & 0xFF00_0000);
            }
            else
            {
                return bits;
            }
        }

        public static int NetworkToHostOrder(int bits) => (int)NetworkToHostOrder((uint)bits);

        public static ulong NetworkToHostOrder(ulong bits)
        {
            if (BitConverter.IsLittleEndian)
            {
                // Swap
                return ((bits >> 56) & 0x00000000000000FF) |
                       ((bits >> 40) & 0x000000000000FF00) |
                       ((bits >> 24) & 0x0000000000FF0000) |
                       ((bits >> 08) & 0x00000000FF000000) |
                       ((bits << 08) & 0x000000FF00000000) |
                       ((bits << 24) & 0x0000FF0000000000) |
                       ((bits << 40) & 0x00FF000000000000) |
                       ((bits << 56) & 0xFF00000000000000);
            }
            else
            {
                return bits;
            }
        }

        public static long NetworkToHostOrder(long bits) => (long)NetworkToHostOrder((ulong)bits);
        public static uint HostToNetworkOrder(uint bits) => NetworkToHostOrder(bits);
        public static int HostToNetworkOrder(int bits) => (int)NetworkToHostOrder((uint)bits);
        public static ulong HostToNetworkOrder(ulong bits) => NetworkToHostOrder(bits);
        public static long HostToNetworkOrder(long bits) => (long)NetworkToHostOrder((ulong)bits);
        #endregion

        public static void UIntToString(char[] chars, ref int offset, int value)
        {
            do
            {
                value = Math.DivRem(value, 10, out var r);
                chars[--offset] = (char)('0' + r);
            } while (value != 0);
        }

        public static string IpAddressToString(uint nativeAddress)
        {
            var buffer = new char[15];
            var offset = 15;

            UIntToString(buffer, ref offset, (int)(nativeAddress & 0xFF));
            buffer[--offset] = '.';
            UIntToString(buffer, ref offset, (int)((nativeAddress >> 8) & 0xFF));
            buffer[--offset] = '.';
            UIntToString(buffer, ref offset, (int)((nativeAddress >> 16) & 0xFF));
            buffer[--offset] = '.';
            UIntToString(buffer, ref offset, (int)((nativeAddress >> 24) & 0xFF));

            return new string(buffer, offset, 15 - offset);
        }

        public static uint StringToIpAddress(string address)
            => StringToIpAddress(address, 0, address.Length);

        public static uint StringToIpAddress(string address, int index, int length)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));
            if (address.Length < index + length)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (length > 15 || length < 7)
                throw new ArgumentOutOfRangeException(nameof(address));

            var result = 0U;
            var shiftAmount = 24;
            var octetIndex = index;
            var octetValue = 0;
            var indexEnd = index + length;

            for (var i = index; i < indexEnd; i++)
            {
                var ipChar = address[i];

                if (ipChar == '.')
                {
                    if ((i - octetIndex) < 1 || (i - octetIndex) > 3 || shiftAmount <= 0 || octetValue > 255)
                        throw new ArgumentOutOfRangeException(nameof(address));

                    result |= (uint)(octetValue << shiftAmount);
                    shiftAmount -= 8;
                    octetValue = 0;
                    octetIndex = i + 1;
                }
                else
                {
                    if (ipChar < '0' || ipChar > '9')
                        throw new ArgumentOutOfRangeException(nameof(address));

                    octetValue = (octetValue * 10) + (ipChar - '0');
                }
            }
            if ((indexEnd - octetIndex) < 1 || (indexEnd - octetIndex) > 3 || shiftAmount != 0 || octetValue > 255)
                throw new ArgumentOutOfRangeException(nameof(address));

            result |= (uint)octetValue;

            return result;
        }

    }
}
