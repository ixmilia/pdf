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
            var body = new StringBuilder();
            body.Append("/DeviceRGB CS\r\n");
            body.Append("0 w\r\n");
            body.Append("0 0 0 SC\r\n");
            var writerStatus = new PdfStreamWriterStatus();
            foreach (var item in Items)
            {
                item.WriteToStream(writerStatus, body);
            }

            body.Append("S\r\n");

            var sb = new StringBuilder();
            sb.Append($"<</Length {body.Length}>>\r\n");
            sb.Append("stream\r\n");
            sb.Append(body.ToString());
            sb.Append("endstream\r\n");
            return sb.ToString().GetBytes();
        }
    }
}
