using System.Globalization;

namespace IxMilia.Pdf.Extensions
{
    internal static class WriterExtensions
    {
        public static string AsObjectReference(this int objectId)
        {
            return $"{objectId} 0 R";
        }

        public static string AsInvariant(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string AsFixed(this double value)
        {
            var str = value.ToString("f2", CultureInfo.InvariantCulture);
            // special case for really small negative values that round to 0
            if (str == "-0.00")
            {
                str = "0.00";
            }

            return str;
        }
    }
}
