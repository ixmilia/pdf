// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace IxMilia.Pdf.Test
{
    public class PdfWriterTests : PdfTestBase
    {
        public static PdfMeasurement PageWidth = PdfMeasurement.Inches(8.5);
        public static PdfMeasurement PageHeight= PdfMeasurement.Inches(11.0);
        public const double ThirtyDegrees = Math.PI / 6.0;
        public const double FortyFiveDegrees = Math.PI / 4.0;
        public const double SixtyDegrees = Math.PI / 3.0;
        public const double OneHundredEightyDegrees = Math.PI;
        public static PdfPoint PageCenter = new PdfPoint(PageWidth / 2.0, PageHeight / 2.0);

        [Fact]
        public void WriteEmptyFileTest()
        {
            var file = new PdfFile();
            file.Pages.Add(PdfPage.NewLetter());
            var expected = @"
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
<</Length 28>>
stream
0 w
0 0 0 RG
0 0 0 rg
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
308
%%EOF
";
            AssertFileEquals(file, expected);

            // verify that floating point values are written correctly
            var existingCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("de-DE");
                AssertFileEquals(file, expected);
            }
            finally
            {
                CultureInfo.CurrentCulture = existingCulture;
            }
        }

        [Fact]
        public void VerifyPageLinesTest()
        {
            var builder = new PdfPathBuilder()
            {
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Zero, PdfMeasurement.Zero),
                    new PdfPoint(PageWidth, PageHeight)),
                new PdfLine(
                    new PdfPoint(PageWidth, PdfMeasurement.Zero),
                    new PdfPoint(PdfMeasurement.Zero, PageHeight))
            };
            AssertPathBuilderContains(builder, @"
0.00 0.00 m
612.00 792.00 l
612.00 0.00 m
0.00 792.00 l
");
        }

        [Fact]
        public void VerifyLineStrokeTest()
        {
            var builder = new PdfPathBuilder()
            {
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(0.0), PdfMeasurement.Points(0.0)),
                    new PdfPoint(PdfMeasurement.Points(1.0), PdfMeasurement.Points(1.0))),
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(2.0), PdfMeasurement.Points(2.0)),
                    new PdfPoint(PdfMeasurement.Points(3.0), PdfMeasurement.Points(3.0)),
                    state: new PdfStreamState(strokeWidth: PdfMeasurement.Points(1.1))),
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(4.0), PdfMeasurement.Points(4.0)),
                    new PdfPoint(PdfMeasurement.Points(5.0), PdfMeasurement.Points(5.0)),
                    state: new PdfStreamState(strokeColor: new PdfColor(1.0, 0.0, 0.0))),
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(6.0), PdfMeasurement.Points(6.0)),
                    new PdfPoint(PdfMeasurement.Points(7.0), PdfMeasurement.Points(7.0)),
                    state: new PdfStreamState(strokeColor: new PdfColor(0.0, 1.0, 0.0), strokeWidth: PdfMeasurement.Points(2.2)))
            };
            AssertPathBuilderContains(builder, @"
0 w
0 0 0 RG
0 0 0 rg
0.00 0.00 m
1.00 1.00 l
S
1.1 w
2.00 2.00 m
3.00 3.00 l
S
0 w
1 0 0 RG
4.00 4.00 m
5.00 5.00 l
S
2.2 w
0 1 0 RG
6.00 6.00 m
7.00 7.00 l
S
");
        }

        [Fact]
        public void VerifyFillTest()
        {
            var builder = new PdfPathBuilder()
            {
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(0.0), PdfMeasurement.Points(0.0)),
                    new PdfPoint(PdfMeasurement.Points(1.0), PdfMeasurement.Points(1.0))),
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(2.0), PdfMeasurement.Points(2.0)),
                    new PdfPoint(PdfMeasurement.Points(3.0), PdfMeasurement.Points(3.0)),
                    state: new PdfStreamState(strokeWidth: PdfMeasurement.Points(1.1))),
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(4.0), PdfMeasurement.Points(4.0)),
                    new PdfPoint(PdfMeasurement.Points(5.0), PdfMeasurement.Points(5.0)),
                    state: new PdfStreamState(nonStrokeColor: new PdfColor(1.0, 0.0, 0.0))),
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Points(6.0), PdfMeasurement.Points(6.0)),
                    new PdfPoint(PdfMeasurement.Points(7.0), PdfMeasurement.Points(7.0)),
                    state: new PdfStreamState(nonStrokeColor: new PdfColor(0.0, 1.0, 0.0), strokeWidth: PdfMeasurement.Points(2.2)))
            };
            AssertPathBuilderContains(builder, @"
0 w
0 0 0 RG
0 0 0 rg
0.00 0.00 m
1.00 1.00 l
S
1.1 w
2.00 2.00 m
3.00 3.00 l
S
0 w
1 0 0 rg
4.00 4.00 m
5.00 5.00 l
S
2.2 w
0 1 0 rg
6.00 6.00 m
7.00 7.00 l
S
");
        }

        [Fact]
        public void VerifyPageTextAndFontTest()
        {
            var file = new PdfFile();
            var page = PdfPage.NewLetter();
            var text = new PdfText("foo", new PdfFontType1(PdfFontType1Type.Helvetica), PdfMeasurement.Points(12.0), new PdfPoint(PdfMeasurement.Inches(1.0), PdfMeasurement.Inches(2.0)));
            page.Items.Add(text);
            file.Pages.Add(page);
            AssertFileContains(file, @"
BT
    /F1 12.00 Tf
    72.00 144.00 Td
    (foo) Tj
ET
");

            AssertFileContains(file, @"
/Resources <</Font <</F1 5 0 R>>>>
");

            AssertFileContains(file, "<</Type /Font /Subtype /Type1 /BaseFont /Helvetica>>");

            text.CharacterWidth = PdfMeasurement.Points(0.25);
            AssertFileContains(file, @"
BT
    /F1 12.00 Tf
    72.00 144.00 Td
    0.25 Tc
    (foo) Tj
ET
");
        }

        [Fact]
        public void VerifyFontsAreAddedOnSaveTest()
        {
            var file = new PdfFile();
            var page = PdfPage.NewLetter();
            var text = new PdfText("foo", new PdfFontType1(PdfFontType1Type.Helvetica), PdfMeasurement.Points(12.0), new PdfPoint());
            page.Items.Add(text);
            file.Pages.Add(page);
            Assert.Equal(0, file.Fonts.Count);

            using (var ms = new MemoryStream())
            {
                file.Save(ms);
            }

            Assert.True(ReferenceEquals(text.Font, file.Fonts.Single()));
        }

        [Fact]
        public void VerifyFontsAreReusedTest()
        {
            var file = new PdfFile();
            var page = PdfPage.NewLetter();
            var font = new PdfFontType1(PdfFontType1Type.Helvetica);

            // font used twice on one page
            page.Items.Add(new PdfText("foo", font, PdfMeasurement.Points(12.0), new PdfPoint()));
            page.Items.Add(new PdfText("foo", font, PdfMeasurement.Points(12.0), new PdfPoint()));
            file.Pages.Add(page);

            // font used a third time on another page
            page = PdfPage.NewLetter();
            page.Items.Add(new PdfText("foo", font, PdfMeasurement.Points(12.0), new PdfPoint()));
            file.Pages.Add(page);

            // font objects should only be listed once
            AssertFileContains(file, "/Resources <</Font <</F1 5 0 R>>>>");
            AssertFileDoesNotContain(file, "/Resources <</Font <</F1 5 0 R>> /Font <</F1 5 0 R>>"); // duplicate font resource object
            AssertFileDoesNotContain(file, "/Resources <</Font <</F1 5 0 R>> /Font <</F2 6 0 R>>"); // duplicate font resource
            AssertFileContainsCount(file, "<</Type /Font /Subtype /Type1 /BaseFont /Helvetica>>", 1);
        }

        [Fact]
        public void VerifyCircleTest()
        {
            AssertPathItemContains(new PdfCircle(PageCenter, PageWidth / 2.0), @"
612.00 396.00 m
612.00 565.00 475.00 702.00 306.00 702.00 c
306.00 702.00 m
137.00 702.00 0.00 565.00 0.00 396.00 c
0.00 396.00 m
0.00 227.00 137.00 90.00 306.00 90.00 c
306.00 90.00 m
475.00 90.00 612.00 227.00 612.00 396.00 c
");
        }

        [Fact]
        public void VerifyEllipseTest()
        {
            AssertPathItemContains(new PdfEllipse(PageCenter, PageWidth / 2.0, PageHeight / 2.0), @"
612.00 396.00 m
612.00 614.70 475.00 792.00 306.00 792.00 c
306.00 792.00 m
137.00 792.00 0.00 614.70 0.00 396.00 c
0.00 396.00 m
0.00 177.30 137.00 0.00 306.00 0.00 c
306.00 0.00 m
475.00 0.00 612.00 177.30 612.00 396.00 c
");
        }

        [Fact]
        public void VerifyRotatedEllipseTest()
        {
            AssertPathItemContains(new PdfEllipse(PageCenter, PageWidth / 2.0, PageHeight / 4.0, rotationAngle: ThirtyDegrees), @"
571.00 549.00 m
516.33 643.70 353.36 651.97 207.00 567.47 c
207.00 567.47 m
60.64 482.97 -13.68 337.70 41.00 243.00 c
41.00 243.00 m
95.67 148.30 258.64 140.03 405.00 224.53 c
405.00 224.53 m
551.36 309.03 625.68 454.30 571.00 549.00 c
");
        }

        [Fact]
        public void VerifyCircularArcsTest()
        {
            // partially fills first quadrant
            AssertPathItemContains(new PdfArc(PageCenter, radius: PageWidth / 2, startAngle: 0.0, endAngle: FortyFiveDegrees), @"
612.00 396.00 m
612.00 477.16 579.76 554.99 522.37 612.37 c
");

            // partially fills first quadrant, fully fills second quadrant, partially fills third quadrant
            AssertPathItemContains(new PdfArc(PageCenter, radius: PageWidth / 2, startAngle: FortyFiveDegrees, endAngle: OneHundredEightyDegrees + FortyFiveDegrees), @"
522.37 612.37 m
464.99 669.76 387.16 702.00 306.00 702.00 c
306.00 702.00 m
137.00 702.00 0.00 565.00 0.00 396.00 c
0.00 396.00 m
0.00 314.84 32.24 237.01 89.63 179.63 c
");

            // partially fills fourth quadrant, partially fills first quadrant, spans zero
            AssertPathItemContains(new PdfArc(PageCenter, radius: PageWidth / 2, startAngle: -FortyFiveDegrees, endAngle: FortyFiveDegrees), @"
612.00 396.00 m
612.00 477.16 579.76 554.99 522.37 612.37 c
522.37 179.63 m
579.76 237.01 612.00 314.84 612.00 396.00 c
");

            // partially fills first and fourth quadrants, fully fills second and third
            AssertPathItemContains(new PdfArc(PageCenter, radius: PageWidth / 2, startAngle: FortyFiveDegrees, endAngle: -FortyFiveDegrees), @"
522.37 612.37 m
464.99 669.76 387.16 702.00 306.00 702.00 c
306.00 702.00 m
137.00 702.00 0.00 565.00 0.00 396.00 c
0.00 396.00 m
0.00 227.00 137.00 90.00 306.00 90.00 c
306.00 90.00 m
387.16 90.00 464.99 122.24 522.37 179.63 c
");

            // partially fills first quadrant, but doesn't touch either end
            AssertPathItemContains(new PdfArc(PageCenter, radius: PageWidth / 2, startAngle: ThirtyDegrees, endAngle: SixtyDegrees), @"
571.00 549.00 m
544.15 595.52 505.52 634.15 459.00 661.00 c
");

            // has two tails in first quadrant and fully fills the other three, spans zero
            AssertPathItemContains(new PdfArc(PageCenter, radius: PageWidth / 2, startAngle: SixtyDegrees, endAngle: ThirtyDegrees), @"
459.00 661.00 m
412.48 687.86 359.71 702.00 306.00 702.00 c
612.00 396.00 m
612.00 449.71 597.86 502.48 571.00 549.00 c
306.00 702.00 m
137.00 702.00 0.00 565.00 0.00 396.00 c
0.00 396.00 m
0.00 227.00 137.00 90.00 306.00 90.00 c
306.00 90.00 m
475.00 90.00 612.00 227.00 612.00 396.00 c
");
        }

        [Fact]
        public void VerifyElipticalArcsTest()
        {
            // partially fills first quadrant, fully fills second
            AssertPathItemContains(new PdfEllipse(PageCenter, radiusX: PageWidth / 2, radiusY: PageWidth / 4, startAngle: FortyFiveDegrees, endAngle: OneHundredEightyDegrees), @"
522.37 504.19 m
464.99 532.88 387.16 549.00 306.00 549.00 c
306.00 549.00 m
137.00 549.00 0.00 480.50 0.00 396.00 c
");

            // partially fills first quadrant, fully fills second, entire ellipse rotated
            AssertPathItemContains(new PdfEllipse(PageCenter, radiusX: PageWidth / 2, radiusY: PageWidth / 4, startAngle: FortyFiveDegrees, endAngle: OneHundredEightyDegrees, rotationAngle: ThirtyDegrees), @"
439.29 597.88 m
375.25 594.04 299.78 569.08 229.50 528.50 c
229.50 528.50 m
83.14 444.00 -1.25 316.18 41.00 243.00 c
");
        }
    }
}
