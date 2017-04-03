// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfPage : PdfObject
    {
        internal PdfStream Stream { get; } = new PdfStream();

        public double Width { get; set; }
        public double Height { get; set; }
        public IList<PdfLine> Lines => Stream.Lines;

        public PdfPage(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override IEnumerable<PdfObject> GetChildren()
        {
            yield return Stream;
        }

        protected override byte[] GetContent()
        {
            return $"<</Type /Page /Parent {Parent.Id} 0 R /Contents {Stream.Id} 0 R /MediaBox [0 0 {Width:f} {Height:f}] /Resources<<>>>>".GetNewLineBytes();
        }
    }
}
