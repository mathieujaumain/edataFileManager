using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using EdataFileManager.NdfBin.Model.Trad;

namespace EdataFileManager.NdfBin
{
    public class TradManager
    {
        private ObservableCollection<TradEntry> _entries = new ObservableCollection<TradEntry>();

        public TradManager(byte[] data)
        {
            ParseTradFile(data);
        }

        public ObservableCollection<TradEntry> Entries
        {
            get { return _entries; }
            private set { _entries = value; }
        }

        protected void ParseTradFile(byte[] data)
        {
            uint entryCount;

            using (var ms = new MemoryStream(data))
            {
                entryCount = ReadHeader(ms);
                Entries = ReadDictionary(entryCount, ms);

                GetEntryContents(ms);
            }
        }

        protected void GetEntryContents(MemoryStream ms)
        {
            byte[] buffer;

            foreach (var entry in Entries)
            {
                ms.Seek(entry.OffsetCont, SeekOrigin.Begin);

                buffer = new byte[entry.ContLen*2];

                ms.Read(buffer, 0, buffer.Length);

                entry.Content = Encoding.Unicode.GetString(buffer);
            }
        }

        protected ObservableCollection<TradEntry> ReadDictionary(uint entryCount, MemoryStream ms)
        {
            var entries = new ObservableCollection<TradEntry>();

            var hashBuffer = new byte[8];
            var buffer = new byte[4];

            for (int i = 0; i < entryCount; i++)
            {
                var entry = new TradEntry { OffsetDic = (uint)ms.Position };

                ms.Read(hashBuffer, 0, hashBuffer.Length);
                entry.Hash = hashBuffer;

                ms.Read(buffer, 0, buffer.Length);
                entry.OffsetCont = BitConverter.ToUInt32(buffer, 0);

                ms.Read(buffer, 0, buffer.Length);
                entry.ContLen = BitConverter.ToUInt32(buffer, 0);

                entries.Add(entry);
            }

            return entries;
        }

        protected uint ReadHeader(MemoryStream ms)
        {
            var buffer = new byte[4];

            ms.Read(buffer, 0, buffer.Length);

            if (Encoding.ASCII.GetString(buffer) != "TRAD")
                throw new ArgumentException("No valid Eugen Systems TRAD (*.dic) file.");

            ms.Read(buffer, 0, buffer.Length);

            return BitConverter.ToUInt32(buffer, 0);
        }
    }
}
