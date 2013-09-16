using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Edata
{
    /// <summary>
    /// struct cndfHeader {
    ///	char header[12];
    ///	DWORD always128;
    ///	DWORD blockSizef;
    ///	DWORD chunk1;
    ///	DWORD len4;
    ///	DWORD chunk2;
    ///	DWORD blockSizePlusE0;
    ///	DWORD chunk3;
    ///	DWORD blockSizePlusE0MinusLen4;
    ///};
    /// </summary>
    public class NdfFileContent : ViewModelBase
    {
        private bool _isCompressedBody;
        private int _blockSize;
        private int _blockSizeE0;
        private int _blockSizeWithoutHeader;

        private byte[] _body;

        public int BlockSize
        {
            get { return _blockSize; }
            set { _blockSize = value; OnPropertyChanged(() => BlockSize); }
        }

        public int BlockSizeE0
        {
            get { return _blockSizeE0; }
            set { _blockSizeE0 = value; OnPropertyChanged(() => BlockSizeE0); }
        }

        public int BlockSizeWithoutHeader
        {
            get { return _blockSizeWithoutHeader; }
            set { _blockSizeWithoutHeader = value; OnPropertyChanged(() => BlockSizeWithoutHeader); }
        }

        public byte[] Body
        {
            get { return _body; }
            set { _body = value; OnPropertyChanged(() => Body); }
        }

        public bool IsCompressedBody
        {
            get { return _isCompressedBody; }
            set { _isCompressedBody = value; OnPropertyChanged(() => IsCompressedBody); }
        }
    }
}
