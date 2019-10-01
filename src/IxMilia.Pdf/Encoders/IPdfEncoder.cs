// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf.Encoders
{
    public interface IPdfEncoder
    {
        string DisplayName { get; }
        byte[] Encode(byte[] data);
    }
}
