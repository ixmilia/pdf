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
            var top = new PdfPoint(Center.X, Center.Y + RadiusY);
            var right = new PdfPoint(Center.X + RadiusX, Center.Y);
            var bottom = new PdfPoint(Center.X, Center.Y - RadiusY);
            var left = new PdfPoint(Center.X - RadiusX, Center.Y);

            yield return new PdfSetState(State);

            // quadrant 1
            {
                var p0 = top;
                var p1 = new PdfPoint(Center.X + BezierConstant * RadiusX, Center.Y + RadiusY);
                var p2 = new PdfPoint(Center.X + RadiusX, Center.Y + BezierConstant * RadiusY);
                var p3 = right;
                yield return new PdfPathMoveTo(p0);
                yield return new PdfCubicBezier(p1, p2, p3);
            }

            // quadrant 4
            {
                var p1 = new PdfPoint(Center.X + RadiusX, Center.Y - BezierConstant * RadiusY);
                var p2 = new PdfPoint(Center.X + BezierConstant * RadiusX, Center.Y - RadiusY);
                var p3 = bottom;
                yield return new PdfCubicBezier(p1, p2, p3);
            }

            // quadrant 3
            {
                var p1 = new PdfPoint(Center.X - BezierConstant * RadiusX, Center.Y - RadiusY);
                var p2 = new PdfPoint(Center.X - RadiusX, Center.Y - BezierConstant * RadiusY);
                var p3 = left;
                yield return new PdfCubicBezier(p1, p2, p3);
            }

            // quadrant 2
            {
                var p1 = new PdfPoint(Center.X - RadiusX, Center.Y + BezierConstant * RadiusY);
                var p2 = new PdfPoint(Center.X - BezierConstant * RadiusX, Center.Y + RadiusY);
                var p3 = top;
                yield return new PdfCubicBezier(p1, p2, p3);
            }
        }
    }
}
