// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    internal sealed class PdfCatalog : PdfObject
    {
        public PdfPages Pages { get; } = new PdfPages();

        protected override string GetContent()
        {
            return $"<</Type /Catalog /Pages {Pages.Id} 0 R>>\r\n";
        }
    }
}
