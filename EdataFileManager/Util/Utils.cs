﻿using System;
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

        public static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            return !a1.Where((t, i) => t != a2[i]).Any();
        }

        public static string Int32ToBigEndianHexByteString(Int32 i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            string format = BitConverter.IsLittleEndian
                ? "{0:X2} {1:X2} {2:X2} {3:X2}"
                : "{3:X2} {2:X2} {1:X2} {0:X2}";
            return String.Format(format, bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        public static string ByteArrayToBigEndianHeyByteString(byte[] data)
        {
            if (data == null)
                return string.Empty;

            var stringBuilderb = new StringBuilder();

            foreach (var b in data)
                stringBuilderb.Append(string.Format("{0:X2}", b));

            return stringBuilderb.ToString();
        }
    }
}