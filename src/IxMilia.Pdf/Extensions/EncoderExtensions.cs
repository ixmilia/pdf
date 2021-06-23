using System.Collections.Generic;
using System.Linq;
using IxMilia.Pdf.Encoders;

namespace IxMilia.Pdf.Extensions
{
    public static class EncoderExtensions
    {
        public static string AsEncoderList(this IList<IPdfEncoder> encoders)
        {
            return $"[{string.Join(" ", encoders.Select(e => "/" + e.DisplayName).Reverse())}]";
        }
    }
}
