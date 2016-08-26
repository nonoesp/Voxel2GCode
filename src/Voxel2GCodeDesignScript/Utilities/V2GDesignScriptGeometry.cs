using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;
using Autodesk.DesignScript.Geometry;

namespace Voxel2GCodeDesignScript.Utilities
{
    public class V2GDesignScriptGeometry
    {
        public static V2GPoint V2GPoint(Autodesk.DesignScript.Geometry.Point p)
        {
            return new V2GPoint(p.X, p.Y, p.Z);
        }
        public static Point Point(V2GPoint p)
        {
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(p.X, p.Y, p.Z);
        }
    }
}
