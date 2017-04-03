// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal class PdfStream : PdfObject
    {
        public IList<PdfStreamItem> Items { get; } = new List<PdfStreamItem>();

        protected override byte[] GetContent()
        {
            var writer = new PdfStreamWriter();
            foreach (var item in Items)
            {
                item.Write(writer);
            }

            writer.Stroke();

            var sb = new StringBuilder();
            sb.Append($"<</Length {writer.Length}>>\r\n");
            sb.Append("stream\r\n");
            sb.Append(writer.ToString());
            sb.Append("endstream\r\n");
            return sb.ToString().GetBytes();
        }
    }
}
