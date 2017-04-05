// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.


namespace IxMilia.Pdf
{
    public class PdfSetState : PdfPathCommand
    {
        public PdfStreamState State { get; set; }

        public PdfSetState(PdfStreamState state)
        {
            State = state;
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.SetState(State);
        }
    }
}
