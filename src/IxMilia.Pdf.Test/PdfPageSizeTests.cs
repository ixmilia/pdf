// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Xunit;

namespace IxMilia.Pdf.Test
{
    public class PdfPageSizeTests : PdfTestBase
    {
        [Fact]
        public void A4SizeTest()
        {
            // portrait
            Assert.Equal(210, PdfPage.NewASeries(4, isPortrait: true).Width.ConvertTo(PdfMeasurementType.Mm).RawValue);
            Assert.Equal(297, PdfPage.NewASeries(4, isPortrait: true).Height.ConvertTo(PdfMeasurementType.Mm).RawValue);

            // landscape
            Assert.Equal(297, PdfPage.NewASeries(4, isPortrait: false).Width.ConvertTo(PdfMeasurementType.Mm).RawValue);
            Assert.Equal(210, PdfPage.NewASeries(4, isPortrait: false).Height.ConvertTo(PdfMeasurementType.Mm).RawValue);
        }

        [Fact]
        public void ASeriesWidthTest()
        {
            Assert.Equal(GetAPageWidth(0), 841);
            Assert.Equal(GetAPageWidth(1), 594);
            Assert.Equal(GetAPageWidth(2), 420);
            Assert.Equal(GetAPageWidth(3), 297);
            Assert.Equal(GetAPageWidth(4), 210);
            Assert.Equal(GetAPageWidth(5), 148);
            Assert.Equal(GetAPageWidth(6), 105);
            Assert.Equal(GetAPageWidth(7), 74);
            Assert.Equal(GetAPageWidth(8), 52);
        }

        [Fact]
        public void ASeriesHeightTest()
        {
            Assert.Equal(GetAPageHeight(0), 1189);
            Assert.Equal(GetAPageHeight(1), 841);
            Assert.Equal(GetAPageHeight(2), 594);
            Assert.Equal(GetAPageHeight(3), 420);
            Assert.Equal(GetAPageHeight(4), 297);
            Assert.Equal(GetAPageHeight(5), 210);
            Assert.Equal(GetAPageHeight(6), 148);
            Assert.Equal(GetAPageHeight(7), 105);
            Assert.Equal(GetAPageHeight(8), 74);
        }

        private static int GetAPageWidth(int n)
        {
            return (int)(PdfPage.NewASeries(n).Width.ConvertTo(PdfMeasurementType.Mm).RawValue);
        }

        private static int GetAPageHeight(int n)
        {
            return (int)(PdfPage.NewASeries(n).Height.ConvertTo(PdfMeasurementType.Mm).RawValue);
        }
    }
}
