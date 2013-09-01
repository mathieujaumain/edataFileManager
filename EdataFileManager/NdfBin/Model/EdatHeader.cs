namespace EdataFileManager.NdfBin.Model
{
    /// <summary>
    /// Thanks to Wargame:EE DAT Unpacker by Giovanni Condello
    /// struct edataHeader
    /// {
    ///	    CHAR edat[4];
    ///	    blob junk[21];
    ///	    DWORD dirOffset;
    ///	    DWORD dirLength;
    ///	    DWORD fileOffset;
    ///	    DWORD fileLength;
    /// };
    /// </summary>
    public class EdatHeader
    {
        public int DirOffset { get; set; }
        public int DirLengh { get; set; }
        public int FileOffset { get; set; }
        public int FileLengh { get; set; }
    }
}
