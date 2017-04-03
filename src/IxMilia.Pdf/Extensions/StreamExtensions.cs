// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using System.Text;

namespace IxMilia.Pdf.Extensions
{
    internal static class StreamExtensions
    {
        private static byte[] NewLine = new byte[] { (byte)'\r', (byte)'\n' };

        public static void Write(this Stream stream, string value)
        {
            stream.WriteBytes(value.GetBytes());
        }

        public static void WriteLine(this Stream stream, string value)
        {
            stream.WriteBytes(value.GetBytes());
            stream.WriteBytes(NewLine);
        }

        public static void WriteBytes(this Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public static byte[] GetBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static byte[] GetNewLineBytes(this string value)
        {
            return (value + "\r\n").GetBytes();
        }
    }
}
