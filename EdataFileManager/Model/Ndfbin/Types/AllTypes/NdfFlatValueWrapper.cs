namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public abstract class NdfFlatValueWrapper : NdfValueWrapper
    {
        private object _value;

        protected NdfFlatValueWrapper(NdfType type, object value, long offset)
            : base(type, offset)
        {
            Value = value;
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        public abstract byte[] GetBytes(object value, NdfType type, out bool valid);

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
