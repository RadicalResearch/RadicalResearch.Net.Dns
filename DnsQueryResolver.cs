namespace RadicalResearch.Net.Dns
{
    /// <summary>
    /// Provides DNS response records for a DNS query DnsMessageBase.
    /// </summary>
    /// <returns>
    /// A DNS response DnsMessageBase.
    /// </returns>
    public delegate DnsResponse DnsQueryResolver(DnsQuery query);
}