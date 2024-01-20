using System.Globalization;
using System.Text.RegularExpressions;

namespace IxMilia.Pdf.Extensions
{
    internal static class WriterExtensions
    {
        private static readonly Regex NegativeZero = new Regex(@"^-0(\.0+)?$", RegexOptions.Compiled);

        public static string AsObjectReference(this int objectId)
        {
            return $"{objectId} 0 R";
        }

        public static string AsInvariant(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string AsFixed(this double value, int decimalPlaces = 2)
        {
            var str = value.ToString($"f{decimalPlaces}", CultureInfo.InvariantCulture);

            // special case for really small negative values that round to 0
            if (NegativeZero.IsMatch(str))
            {
                str = str.Substring(1);
            }

            return str;
        }
    }
}
