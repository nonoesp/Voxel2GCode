using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MalachiteNet;
using MonolithLib;
using CGeometrica;
using CSerializer;
using Voxel2GCodeCore.Utilities;

namespace Voxel2GCodeCore.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    public class V2GVoxelPoint
    {
        public V2GPoint Position;
        public V2GPoint ContourVector3d;
        public V2GPoint ContourVector;
        public V2GPoint GradientVector;
        public double FieldValue;

        /// <summary>
        /// A V2GPoint with voxel metadata.
        /// </summary>
        /// <param name="_Voxel"></param>
        /// <param name="_Point"></param>
        public V2GVoxelPoint(VoxelImage _Voxel, V2GPoint _Point)
        {
            this.Position = _Point;
            this.SetPropertiesWith(_Voxel);
        }

        /// <summary>
        /// Calculate voxel point properties.
        /// </summary>
        /// <param name="_Voxel"></param>
        public void SetPropertiesWith(VoxelImage _Voxel) {
            double delta = 0.000001;

            VoxelImage v = _Voxel as VoxelImage;
            VoxelChannel vc = v.GetChannel(VoxelImageLayout.SHAPECHANNEL) as VoxelChannel;

            V2GPoint x0 = this.Position - V2GPoint.XAxis * delta;
            V2GPoint x1 = this.Position + V2GPoint.XAxis * delta;
            V2GPoint y0 = this.Position - V2GPoint.YAxis * delta;
            V2GPoint y1 = this.Position + V2GPoint.YAxis * delta;
            V2GPoint z0 = this.Position - V2GPoint.ZAxis * delta;
            V2GPoint z1 = this.Position + V2GPoint.ZAxis * delta;

            double x = V2GVoxel.GetVoxelFieldValue(vc, x1) - V2GVoxel.GetVoxelFieldValue(vc, x0);
            double y = V2GVoxel.GetVoxelFieldValue(vc, y1) - V2GVoxel.GetVoxelFieldValue(vc, y0);
            double z = V2GVoxel.GetVoxelFieldValue(vc, z1) - V2GVoxel.GetVoxelFieldValue(vc, z0);

            V2GPoint UnitVector = new V2GPoint(x, y, z);
            UnitVector.Normalize();

            double fieldValue = V2GVoxel.GetVoxelFieldValue(vc, this.Position);
            V2GPoint RealVector = UnitVector * fieldValue;

            V2GPoint UnitVectorProjected = new V2GPoint(RealVector.X, RealVector.Y, 0);
            V2GPoint UnitVectorPerpendicularProjected = V2GPoint.CrossProduct(UnitVectorProjected, V2GPoint.ZAxis);
            UnitVectorProjected.Normalize();
            UnitVectorPerpendicularProjected.Normalize();

            this.ContourVector3d = UnitVector;
            this.ContourVector = UnitVectorPerpendicularProjected;
            this.GradientVector = UnitVectorProjected;
            this.FieldValue = fieldValue;
        }
    }
}
