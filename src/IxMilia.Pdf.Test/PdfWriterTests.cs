using System;
using System.Globalization;
using System.IO;
using System.Linq;
using IxMilia.Pdf.Encoders;
using Xunit;

namespace IxMilia.Pdf.Test
{
    public class PdfWriterTests : PdfTestBase
    {
        public static PdfMeasurement PageWidth = PdfMeasurement.Inches(8.5);
        public static PdfMeasurement PageHeight = PdfMeasurement.Inches(11.0);
        public const double ThirtyDegrees = Math.PI / 6.0;
        public const double FortyFiveDegrees = Math.PI / 4.0;
        public const double SixtyDegrees = Math.PI / 3.0;
        public const double OneHundredEightyDegrees = Math.PI;
        public static PdfPoint PageCenter = new PdfPoint(PageWidth / 2.0, PageHeight / 2.0);

        [Fact]
        public void FileSystemAPITest()
        {
            var filePath = Path.GetTempFileName();
            var file = new PdfFile();
            file.Save(filePath);
            var fileText = File.ReadAllText(filePath);
            Assert.Contains("%PDF-1.6", fileText);

            try
            {
                File.Delete(filePath);
            }
            catch
            {
            }
        }

        [Fact]
        public void WriteEmptyFileTest()
        {
            var file = new PdfFile();
            file.Pages.Add(PdfPage.NewLetter());
            var expected = @"
%PDF-1.6
%" + "\u00E6\u00E6\u00E6\u00E6" /* need to ensure the `�` character isn't mangled by the test */ + @"
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
<</Length 52>>
stream
0 w
0.000 0.000 0.000 RG
0.000 0.000 0.000 rg
S
endstream
endobj
xref
0 5
0000000000 65535 f
0000000017 00000 n
0000000067 00000 n
0000000125 00000 n
0000000235 00000 n
trailer <</Size 5 /Root 1 0 R>>
startxref
339
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
0.000 0.000 0.000 RG
0.000 0.000 0.000 rg
0.00 0.00 m
1.00 1.00 l
S
1.1 w
2.00 2.00 m
3.00 3.00 l
S
0 w
1.000 0.000 0.000 RG
4.00 4.00 m
5.00 5.00 l
S
2.2 w
0.000 1.000 0.000 RG
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
0.000 0.000 0.000 RG
0.000 0.000 0.000 rg
0.00 0.00 m
1.00 1.00 l
S
1.1 w
2.00 2.00 m
3.00 3.00 l
S
0 w
1.000 0.000 0.000 rg
4.00 4.00 m
5.00 5.00 l
S
2.2 w
0.000 1.000 0.000 rg
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
    [(foo)] TJ
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
    [(foo)] TJ
ET
");
        }

