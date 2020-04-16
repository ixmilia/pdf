using System.Collections.Generic;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal sealed class PdfCatalog : PdfObject
    {
        public PdfPages Pages { get; } = new PdfPages();
        public IList<PdfFont> Fonts { get; } = new List<PdfFont>();

        public override IEnumerable<PdfObject> GetChildren()
        {
            yield return Pages;
            foreach (var font in Fonts)
            {
                yield return font;
            }
        }

        public override void BeforeWrite()
        {
            var seenFonts = new HashSet<PdfFont>(Fonts);
            var fontId = 1;
            AssignFontIds(this, seenFonts, ref fontId);
        }

        private void AssignFontIds(PdfObject obj, HashSet<PdfFont> seenFonts, ref int fontId)
        {
            if (obj is PdfFont font)
            {
                if (seenFonts.Add(font))
                {
                    font.FontId = fontId++;
                    Fonts.Add(font);
                }
            }

            foreach (var child in obj.GetChildren())
            {
                AssignFontIds(child, seenFonts, ref fontId);
            }
        }

        protected override byte[] GetContent()
        {
            return $"<</Type /Catalog /Pages {Pages.Id.AsObjectReference()}>>".GetNewLineBytes();
        }
    }
}
