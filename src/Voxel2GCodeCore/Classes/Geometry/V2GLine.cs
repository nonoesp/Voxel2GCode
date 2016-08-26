using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel2GCodeCore.Geometry
{
    class V2GLine
    {
        public V2GPoint StartPoint;
        public V2GPoint EndPoint;

        public V2GLine(V2GPoint startPoint, V2GPoint endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }
    }
}
