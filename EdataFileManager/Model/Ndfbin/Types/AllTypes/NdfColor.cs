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

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            valid = true;

            var colObj = ColorConverter.ConvertFromString(value.ToString());

            if (colObj == null)
            {
                valid = false;
                return new byte[0];
            }

            var col = (Color)colObj;

            var colorArray = new byte[] { col.A, col.R, col.G, col.B };

            return colorArray;
        }
    }
}
