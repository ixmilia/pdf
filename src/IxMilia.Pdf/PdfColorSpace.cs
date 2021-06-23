using System;

namespace IxMilia.Pdf
{
    public enum PdfColorSpace
    {
        DeviceGray,
        DeviceRGB,
        DeviceCMYK,
    }

    public static class PdfColorSpaceExtensions
    {
        public static string ToPatternName(this PdfColorSpace colorSpace)
        {
            switch (colorSpace)
            {
                case PdfColorSpace.DeviceGray:
                    return "DeviceGray";
                case PdfColorSpace.DeviceRGB:
                    return "DeviceRGB";
                case PdfColorSpace.DeviceCMYK:
                    return "DeviceCMYK";
                default:
                    throw new ArgumentException(nameof(colorSpace));
            }
        }
    }
}
