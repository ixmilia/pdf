// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Xunit;

namespace IxMilia.Pdf.Test
{
    public class CompressionTests
    {
        private static string Ascii85Convert(string value)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms, encoding: Encoding.ASCII, bufferSize: 1024, leaveOpen: true))
                {
                    writer.Write(value);
                }

                ms.Seek(0, SeekOrigin.Begin);
                return PdfStream.ConvertToAscii85(ms);
            }
        }

        [Fact]
        public void Ascii85ConvertTest()
        {
            var text = "asdf";
            var expected = "@<5sk";
            var actual = Ascii85Convert(text);
            Assert.Equal(expected, actual);
        }
    }
}
