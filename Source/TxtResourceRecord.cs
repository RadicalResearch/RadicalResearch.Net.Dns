namespace RadicalResearch.Net.Dns
{
    using System.IO;
    using System.Text;

    public class TxtResourceRecord : ResourceRecordBase
    {
        private readonly string[] strings;

        public TxtResourceRecord(DnsQuery query, uint timeToLive, string[] strings)
            : base(query, timeToLive)
        {
            this.strings = strings;
        }

        public override RecordType Type
        {
            get
            {
                return RecordType.TXT;
            }
        }

        public override byte[] GetData(DnsNameWriter dnsNameWriter)
        {
            using (var stream = new MemoryStream())
            {
                foreach (var s in this.strings)
                {
                    var buffer = Encoding.ASCII.GetBytes(s);
                    stream.WriteByte((byte)buffer.Length);
                    stream.Write(buffer, 0, buffer.Length);
                }

                return stream.ToArray();
            }
        }
    }
}
