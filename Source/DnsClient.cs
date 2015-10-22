using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadicalResearch.Net.Dns
{
    class DnsClient
    {
        public void Send(DnsQuery query)
        {
            System.Net.Dns.GetHostAddresses("");
        }
    }
}
