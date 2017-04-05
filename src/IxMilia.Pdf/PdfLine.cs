// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfLine : PdfStreamItem
    {
        public PdfPoint P1 { get; set; }
        public PdfPoint P2 { get; set; }

        public PdfLine(PdfPoint p1, PdfPoint p2, PdfStreamState state = default(PdfStreamState))
            : base(state)
        {
            P1 = p1;
            P2 = p2;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.SetState(State);
            writer.WriteLine($"{P1} m");
            writer.WriteLine($"{P2} l");
        }
    }
}
