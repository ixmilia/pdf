using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public struct PdfColor
    {
        private static readonly int DecimalPlaces = 3;

        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public PdfColor(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override string ToString()
        {
            return $"{R.AsFixed(DecimalPlaces)} {G.AsFixed(DecimalPlaces)} {B.AsFixed(DecimalPlaces)}";
        }

        public static bool operator ==(PdfColor a, PdfColor b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(PdfColor a, PdfColor b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (R.GetHashCode() * 17 ^ G.GetHashCode()) * 17 ^ B.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is PdfColor)
                return this == (PdfColor)obj;
            return false;
        }
    }
}
