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
            NdfClass = cls;
            InstanceId = instance;
        }

        public NdfClass NdfClass
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

        public override string ToString()
        {
            if (NdfClass == null)
                return string.Format("Class does not exist : {0}", InstanceId);

            return string.Format("{0} : {1} - {2}", NdfClass.Id, InstanceId, NdfClass.Name);

        }
    }
}
