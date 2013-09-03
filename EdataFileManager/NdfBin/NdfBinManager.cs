using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using EdataFileManager.NdfBin.Model.Ndfbin;
using EdataFileManager.Util;

namespace EdataFileManager.NdfBin
{
    public class NdfbinManager
    {
        public byte[] Data { get; set; }
        public NdfbinFooter Footer { get; protected set; }
        public ObservableCollection<NdfbinClass> Classes { get; set; }

        public NdfbinManager(byte[] data)
        {
            Data = data;
        }

        public void ParseData()
        {
            ReadFooter();
            ReadClasses();
            ReadProperties();
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
                    var property = new NdfbinProperty() { Offset = ms.Position, Id = i };

                    ms.Read(buffer, 0, buffer.Length);
                    var strLen = BitConverter.ToInt32(buffer, 0);

                    var strBuffer = new byte[strLen];
                    ms.Read(strBuffer, 0, strBuffer.Length);

                    property.Name = Encoding.GetEncoding("ISO-8859-1").GetString(strBuffer);

                    ms.Read(buffer, 0, buffer.Length);
                    property.ClassId = BitConverter.ToInt32(buffer,0);

                    i++;
                    Classes.Single(x => x.Id == property.ClassId).Properties.Add(property);
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
                    var nclass = new NdfbinClass { Offset = ms.Position, Id = i };

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
