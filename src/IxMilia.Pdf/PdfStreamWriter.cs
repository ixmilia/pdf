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
            WriteStrokeWidth(_lastState.StrokeWidth);
            WriteStrokeColor(_lastState.StrokeColor);
            WriteNonStrokeColor(_lastState.NonStrokeColor);
        }

        public void WriteLine(string value)
        {
            _sb.Append(value);
            _sb.Append("\r\n");
        }

        public void SetState(PdfStreamState state)
        {
            if (state.StrokeColor != _lastState.StrokeColor || state.StrokeWidth != _lastState.StrokeWidth)
            {
                Stroke();
            }

            if (state.StrokeWidth != _lastState.StrokeWidth)
            {
                WriteStrokeWidth(state.StrokeWidth);
            }

            if (state.StrokeColor != _lastState.StrokeColor)
            {
                WriteStrokeColor(state.StrokeColor);
            }

            if (state.NonStrokeColor != _lastState.NonStrokeColor)
            {
                WriteNonStrokeColor(state.NonStrokeColor);
            }

            _lastState = state;
        }

        private void WriteStrokeWidth(double strokeWidth)
        {
            WriteLine($"{strokeWidth.AsInvariant()} w");
        }

        private void WriteStrokeColor(PdfColor color)
        {
            WriteLine($"{color} RG");
        }

        private void WriteNonStrokeColor(PdfColor color)
        {
            WriteLine($"{color} rg");
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
