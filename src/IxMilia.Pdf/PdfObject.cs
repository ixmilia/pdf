// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;

namespace IxMilia.Pdf
{
    public abstract class PdfObject
    {
        internal int Id { get; set; }

        protected abstract string GetContent();

        internal void WriteTo(StreamWriter writer)
        {
            writer.Write($"{Id} 0 obj\r\n");
            writer.Write(GetContent());
            writer.Write("endobj\r\n");
        }
    }
}
