// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfArc : PdfEllipse
    {
        public PdfArc(PdfPoint center, PdfMeasurement radius, double startAngle, double endAngle, PdfStreamState state = default(PdfStreamState))
            : base(center, radius, radius, startAngle: startAngle, endAngle: endAngle, state: state)
        {
        }
    }
}
