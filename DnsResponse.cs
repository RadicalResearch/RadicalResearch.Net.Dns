namespace RadicalResearch.Net.Dns
{
    using System.IO;

    public class DnsResponse : DnsMessageBase
    {
        public DnsResponse(DnsQuery query) :
            this(query.MessageId, query.QueryName, query.QueryType, query.QueryClass)
        {
        }

        private DnsResponse(ushort messageId, DnsName queryName, RecordType queryType, RecordClass queryClass)
        {
            this.MessageId = messageId;
            this.QueryName = queryName;
            this.QueryType = queryType;
            this.QueryClass = queryClass;
        }

        public override MessageFlags Flags
        {
            get
            {
                var flags = (MessageFlags)0;
                flags |= MessageFlags.Response;
                //flags |= MessageFlags.RecursionAvailable;
                flags |= MessageFlags.AuthoritativeAnswer;
                return flags;
            }
        }

        public byte[] GetBytes()
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new DnsWriter(0, memoryStream))
            {
                var dnsNameWriter = new DnsNameWriter(memoryStream);

                writer.Write(this.MessageId.ToNetworkByteOrder());
                writer.Write(((ushort)this.Flags).ToNetworkByteOrder());

                writer.Write(this.QueryCount.ToNetworkByteOrder());
                writer.Write(((ushort)this.AnswerRecords.Count).ToNetworkByteOrder());
                writer.Write(((ushort)this.NameServerRecords.Count).ToNetworkByteOrder());
                writer.Write(((ushort)this.AdditionalRecords.Count).ToNetworkByteOrder());

                foreach (var resourceRecord in this.AnswerRecords)
                {
                    resourceRecord.Write(dnsNameWriter, writer);
                }

                foreach (var resourceRecord in this.NameServerRecords)
                {
                    resourceRecord.Write(dnsNameWriter, writer);
                }

                foreach (var resourceRecord in this.AdditionalRecords)
                {
                    resourceRecord.Write(dnsNameWriter, writer);
                }

                return memoryStream.ToArray();
            }
        }
    }
}