// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfCircle : IPdfPathItem
    {
        private const double BezierConstant = 0.551915024494;

        public PdfPoint Center { get; set; }
        public double Radius { get; set; }
        public PdfStreamState State { get; set; }

        public PdfCircle(PdfPoint center, double radius, PdfStreamState state = default(PdfStreamState))
        {
            Center = center;
            Radius = radius;
            State = state;
        }

        public IEnumerable<PdfPathCommand> GetCommands()
        {
            var top = new PdfPoint(Center.X, Center.Y + Radius);
            var right = new PdfPoint(Center.X + Radius, Center.Y);
            var bottom = new PdfPoint(Center.X, Center.Y - Radius);
            var left = new PdfPoint(Center.X - Radius, Center.Y);

            yield return new PdfSetState(State);

            // quadrant 1
            {
                var p0 = top;
                var p1 = new PdfPoint(Center.X + BezierConstant * Radius, Center.Y + Radius);
                var p2 = new PdfPoint(Center.X + Radius, Center.Y + BezierConstant * Radius);
                var p3 = right;
                yield return new PdfPathMoveTo(p0);
                yield return new PdfCubicBezier(p1, p2, p3);
            }

            // quadrant 4
            {
                var p1 = new PdfPoint(Center.X + Radius, Center.Y - BezierConstant * Radius);
                var p2 = new PdfPoint(Center.X + BezierConstant * Radius, Center.Y - Radius);
                var p3 = bottom;
                yield return new PdfCubicBezier(p1, p2, p3);
            }

            // quadrant 3
            {
                var p1 = new PdfPoint(Center.X - BezierConstant * Radius, Center.Y - Radius);
                var p2 = new PdfPoint(Center.X - Radius, Center.Y - BezierConstant * Radius);
                var p3 = left;
                yield return new PdfCubicBezier(p1, p2, p3);
            }

            // quadrant 2
            {
                var p1 = new PdfPoint(Center.X - Radius, Center.Y + BezierConstant * Radius);
                var p2 = new PdfPoint(Center.X - BezierConstant * Radius, Center.Y + Radius);
                var p3 = top;
                yield return new PdfCubicBezier(p1, p2, p3);
            }
        }
    }
}
