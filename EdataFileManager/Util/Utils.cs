using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EdataFileManager.Util
{
    public static class Utils
    {
        public static string ReadString(Stream fs)
        {
            var b = new StringBuilder();
            var buffer = new byte[1];
            char c;

            do
            {
                fs.Read(buffer, 0, 1);
                c = Encoding.GetEncoding("ISO-8859-1").GetChars(buffer)[0];
                b.Append(c);
            } while (c != '\0');

            return StripString(b.ToString());
        }

        public static string StripString(string s)
        {
            return s.Split('\0')[0].TrimEnd();
        }
    }
}
