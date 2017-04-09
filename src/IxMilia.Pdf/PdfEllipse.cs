// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfEllipse : IPdfPathItem
    {
        private const double BezierConstant = 0.551915024494;

        public PdfPoint Center { get; set; }
        public virtual double RadiusX { get; set; }
        public virtual double RadiusY { get; set; }
        public PdfStreamState State { get; set; }

        public PdfEllipse(PdfPoint center, double radiusX, double radiusY, PdfStreamState state = default(PdfStreamState))
        {
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
            State = state;
        }

        public IEnumerable<PdfPathCommand> GetCommands()
        {
            var xaxis = new PdfPoint(RadiusX, 0.0);
            var yaxis = new PdfPoint(0.0, RadiusY);

            var right = Center + xaxis;
            var top = Center + yaxis;
            var left = Center - xaxis;
            var bottom = Center - yaxis;

            var xoffset = xaxis * BezierConstant;
            var yoffset = yaxis * BezierConstant;

            yield return new PdfSetState(State);
            yield return new PdfPathMoveTo(right);
            yield return new PdfCubicBezier(right + yoffset, top + xoffset, top);
            yield return new PdfCubicBezier(top - xoffset, left + yoffset, left);
            yield return new PdfCubicBezier(left - yoffset, bottom - xoffset, bottom);
            yield return new PdfCubicBezier(bottom + xoffset, right - yoffset, right);
        }
    }
}
