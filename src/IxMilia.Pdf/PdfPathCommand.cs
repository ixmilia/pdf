namespace IxMilia.Pdf
{
    public abstract class PdfPathCommand
    {
        internal abstract void Write(PdfStreamWriter writer);
    }
}
