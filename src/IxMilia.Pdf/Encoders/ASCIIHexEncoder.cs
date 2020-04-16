using System.IO;

namespace IxMilia.Pdf.Encoders
{
    public class ASCIIHexEncoder : IPdfEncoder
    {
        private const int BufferSize = 1024;
        private const int LineSize = 64;
        private readonly byte[] Characters = new byte[]
        {
            (byte)'0',
            (byte)'1',
            (byte)'2',
            (byte)'3',
            (byte)'4',
            (byte)'5',
            (byte)'6',
            (byte)'7',
            (byte)'8',
            (byte)'9',
            (byte)'A',
            (byte)'B',
            (byte)'C',
            (byte)'D',
            (byte)'E',
            (byte)'F',
        };

        public string DisplayName => "ASCIIHexDecode";

        public byte[] Encode(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                var buffer = new byte[BufferSize];
                var writtenLineSize = 0;
                foreach (var b in data)
                {
                    var lower = b & 0b1111;
                    var upper = (b >> 4) & 0b1111;
                    output.WriteByte(Characters[upper]);
                    output.WriteByte(Characters[lower]);
                    writtenLineSize++;
                    if (writtenLineSize >= LineSize)
                    {
                        output.WriteByte((byte)'\r');
                        output.WriteByte((byte)'\n');
                        writtenLineSize = 0;
                    }
                }

                output.WriteByte((byte)'>');
                output.WriteByte((byte)'\r');
                output.WriteByte((byte)'\n');
                return output.ToArray();
            }
        }
    }
}
