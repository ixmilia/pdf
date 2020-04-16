namespace IxMilia.Pdf
{
    public struct PdfStreamState
    {
        public PdfColor NonStrokeColor { get; set; }
        public PdfColor StrokeColor { get; set; }
        public PdfMeasurement StrokeWidth { get; set; }

        public PdfStreamState(PdfColor? nonStrokeColor = null, PdfColor? strokeColor = null, PdfMeasurement? strokeWidth = null)
        {
            NonStrokeColor = nonStrokeColor ?? default(PdfColor);
            StrokeColor = strokeColor ?? default(PdfColor);
            StrokeWidth = strokeWidth ?? default(PdfMeasurement);
        }
    }
}
