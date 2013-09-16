using System;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;
using EdataFileManager.Util;

namespace EdataFileManager.Model.Ndfbin.Types
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

        public static object GetValue(byte[] data, NdfType type, NdfbinManager mgr, long pos)
        {
            //if (data.Length != SizeofType(type))
            //    return null;

            switch (type)
            {
                case NdfType.Boolean:
                case NdfType.Boolean2:
                    return new NdfBoolean(BitConverter.ToBoolean(data, 0), pos);
                case NdfType.Int32:
                    return new NdfInt32(BitConverter.ToInt32(data, 0), pos);
                case NdfType.UInt32:
                    return new NdfUInt32(BitConverter.ToUInt32(data, 0), pos);
                case NdfType.Float32:
                    return new NdfSingle(BitConverter.ToSingle(data, 0), pos);
                case NdfType.Float64:
                    return new NdfDouble(BitConverter.ToDouble(data, 0), pos);
                case NdfType.TableStringFile:
                    var id = BitConverter.ToInt32(data, 0);
                    return mgr.Strings[id];
                case NdfType.TableString:
                    var id2 = BitConverter.ToInt32(data, 0);
                    return mgr.Strings[id2];
                case NdfType.Color32:
                    return Color.FromArgb(data[0], data[1], data[2], data[3]);
                case NdfType.Vector:
                    var px = data.Take(4).ToArray();
                    var py = data.Skip(4).Take(4).ToArray();
                    var pz = data.Skip(8).ToArray();
                    return new Point3D(BitConverter.ToSingle(px, 0),
                                       BitConverter.ToSingle(py, 0),
                                       BitConverter.ToSingle(pz, 0));
                case NdfType.ObjectReference:
                    string s = string.Format("{1} : {0}", BitConverter.ToUInt32(data.Take(4).ToArray(), 0), BitConverter.ToUInt32(data.Skip(4).ToArray(), 0));
                    //var cls = mgr.Classes.SingleOrDefault(x => x.Id == BitConverter.ToUInt32(data, 0));
                    // TODO: object
                    return s;

                case NdfType.Guid:
                    return new Guid(data);

                case NdfType.WideString:
                    return Encoding.Unicode.GetString(data);

                case NdfType.TransTableReference:
                    return mgr.Trans.Single(x => x.Id == BitConverter.ToInt32(data, 0));

                case NdfType.LocalisationHash:
                    return Utils.ByteArrayToBigEndianHeyByteString(data);


                default:
                    return null;
            }
        }

        public static uint SizeofType(NdfType type)
        {
            switch (type)
            {
                case NdfType.Boolean:
                case NdfType.Boolean2:
                    return 1;
                case NdfType.Int32:
                case NdfType.UInt32:
                case NdfType.Float32:
                case NdfType.TableStringFile:
                case NdfType.TableString:
                case NdfType.Color32:
                case NdfType.WideString:
                    return 4;
                case NdfType.LocalisationHash:
                case NdfType.ObjectReference:
                    return 8;
                case NdfType.Vector:
                    return 12;
                case NdfType.Guid:
                    return 16;

                case NdfType.Map:
                    return 0;
                case NdfType.List:
                case NdfType.MapList:
                    return 4;

                case NdfType.Float64:
                    return 8;

                case NdfType.TransTableReference:
                    return 4;

                default:
                    return 0;
            }
        }

        public static byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            valid = true;

            try
            {
                switch (type)
                {
                    case NdfType.Boolean2:
                    case NdfType.Boolean:
                        return BitConverter.GetBytes(Convert.ToBoolean(value));
                        break;
                    case NdfType.Int32:
                        return BitConverter.GetBytes(Convert.ToInt32(value));
                        break;
                    case NdfType.UInt32:
                        return BitConverter.GetBytes(Convert.ToUInt32(value));
                        break;
                    case NdfType.Float32:
                        return BitConverter.GetBytes(Convert.ToSingle(value));
                        break;
                    case NdfType.Float64:
                        return BitConverter.GetBytes(Convert.ToDouble(value));
                        break;
                    default:
                        valid = false;
                        return new byte[0];
                        break;
                }
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }

        }

    }
}
