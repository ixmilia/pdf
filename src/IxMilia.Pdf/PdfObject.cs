using System.Collections.Generic;
using System.IO;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public abstract class PdfObject
    {
        internal int Id { get; set; }

        public PdfObject Parent { get; internal set; }

        protected abstract byte[] GetContent();

        public virtual void BeforeWrite()
        {
        }

        public virtual IEnumerable<PdfObject> GetChildren()
        {
            yield break;
        }

        internal void WriteTo(Stream stream)
        {
            stream.WriteLine($"{Id} 0 obj");
            stream.WriteBytes(GetContent());
            stream.WriteLine("endobj");
        }
    }
}
