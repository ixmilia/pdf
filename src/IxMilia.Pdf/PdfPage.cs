// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfPage : PdfObject
    {
        internal PdfStream Stream { get; } = new PdfStream();

        public double Width { get; set; }
        public double Height { get; set; }
        public IList<PdfStreamItem> Items => Stream.Items;
        public IList<PdfFont> Fonts = new List<PdfFont>();

        public PdfPage(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override IEnumerable<PdfObject> GetChildren()
        {
            yield return Stream;
            foreach (var font in GetAllFonts())
            {
                yield return font;
            }
        }

        public override void BeforeWrite()
        {
            var id = 1;
            foreach (var font in GetAllFonts())
            {
                font.FontId = id++;
            }
        }

        protected override byte[] GetContent()
        {
            var resources = new List<string>();
            foreach (var font in GetAllFonts())
            {
                resources.Add($"/Font <</F{font.FontId} {font.Id.AsObjectReference()}>>");
            }

            return $"<</Type /Page /Parent {Parent.Id.AsObjectReference()} /Contents {Stream.Id.AsObjectReference()} /MediaBox [0 0 {Width.AsFixed()} {Height.AsFixed()}] /Resources <<{string.Join(" ", resources)}>>>>".GetNewLineBytes();
        }

        private IEnumerable<PdfFont> GetAllFonts()
        {
            return Fonts.Concat(Items.OfType<PdfText>().Select(t => t.Font));
        }
    }
}
