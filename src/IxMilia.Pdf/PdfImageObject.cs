using System.Collections.Generic;
using System.Linq;
using System.Text;
using IxMilia.Pdf.Encoders;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfImageObject : PdfObject
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public PdfColorSpace ColorSpace { get; set; }
        public int BitsPerComponent { get; set; }
        public byte[] Data { get; set; }
        public IList<IPdfEncoder> Encoders { get; } = new List<IPdfEncoder>();

        public string ReferenceId => $"Im{Id}";

        public PdfImageObject(int width, int height, PdfColorSpace colorSpace, int bitsPerComponent, byte[] data, params IPdfEncoder[] encoders)
        {
            Width = width;
            Height = height;
            ColorSpace = colorSpace;
            BitsPerComponent = bitsPerComponent;
            Data = data;
            Encoders = encoders.ToList();
        }

        protected override byte[] GetContent()
        {
            var data = Data;
            foreach (var encoder in Encoders)
            {
                data = encoder.Encode(data);
            }

            var sb = new StringBuilder();
            sb.Append("<</Type /XObject\r\n");
            sb.Append("  /Subtype /Image\r\n");
            sb.Append($"  /Width {Width}\r\n");
            sb.Append($"  /Height {Height}\r\n");
            sb.Append($"  /ColorSpace /{ColorSpace.ToPatternName()}\r\n");
            sb.Append($"  /BitsPerComponent {BitsPerComponent}\r\n");
            sb.Append($"  /Length {data.Length}");
            if (Encoders.Count > 0)
            {
                sb.Append("\r\n");
                sb.Append($"  /Filter {Encoders.AsEncoderList()}");
            }

            sb.Append(">>");

            var bytes = new List<byte>();
            bytes.AddRange(sb.ToString().GetNewLineBytes());
            bytes.AddRange("stream".GetNewLineBytes());
            bytes.AddRange(data);
            if (data.Length >= 2 &&
                data[data.Length - 2] != '\r' &&
                data[data.Length - 1] != '\n')
            {
                // add newline if not already present
                bytes.AddRange("\r\n".GetBytes());
            }

            bytes.AddRange("endstream".GetNewLineBytes());

            return bytes.ToArray();
        }
    }
}
