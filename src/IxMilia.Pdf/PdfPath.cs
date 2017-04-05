// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfPath : PdfStreamItem
    {
        public IList<PdfPathCommand> Commands { get; } = new List<PdfPathCommand>();

        internal override void Write(PdfStreamWriter writer)
        {
            foreach (var command in Commands)
            {
                command.Write(writer);
            }
        }
    }
}
