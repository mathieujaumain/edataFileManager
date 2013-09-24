using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfFileNameString : NdfString
    {
        public NdfFileNameString(NdfStringReference value, long offset) : base(value, offset)
        {
            Type = NdfType.TableStringFile;
        }
    }
}
