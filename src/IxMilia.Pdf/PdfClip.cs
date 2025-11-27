using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfClip : PdfPathCommand
    {
        public PdfMeasurement X { get; set; }
        public PdfMeasurement Y { get; set; }
        public PdfMeasurement Width { get; set; }
        public PdfMeasurement Height { get; set; }

        public PdfClip(PdfMeasurement x, PdfMeasurement y, PdfMeasurement width, PdfMeasurement height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.WriteLine("n"); // end previous path
            writer.WriteLine($"{X.AsPoints().AsFixed()} {Y.AsPoints().AsFixed()} {Width.AsPoints().AsFixed()} {Height.AsPoints().AsFixed()} re");
            writer.WriteLine("W"); // clip
            writer.WriteLine("n"); // end this path
        }
    }
}
