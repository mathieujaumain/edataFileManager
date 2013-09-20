using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfColor : NdfFlatValueWrapper
    {
        public NdfColor(Color value, long offset)
            : base(NdfType.Color32, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            var col = (Color)Value;

            var colorArray = new byte[] { col.A, col.R, col.G, col.B };

            return colorArray;
        }
    }
}
