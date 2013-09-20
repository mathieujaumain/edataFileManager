using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfCollection : NdfValueWrapper, IList<NdfValueWrapper>
    {
        private readonly ObservableCollection<NdfValueWrapper> _innerList = new ObservableCollection<NdfValueWrapper>();

        public NdfCollection(long offset)
            : base(NdfType.List, offset)
        {

        }

        public NdfCollection(IEnumerable<NdfValueWrapper> list, long offset)
            : this(offset)
        {
            if (list != null)
                foreach (NdfValueWrapper wrapper in list)
                    InnerList.Add(wrapper);
        }

        protected ObservableCollection<NdfValueWrapper> InnerList
        {
            get { return _innerList; }
        }

        public override string ToString()
        {
            return string.Format("Collection[{0}]", InnerList.Count);
        }

        #region IList<NdfValueWrapper> Members

        public int IndexOf(NdfValueWrapper item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, NdfValueWrapper item)
        {
            InnerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InnerList.RemoveAt(index);
        }

        public NdfValueWrapper this[int index]
        {
            get { return InnerList[index]; }
            set { InnerList[index] = value; }
        }

        public void Add(NdfValueWrapper item)
        {
            InnerList.Add(item);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public bool Contains(NdfValueWrapper item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(NdfValueWrapper[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(NdfValueWrapper item)
        {
            return InnerList.Remove(item);
        }

        public IEnumerator<NdfValueWrapper> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        #endregion

        public override byte[] GetBytes(out bool valid)
        {
            throw new System.NotImplementedException();
        }
    }
}