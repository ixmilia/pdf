using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public struct PdfMatrix
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }
        public double E { get; set; }
        public double F { get; set; }

        public PdfMatrix(double a, double b, double c, double d, double e, double f)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
        }

        public override string ToString()
        {
            return $"{A.AsFixed()} {B.AsFixed()} {C.AsFixed()} {D.AsFixed()} {E.AsFixed()} {F.AsFixed()} cm";
        }

        public static PdfMatrix ScaleThenTranslate(double xScale, double yScale, double xOffset, double yOffset)
        {
            return new PdfMatrix(xScale, 0.0, 0.0, yScale, xOffset, yOffset);
        }
    }
}
