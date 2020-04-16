namespace IxMilia.Pdf
{
    public class PdfArc : PdfEllipse
    {
        public PdfArc(PdfPoint center, PdfMeasurement radius, double startAngle, double endAngle, PdfStreamState state = default(PdfStreamState))
            : base(center, radius, radius, startAngle: startAngle, endAngle: endAngle, state: state)
        {
        }
    }
}
