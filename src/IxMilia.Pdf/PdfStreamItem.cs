namespace IxMilia.Pdf
{
    public abstract class PdfStreamItem
    {
        internal abstract void Write(PdfStreamWriter writer);
    }
}
