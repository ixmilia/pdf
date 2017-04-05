// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public abstract class PdfPathCommand
    {
        internal abstract void Write(PdfStreamWriter writer);
    }
}
