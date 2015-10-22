namespace RadicalResearch.Net.Dns
{
    using System;

    internal static class DnsExtensions
    {
        public static ushort ToNetworkByteOrder(this ushort us)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return us;
            }

            var bytes = BitConverter.GetBytes(us);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static uint ToNetworkByteOrder(this uint ui)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return ui;
            }

            var bytes = BitConverter.GetBytes(ui);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static ushort FromNetworkByteOrder(this ushort us)
        {
            return ToNetworkByteOrder(us);
        }

        public static uint FromNetworkByteOrder(this uint ui)
        {
            return ToNetworkByteOrder(ui);
        }
    }
}