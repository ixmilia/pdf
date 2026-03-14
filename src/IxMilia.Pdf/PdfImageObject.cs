using System.Collections.Generic;
using System.Linq;
using System.Text;
using IxMilia.Pdf.Encoders;
using IxMilia.Pdf.Extensions;
using IxMilia.Pdf.Objects;

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

            var obj = new PdfDictionary();
            obj.Items["Type"] = new PdfName("XObject");
            obj.Items["Subtype"] = new PdfName("Image");
            obj.Items["Width"] = new PdfInteger(Width);
            obj.Items["Height"] = new PdfInteger(Height);
            obj.Items["ColorSpace"] = new PdfName(ColorSpace.ToPatternName());
            obj.Items["BitsPerComponent"] = new PdfInteger(BitsPerComponent);
            obj.Items["Length"] = new PdfInteger(data.Length);
            if (Encoders.Count > 0)
            {
                obj.Items["Filter"] = new PdfArray(Encoders.Reverse().Select(e => new PdfName(e.DisplayName)).ToArray());
            }

            var sb = new StringBuilder();
            sb.Append(obj.ToString());

            var bytes = new List<byte>();
            bytes.AddRange(sb.ToString().GetBytes());
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
