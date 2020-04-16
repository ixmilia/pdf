using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfLine : IPdfPathItem
    {
        public PdfPoint P1 { get; set; }
        public PdfPoint P2 { get; set; }
        public PdfStreamState State { get; set; }

        public PdfLine(PdfPoint p1, PdfPoint p2, PdfStreamState state = default(PdfStreamState))
        {
            P1 = p1;
            P2 = p2;
            State = state;
        }

        public IEnumerable<PdfPathCommand> GetCommands()
        {
            yield return new PdfSetState(State);
            yield return new PdfPathMoveTo(P1);
            yield return new PdfPathLineTo(P2);
        }
    }
}
