// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using Xunit;

namespace IxMilia.Pdf.Test
{
    public abstract class PdfTestBase
    {
        public void AssertFileEquals(PdfFile file, string expected)
        {
            using (var ms = new MemoryStream())
            {
                file.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(ms))
                {
                    var actual = reader.ReadToEnd();
                    Assert.Equal(expected.Trim(), actual);
                }
            }
        }
    }
}
