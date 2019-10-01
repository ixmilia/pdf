// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Text;
using IxMilia.Pdf.Encoders;
using Xunit;

namespace IxMilia.Pdf.Test
{
    public class EncodingTests
    {
        private static void CheckEncoding(string expected, byte[] input, params IPdfEncoder[] encoders)
        {
            var data = input;
            foreach (var encoder in encoders)
            {
                data = encoder.Encode(data);
            }

            var actual = Encoding.ASCII.GetString(data);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LongAsciiHexEncodedValue()
        {
            var data = new byte[80];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }

            CheckEncoding(
                "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D3E3F\r\n" +
                "404142434445464748494A4B4C4D4E4F>\r\n", data, new ASCIIHexEncoder());
        }
    }
}
