// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Text;

namespace IxMilia.Pdf
{
    public abstract class PdfStreamItem
    {
        internal abstract void WriteToStream(PdfStreamWriterStatus writerStatus, StringBuilder stringBuilder);
    }
}
