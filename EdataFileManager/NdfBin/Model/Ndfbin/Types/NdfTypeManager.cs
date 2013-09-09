﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
                case NdfType.Vector:
                    var px = data.Take(4).ToArray();
                    var py = data.Skip(4).Take(4).ToArray();
                    var pz = data.Skip(8).ToArray();
                    return new Point3D(BitConverter.ToSingle(px, 0),
                                       BitConverter.ToSingle(py, 0),
                                       BitConverter.ToSingle(pz, 0));
                case NdfType.ObjectReference:
                    var cls = mgr.Classes.SingleOrDefault(x => x.Id == BitConverter.ToUInt32(data, 0));
                    // TODO: object
                    return cls;

                case NdfType.DescriptorId:
                    return BitConverter.ToInt32(data.Skip(12).ToArray(), 0);


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
                case NdfType.Unknown8Byte:
                case NdfType.ObjectReference:
                    return 8;
                case NdfType.Vector:
                    return 12;
                case NdfType.DescriptorId:
                    return 16;
                    

                default:
                    return 0;
            }
        }
    }
}