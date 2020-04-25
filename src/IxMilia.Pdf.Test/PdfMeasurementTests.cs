using Xunit;

namespace IxMilia.Pdf.Test
{
    public class PdfMeasurementTests
    {
        [Fact]
        public void RawPointsNoopTest()
        {
            var m = PdfMeasurement.Points(42.0);
            Assert.Equal(42.0, m.AsPoints());
        }

        [Fact]
        public void ConvertTest()
        {
            var oneInch = PdfMeasurement.Inches(1.0);
            Assert.Equal(72.0, oneInch.AsPoints());
            Assert.Equal(25.4, oneInch.ConvertTo(PdfMeasurementType.Mm).RawValue);
        }

        [Fact]
        public void EqualityTest()
        {
            var oneInch1 = PdfMeasurement.Inches(1.0);
            var oneInch2 = PdfMeasurement.Mm(25.4);
            Assert.Equal(oneInch1, oneInch2);
        }

        [Fact]
        public void BinaryOperatorTest()
        {
            var oneInch1 = PdfMeasurement.Inches(1.0);
            var oneInch2 = PdfMeasurement.Mm(25.4);
            Assert.Equal(2.0, (oneInch1 + oneInch2).RawValue);
            Assert.Equal(50.8, (oneInch2 + oneInch1).RawValue);
        }

        [Fact]
        public void PointConstructionWithLessVerboseMeasurementTest()
        {
            var point = new PdfPoint(1.0, 2.0, PdfMeasurementType.Inch);
            Assert.Equal(25.4, point.X.ConvertTo(PdfMeasurementType.Mm).RawValue);
            Assert.Equal(50.8, point.Y.ConvertTo(PdfMeasurementType.Mm).RawValue);
        }
    }
}
