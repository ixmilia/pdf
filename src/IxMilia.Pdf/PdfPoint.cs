// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

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
            return $"{X:f2} {Y:f2}";
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
