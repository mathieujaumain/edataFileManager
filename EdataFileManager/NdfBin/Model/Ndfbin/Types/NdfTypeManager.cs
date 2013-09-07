using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            //foreach (var val in Enum.GetValues((typeof(NdfType))))
            //    if ((NdfType)val == (NdfType)BitConverter.ToUInt32(data, 0))
            //        return (NdfType)val;

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
                //case NdfType.Int64:
                //    return BitConverter.ToInt64(data, 0);
                case NdfType.Float32:
                    return BitConverter.ToSingle(data, 0);
                case NdfType.TableString:
                    return mgr.Strings.Single(x => x.Id == BitConverter.ToInt32(data, 0));
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
                case NdfType.Float32:
                case NdfType.TableString:

                case NdfType.Unknown4Byte:
                    return 4;

                default:
                    return 0;
            }
        }
    }
}
