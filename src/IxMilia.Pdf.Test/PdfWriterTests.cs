// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using Xunit;

namespace IxMilia.Pdf.Test
{
    public class PdfWriterTests : PdfTestBase
    {
        [Fact]
        public void WriteEmptyFileTest()
        {
            var file = new PdfFile();
            file.Pages.Add(new PdfPage(8.5 * 72, 11 * 72));
            AssertFileEquals(file, @"
%PDF-1.6
1 0 obj
<</Type /Catalog /Pages 2 0 R>>
endobj
2 0 obj
<</Type /Pages /Kids [3 0 R] /Count 1>>
endobj
3 0 obj
<</Type /Page /Parent 2 0 R /Contents 4 0 R /MediaBox [0 0 612.00 792.00] /Resources<<>>>>
endobj
4 0 obj
<</Length 0>>
stream

endstream
endobj
xref
0 5
0000000000 65535 f
0000000010 00000 n
0000000060 00000 n
0000000118 00000 n
0000000227 00000 n
trailer <</Size 5 /Root 1 0 R>>
startxref
227
%%EOF
");
        }
    }
}
