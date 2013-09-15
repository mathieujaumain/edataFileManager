using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EdataFileManager.NdfBin.Model.Ndfbin;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;
using EdataFileManager.Util;

namespace EdataFileManager.NdfBin
{
    public class NdfbinManager
    {
        public byte[] Data { get; set; }
        public NdfbinFooter Footer { get; protected set; }
        public ObservableCollection<NdfbinClass> Classes { get; set; }
        public ObservableCollection<NdfbinString> Strings { get; set; }
        public ObservableCollection<NdfbinTran> Trans { get; set; }

        protected List<byte[]> _unknownTypes = new List<byte[]>();
        protected List<int> _unknownTypesCount = new List<int>();

        public NdfbinManager(byte[] data)
        {
            Data = data;
        }

        public void ParseData()
        {
            ReadFooter();
            ReadClasses();
            ReadProperties();

            ReadStrings();
            ReadTrans();

            ReadObjects();
        }

        protected void ReadTrans()
        {
            var trans = new ObservableCollection<NdfbinTran>();

            var stringEntry = Footer.Entries.Single(x => x.Name == "TRAN");

            //TODO: int cast is a bit too hacky here, solution needed
            using (var ms = new MemoryStream(Data, (int)stringEntry.Offset - 40, (int)stringEntry.Size))
            {
                int i = 0;
                var buffer = new byte[4];
                while (ms.Position < ms.Length)
                {
                    var ntran = new NdfbinTran { Offset = ms.Position, Id = i };

                    ms.Read(buffer, 0, buffer.Length);
                    var strLen = BitConverter.ToInt32(buffer, 0);

                    var strBuffer = new byte[strLen];
                    ms.Read(strBuffer, 0, strBuffer.Length);

                    ntran.Value = Encoding.GetEncoding("ISO-8859-1").GetString(strBuffer);

                    i++;
                    trans.Add(ntran);
                }
            }

            Trans = trans;
        }

        protected void ReadStrings()
        {
            var strings = new ObservableCollection<NdfbinString>();

            var stringEntry = Footer.Entries.Single(x => x.Name == "STRG");

            //TODO: int cast is a bit too hacky here, solution needed
            using (var ms = new MemoryStream(Data, (int)stringEntry.Offset - 40, (int)stringEntry.Size))
            {
                int i = 0;
                var buffer = new byte[4];
                while (ms.Position < ms.Length)
                {
                    var nstring = new NdfbinString { Offset = ms.Position, Id = i };

                    ms.Read(buffer, 0, buffer.Length);
                    var strLen = BitConverter.ToInt32(buffer, 0);

                    var strBuffer = new byte[strLen];
                    ms.Read(strBuffer, 0, strBuffer.Length);

                    nstring.Value = Encoding.GetEncoding("ISO-8859-1").GetString(strBuffer);

                    i++;
                    strings.Add(nstring);
                }
            }

            Strings = strings;
        }

        protected void ReadProperties()
        {
            var propEntry = Footer.Entries.Single(x => x.Name == "PROP");

            //TODO: int cast is a bit too hacky here, solution needed
            using (var ms = new MemoryStream(Data, (int)propEntry.Offset - 40, (int)propEntry.Size))
            {
                int i = 0;
                var buffer = new byte[4];
                while (ms.Position < ms.Length)
                {
                    var property = new NdfbinProperty { Offset = ms.Position, Id = i };

                    ms.Read(buffer, 0, buffer.Length);
                    var strLen = BitConverter.ToInt32(buffer, 0);

                    var strBuffer = new byte[strLen];
                    ms.Read(strBuffer, 0, strBuffer.Length);

                    property.Name = Encoding.GetEncoding("ISO-8859-1").GetString(strBuffer);

                    ms.Read(buffer, 0, buffer.Length);

                    var cls = Classes.Single(x => x.Id == BitConverter.ToInt32(buffer, 0));
                    property.Class = cls;

                    cls.Properties.Add(property);


                    i++;
                }
            }
        }

