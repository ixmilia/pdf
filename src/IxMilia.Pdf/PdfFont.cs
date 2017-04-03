// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfFont : PdfObject
    {
        internal int FontId { get; set; }

        public string SubType { get; set; } = "Type1";
        public string BaseFont { get; set; }

        public PdfFont(string baseFont)
        {
            BaseFont = baseFont;
        }

        protected override byte[] GetContent()
        {
            return $"<</Type /Font /Subtype /{SubType} /BaseFont /{BaseFont}>>".GetNewLineBytes();
        }
    }
}
