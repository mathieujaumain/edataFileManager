using System.IO;
using System;
using zlib;

namespace EdataFileManager.Compressing
{
    public static class Compressing
    {
        public static byte[] Decomp(byte[] input)
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
