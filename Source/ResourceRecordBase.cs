namespace RadicalResearch.Net.Dns
{
    using System;

    public abstract class ResourceRecordBase
    {
        protected ResourceRecordBase(DnsQuery query, uint timeToLive)
            : this(query.QueryName, query.QueryType, query.QueryClass, timeToLive)
        {
        }

        protected ResourceRecordBase(DnsName name, RecordType recordType, RecordClass recordClass, uint timeToLive)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.RecordType = recordType;
            this.RecordClass = recordClass;
            this.TimeToLive = timeToLive;
        }

        public DnsName Name { get; set; }

        public RecordType RecordType { get; set; }

        public RecordClass RecordClass { get; set; }

        public abstract RecordType Type { get; }

        public RecordClass Class { get; set; }

        public uint TimeToLive { get; set; }

        internal void Write(DnsNameWriter dnsNameWriter, DnsWriter dnsWriter)
        {
            dnsNameWriter.Write(this.Name);
            dnsWriter.Write(((ushort)this.Type).ToNetworkByteOrder());
            dnsWriter.Write(((ushort)this.Class).ToNetworkByteOrder());
            dnsWriter.Write(this.TimeToLive.ToNetworkByteOrder());

            var data = this.GetData(dnsNameWriter);
            dnsWriter.Write(((ushort)data.Length).ToNetworkByteOrder());
            dnsWriter.Write(data);
        }

        public abstract byte[] GetData(DnsNameWriter dnsNameWriter);
    }
}
