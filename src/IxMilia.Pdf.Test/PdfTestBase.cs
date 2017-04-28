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

        public void AssertFileDoesNotContain(PdfFile file, string notExpected)
        {
            var actual = GetFileContents(file);
            Assert.DoesNotContain(NormalizeCrLf(notExpected.Trim()), actual);
        }

        public void AssertFileContainsCount(PdfFile file, string expected, int expectedCount)
        {
            var actual = GetFileContents(file);
            expected = NormalizeCrLf(expected.Trim());
            var count = 0;
            var offset = 0;
            while (true)
            {
                offset = actual.IndexOf(expected, offset);
                if (offset >= 0)
                {
                    count++;
                    offset++;
                }
                else
                {
                    break;
                }
            }

            Assert.Equal(expectedCount, count);
        }

        public void AssertPageContains(PdfPage page, string expected)
        {
            var file = new PdfFile();
            file.Pages.Add(page);
            AssertFileContains(file, expected);
        }

        public void AssertPathBuilderContains(PdfPathBuilder builder, string expected)
        {
            var page = PdfPage.NewLetter();
            page.Items.Add(builder.ToPath());
            AssertPageContains(page, expected);
        }
    }
}
