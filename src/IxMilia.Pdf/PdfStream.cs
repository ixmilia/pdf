// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using IxMilia.Pdf.Extensions;

namespace IxMilia.Pdf
{
    internal class PdfStream : PdfObject
    {
        public IList<PdfStreamItem> Items { get; } = new List<PdfStreamItem>();

        public bool Compress { get; }

        public PdfStream(bool compress)
        {
            Compress = compress;
        }

        protected override byte[] GetContent()
        {
            var writer = new PdfStreamWriter();
            foreach (var item in Items)
            {
                item.Write(writer);
            }

            writer.Stroke();
            var streamBody = Compress
                ? GetCompressedStream(writer) + "~>"
                : writer.ToString();
            var objectArgs = Compress
                ? " /Filter [/ASCII85Decode /FlateDecode]"
                : string.Empty;

            var sb = new StringBuilder();
            sb.Append($"<</Length {streamBody.Length}{objectArgs}>>\r\n");
            sb.Append("stream\r\n");
            sb.Append(streamBody);
            sb.Append("\r\n");
            sb.Append("endstream\r\n");
            return sb.ToString().GetBytes();
        }

        internal static string ConvertToAscii85(MemoryStream ms)
        {
            // convert to base-85
            // add 33
            // get ascii representation
            // special-case: if value == "!!!!!", use "z"
            // if not 4 bytes, append 0 and don't do !!!!! fix, then write n + 1 characters
            // write "~>" EOD marker
            var result = new StringBuilder();
            var buffer = new byte[4];
            var group = new StringBuilder();
            for (int i = 0; i < ms.Length; i += 4)
            {
                Array.Clear(buffer, 0, buffer.Length);
                var read = ms.Read(buffer, 0, buffer.Length);
                var padding = buffer.Length - read;
                //var value = (uint)buffer[3] * 24 + (uint)buffer[2] * 16 + (uint)buffer[1] * 8 + (uint)buffer[0];
                var value = BitConverter.ToUInt32(buffer, 0);
                if (read == buffer.Length && value == 0)
                {
                    result.Append("z");
                }
                else
                {
                    group.Clear();
                    for (int j = 0; j < 5; j++)
                    {
                        var next = value % 85;
                        var c = (char)(next + 33);
                        group.Append(c);
                        value /= 85;
                    }
                    group.Remove(group.Length - padding, padding);
                    result.Append(group.ToString());
                }
            }

            return result.ToString();
        }

        private static string GetCompressedStream(PdfStreamWriter writer)
        {
            using (var ms = new MemoryStream())
            {
                using (var deflate = new DeflateStream(ms, CompressionMode.Compress, leaveOpen: true))
                {
                    deflate.Write(writer.ToString());
                }

                ms.Seek(0, SeekOrigin.Begin);
                return ConvertToAscii85(ms);
            }
        }
    }
}
