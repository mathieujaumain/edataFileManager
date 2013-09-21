using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfCollection : NdfValueWrapper, IList<CollectionItemValueHolder>, INotifyCollectionChanged, IList
    {
        private readonly ObservableCollection<CollectionItemValueHolder> _innerList = new ObservableCollection<CollectionItemValueHolder>();

        public NdfCollection(long offset)
            : base(NdfType.List, offset)
        {

        }

        public NdfCollection(IEnumerable<CollectionItemValueHolder> list, long offset)
            : this(offset)
        {
            if (list != null)
                foreach (CollectionItemValueHolder wrapper in list)
                    InnerList.Add(wrapper);
        }

        public ObservableCollection<CollectionItemValueHolder> InnerList
        {
            get { return _innerList; }
        }

        public override string ToString()
        {
            return string.Format("Collection[{0}]", InnerList.Count);
        }

        #region IList<NdfValueWrapper> Members

        public int IndexOf(CollectionItemValueHolder item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, CollectionItemValueHolder item)
        {
            InnerList.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveAt(int index)
        {
            var el = InnerList[index];

            InnerList.RemoveAt(index);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public CollectionItemValueHolder this[int index]
        {
            get { return InnerList[index]; }
            set { InnerList[index] = value; }
        }

        public void Add(CollectionItemValueHolder item)
        {
            InnerList.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear()
        {
            InnerList.Clear();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(CollectionItemValueHolder item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(CollectionItemValueHolder[] array, int arrayIndex)
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

        public bool Remove(CollectionItemValueHolder item)
        {
            var res = InnerList.Remove(item);

            if (res)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            return res;
        }

        public IEnumerator<CollectionItemValueHolder> GetEnumerator()
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
            valid = false;

            return new byte[0];
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        public int Add(object value)
        {
            var val = value as CollectionItemValueHolder;
            if (val == null)
                return -1;

            Add(val);

            return IndexOf(value);
        }

        public bool Contains(object value)
        {
            var val = value as CollectionItemValueHolder;
            if (val == null)
                return false;

            return InnerList.Contains(value as CollectionItemValueHolder);
        }

        public int IndexOf(object value)
        {
            return IndexOf(value as CollectionItemValueHolder);
        }

        public void Insert(int index, object value)
        {
            Insert(index, value as CollectionItemValueHolder);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            Remove(value as CollectionItemValueHolder);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value as CollectionItemValueHolder;
            }
        }

        public void CopyTo(System.Array array, int index)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }
    }
}