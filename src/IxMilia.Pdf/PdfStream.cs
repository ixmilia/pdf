// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Text;

namespace IxMilia.Pdf
{
    internal class PdfStream : PdfObject
    {
        protected override string GetContent()
        {
            var body = "";
            var sb = new StringBuilder();
            sb.Append($"<</Length {body.Length}>>\r\n");
            sb.Append("stream\r\n");
            sb.Append(body);
            sb.Append("\r\n");
            sb.Append("endstream\r\n");
            return sb.ToString();
        }
    }
}
