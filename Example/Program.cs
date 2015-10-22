using System;

namespace RadicalResearch.Example
{
    using System.Net;
    using RadicalResearch.Net.Dns;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var listener = new DnsListener(Resolver))
            {
                listener.Start();
                Console.ReadLine();
                listener.Stop();
            }
        }
        
        public static DnsResponse Resolver(DnsQuery query)
        {
            const uint TimeToLive = 300;
            var response = query.CreateResponse();

            switch (query.QueryType)
            {
                // Resolve all A queries to localhost
                case RecordType.A:
                    var aResourceRecord = new AResourceRecord(query, TimeToLive, IPAddress.Loopback);
                    response.AnswerRecords.Add(aResourceRecord);
                    break;

                // Resolve all TXT to "Hello World"
                case RecordType.TXT:
                    var txtResourceRecord = new TxtResourceRecord(query, TimeToLive, new []{ "Hello world"});
                    response.AnswerRecords.Add(txtResourceRecord);
                    break;
            }

            return response;
        }
    }
}
