namespace IxMilia.Pdf
{
    public class PdfSetState : PdfPathCommand
    {
        public PdfStreamState State { get; set; }

        public PdfSetState(PdfStreamState state)
        {
            State = state;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.SetState(State);
        }
    }
}
