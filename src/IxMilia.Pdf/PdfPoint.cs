// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public struct PdfPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        
        public PdfPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X.AsFixed()} {Y.AsFixed()}";
        }

        public static PdfPoint operator +(PdfPoint a, PdfPoint b)
        {
            return new PdfPoint(a.X + b.X, a.Y + b.Y);
        }

        public static PdfPoint operator -(PdfPoint a, PdfPoint b)
        {
            return new PdfPoint(a.X - b.X, a.Y - b.Y);
        }

        public static PdfPoint operator -(PdfPoint p)
        {
            return p * -1.0;
        }

        public static PdfPoint operator *(PdfPoint p, double s)
        {
            return new PdfPoint(p.X * s, p.Y * s);
        }

        public static PdfPoint operator *(double s, PdfPoint p)
        {
            return p * s;
        }

        public static PdfPoint operator /(PdfPoint p, double s)
        {
            return p * (1.0 / s);
        }

        public static bool operator ==(PdfPoint a, PdfPoint b)
        {
            if (ReferenceEquals(a, b))
                return true;
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(PdfPoint a, PdfPoint b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * 17 ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is PdfPoint)
                return this == (PdfPoint)obj;
            return false;
        }
    }
}
