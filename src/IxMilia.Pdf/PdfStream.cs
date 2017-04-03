// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal class PdfStream : PdfObject
    {
        public IList<PdfLine> Lines { get; } = new List<PdfLine>();

        protected override byte[] GetContent()
        {
            var body = new StringBuilder();
            body.Append("/DeviceRGB CS\r\n");
            body.Append("0 w\r\n");
            body.Append("0 0 0 SC\r\n");
            var lastWidth = 0.0;
            var lastColor = new PdfColor(); // black
            foreach (var line in Lines)
            {
                if (lastWidth != line.StrokeWidth || lastColor != line.Color)
                {
                    body.Append("S\r\n");
                }

                if (lastWidth != line.StrokeWidth)
                {
                    body.Append($"{line.StrokeWidth} w\r\n");
                    lastWidth = line.StrokeWidth;
                }

                if (lastColor != line.Color)
                {
                    body.Append($"{line.Color.R} {line.Color.G} {line.Color.B} SC\r\n");
                    lastColor = line.Color;
                }

                body.Append($"{line.P1.X:f} {line.P1.Y:f} m\r\n");
                body.Append($"{line.P2.X:f} {line.P2.Y:f} l\r\n");
            }

            body.Append("S\r\n");

            var sb = new StringBuilder();
            sb.Append($"<</Length {body.Length}>>\r\n");
            sb.Append("stream\r\n");
            sb.Append(body.ToString());
            sb.Append("endstream\r\n");
            return sb.ToString().GetBytes();
        }
    }
}
