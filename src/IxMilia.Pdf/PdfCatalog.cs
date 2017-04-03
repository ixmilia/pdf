// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal sealed class PdfCatalog : PdfObject
    {
        public PdfPages Pages { get; } = new PdfPages();

        public override IEnumerable<PdfObject> GetChildren()
        {
            yield return Pages;
        }

        protected override byte[] GetContent()
        {
            return $"<</Type /Catalog /Pages {Pages.Id} 0 R>>".GetNewLineBytes();
        }
    }
}
