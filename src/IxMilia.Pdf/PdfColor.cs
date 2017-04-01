// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public struct PdfColor
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public PdfColor(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static bool operator ==(PdfColor a, PdfColor b)
        {
            if (ReferenceEquals(a, b))
                return true;
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(PdfColor a, PdfColor b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (R.GetHashCode() * 17 ^ G.GetHashCode()) * 17 ^ B.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is PdfColor)
                return this == (PdfColor)obj;
            return false;
        }
    }
}
