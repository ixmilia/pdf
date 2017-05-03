// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfEllipse : IPdfPathItem
    {
        public PdfPoint Center { get; set; }
        public virtual double RadiusX { get; set; }
        public virtual double RadiusY { get; set; }
        public double RotationAngle { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public PdfStreamState State { get; set; }

        private double EndAngleNormalized => EndAngle > StartAngle ? EndAngle : EndAngle + Math.PI * 2.0;

        public PdfEllipse(PdfPoint center, double radiusX, double radiusY, double rotationAngle = 0.0, double startAngle = 0.0, double endAngle = Math.PI * 2.0, PdfStreamState state = default(PdfStreamState))
        {
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
            RotationAngle = rotationAngle;
            StartAngle = startAngle;
            EndAngle = endAngle;
            State = state;

            while (StartAngle < 0.0)
            {
                StartAngle += Math.PI * 2.0;
            }

            while (EndAngle < 0.0)
            {
                EndAngle += Math.PI * 2.0;
            }
        }

        public IEnumerable<PdfPathCommand> GetCommands()
        {
            yield return new PdfSetState(State);
            foreach (var command in GetArcCommands(1))
            {
                yield return command;
            }

            foreach (var command in GetArcCommands(2))
            {
                yield return command;
            }

            foreach (var command in GetArcCommands(3))
            {
                yield return command;
            }

            foreach (var command in GetArcCommands(4))
            {
                yield return command;
            }
        }

        private IEnumerable<PdfPathCommand> GetArcCommands(int quadrant)
        {
            var qStart = Math.PI / 2.0 * (quadrant - 1);
            var qEnd = qStart + Math.PI / 2.0;

            if (StartAngle <= qStart && EndAngleNormalized >= qEnd)
            {
                // included angle spans quadrant; draw entire thing
                foreach (var command in GetCommands(qEnd - qStart, qStart))
                {
                    yield return command;
                }
            }
            else
            {
                var includesStart = StartAngle >= qStart && StartAngle <= qEnd;
                var includesEnd = EndAngle >= qStart && EndAngle <= qEnd;

                if (includesStart && includesEnd)
                {
                    if (EndAngle > StartAngle)
                    {
                        // acute angle
                        foreach (var command in GetCommands(EndAngle - StartAngle, StartAngle))
                        {
                            yield return command;
                        }
                    }
                    else
                    {
                        // two small angles
                        foreach (var command in GetCommands(qEnd - StartAngle, StartAngle))
                        {
                            yield return command;
                        }

                        foreach (var command in GetCommands(EndAngle - qStart, qStart))
                        {
                            yield return command;
                        }
                    }
                }
                else if (includesStart)
                {
                    // only includes start angle
                    foreach (var command in GetCommands(qEnd - StartAngle, StartAngle))
                    {
                        yield return command;
                    }
                }
                else if (includesEnd)
                {
                    // only includes end angle
                    foreach (var command in GetCommands(EndAngle - qStart, qStart))
                    {
                        yield return command;
                    }
                }
            }
        }

        private IEnumerable<PdfPathCommand> GetCommands(double theta, double startAngle)
        {
            if (theta <= 0.0)
            {
                yield break;
            }

            // from http://www.tinaja.com/glib/bezcirc2.pdf
            var x0 = Math.Cos(theta * 0.5);
            var y0 = -Math.Sin(theta * 0.5);

            var x3 = x0;
            var y3 = -y0;

            var x1 = (4.0 - x0) / 3.0;
            var y1 = ((1.0 - x0) * (3.0 - x0)) / (3.0 * y0);

            var x2 = x1;
            var y2 = -y1;

            var p0 = new PdfPoint(x0, y0);
            var p1 = new PdfPoint(x1, y1);
            var p2 = new PdfPoint(x2, y2);
            var p3 = new PdfPoint(x3, y3);

            // now rotate points by (theta / 2) + startAngle
            var rotTheta = theta * 0.5 + startAngle;
            p0 = p0.RotateAboutOrigin(rotTheta);
            p1 = p1.RotateAboutOrigin(rotTheta);
            p2 = p2.RotateAboutOrigin(rotTheta);
            p3 = p3.RotateAboutOrigin(rotTheta);

            // multiply by the radius
            p0 = new PdfPoint(p0.X * RadiusX, p0.Y * RadiusY);
            p1 = new PdfPoint(p1.X * RadiusX, p1.Y * RadiusY);
            p2 = new PdfPoint(p2.X * RadiusX, p2.Y * RadiusY);
            p3 = new PdfPoint(p3.X * RadiusX, p3.Y * RadiusY);

            // do final rotation
            p0 = p0.RotateAboutOrigin(RotationAngle);
            p1 = p1.RotateAboutOrigin(RotationAngle);
            p2 = p2.RotateAboutOrigin(RotationAngle);
            p3 = p3.RotateAboutOrigin(RotationAngle);

            // offset for the center
            p0 += Center;
            p1 += Center;
            p2 += Center;
            p3 += Center;

            yield return new PdfPathMoveTo(p0);
            yield return new PdfCubicBezier(p1, p2, p3);
        }
    }
}
