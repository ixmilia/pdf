// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfPathLineTo : PdfPathCommand
    {
        public PdfPoint Point { get; set; }

        public PdfPathLineTo(PdfPoint point)
        {
            Point = point;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.WriteLine($"{Point} l");
        }
    }
}
