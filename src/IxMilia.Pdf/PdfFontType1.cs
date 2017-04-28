// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Pdf
{
    public class PdfFontType1 : PdfFont
    {
        public override string SubType => "Type1";

        public PdfFontType1Type FontType { get; set; }

        public PdfFontType1(PdfFontType1Type fontType)
        {
            FontType = fontType;
        }

        protected override string GetAdditionalProperties()
        {
            return $"/BaseFont /{FontType.ToBaseFont()}";
        }
    }
}
