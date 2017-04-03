// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Text;

namespace IxMilia.Pdf
{
    public class PdfText : PdfStreamItem
    {
        public string Value { get; set; }
        public PdfFont Font { get; set; }
        public double FontSize { get; set; }
        public PdfPoint Location { get; set; }
        public PdfColor Color { get; set; }

        public PdfText(string value, PdfFont font, double fontSize, PdfPoint location)
        {
            Value = value;
            Font = font;
            FontSize = fontSize;
            Location = location;
        }

        internal override void WriteToStream(PdfStreamWriterStatus writerStatus, StringBuilder stringBuilder)
        {
            if (writerStatus.LastWrittenColor != Color)
            {
                stringBuilder.Append("S\r\n");
                stringBuilder.Append($"{Color.R} {Color.G} {Color.B} SC\r\n");
                writerStatus.LastWrittenColor = Color;
            }

            stringBuilder.Append("BT\r\n");
            stringBuilder.Append($"    /F{Font.FontId} {FontSize} Tf\r\n");
            stringBuilder.Append($"    {Location.X:f2} {Location.Y:f2} Td\r\n");
            stringBuilder.Append($"    ({Value}) Tj\r\n");
            stringBuilder.Append("ET\r\n");
        }
    }
}
