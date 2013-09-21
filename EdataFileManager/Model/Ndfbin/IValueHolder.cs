using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;

namespace EdataFileManager.Model.Ndfbin
{
    public interface IValueHolder
    {
        NdfValueWrapper Value { get; set; }

        NdfbinManager Manager { get; }

        long InstanceOffset { get; }
    }
}
