// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using IxMilia.Pdf.Extensions;
using System.Text;

namespace IxMilia.Pdf
{
    public class PdfLine : PdfStreamItem
    {
        public PdfPoint P1 { get; set; }
        public PdfPoint P2 { get; set; }
        public double StrokeWidth { get; set; }
        public PdfColor Color { get; set; }

        public PdfLine(PdfPoint p1, PdfPoint p2, double strokeWidth = 0.0, PdfColor color = default(PdfColor))
        {
            P1 = p1;
            P2 = p2;
            StrokeWidth = strokeWidth;
            Color = color;
        }

        internal override void WriteToStream(PdfStreamWriterStatus writerStatus, StringBuilder stringBuilder)
        {
            if (writerStatus.LastStrokeWidth != StrokeWidth || writerStatus.LastWrittenColor != Color)
            {
                stringBuilder.Append("S\r\n");
            }

            if (writerStatus.LastStrokeWidth != StrokeWidth)
            {
                stringBuilder.Append($"{StrokeWidth} w\r\n");
                writerStatus.LastStrokeWidth = StrokeWidth;
            }

            if (writerStatus.LastWrittenColor != Color)
            {
                stringBuilder.Append($"{Color.AsWritable()} SC\r\n");
                writerStatus.LastWrittenColor = Color;
            }

            stringBuilder.Append($"{P1} m\r\n");
            stringBuilder.Append($"{P2} l\r\n");
        }
    }
}
