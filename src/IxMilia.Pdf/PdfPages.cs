using System.Collections.Generic;
using System.Linq;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal sealed class PdfPages : PdfObject
    {
        public IList<PdfPage> Pages { get; } = new List<PdfPage>();

        public override IEnumerable<PdfObject> GetChildren()
        {
            return Pages;
        }

        protected override byte[] GetContent()
        {
            return $"<</Type /Pages /Kids [{string.Join(" ", Pages.Select(p => p.Id.AsObjectReference()))}] /Count {Pages.Count}>>".GetNewLineBytes();
        }
    }
}