        protected void ReadClasses()
        {
            var classes = new ObservableCollection<NdfbinClass>();

            var classEntry = Footer.Entries.Single(x => x.Name == "CLAS");

            //TODO: int cast is a bit too hacky here, solution needed
            using (var ms = new MemoryStream(Data, (int)classEntry.Offset - 40, (int)classEntry.Size))
            {
                int i = 0;
                var buffer = new byte[4];
                while (ms.Position < ms.Length)
                {
                    var nclass = new NdfbinClass(this) { Offset = ms.Position, Id = i };

                    ms.Read(buffer, 0, buffer.Length);
                    var strLen = BitConverter.ToInt32(buffer, 0);

                    var strBuffer = new byte[strLen];
                    ms.Read(strBuffer, 0, strBuffer.Length);

                    nclass.Name = Encoding.GetEncoding("ISO-8859-1").GetString(strBuffer);

                    i++;
                    classes.Add(nclass);
                }
            }

            Classes = classes;
        }

        protected ObservableCollection<NdfbinObject> ReadObjects()
        {
            var objects = new ObservableCollection<NdfbinObject>();

            var objEntry = Footer.Entries.Single(x => x.Name == "OBJE");

            //TODO: int cast is a bit too hacky here, solution needed
            using (var ms = new MemoryStream(Data, (int)objEntry.Offset - 40, (int)objEntry.Size))
            {
                var instanceOffsets = GetInstanceOffsets(ms).ToArray();

                byte[] buffer;
                long size;

                for (int i = 0; i < instanceOffsets.Length; i++)
                {
                    if (i == instanceOffsets.Length - 1)
                        size = ms.Length - instanceOffsets[i];
                    else
                        size = instanceOffsets[i + 1] - instanceOffsets[i] - 4;

                    buffer = new byte[size];
                    ms.Read(buffer, 0, buffer.Length);
                    ms.Seek(4, SeekOrigin.Current);

                    objects.Add(ParseObject(buffer, i));
                }
            }

            return objects;
        }

        private NdfbinObject ParseObject(byte[] data, int index)
        {
            var instance = new NdfbinObject { Id = index, Data = data };

            using (var ms = new MemoryStream(data))
            {
                var buffer = new byte[4];

                ms.Read(buffer, 0, buffer.Length);
                int classId = BitConverter.ToInt32(buffer, 0);

                var cls = instance.Class = Classes.SingleOrDefault(x => x.Id == classId);

                if (cls != null)
                    cls.Instances.Add(instance);

                NdfbinPropertyValue prop;
                bool triggerBreak;

                // Read properties
                while (ms.Position < ms.Length)
                {
                    prop = new NdfbinPropertyValue();
                    instance.PropertyValues.Add(prop);

                    ms.Read(buffer, 0, buffer.Length);
                    prop.Property = cls.Properties.Single(x => x.Id == BitConverter.ToInt32(buffer, 0));

                    var res = ReadTypeValuePair(ms, out triggerBreak, prop);

                    prop.Type = res.Key;
                    prop.Value = res.Value;

                    if (triggerBreak)
                        break;
                }
            }
            return instance;
        }

