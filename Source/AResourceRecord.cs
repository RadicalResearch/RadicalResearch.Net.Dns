namespace RadicalResearch.Net.Dns
{
    using System.Net;

    public class AResourceRecord : ResourceRecordBase
    {
        private readonly IPAddress ipAddress;

        public AResourceRecord(DnsQuery query, uint timeToLive, IPAddress ipAddress)
            :base(query, timeToLive)
        {
            this.ipAddress = ipAddress;
        }

        public override RecordType Type
        {
            get
            {
                return RecordType.A;
            }
        }

        public IPAddress IpAddress
        {
            get
            {
                return ipAddress;
            }
        }

        public override byte[] GetData(DnsNameWriter dnsNameWriter)
        {
            return this.ipAddress.GetAddressBytes();
        }
    }
}
