using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfObjectReference : NdfValueWrapper
    {
        private uint _instanceId;

        public NdfObjectReference(NdfClass cls, uint instance, long offset)
            : base(NdfType.ObjectReference, offset)
        {
            Class = cls;
            InstanceId = instance;
        }

        public NdfClass Class
        {
            get;
            protected set;
        }

        public uint InstanceId
        {
            get { return _instanceId; }
            set
            {
                _instanceId = value;
                OnPropertyChanged("InstanceId");
            }
        }

        public NdfObject Instance
        {
            get { return Class.Instances.Single(x => x.Id == InstanceId); }
            set
            {
                if (!Class.Instances.Contains(value))
                    throw new ArgumentException("instance");
                InstanceId = value.Id;
                OnPropertyChanged("Instance");
                OnPropertyChanged("InstanceId");
            }
        }

        public override string ToString()
        {
            if (Class == null)
                return string.Format("Class does not exist : {0}", InstanceId);

            return string.Format("{0} : {1} - {2}", Class.Id, InstanceId, Class.Name);

        }

        public override byte[] GetBytes(out bool valid)
        {
            throw new NotImplementedException();
        }
    }
}