        protected KeyValuePair<NdfType, object> ReadTypeValuePair(MemoryStream ms, out bool triggerBreak, NdfbinPropertyValue prop = null)
        {
            var buffer = new byte[4];
            uint contBufferlen;
            object value;
            triggerBreak = false;

            ms.Read(buffer, 0, buffer.Length);
            var type = NdfTypeManager.GetType(buffer);

            if (type == NdfType.Reference)
            {
                ms.Read(buffer, 0, buffer.Length);
                type = NdfTypeManager.GetType(buffer);
            }

            switch (type)
            {
                case NdfType.WideString:
                case NdfType.List:
                case NdfType.MapList:
                    ms.Read(buffer, 0, buffer.Length);
                    contBufferlen = BitConverter.ToUInt32(buffer, 0);
                    break;
                default:
                    contBufferlen = NdfTypeManager.SizeofType(type);
                    break;
            }

            if (type == NdfType.Unknown)
            {
                var t = _unknownTypes.SingleOrDefault(x => Utils.ByteArrayCompare(x, buffer));

                if (t == default(byte[]))
                {
                    _unknownTypes.Add(buffer);
                    _unknownTypesCount.Add(1);
                }
                else
                    _unknownTypesCount[_unknownTypes.IndexOf(t)]++;

                triggerBreak = true;
                return new KeyValuePair<NdfType, object>(type, null);
            }

            if (type == NdfType.List)
            {
                var lstValue = new List<object>();

                for (int i = 0; i < contBufferlen; i++)
                {
                    var res = ReadTypeValuePair(ms, out triggerBreak);

                    if (triggerBreak)
                        break;

                    lstValue.Add(res.Value);
                }

                value = lstValue;
            }
            else if (type == NdfType.MapList)
            {
                var lstValue = new List<object>();

                for (int i = 0; i < contBufferlen; i++)
                {
                    var res = new KeyValuePair<object, object>(ReadTypeValuePair(ms, out triggerBreak).Value, ReadTypeValuePair(ms, out triggerBreak).Value);

                    if (triggerBreak)
                        break;

                    lstValue.Add(res.Value);
                }

                value = lstValue;
            }
            else if (type == NdfType.Map)
            {
                value = new KeyValuePair<object, object>(ReadTypeValuePair(ms, out triggerBreak).Value, ReadTypeValuePair(ms, out triggerBreak).Value);
            }
            else
            {
                var contBuffer = new byte[contBufferlen];
                ms.Read(contBuffer, 0, contBuffer.Length);

                if (prop != null)
                    prop.ValueData = contBuffer;

                value = NdfTypeManager.GetValue(contBuffer, type, this);
            }

            return new KeyValuePair<NdfType, object>(type, value);
        }

        private List<long> GetInstanceOffsets(MemoryStream ms)
        {
            const byte ab = 0xAB;

            var offsets = new List<long> { 0 };

            byte abCount = 0;
            byte[] buffer;

            while (ms.Position < ms.Length)
            {
                buffer = new byte[1];
                ms.Read(buffer, 0, buffer.Length);

                if (buffer[0] == ab)
                {
                    abCount++;
                    if (abCount == 4)
                    {
                        offsets.Add(ms.Position);
                        abCount = 0;
                    }
                }
                else if (abCount > 0)
                    abCount = 0;
            }

            ms.Position = 0;

            return offsets;
        }

        protected void ReadFooter()
        {
            // Footer is 224 bytes
            const int footerLength = 224;
            var footer = new NdfbinFooter();

            using (var ms = new MemoryStream(Data, Data.Length - footerLength, footerLength))
            {
                var qwdbuffer = new byte[8];
                var dwdbufer = new byte[4];

                ms.Read(qwdbuffer, 0, qwdbuffer.Length);
                footer.Header = Encoding.ASCII.GetString(qwdbuffer);

                while (ms.Position < ms.Length)
                {
                    var entry = new NdfbinFooterEntry();

                    ms.Read(dwdbufer, 0, dwdbufer.Length);
                    entry.Name = Encoding.ASCII.GetString(dwdbufer);

                    ms.Seek(4, SeekOrigin.Current);

                    ms.Read(qwdbuffer, 0, qwdbuffer.Length);
                    entry.Offset = BitConverter.ToInt64(qwdbuffer, 0);

                    ms.Read(qwdbuffer, 0, qwdbuffer.Length);
                    entry.Size = BitConverter.ToInt64(qwdbuffer, 0);

                    footer.Entries.Add(entry);
                }
            }

            Footer = footer;
        }
    }
}
