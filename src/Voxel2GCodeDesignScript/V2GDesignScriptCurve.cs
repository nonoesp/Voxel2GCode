using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;
using Autodesk.DesignScript.Geometry;

namespace Voxel2GCodeDesignScript
{
    class V2GDesignScriptCurve : V2GCurve
    {
        public Autodesk.DesignScript.Geometry.Curve curve;

        public override V2GPoint StartPoint
        {
            get
            {
                return new V2GPoint(curve.StartPoint.X, curve.StartPoint.Y, curve.StartPoint.Z);
            }
        }

        public override V2GPoint EndPoint
        {
            get
            {
                return new V2GPoint(curve.EndPoint.X, curve.EndPoint.Y, curve.EndPoint.Z);
            }
        }

        public override void Reverse()
        {
            curve.Reverse();
        }

        public V2GDesignScriptCurve(Curve c)
        {
            this.curve = c;
        }
    }
}
