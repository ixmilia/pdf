using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public interface IPdfPathItem
    {
        IEnumerable<PdfPathCommand> GetCommands();
    }
}
