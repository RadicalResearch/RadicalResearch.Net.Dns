namespace RadicalResearch.Net.Dns
{
    using System.Collections.Generic;

    public abstract class DnsMessageBase
    {
        public ushort MessageId { get; protected set; }

        public abstract MessageFlags Flags { get; }

        public ushort QueryCount { get; protected set; }

        public DnsName QueryName { get; protected set; }

        public RecordType QueryType { get; protected set; }

        public RecordClass QueryClass { get; protected set; }

        public IList<ResourceRecordBase> AnswerRecords { get; protected set; }

        public IList<ResourceRecordBase> NameServerRecords { get; protected set; }

        public IList<ResourceRecordBase> AdditionalRecords { get; protected set; }

        protected DnsMessageBase(ushort messageId, DnsName queryName, RecordType queryType, RecordClass queryClass)
            : this()
        {
            this.MessageId = messageId;
            this.QueryCount = 1;
            this.QueryName = queryName;
            this.QueryType = queryType;
            this.QueryClass = queryClass;
        }

        protected DnsMessageBase()
        {
            this.AnswerRecords = new List<ResourceRecordBase>();
            this.NameServerRecords = new List<ResourceRecordBase>();
            this.AdditionalRecords = new List<ResourceRecordBase>();
        }

        public override string ToString()
        {
            return string.Concat(this.QueryName, " ", this.QueryClass, " ", this.QueryType);
        }
    }
}
