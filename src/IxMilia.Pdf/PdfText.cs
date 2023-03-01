using System;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfText : PdfStreamItem
    {
        public string Value { get; set; }
        public PdfFont Font { get; set; }
        public PdfMeasurement FontSize { get; set; }
        public PdfPoint Location { get; set; }
        public double RotationAngle { get; set; }

        public PdfStreamState State { get; set; }
        public PdfMeasurement CharacterWidth { get; set; }

        public PdfText(string value, PdfFont font, PdfMeasurement fontSize, PdfPoint location, PdfStreamState state = default(PdfStreamState))
            : this(value, font, fontSize, location, 0.0, state)
        {
        }

        public PdfText(string value, PdfFont font, PdfMeasurement fontSize, PdfPoint location, double rotationAngle, PdfStreamState state = default(PdfStreamState))
        {
            Value = value;
            Font = font;
            FontSize = fontSize;
            Location = location;
            RotationAngle = rotationAngle;
            State = state;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.SetState(State);
            writer.WriteLine("BT");
            writer.WriteLine($"    /F{Font.FontId} {FontSize.AsPoints().AsFixed()} Tf");
            if (RotationAngle == 0.0)
            {
                // just write location
                writer.WriteLine($"    {Location} Td");
            }
            else
            {
                // write the whole matrix
                var cos = Math.Cos(RotationAngle);
                var sin = Math.Sin(RotationAngle);
                writer.WriteLine($"    {cos.AsFixed()} {sin.AsFixed()} {(-sin).AsFixed()} {cos.AsFixed()} {Location} Tm");
            }

            if (CharacterWidth.RawValue != 0.0)
            {
                writer.WriteLine($"    {CharacterWidth.AsPoints().AsFixed()} Tc");
            }

            writer.Write("    [(");
            foreach (var c in Value)
            {
                switch (c)
                {
                    case '(':
                    case ')':
                    case '\\':
                        writer.Write((byte)'\\');
                        break;
                }

                writer.Write((byte)c);
            }

            writer.WriteLine(")] TJ");
            writer.WriteLine("ET");
        }
    }
}
