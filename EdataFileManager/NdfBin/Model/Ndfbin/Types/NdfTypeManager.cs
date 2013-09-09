using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types
{
    public static class NdfTypeManager
    {
        public static NdfType GetType(byte[] data)
        {
            if (data.Length != 4)
                return NdfType.Unknown;

            uint value = BitConverter.ToUInt32(data, 0);

            if (Enum.IsDefined(typeof(NdfType), value))
                return (NdfType)value;

            return NdfType.Unknown;
        }

        public static object GetValue(byte[] data, NdfType type, NdfbinManager mgr)
        {
            if (data.Length != SizeofType(type))
                return null;

            switch (type)
            {
                case NdfType.Boolean:
                    return BitConverter.ToBoolean(data, 0);
                case NdfType.Int32:
                    return BitConverter.ToInt32(data, 0);
                case NdfType.UInt32:
                    return BitConverter.ToUInt32(data, 0);
                case NdfType.Float32:
                    return BitConverter.ToSingle(data, 0);
                case NdfType.TableStringFile:
                    return mgr.Strings.Single(x => x.Id == BitConverter.ToInt32(data, 0));
                case NdfType.TableString:
                    return mgr.Strings.Single(x => x.Id == BitConverter.ToInt32(data, 0));
                case NdfType.Color32:
                    return Color.FromArgb(data[0], data[1], data[2], data[3]);
                default:
                    return null;
            }
        }

        public static byte SizeofType(NdfType type)
        {
            switch (type)
            {
                case NdfType.Boolean:
                    return 1;
                case NdfType.Int32:
                case NdfType.UInt32:
                case NdfType.Float32:
                case NdfType.TableStringFile:
                case NdfType.TableString:
                case NdfType.Color32:
                    return 4;

                default:
                    return 0;
            }
        }
    }
}
