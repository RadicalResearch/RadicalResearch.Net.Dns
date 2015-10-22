namespace RadicalResearch.Net.Dns
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class DnsNameWriter
    {
        private readonly Stream stream;

        private readonly LabelNode rootLabelNode;

        public DnsNameWriter(Stream stream)
        {
            this.stream = stream;
            this.rootLabelNode = new LabelNode(null, string.Empty);
        }

        public byte[] GetBytes(DnsName dnsName)
        {
            using (var stream = new MemoryStream())
            using (var writer = new DnsWriter(this.stream.Position, stream))
            {
                this.Write(dnsName, writer);
                return stream.ToArray();
            }
        }

        public void Write(DnsName dnsName)
        {
            using (var writer = new DnsWriter(0, this.stream))
            {
                this.rootLabelNode.Write(dnsName, writer);
            }
        }

        private void Write(DnsName dnsName, DnsWriter dnsWriter)
        {
            this.rootLabelNode.Write(dnsName, dnsWriter);
        }


        private class LabelNode
        {
            private readonly LabelNode parent;

            private string label { get; set; }

            private ushort? position { get; set; }

            private readonly IList<LabelNode> children;

            public LabelNode(LabelNode parent, string label)
            {
                this.label = label;
                this.parent = parent;
                this.children = new List<LabelNode>();
            }

            public void Write(IEnumerable<string> labels, DnsWriter dnsWriter)
            {
                var node = this.Create(labels);
                node.Write(dnsWriter);
            }

            private LabelNode Create(IEnumerable<string> labels)
            {
                var enumerable = labels as string[] ?? labels.ToArray();
                var lastLabel = enumerable.Last();
                var child = this.children.FirstOrDefault(x => x.label == lastLabel);
                if (child == null)
                {
                    child = new LabelNode(this, lastLabel);
                    this.children.Add(child);
                }

                var count = enumerable.Count() - 1;
                if (count > 0)
                {
                    child = child.Create(enumerable.Take(count));
                }

                return child;
            }

            private void Write(DnsWriter dnsWriter)
            {
                if (this.position.HasValue)
                {
                    this.WritePointer(dnsWriter);
                }
                else
                {
                    this.WriteLabel(dnsWriter);
                    if (this.parent != null)
                    {
                        this.parent.Write(dnsWriter);
                    }
                }
            }

            private void WriteLabel(DnsWriter dnsWriter)
            {
                this.position = (ushort)dnsWriter.Position;
                var bytes = Encoding.ASCII.GetBytes(this.label);
                dnsWriter.Write((byte)bytes.Length);
                dnsWriter.Write(bytes);
            }

            private void WritePointer(DnsWriter dnsWriter)
            {
                var pointer = (ushort)(this.position.Value | 0xc000);
                dnsWriter.Write(pointer.ToNetworkByteOrder());
            }
        }
    }
}
