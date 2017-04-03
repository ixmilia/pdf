// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfFile
    {
        internal PdfCatalog Catalog { get; } = new PdfCatalog();
        public IList<PdfPage> Pages => Catalog.Pages.Pages;

        private List<int> _offsets = new List<int>();

        public void Save(Stream stream)
        {
            _offsets.Clear();
            AssignIds();

            stream.WriteLine("%PDF-1.6");
            WriteObject(Catalog, stream);
            WriteObject(Catalog.Pages, stream);

            foreach (var page in Pages)
            {
                page.Parent = Catalog.Pages;
                WriteObject(page, stream);
                WriteObject(page.Stream, stream);
            }

            var xrefCount = _offsets.Count + 1; // to account for the required zero-id object
            var xrefLoc = stream.Position;
            stream.WriteLine("xref");
            stream.WriteLine($"0 {xrefCount}");
            stream.WriteLine($"0000000000 {ushort.MaxValue} f"); // said required zero-id free object
            for (var i = 0; i < _offsets.Count; i++)
            {
                stream.WriteLine($"{_offsets[i].ToString().PadLeft(10, '0')} {(0).ToString().PadLeft(5, '0')} n");
            }

            stream.WriteLine($"trailer <</Size {xrefCount} /Root {Catalog.Id} 0 R>>");
            stream.WriteLine("startxref");
            stream.Write(xrefLoc.ToString());
            stream.WriteLine("");
            stream.Write("%%EOF");
        }

        private void WriteObject(PdfObject obj, Stream stream)
        {
            _offsets.Add((int)stream.Position);
            obj.WriteTo(stream);
        }

        private void AssignIds()
        {
            var id = 1;
            Catalog.Id = id++;
            Catalog.Pages.Id = id++;
            foreach (var page in Pages)
            {
                page.Id = id++;
                page.Stream.Id = id++;
            }
        }
    }
}
