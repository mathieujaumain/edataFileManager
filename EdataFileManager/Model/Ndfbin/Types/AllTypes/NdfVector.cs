using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfVector : NdfFlatValueWrapper
    {
        public NdfVector(Point3D value, long offset)
            : base(NdfType.Vector, value, offset)
        {

        }

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            valid = true;

            var pt = Point3D.Parse((string) value);

            var vector = new List<byte>();

            vector.AddRange(BitConverter.GetBytes(pt.X));
            vector.AddRange(BitConverter.GetBytes(pt.Y));
            vector.AddRange(BitConverter.GetBytes(pt.Z));

            return vector.ToArray();
        }
    }
}
