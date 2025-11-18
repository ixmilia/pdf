using System;

namespace IxMilia.Pdf
{
    public enum PdfMeasurementType
    {
        Point = 0,
        Inch = 1,
        Mm = 2,
    }

    public struct PdfMeasurement : IEquatable<PdfMeasurement>
    {
        private const double PointsPerInch = 72.0;
        private const double PointsPerMm = PointsPerInch / 25.4;

        public double RawValue { get; }
        public PdfMeasurementType MeasurementType { get; }

        public PdfMeasurement(double value, PdfMeasurementType measurementType)
            : this()
        {
            RawValue = value;
            MeasurementType = measurementType;
        }

        public double AsPoints()
        {
            return ConvertTo(PdfMeasurementType.Point).RawValue;
        }

        public PdfMeasurement ConvertTo(PdfMeasurementType targetMeasurementType)
        {
            var toPointsScale = GetConverterScale(MeasurementType);
            var toTargetScale = GetConverterScale(targetMeasurementType);
            var scale = toPointsScale / toTargetScale;
            var newValue = RawValue * scale;
            return new PdfMeasurement(newValue, targetMeasurementType);
        }

        public static bool operator ==(PdfMeasurement a, PdfMeasurement b)
        {
            return a.AsPoints() == b.AsPoints();
        }

        public static bool operator !=(PdfMeasurement a, PdfMeasurement b)
        {
            return !(a == b);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is PdfMeasurement))
            {
                return false;
            }

            return Equals((PdfMeasurement)obj);
        }

        public bool Equals(PdfMeasurement other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return AsPoints().GetHashCode();
        }

        public static PdfMeasurement operator +(PdfMeasurement a, PdfMeasurement b)
        {
            return new PdfMeasurement(a.RawValue + b.ConvertTo(a.MeasurementType).RawValue, a.MeasurementType);
        }

        public static PdfMeasurement operator -(PdfMeasurement a, PdfMeasurement b)
        {
            return new PdfMeasurement(a.RawValue - b.ConvertTo(a.MeasurementType).RawValue, a.MeasurementType);
        }

        public static PdfMeasurement operator *(PdfMeasurement a, PdfMeasurement b)
        {
            return new PdfMeasurement(a.RawValue * b.ConvertTo(a.MeasurementType).RawValue, a.MeasurementType);
        }

        public static PdfMeasurement operator *(PdfMeasurement a, double b)
        {
            return new PdfMeasurement(a.RawValue * b, a.MeasurementType);
        }

        public static PdfMeasurement operator /(PdfMeasurement a, PdfMeasurement b)
        {
            return new PdfMeasurement(a.RawValue / b.ConvertTo(a.MeasurementType).RawValue, a.MeasurementType);
        }

        public static PdfMeasurement operator /(PdfMeasurement a, double b)
        {
            return new PdfMeasurement(a.RawValue / b, a.MeasurementType);
        }

        private static double GetConverterScale(PdfMeasurementType measurementType)
        {
            switch (measurementType)
            {
                case PdfMeasurementType.Point:
                    return 1.0;
                case PdfMeasurementType.Inch:
                    return PointsPerInch;
                case PdfMeasurementType.Mm:
                    return PointsPerMm;
                default:
                    throw new InvalidOperationException($"Unexpected measurement type '{measurementType}'.");
            }
        }

        public static PdfMeasurement Points(double value)
        {
            return new PdfMeasurement(value, PdfMeasurementType.Point);
        }

        public static PdfMeasurement Inches(double value)
        {
            return new PdfMeasurement(value, PdfMeasurementType.Inch);
        }

        public static PdfMeasurement Mm(double value)
        {
            return new PdfMeasurement(value, PdfMeasurementType.Mm);
        }

        public static PdfMeasurement Zero => Points(0.0);
    }
}
