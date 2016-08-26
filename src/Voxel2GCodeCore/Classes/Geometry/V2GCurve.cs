using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel2GCodeCore.Geometry
{
    abstract public class V2GCurve
    {
        /// <summary>
        /// 
        /// </summary>
        abstract public V2GPoint StartPoint
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        abstract public V2GPoint EndPoint
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        abstract public void Reverse();

        /*public V2GCurve()
        {
            //this.StartPoint = startPoint;
            //this.EndPoint = endPoint;
        }*/
    }

    /*
    class V2GCurveRhino: V2GCurve
    {
        Rhino.Geometry.Curve curve;

        virtual public V2GPoint StartPoint
        {
            get
            {
                return new V2GPoint(curve.PointAtStart.X, curve.PointAtStart.Y, curve.PointAtStart.Z);
            };
        }

        virtual public V2GPoint EndPoint
        {
            get
            {
                return new V2GPoint(curve.PointAtEnd.X, curve.PointAtEnd.Y, curve.PointAtEnd.Z);
            };
        }
    }*/
}
