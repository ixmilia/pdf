// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using IxMilia.Pdf.Extensions;
using System.Text;

namespace IxMilia.Pdf
{
    internal class PdfStreamWriter
    {
        private StringBuilder _sb = new StringBuilder();
        private PdfColor _lastColor = new PdfColor(); // black
        private double _lastWidth = 0.0;

        public PdfStreamWriter()
        {
            // set initial state
            WriteLine("/DeviceRGB CS");
            WriteStrokeWidth(_lastWidth);
            WriteColor(_lastColor);
        }

        public void WriteLine(string value)
        {
            _sb.Append(value);
            _sb.Append("\r\n");
        }

        public void SetState(PdfColor? color = null, double? strokeWidth = null)
        {
            var newColor = color ?? _lastColor;
            var newWidth = strokeWidth ?? _lastWidth;

            if (newColor != _lastColor || newWidth != _lastWidth)
            {
                Stroke();
            }

            if (newWidth != _lastWidth)
            {
                WriteStrokeWidth(newWidth);
            }

            if (newColor != _lastColor)
            {
                WriteColor(newColor);
            }

            _lastWidth = newWidth;
            _lastColor = newColor;
        }

        private void WriteStrokeWidth(double strokeWidth)
        {
            WriteLine($"{strokeWidth.AsInvariant()} w");
        }

        private void WriteColor(PdfColor color)
        {
            WriteLine($"{color} SC");
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        public int Length => _sb.Length;

        internal void Stroke()
        {
            WriteLine("S");
        }
    }
}
