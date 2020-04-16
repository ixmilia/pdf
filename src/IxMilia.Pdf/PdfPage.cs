using System;
using System.Collections.Generic;
using System.Linq;
using IxMilia.Pdf.Encoders;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfPage : PdfObject
    {
        public const double LetterWidth = 8.5;
        public const double LetterHeight = 11.0;

        internal PdfStream Stream { get; }

        public PdfMeasurement Width { get; set; }
        public PdfMeasurement Height { get; set; }
        public IList<PdfStreamItem> Items => Stream.Items;

        public PdfPage(PdfMeasurement width, PdfMeasurement height, params IPdfEncoder[] encoders)
        {
            Width = width;
            Height = height;
            Stream = new PdfStream(encoders);
        }

        public static PdfPage NewLetter()
        {
            return new PdfPage(PdfMeasurement.Inches(LetterWidth), PdfMeasurement.Inches(LetterHeight));
        }

        public static PdfPage NewLetterLandscape()
        {
            return new PdfPage(PdfMeasurement.Inches(LetterHeight), PdfMeasurement.Inches(LetterWidth));
        }

        public static PdfPage NewASeries(int n, bool isPortrait = true)
        {
            var longSide = (int)(1000.0 / Math.Pow(2.0, (2.0 * n - 1.0) / 4.0) + 0.2);
            var shortSide = (int)(longSide / Math.Sqrt(2.0));
            switch (n)
            {
                case 0:
                case 3:
                case 6:
                    // manually correct rounding errors
                    shortSide++;
                    break;
            }

            var width = isPortrait ? shortSide : longSide;
            var height = isPortrait ? longSide : shortSide;
            return new PdfPage(PdfMeasurement.Mm(width), PdfMeasurement.Mm(height));
        }

        public override IEnumerable<PdfObject> GetChildren()
        {
            yield return Stream;
            foreach (var text in Items.OfType<PdfText>())
            {
                yield return text.Font;
            }
        }

        protected override byte[] GetContent()
        {
            var resources = new List<string>();
            var seenFonts = new HashSet<PdfFont>();
            foreach (var font in GetAllFonts())
            {
                if (seenFonts.Add(font))
                {
                    resources.Add($"/Font <</F{font.FontId} {font.Id.AsObjectReference()}>>");
                }
            }

            return $"<</Type /Page /Parent {Parent.Id.AsObjectReference()} /Contents {Stream.Id.AsObjectReference()} /MediaBox [0 0 {Width.AsPoints().AsFixed()} {Height.AsPoints().AsFixed()}] /Resources <<{string.Join(" ", resources)}>>>>".GetNewLineBytes();
        }

        private IEnumerable<PdfFont> GetAllFonts()
        {
            return Items.OfType<PdfText>().Select(t => t.Font);
        }
    }
}
