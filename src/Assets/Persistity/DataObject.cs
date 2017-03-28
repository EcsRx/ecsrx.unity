namespace Persistity
{
    public struct DataObject
    {
        private readonly string _stringData;
        private readonly byte[] _byteData;

        public string AsString
        {
            get
            { return _stringData ?? DefaultEncoding.Encoder.GetString(_byteData); }
        }

        public byte[] AsBytes
        {
            get
            { return _byteData ?? DefaultEncoding.Encoder.GetBytes(_stringData); }
        }

        public DataObject(string data)
        {
            _stringData = data;
            _byteData = null;
        }

        public DataObject(byte[] data)
        {
            _stringData = null;
            _byteData = data;
        }
        
    }
}