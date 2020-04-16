namespace IxMilia.Pdf
{
    public class PdfPathMoveTo : PdfPathCommand
    {
        public PdfPoint Point { get; set; }

        public PdfPathMoveTo(PdfPoint point)
        {
            Point = point;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.WriteLine($"{Point} m");
        }
    }
}
