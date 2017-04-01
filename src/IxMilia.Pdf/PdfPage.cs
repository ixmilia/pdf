// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfPage : PdfObject
    {
        internal PdfPages Parent { get; set; }
        internal PdfStream Stream { get; } = new PdfStream();

        public double Width { get; set; }
        public double Height { get; set; }

        public PdfPage(double width, double height)
        {
            Width = width;
            Height = height;
        }

        protected override string GetContent()
        {
            return $"<</Type /Page /Parent {Parent.Id} 0 R /Contents {Stream.Id} 0 R /MediaBox [0 0 {Width:f} {Height:f}] /Resources<<>>>>\r\n";
        }
    }
}
