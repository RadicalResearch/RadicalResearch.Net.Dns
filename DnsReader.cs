namespace RadicalResearch.Net.Dns
{
    using System;
    using System.IO;

    public class DnsReader : IDisposable
    {
        private readonly Stream stream;

        public DnsReader(Stream stream)
        {
            this.stream = stream;
        }

        public ushort ReadUInt16()
        {
            var buffer = new byte[2];
            this.stream.Read(buffer, 0, buffer.Length);
            return (ushort)Aggregate(0, buffer);
        }

        /// <summary>
        /// Read message flags at the current position.
        /// </summary>
        /// <returns>The <see cref="MessageFlags"/>.</returns>
        public MessageFlags ReadFlags()
        {
            return (MessageFlags)this.ReadUInt16();
        }

        public byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];
            this.stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public void Skip(int count)
        {
            this.stream.Seek(count, SeekOrigin.Current);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.stream.Dispose();
        }

        /// <summary>
        /// Aggregate a value from the specified bytes.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="buffer">The bytes to aggregate.</param>
        /// <param name="start">The index withing the buffer to start.</param>
        /// <param name="count">The number of bytes to aggregate.</param>
        /// <returns>The aggregated <see cref="int"/> value.</returns>
        private static ulong Aggregate(ulong value, byte[] buffer, int start, int count)
        {
            for (var i = 0; i < count; i++)
            {
                value <<= 8;
                value |= buffer[start + i];
            }

            return value;
        }

        /// <summary>
        /// Aggregate a value the specified bytes.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="buffer">The bytes to aggregate.</param>
        /// <returns>The aggregated <see cref="int"/> value.</returns>
        private static ulong Aggregate(ulong value, byte[] buffer)
        {
            return Aggregate(value, buffer, 0, buffer.Length);
        }
    }
}
