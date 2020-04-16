namespace IxMilia.Pdf
{
    public class PdfPathLineTo : PdfPathCommand
    {
        public PdfPoint Point { get; set; }

        public PdfPathLineTo(PdfPoint point)
        {
            Point = point;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.WriteLine($"{Point} l");
        }
    }
}
