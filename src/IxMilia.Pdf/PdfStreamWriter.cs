// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using IxMilia.Pdf.Extensions;
using System.Text;

namespace IxMilia.Pdf
{
    internal class PdfStreamWriter
    {
        private StringBuilder _sb = new StringBuilder();
        private PdfStreamState _lastState = default(PdfStreamState);

        public PdfStreamWriter()
        {
            // set initial state
            WriteLine("/DeviceRGB CS");
            WriteStrokeWidth(_lastState.StrokeWidth);
            WriteColor(_lastState.Color);
        }

        public void WriteLine(string value)
        {
            _sb.Append(value);
            _sb.Append("\r\n");
        }

        public void SetState(PdfStreamState state)
        {
            if (state.Color != _lastState.Color || state.StrokeWidth != _lastState.StrokeWidth)
            {
                Stroke();
            }

            if (state.StrokeWidth != _lastState.StrokeWidth)
            {
                WriteStrokeWidth(state.StrokeWidth);
            }

            if (state.Color != _lastState.Color)
            {
                WriteColor(state.Color);
            }

            _lastState = state;
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
