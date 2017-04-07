// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfCircle : PdfEllipse
    {
        public double Radius { get; set; }

        public override double RadiusX { get => Radius; set => Radius = value; }
        public override double RadiusY { get => Radius; set => Radius = value; }

        public PdfCircle(PdfPoint center, double radius, PdfStreamState state = default(PdfStreamState))
            : base(center, radius, radius, state)
        {
            Radius = radius;
        }
    }
}
