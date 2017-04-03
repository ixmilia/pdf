// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf.Extensions
{
    internal static class WriterExtensions
    {
        public static string AsObjectReference(this int objectId)
        {
            return $"{objectId} 0 R";
        }

        public static string AsWritable(this PdfColor color)
        {
            return $"{color.R} {color.G} {color.B}";
        }
    }
}
