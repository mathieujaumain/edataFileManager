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

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            var pt = (Point3D)Value;

            var vector = new List<byte>();

            vector.AddRange(BitConverter.GetBytes((Single)pt.X));
            vector.AddRange(BitConverter.GetBytes((Single)pt.Y));
            vector.AddRange(BitConverter.GetBytes((Single)pt.Z));

            return vector.ToArray();
        }
    }
}
