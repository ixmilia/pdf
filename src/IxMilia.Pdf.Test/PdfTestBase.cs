// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using Xunit;

namespace IxMilia.Pdf.Test
{
    public abstract class PdfTestBase
    {
        private string NormalizeCrLf(string value)
        {
            // ensure all newlines are CRLF
            return value.Replace("\r", "").Replace("\n", "\r\n");
        }

        private string GetFileContents(PdfFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(ms))
                {
                    var actual = reader.ReadToEnd();
                    return actual;
                }
            }
        }

        public void AssertFileEquals(PdfFile file, string expected)
        {
            var actual = GetFileContents(file);
            Assert.Equal(NormalizeCrLf(expected.Trim()), actual);
        }

        public void AssertFileContains(PdfFile file, string expected)
        {
            var actual = GetFileContents(file);
            Assert.Contains(NormalizeCrLf(expected.Trim()), actual);
        }

        public void AssertPageContains(PdfPage page, string expected)
        {
            var file = new PdfFile();
            file.Pages.Add(page);
            AssertFileContains(file, expected);
        }

        public void AssertPathBuilderContains(PdfPathBuilder builder, string expected)
        {
            var page = new PdfPage(8.5 * 72, 11 * 72);
            page.Items.Add(builder.ToPath());
            AssertPageContains(page, expected);
        }
    }
}
