namespace RadicalResearch.Net.Dns
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class DnsName : List<string>
    {
        public DnsName()
        {
        }

        public DnsName(string hostName) : this(hostName.Split(new[] { '.' }))
        {
        }

        public DnsName(string[] labels)
        {
            foreach (var label in labels)
            {
                this.Add(label);
            }
        }

        internal static DnsName Read(Stream stream)
        {
            var labels = new List<string>();
            var labelLength = stream.ReadByte();
            while (labelLength > 0)
            {
                var labelBytes = new byte[labelLength];
                stream.Read(labelBytes, 0, labelLength);
                var label = Encoding.ASCII.GetString(labelBytes);
                labels.Add(label);
                labelLength = stream.ReadByte();
            }

            return new DnsName(labels.ToArray());
        }

        public override string ToString()
        {
            return string.Join(".", this);
        }

        public byte[] GetBytes()
        {
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                for (var index = 0; index < this.Count; index++)
                {
                    var label = this[index];
                    var labelBytes = Encoding.ASCII.GetBytes(label);
                    binaryWriter.Write((byte)labelBytes.Length);
                    binaryWriter.Write(labelBytes);
                }

                binaryWriter.Write((byte)0);

                var bytes = memoryStream.ToArray();

                return bytes;
            }
        }
    }
}
