// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public abstract class PdfStreamItem
    {
        public PdfStreamState State { get; private set; }

        protected PdfStreamItem(PdfStreamState state)
        {
            State = state;
        }

        internal abstract void Write(PdfStreamWriter writer);
    }
}
