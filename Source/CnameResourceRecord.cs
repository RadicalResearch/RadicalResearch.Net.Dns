namespace RadicalResearch.Net.Dns
{
    public class CnameResourceRecord : ResourceRecordBase
    {
        private readonly DnsName dnsName;

        public CnameResourceRecord(DnsQuery query, uint timeToLive, DnsName dnsName)
            : base(query, timeToLive)
        {
            this.dnsName = dnsName;
        }

        public override RecordType Type
        {
            get
            {
                return RecordType.CNAME;
            }
        }

        public override byte[] GetData(DnsNameWriter dnsNameWriter)
        {
            return dnsNameWriter.GetBytes(this.dnsName);
        }
    }
}
