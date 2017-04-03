// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public class PdfFile
    {
        internal PdfCatalog Catalog { get; } = new PdfCatalog();
        public IList<PdfPage> Pages => Catalog.Pages.Pages;

        public void Save(Stream stream)
        {
            AssignIds();
            var offsets = new List<int>();
            var encoding = new UTF8Encoding(false);
            void AddOffset()
            {
                offsets.Add((int)stream.Position);
            }

            stream.WriteLine("%PDF-1.6");

            AddOffset();
            Catalog.WriteTo(stream);

            AddOffset();
            Catalog.Pages.WriteTo(stream);

            foreach (var page in Pages)
            {
                AddOffset();
                page.Parent = Catalog.Pages;
                page.WriteTo(stream);
                AddOffset();
                page.Stream.WriteTo(stream);
            }

            var xrefCount = offsets.Count + 1; // to account for the required zero-id object
            var xrefLoc = stream.Position;
            stream.WriteLine("xref");
            stream.WriteLine($"0 {xrefCount}");
            stream.WriteLine($"0000000000 {ushort.MaxValue} f"); // said required zero-id free object
            for (var i = 0; i < offsets.Count; i++)
            {
                stream.WriteLine($"{offsets[i].ToString().PadLeft(10, '0')} {(0).ToString().PadLeft(5, '0')} n");
            }

            stream.WriteLine($"trailer <</Size {xrefCount} /Root {Catalog.Id} 0 R>>");
            stream.WriteLine("startxref");
            stream.Write(xrefLoc.ToString());
            stream.WriteLine("");
            stream.Write("%%EOF");
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
