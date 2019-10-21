// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal class PdfStreamWriter
    {
        private List<byte> _bytes = new List<byte>();
        private PdfStreamState _lastState = default(PdfStreamState);

        public PdfStreamWriter()
        {
            // set initial state
            WriteStrokeWidth(_lastState.StrokeWidth);
            WriteStrokeColor(_lastState.StrokeColor);
            WriteNonStrokeColor(_lastState.NonStrokeColor);
        }

        public void Write(byte b)
        {
            _bytes.Add(b);
        }

        public void Write(string value)
        {
            _bytes.AddRange(value.GetBytes());
        }

        public void WriteLine(string value)
        {
            Write(value);
            Write("\r\n");
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

        private void WriteStrokeWidth(PdfMeasurement strokeWidth)
        {
            WriteLine($"{strokeWidth.AsPoints().AsInvariant()} w");
        }

        private void WriteStrokeColor(PdfColor color)
        {
            WriteLine($"{color} RG");
        }

        private void WriteNonStrokeColor(PdfColor color)
        {
            WriteLine($"{color} rg");
        }

        public byte[] GetBytes()
        {
            return _bytes.ToArray();
        }

        public int Length => _bytes.Count;

        internal void Stroke()
        {
            WriteLine("S");
        }
    }
}
