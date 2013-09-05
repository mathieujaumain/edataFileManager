using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using EdataFileManager.NdfBin.Model.Edata;
using EdataFileManager.Util;

namespace EdataFileManager.NdfBin
{
    /// <summary>
    /// Thanks to Giovanni Condello. He created the "WargameEE DAT unpacker" which inspired me to make this software.
    /// </summary>
    public class EdataManager
    {
        public EdataManager(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; protected set; }

        public EdataHeader Header { get; set; }

        public ObservableCollection<NdfFile> Files { get; set; }

        public void ParseEdataFile()
        {
            Header = ReadEdatHeader();
            Files = ReadEdatDictionary();

            //foreach (var file in Files)
            //{
            //    file.Content = GetNdfContent(file);
            //}
        }

        public NdfFileContent GetNdfContent(NdfFile f) //async Task<NdfFileContent> GetNdfContent(NdfFile f)
        {
            byte[] buffer;

            using (FileStream fs = File.Open(FilePath, FileMode.Open))
            {
                long offset = Header.FileOffset + f.Offset;

                buffer = new byte[f.Size];

                fs.Seek(offset, SeekOrigin.Begin);

                fs.Read(buffer, 0, buffer.Length);
            }

            return ParseNdfbinContent(buffer, f);
        }

        public byte[] GetRawData(NdfFile f)
        {
            byte[] buffer;

            using (FileStream fs = File.Open(FilePath, FileMode.Open))
            {
                long offset = Header.FileOffset + f.Offset;
                fs.Seek(offset, SeekOrigin.Begin);

                buffer = new byte[f.Size];
                fs.Read(buffer, 0, buffer.Length);
            }

            return buffer;
        }

        protected NdfFileContent ParseNdfbinContent(byte[] content, NdfFile f)
        {
            var fileContent = new NdfFileContent();

            using (var ms = new MemoryStream(content))
            {
                ms.Seek(12, SeekOrigin.Begin);
                var buffer = new byte[4];

                ms.Read(buffer, 0, buffer.Length);
                fileContent.IsCompressedBody = BitConverter.ToInt32(buffer, 0) == 128;

                ms.Read(buffer, 0, 4);
                fileContent.BlockSize = BitConverter.ToInt32(buffer, 0);

                ms.Seek(12, SeekOrigin.Current);

                ms.Read(buffer, 0, 4);
                fileContent.BlockSizeE0 = BitConverter.ToInt32(buffer, 0);

                ms.Seek(4, SeekOrigin.Current);

                if (fileContent.IsCompressedBody)
                {
                    ms.Read(buffer, 0, 4);
                    fileContent.BlockSizeWithoutHeader = BitConverter.ToInt32(buffer, 0);
                }

                buffer = new byte[f.Size - ms.Position];

                ms.Read(buffer, 0, buffer.Length);

                if (fileContent.IsCompressedBody)
                    fileContent.Body = Compressing.Compressing.Decomp(buffer);
                else
                    fileContent.Body = buffer;
            }

            return fileContent;
        }

        /// <summary>
        /// The only tricky part about that algorythm is that you have to skip one byte if the length of the File/Dir name PLUS nullbyte is an odd number.
        /// </summary>
        /// <returns></returns>
        protected ObservableCollection<NdfFile> ReadEdatDictionary()
        {
            var files = new ObservableCollection<NdfFile>();
            var dirs = new List<NdfDir>();
            var endings = new List<long>();

            using (FileStream fileStream = File.Open(FilePath, FileMode.Open))
            {
                fileStream.Seek(Header.DirOffset, SeekOrigin.Begin);

                long dirEnd = Header.DirOffset + Header.DirLengh;

                while (fileStream.Position < dirEnd)
                {
                    var buffer = new byte[4];
                    fileStream.Read(buffer, 0, 4);
                    int fileGroupId = BitConverter.ToInt32(buffer, 0);

                    if (fileGroupId == 0)
                    {
                        var file = new NdfFile();
                        fileStream.Read(buffer, 0, 4);
                        file.FileEntrySize = BitConverter.ToInt32(buffer, 0);

                        buffer = new byte[8];
                        fileStream.Read(buffer, 0, buffer.Length);
                        file.Offset = BitConverter.ToInt64(buffer, 0);

                        fileStream.Read(buffer, 0, buffer.Length);
                        file.Size = BitConverter.ToInt64(buffer, 0);

                        var checkSum = new byte[16];
                        fileStream.Read(checkSum, 0, checkSum.Length);
                        file.Checksum = checkSum;

                        file.Name = Utils.ReadString(fileStream);
                        file.Path = MergePath(dirs, file.Name);

                        if ((file.Name.Length + 1) % 2 == 1)
                            fileStream.Seek(1, SeekOrigin.Current);

                        files.Add(file);

                        while (endings.Count > 0 && fileStream.Position == endings.Last())
                        {
                            dirs.Remove(dirs.Last());
                            endings.Remove(endings.Last());
                        }
                    }
                    else if (fileGroupId > 0)
                    {
                        var dir = new NdfDir();

                        fileStream.Read(buffer, 0, 4);
                        dir.FileEntrySize = BitConverter.ToInt32(buffer, 0);

                        if (dir.FileEntrySize != 0)
                            endings.Add(dir.FileEntrySize + fileStream.Position - 8);
                        else if (endings.Count > 0)
                            endings.Add(endings.Last());

                        dir.Name = Utils.ReadString(fileStream);

                        if ((dir.Name.Length + 1) % 2 == 1)
                            fileStream.Seek(1, SeekOrigin.Current);

                        dirs.Add(dir);
                    }
                }
            }
            return files;
        }

        protected string MergePath(IEnumerable<NdfDir> dirs, string p)
        {
            var b = new StringBuilder();

            foreach (NdfDir dir in dirs)
                b.Append(dir.Name);

            b.Append(p);

            return b.ToString();
        }

        protected EdataHeader ReadEdatHeader()
        {
            var header = new EdataHeader();

            using (FileStream fileStream = File.Open(FilePath, FileMode.Open))
            {
                var buffer = new byte[4];

                fileStream.Seek(0x19, SeekOrigin.Begin);
                fileStream.Read(buffer, 0, 4);
                header.DirOffset = BitConverter.ToInt32(buffer, 0);

                fileStream.Read(buffer, 0, 4);
                header.DirLengh = BitConverter.ToInt32(buffer, 0);

                fileStream.Read(buffer, 0, 4);
                header.FileOffset = BitConverter.ToInt32(buffer, 0);

                fileStream.Read(buffer, 0, 4);
                header.FileLengh = BitConverter.ToInt32(buffer, 0);
            }

            return header;
        }
    }
}