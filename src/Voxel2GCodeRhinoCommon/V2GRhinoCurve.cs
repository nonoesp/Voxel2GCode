using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;
using Rhino.Geometry;

namespace Voxel2GCodeRhino
{
    public class V2GRhinoCurve : V2GCurve
    {
        public Rhino.Geometry.Curve curve;

        public override V2GPoint StartPoint
        {
            get
            {
                return new V2GPoint(curve.PointAtStart.X, curve.PointAtStart.Y, curve.PointAtStart.Z);
            }
        }

        public override V2GPoint EndPoint
        {
            get
            {
                return new V2GPoint(curve.PointAtEnd.X, curve.PointAtEnd.Y, curve.PointAtEnd.Z);
            }
        }

        public override void Reverse()
        {
            curve.Reverse();
        }

        public V2GRhinoCurve(Curve c)
        {
            this.curve = c;
        } 
    }
}
