using System.IO;
using System;
using System.IO.Compression;
using zlib;

namespace EdataFileManager.Compressing
{
    public static class Compressing
    {
        /// <summary>
        /// .NET internal deflate lib seems to be too strict for Eugen Systems :) This doesnt work
        /// p/summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] DecompAlt(byte[] input)
        {
            using (var deflateStream = new DeflateStream(new MemoryStream(input), CompressionMode.Decompress))
            {
                using (var outputStream = new MemoryStream())
                {
                    deflateStream.CopyTo(outputStream);

                    return outputStream.ToArray();
                }
            }
        }

        /// <summary>
        /// good ol' zlib.NET implementation likes Eugen
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Decomp(byte[] input)
        {
            using (var output = new MemoryStream())
            {
                using (var zipStream = new ZOutputStream(output))
                {
                    using (var inputStream = new MemoryStream(input))
                    {
                        var buffer = new byte[4096];
                        int size = 1;

                        while (size > 0)
                        {
                            size = inputStream.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, size);
                        }
                        return output.ToArray();
                    }
                }
            }
        }
    }
}
