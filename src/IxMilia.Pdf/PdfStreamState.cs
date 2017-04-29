// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public struct PdfStreamState
    {
        public PdfColor NonStrokeColor { get; set; }
        public PdfColor StrokeColor { get; set; }
        public double StrokeWidth { get; set; }

        public PdfStreamState(PdfColor? nonStrokeColor = null, PdfColor? strokeColor = null, double? strokeWidth = null)
        {
            NonStrokeColor = nonStrokeColor ?? default(PdfColor);
            StrokeColor = strokeColor ?? default(PdfColor);
            StrokeWidth = strokeWidth ?? default(double);
        }
    }
}
