using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfDouble_2 : NdfDouble
    {
        public NdfDouble_2(double value, long offset) : base(value, offset)
        {
            Type = NdfType.Float64_2;
        }
    }
}
