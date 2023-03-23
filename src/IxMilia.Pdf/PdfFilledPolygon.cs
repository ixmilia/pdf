using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfFilledPolygon : IPdfPathItem
    {
        public PdfPoint[] Points { get; set; }
        public PdfStreamState State { get; set; }

        public PdfFilledPolygon(PdfPoint[] points, PdfStreamState state)
        {
            Points = points;
            State = state;
        }

        public IEnumerable<PdfPathCommand> GetCommands()
        {
            yield return new PdfSetState(State);
            yield return new PdfPathMoveTo(Points[0]);
            for (int i = 1; i < Points.Length; i++)
            {
                yield return new PdfPathLineTo(Points[i]);
            }

            yield return new PdfPathFill();
        }
    }
}
