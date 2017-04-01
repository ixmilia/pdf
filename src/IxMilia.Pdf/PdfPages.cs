// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Pdf
{
    internal sealed class PdfPages : PdfObject
    {
        public IList<PdfPage> Pages { get; } = new List<PdfPage>();

        protected override string GetContent()
        {
            return $"<</Type /Pages /Kids [{string.Join(" ", Pages.Select(p => $"{p.Id} 0 R"))}] /Count {Pages.Count}>>\r\n";
        }
    }
}
