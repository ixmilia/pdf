// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfLine
    {
        public PdfPoint P1 { get; set; }
        public PdfPoint P2 { get; set; }
        public double StrokeWidth { get; set; }
        public PdfColor Color { get; set; }

        public PdfLine(PdfPoint p1, PdfPoint p2, double strokeWidth = 0.0, PdfColor color = default(PdfColor))
        {
            P1 = p1;
            P2 = p2;
            StrokeWidth = strokeWidth;
            Color = color;
        }
    }
}
