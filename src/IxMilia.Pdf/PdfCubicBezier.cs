namespace IxMilia.Pdf
{
    public class PdfCubicBezier : PdfPathCommand
    {
        public PdfPoint P1 { get; set; }
        public PdfPoint P2 { get; set; }
        public PdfPoint P3 { get; set; }

        public PdfCubicBezier(PdfPoint p1, PdfPoint p2, PdfPoint p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.WriteLine($"{P1} {P2} {P3} c");
        }
    }
}