        [Fact]
        public void VerifyTextIsRenderedAsRotatedTest()
        {
            var file = new PdfFile();
            var page = PdfPage.NewLetter();
            var angle = 45.0 * Math.PI / 180.0;
            var text = new PdfText("foo", new PdfFontType1(PdfFontType1Type.Helvetica), PdfMeasurement.Points(12.0), new PdfPoint(PdfMeasurement.Inches(1.0), PdfMeasurement.Inches(2.0)), angle);
            page.Items.Add(text);
            file.Pages.Add(page);
            AssertFileContains(file, @"
BT
    /F1 12.00 Tf
    0.71 0.71 -0.71 0.71 72.00 144.00 Tm
    [(foo)] TJ
ET
");
        }

        [Fact]
        public void StringEscapeSequencesTest()
        {
            var page = PdfPage.NewLetter();
            var text = new PdfText(@"outer (inner \backslash) after", new PdfFontType1(PdfFontType1Type.Helvetica), PdfMeasurement.Points(12.0), new PdfPoint());
            page.Items.Add(text);
            AssertPageContains(page, @"[(outer \(inner \\backslash\) after)] TJ");
        }

        [Fact]
        public void VerifyFontsAreAddedOnSaveTest()
        {
            var file = new PdfFile();
            var page = PdfPage.NewLetter();
            var text = new PdfText("foo", new PdfFontType1(PdfFontType1Type.Helvetica), PdfMeasurement.Points(12.0), new PdfPoint());
            page.Items.Add(text);
            file.Pages.Add(page);
            Assert.Empty(file.Fonts);

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
        public void VerifyEllipticalArcsTest()
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

        [Fact]
        public void VerifyEllipseReallyCloseToQuadrants1And4()
        {
            var el = new PdfEllipse(
                new PdfPoint(272.0, 396.0, 0.0),
                new PdfMeasurement(340.0, PdfMeasurementType.Point),
                new PdfMeasurement(339.99999999999994, PdfMeasurementType.Point),
                rotationAngle: 0.0,
                startAngle: 4.712388980384712,
                endAngle: 1.5707963267949143);

            var page = PdfPage.NewLetter();
            var builder = new PdfPathBuilder() { el };
            page.Items.Add(builder.ToPath());
            var file = new PdfFile();
            file.Pages.Add(page);

            AssertPathItemContains(el, @"
612.00 396.00 m
612.00 583.78 459.78 736.00 272.00 736.00 c
272.00 736.00 m
272.00 736.00 272.00 736.00 272.00 736.00 c
272.00 56.00 m
459.78 56.00 612.00 208.22 612.00 396.00 c
");
        }

        [Fact]
        public void VerifyFilledPolygon()
        {
            var p = new PdfFilledPolygon(new[]
            {
                new PdfPoint(0.0, 0.0, PdfMeasurementType.Point),
                new PdfPoint(1.0, 0.0, PdfMeasurementType.Point),
                new PdfPoint(1.0, 1.0, PdfMeasurementType.Point),
            }, new PdfStreamState());

            AssertPathItemContains(p, @"
0.00 0.00 m
1.00 0.00 l
1.00 1.00 l
B
");
        }

        [Fact]
        public void VerifyStreamFilterTest()
        {
            var page = new PdfPage(PdfMeasurement.Inches(8.5), PdfMeasurement.Inches(11.0), new ASCIIHexEncoder());
            var text = new PdfText("foo", new PdfFontType1(PdfFontType1Type.Helvetica), PdfMeasurement.Points(12.0), new PdfPoint(PdfMeasurement.Inches(1.0), PdfMeasurement.Inches(1.0)));
            page.Items.Add(text);

            var expected = @"
<</Length 233
  /Filter [/ASCIIHexDecode]
>>
stream
3020770D0A302E30303020302E30303020302E3030302052470D0A302E30303020302E30303020302E3030302072670D0A42540D0A202020202F46312031322E
30302054660D0A2020202037322E30302037322E30302054640D0A202020205B28666F6F295D20544A0D0A45540D0A530D0A>
endstream
endobj
";

            AssertPageContains(page, expected);
        }

        [Fact]
        public void VerifyGraphicsStreamTest()
        {
            var page = new PdfPage(PdfMeasurement.Inches(8.5), PdfMeasurement.Inches(11.0));

            var width = 100;
            var height = 100;
            var bpp = 3;
            var imageBytes = new byte[width * height * bpp]; // 0 is fine for this
            var imageObject = new PdfImageObject(width, height, PdfColorSpace.DeviceRGB, 8, imageBytes, new ASCIIHexEncoder());
            var xScale = 400;
            var yScale = 400;
            var xOffset = 100;
            var yOffset = 100;
            var transform = PdfMatrix.ScaleThenTranslate(xScale, yScale, xOffset, yOffset);
            var imageItem = new PdfImageItem(imageObject, transform);
            page.Items.Add(imageItem);

            // placement of image
            var expectedStream = @"
q
  400.00 0.00 0.00 400.00 100.00 100.00 cm
  /Im5 Do
Q
";
            AssertPageContains(page, expectedStream);

            // raw image data
            var expectedImageData = @"
<</Type /XObject
  /Subtype /Image
  /Width 100
  /Height 100
  /ColorSpace /DeviceRGB
  /BitsPerComponent 8
  /Length 60939
  /Filter [/ASCIIHexDecode]>>
stream
";
            AssertPageContains(page, expectedImageData);
        }

        [Fact]
        public void ClipTest()
        {
            var path = new PdfPath();
            var builder = new PdfPathBuilder()
            {
                new PdfLine(
                    new PdfPoint(PdfMeasurement.Zero, PageHeight / 2.0), // left middle
                    new PdfPoint(PageWidth / 2.0, PageHeight)), // top middle
                new PdfLine(
                    new PdfPoint(PageWidth / 2.0, PageHeight), // top middle
                    new PdfPoint(PageWidth, PageHeight / 2.0)), // right middle
                new PdfLine(
                    new PdfPoint(PageWidth, PageHeight / 2.0), // right middle
                    new PdfPoint(PageWidth / 2.0, PdfMeasurement.Zero)), // bottom middle
                new PdfLine(
                    new PdfPoint(PageWidth / 2.0, PdfMeasurement.Zero), // bottom middle
                    new PdfPoint(PdfMeasurement.Zero, PageHeight / 2.0)), // left middle
            };

            path.Commands.Add(new PdfClip(
                new PdfMeasurement(0.25, PdfMeasurementType.Inch),
                new PdfMeasurement(0.25, PdfMeasurementType.Inch),
                PageWidth - new PdfMeasurement(0.5, PdfMeasurementType.Inch),
                PageHeight - new PdfMeasurement(0.5, PdfMeasurementType.Inch)));
            path.Commands.AddRange(builder.ToPath().Commands);

            var page = PdfPage.NewLetter();
            page.Items.Add(path);
            var file = new PdfFile();
            file.Pages.Add(page);

            // first 4 lines are the clipping rectangle; remaining are the lines
            AssertPageContains(page, """
                n
                18.00 18.00 576.00 756.00 re
                W
                n
                0.00 396.00 m
                306.00 792.00 l
                306.00 792.00 m
                612.00 396.00 l
                612.00 396.00 m
                306.00 0.00 l
                306.00 0.00 m
                0.00 396.00 l
                S
                """);
        }
    }
}
