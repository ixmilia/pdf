// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf.Extensions
{
    internal static class ReferenceExtensions
    {
        public static string AsObjectReference(this int objectId)
        {
            return $"{objectId} 0 R";
        }
    }
}
