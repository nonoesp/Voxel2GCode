using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;
using Rhino.Geometry;
using Voxel2GCodeCore.Utilities;

namespace Voxel2GCodeGH
{
    class V2GH
    {
        public static Point3d Point3d(V2GPoint p)
        {
            return new Point3d(p.X, p.Y, p.Z);
        }

        public static Vector3d Vector3d(V2GPoint p)
        {
            return new Vector3d(p.X, p.Y, p.Z);
        }

        public static V2GPoint V2GPoint(Point3d p)
        {
            return new V2GPoint(p.X, p.Y, p.Z);
        }
    }
}
