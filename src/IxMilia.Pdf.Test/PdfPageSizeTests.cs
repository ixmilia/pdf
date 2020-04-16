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
            Assert.Equal(841, GetAPageWidth(0));
            Assert.Equal(594, GetAPageWidth(1));
            Assert.Equal(420, GetAPageWidth(2));
            Assert.Equal(297, GetAPageWidth(3));
            Assert.Equal(210, GetAPageWidth(4));
            Assert.Equal(148, GetAPageWidth(5));
            Assert.Equal(105, GetAPageWidth(6));
            Assert.Equal(74, GetAPageWidth(7));
            Assert.Equal(52, GetAPageWidth(8));
        }

        [Fact]
        public void ASeriesHeightTest()
        {
            Assert.Equal(1189, GetAPageHeight(0));
            Assert.Equal(841, GetAPageHeight(1));
            Assert.Equal(594, GetAPageHeight(2));
            Assert.Equal(420, GetAPageHeight(3));
            Assert.Equal(297, GetAPageHeight(4));
            Assert.Equal(210, GetAPageHeight(5));
            Assert.Equal(148, GetAPageHeight(6));
            Assert.Equal(105, GetAPageHeight(7));
            Assert.Equal(74, GetAPageHeight(8));
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
