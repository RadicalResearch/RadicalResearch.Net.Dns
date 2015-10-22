namespace RadicalResearch.Net.Dns
{
    using System.IO;
    using System.Text;

    internal sealed class DnsWriter : BinaryWriter
    {
        private readonly long offset;

        public DnsWriter(long offset, Stream output)
            : base(output, Encoding.ASCII, true)
        {
            this.offset = offset;
        }

        public long Position
        {
            get
            {
                return this.OutStream.Position + this.offset;
            }
        }
    }
}