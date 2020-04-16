using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfText : PdfStreamItem
    {
        public string Value { get; set; }
        public PdfFont Font { get; set; }
        public PdfMeasurement FontSize { get; set; }
        public PdfPoint Location { get; set; }

        public PdfStreamState State { get; set; }
        public PdfMeasurement CharacterWidth { get; set; }

        public PdfText(string value, PdfFont font, PdfMeasurement fontSize, PdfPoint location, PdfStreamState state = default(PdfStreamState))
        {
            Value = value;
            Font = font;
            FontSize = fontSize;
            Location = location;
            State = state;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.SetState(State);
            writer.WriteLine("BT");
            writer.WriteLine($"    /F{Font.FontId} {FontSize.AsPoints().AsFixed()} Tf");
            writer.WriteLine($"    {Location} Td");
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
