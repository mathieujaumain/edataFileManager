using System.Globalization;

namespace EdataFileManager.NdfBin.Model
{
    /// <summary>
    /// The reversed struct from Hex Workshop - much love to Hob_gadling for his hard work and help.
    /// The chunks after offset and fileSize are because of long (int64)
    /// 
    /// struct dictFileEntry {
    ///     DWORD groupId;
    ///     DWORD fileEntrySize;
    ///     DWORD offset;
    ///     DWORD chunk2;   
    ///     DWORD fileSize;
    ///     DWORD chunk4;
    ///     blob checksum[16];
    ///     zstring name;
    /// };
    /// </summary>
    public class NdfFile : NdfEntity
    {
        private string _path;
        private long _offset;
        private long _size;
        private byte[] _checkSum = new byte[16];

        private NdfFileContent _content;

        public string Path
        {
            get { return _path; }
            set { _path = value; OnPropertyChanged(() => Path); }
        }

        public long Offset
        {
            get { return _offset; }
            set { _offset = value; OnPropertyChanged(() => Offset); }
        }

        public long Size
        {
            get { return _size; }
            set { _size = value; OnPropertyChanged(() => Size); }
        }

        public byte[] Checksum
        {
            get { return _checkSum; }
            set { _checkSum = value; OnPropertyChanged(() => Checksum); }
        }

        public NdfFileContent Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(() => Content); }
        }

        public override string ToString()
        {
            return Content.BodyReadable ?? Path.ToString(CultureInfo.CurrentCulture);
        }
    }
}
