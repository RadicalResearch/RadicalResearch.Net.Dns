namespace RadicalResearch.Net.Dns
{
    using System.IO;

    public sealed class DnsQuery : DnsMessageBase
    {
        private MessageFlags flags;

        public DnsResponse CreateResponse()
        {
            return new DnsResponse(this);
        }

        public override MessageFlags Flags
        {
            get
            {
                return this.flags;
            }
        }

        internal static DnsQuery Read(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return Read(stream);
            }
        }

        internal static DnsQuery Read(Stream stream)
        {
            using (var reader = new DnsReader(stream))
            {
                var messageId = reader.ReadUInt16();
                var flags = reader.ReadFlags();
                var queryCount = reader.ReadUInt16();

                reader.ReadBytes(6);
                var queryName = DnsName.Read(stream);
                var queryType = reader.ReadUInt16();
                var queryClass = reader.ReadUInt16();

                var query = new DnsQuery
                {
                    flags = flags,
                    MessageId = messageId,
                    QueryCount = queryCount,
                    QueryType = (RecordType)queryType,
                    QueryClass = (RecordClass)queryClass,
                    QueryName = queryName
                };

                return query;
            }
        }
    }
}