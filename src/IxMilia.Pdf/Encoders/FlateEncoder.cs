using System.IO;
using System.IO.Compression;

namespace IxMilia.Pdf.Encoders
{
    public class FlateEncoder : IPdfEncoder
    {
        public string DisplayName => "FlateDecode";

        public byte[] Encode(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                byte[] compressedData;
                using (var compressed = new MemoryStream())
                {
                    using (var deflate = new DeflateStream(compressed, CompressionLevel.Fastest, leaveOpen: true))
                    {
                        deflate.Write(data, 0, data.Length);
                    }

                    compressedData = compressed.ToArray();
                }

                if (compressedData.Length >= 2 && IsValidMagicHeader(compressedData[0], compressedData[1]))
                {
                    // zlib default magic header of 0x78, 0x__ is present
                }
                else
                {
                    // prepend magic headers, assuming 0x9C for default compression
                    ms.WriteByte(0x78);
                    ms.WriteByte(0x9C);
                }

                ms.Write(compressedData, 0, compressedData.Length);

                ms.WriteByte((byte)'\r');
                ms.WriteByte((byte)'\n');
                return ms.ToArray();
            }
        }

        private static bool IsValidMagicHeader(byte b1, byte b2)
        {
            return b1 == 0x78
                && (b2 == 0x01 || b2 == 0x5E || b2 == 0x9C || b2 == 0xDA);
        }
    }
}
