// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfCircle : PdfEllipse
    {
        public PdfMeasurement Radius { get; set; }

        public override PdfMeasurement RadiusX { get => Radius; set => Radius = value; }
        public override PdfMeasurement RadiusY { get => Radius; set => Radius = value; }

        public PdfCircle(PdfPoint center, PdfMeasurement radius, PdfStreamState state = default(PdfStreamState))
            : base(center, radius, radius, state: state)
        {
            Radius = radius;
        }
    }
}
