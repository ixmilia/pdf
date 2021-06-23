using System.Collections.Generic;
using System.Linq;
using System.Text;
using IxMilia.Pdf.Encoders;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal class PdfStream : PdfObject
    {
        public IList<PdfStreamItem> Items { get; } = new List<PdfStreamItem>();
        public IList<IPdfEncoder> Encoders { get; } = new List<IPdfEncoder>();

        public PdfStream(params IPdfEncoder[] encoders)
        {
            Encoders = encoders.ToList();
        }

        protected override byte[] GetContent()
        {
            var bytes = new List<byte>();
            var writer = new PdfStreamWriter();
            foreach (var item in Items)
            {
                item.Write(writer);
            }

            writer.Stroke();

            var data = writer.GetBytes();
            foreach (var encoder in Encoders)
            {
                data = encoder.Encode(data);
            }

            var sb = new StringBuilder();
            sb.Append($"<</Length {data.Length}");
            if (Encoders.Count > 0)
            {
                sb.Append("\r\n");
                sb.Append($"  /Filter {Encoders.AsEncoderList()}\r\n");
            }

            sb.Append(">>\r\n");
            sb.Append("stream\r\n");
            bytes.AddRange(sb.ToString().GetBytes());

            bytes.AddRange(data);
            bytes.AddRange("endstream\r\n".GetBytes());

            return bytes.ToArray();
        }
    }
}
