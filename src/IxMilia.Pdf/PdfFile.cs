// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text;

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
            using (var writer = new StreamWriter(stream, encoding, 1024, leaveOpen: true))
            {
                void AddOffset()
                {
                    writer.Flush();
                    offsets.Add((int)stream.Position);
                }

                writer.Write("%PDF-1.6\r\n");

                AddOffset();
                Catalog.WriteTo(writer);

                AddOffset();
                Catalog.Pages.WriteTo(writer);

                foreach (var page in Pages)
                {
                    AddOffset();
                    page.Parent = Catalog.Pages;
                    page.WriteTo(writer);
                    AddOffset();
                    page.Stream.WriteTo(writer);
                }

                var xrefCount = offsets.Count + 1; // to account for the required zero-id object
                var xrefLoc = stream.Position;
                writer.Write("xref\r\n");
                writer.Write($"0 {xrefCount}\r\n");
                writer.Write($"0000000000 {ushort.MaxValue} f\r\n"); // said required zero-id free object
                for (var i = 0; i < offsets.Count; i++)
                {
                    writer.Write($"{offsets[i].ToString().PadLeft(10, '0')} {(0).ToString().PadLeft(5, '0')} n\r\n");
                }

                writer.Write($"trailer <</Size {xrefCount} /Root {Catalog.Id} 0 R>>\r\n");
                writer.Write("startxref\r\n");
                writer.Write(xrefLoc.ToString());
                writer.Write("\r\n");
                writer.Write("%%EOF");
            }
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
