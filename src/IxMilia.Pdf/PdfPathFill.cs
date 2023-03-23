namespace IxMilia.Pdf
{
    public class PdfPathFill : PdfPathCommand
    {
        internal override void Write(PdfStreamWriter writer)
        {
            writer.WriteLine("B");
        }
    }
}
