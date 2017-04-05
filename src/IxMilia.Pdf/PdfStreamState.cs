// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public struct PdfStreamState
    {
        public PdfColor Color { get; set; }
        public double StrokeWidth { get; set; }

        public PdfStreamState(PdfColor? color = null, double? strokeWidth = null)
        {
            Color = color ?? default(PdfColor);
            StrokeWidth = strokeWidth ?? default(double);
        }
    }
}
