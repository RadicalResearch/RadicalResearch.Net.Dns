namespace RadicalResearch.Net.Dns
{
    public abstract class ResourceRecordBase
    {
        public DnsName Name { get; set; }

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
