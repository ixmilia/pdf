using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public abstract class PdfFont : PdfObject
    {
        internal int FontId { get; set; }

        public abstract string SubType { get; }

        protected abstract string GetAdditionalProperties();

        protected override byte[] GetContent()
        {
            return $"<</Type /Font /Subtype /{SubType} {GetAdditionalProperties()}>>".GetNewLineBytes();
        }
    }
}
