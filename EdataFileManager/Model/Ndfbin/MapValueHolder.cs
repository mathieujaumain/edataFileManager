using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;

namespace EdataFileManager.Model.Ndfbin
{
    public class MapValueHolder : CollectionItemValueHolder
    {
        public MapValueHolder(NdfValueWrapper value, NdfbinManager manager, long instanceOffset) : base(value, manager, instanceOffset)
        {
        }
    }
}
