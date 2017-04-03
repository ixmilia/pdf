// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    internal class PdfStreamWriterStatus
    {
        public PdfColor LastWrittenColor { get; set; }
        public double LastStrokeWidth { get; set; }

        public PdfStreamWriterStatus()
        {
            LastWrittenColor = new PdfColor(); // black
            LastStrokeWidth = 0.0;
        }
    }
}
