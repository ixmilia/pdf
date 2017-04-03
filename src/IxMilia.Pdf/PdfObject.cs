// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    public abstract class PdfObject
    {
        internal int Id { get; set; }

        protected abstract byte[] GetContent();

        internal void WriteTo(Stream stream)
        {
            stream.WriteLine($"{Id} 0 obj");
            stream.WriteBytes(GetContent());
            stream.WriteLine("endobj");
        }
    }
}
