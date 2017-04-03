// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

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
<</Type /Page /Parent 2 0 R /Contents 4 0 R /MediaBox [0 0 612.00 792.00] /Resources <<>>>>
endobj
4 0 obj
<</Length 33>>
stream
/DeviceRGB CS
0 w
0 0 0 SC
S
endstream
endobj
xref
0 5
0000000000 65535 f
0000000010 00000 n
0000000060 00000 n
0000000118 00000 n
0000000228 00000 n
trailer <</Size 5 /Root 1 0 R>>
startxref
313
%%EOF
");
        }

        [Fact]
        public void VerifyPageLinesTest()
        {
            var file = new PdfFile();
            var page = new PdfPage(8.5 * 72, 11 * 72);
            page.Items.Add(new PdfLine(
                new PdfPoint(0.0, 0.0),
                new PdfPoint(8.5 * 72, 11 * 72)));
            page.Items.Add(new PdfLine(
                new PdfPoint(8.5 * 72, 0.0),
                new PdfPoint(0.0, 11 * 72)));
            file.Pages.Add(page);
            AssertFileContains(file, @"
0.00 0.00 m
612.00 792.00 l
612.00 0.00 m
0.00 792.00 l
");
        }

        [Fact]
        public void VerifyLineStrokeTest()
        {
            var page = new PdfPage(8.5 * 72, 11 * 72);
            page.Items.Add(new PdfLine(
                new PdfPoint(0.0, 0.0),
                new PdfPoint(1.0, 1.0)
            ));
            page.Items.Add(new PdfLine(
                new PdfPoint(2.0, 2.0),
                new PdfPoint(3.0, 3.0),
                strokeWidth: 1.1
            ));
            page.Items.Add(new PdfLine(
                new PdfPoint(4.0, 4.0),
                new PdfPoint(5.0, 5.0),
                color: new PdfColor(1.0, 0.0, 0.0)
            ));
            page.Items.Add(new PdfLine(
                new PdfPoint(6.0, 6.0),
                new PdfPoint(7.0, 7.0),
                strokeWidth: 2.2,
                color: new PdfColor(0.0, 1.0, 0.0)
            ));
            AssertPageContains(page, @"
0 w
0 0 0 SC
0.00 0.00 m
1.00 1.00 l
S
1.1 w
2.00 2.00 m
3.00 3.00 l
S
0 w
1 0 0 SC
4.00 4.00 m
5.00 5.00 l
S
2.2 w
0 1 0 SC
6.00 6.00 m
7.00 7.00 l
S
");
        }

        [Fact]
        public void VerifyPageTextAndFontTest()
        {
            var file = new PdfFile();
            var page = new PdfPage(8.5 * 72, 11 * 72);
            page.Items.Add(new PdfText("foo", new PdfFont("Helvetica"), 12.0, new PdfPoint(1.0 * 72, 2.0 * 72)));
            file.Pages.Add(page);
            AssertFileContains(file, @"
BT
    /F1 12 Tf
    72.00 144.00 Td
    (foo) Tj
ET
");

            AssertFileContains(file, @"
/Resources <</Font <</F1 5 0 R>>>>
");

            AssertFileContains(file, "<</Type /Font /Subtype /Type1 /BaseFont /Helvetica>>");
        }
    }
}
