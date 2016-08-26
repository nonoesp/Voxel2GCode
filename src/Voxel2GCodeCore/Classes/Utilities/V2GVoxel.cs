using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MalachiteNet;
using MonolithLib;
using CGeometrica;
using CSerializer;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeCore.Utilities
{
    /// <summary>
    /// A class with utilities for Monolith voxel-based model operations.
    /// </summary>
    public class V2GVoxel
    {
        /// <summary>
        /// Augment a point with voxel information.
        /// </summary>
        /// <param name="Voxel"></param>
        /// <param name="_Point"></param>
        /// <returns></returns>
        public static V2GVoxelPoint GetVoxelPoint(VoxelImage Voxel, V2GPoint _Point)
        {
            return new V2GVoxelPoint(Voxel, _Point);
        }

        /// <summary>
        /// Get the interpolated value of a voxel field at a given point with coordinates.
        /// </summary>
        /// <param name="vc"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double GetVoxelFieldValue(VoxelChannel vc, double x, double y, double z)
        {
            double res = 0.0;
            if (vc.GetDataAtPointTriL(new CVector(x, y, z), out res)) return res;
            return 0.0;
        }

        /// <summary>
        /// Get the interpolated value of a voxel field at a given Point3d.
        /// </summary>
        /// <param name="vc"></param>
        /// <param name="p"></param>
        /// <returns></returns>

        public static double GetVoxelFieldValue(VoxelChannel vc, V2GPoint p)
        {
            double res = 0.0;
            if (vc.GetDataAtPointTriL(new CVector(p.X, p.Y, p.Z), out res)) return res;
            return 0.0;
        }

        /// <summary>
        /// Walk through the contour and gradient curves of a voxel field.
        /// </summary>
        /// <param name="VoxelImage">A voxel.</param>
        /// <param name="Point">A Point3d.</param>
        /// <param name="SegmentLength"></param>
        /// <param name="MaxIterations"></param>
        /// <param name="VectorType"></param>
        /// <returns></returns>

        public static List<V2GPoint> VoxelCurvePoints(VoxelImage VoxelImage, V2GPoint Point, double SegmentLength = 0.0001, int MaxIterations = 10000, int VectorType = 0)
        {
            List<V2GPoint> Points = new List<V2GPoint>();
            List<V2GLine> lines = new List<V2GLine>();

            V2GPoint lastPoint = Point;
            bool ContourEnded = false;

            int i = 0;
            Points.Add(Point);
            while (ContourEnded == false)
            {
                if (i > MaxIterations) ContourEnded = true;
                V2GPoint pathVector = new V2GPoint();
                V2GVoxelPoint voxelPoint = V2GVoxel.GetVoxelPoint(VoxelImage as VoxelImage, lastPoint);
                if (voxelPoint.FieldValue > 0.995) ContourEnded = true;
                if (voxelPoint.FieldValue < 0.005) ContourEnded = true;

                if (Points.Count > 500 && lastPoint.DistanceTo(Points[Points.Count - 20]) < 0.001) ContourEnded = true;

                switch (VectorType)
                {
                    case 0:
                        pathVector = voxelPoint.ContourVector;
                        break;
                    case 1:
                        pathVector = voxelPoint.GradientVector;
                        break;
                    case 2:
                        pathVector = voxelPoint.ContourVector3d;
                        break;
                    default:
                        pathVector = voxelPoint.ContourVector;
                        break;
                }

                V2GPoint newPoint = lastPoint + pathVector * SegmentLength;

                lines.Add(new V2GLine(newPoint, Point));
                Points.Add(newPoint);
                lastPoint = newPoint;
                if (i > 50 && newPoint.DistanceTo(Point) < 0.001)
                {
                    ContourEnded = true;
                    Points.Add(Point);
                }
                ++i;
            }
            return Points;
        }

        /// <summary>
        /// Walk through the contour and gradient curves of a voxel field limiting the field range (for gradient curves).
        /// </summary>
        /// <param name="VoxelImage">A voxel.</param>
        /// <param name="Point">A Point3d.</param>
        /// <param name="SegmentLength">The length of each of the steps to walk.</param>
        /// <param name="MaxIterations"></param>
        /// <param name="FieldRange">Restrict the maximum field range to walk (only for contour curves).</param>
        /// <param name="VectorType"></param>
        /// <returns></returns>

        public static List<V2GPoint> VoxelCurvePoints(VoxelImage VoxelImage, V2GPoint Point, double SegmentLength = 0.0001, int MaxIterations = 10000, double FieldRange = 0.1, int VectorType = 1)
        {
            List<V2GPoint> Points = new List<V2GPoint>();
            List<V2GLine> lines = new List<V2GLine>();

            VoxelImage voxelImage = VoxelImage as VoxelImage;
            V2GVoxelPoint StartVoxelPoint = V2GVoxel.GetVoxelPoint(voxelImage, Point);

            V2GPoint lastPoint = Point;
            bool ContourEnded = false;

            int i = 0;
            Points.Add(Point);
            while (ContourEnded == false)
            {
                if (i > MaxIterations) ContourEnded = true;
                V2GPoint pathVector = new V2GPoint();
                V2GVoxelPoint voxelPoint = V2GVoxel.GetVoxelPoint(voxelImage, lastPoint);

                if (voxelPoint.FieldValue > 0.995 ||
                    voxelPoint.FieldValue < 0.005 ||
                    Points.Count > 500 && lastPoint.DistanceTo(Points[Points.Count - 20]) < 0.001 ||
                    Math.Abs(StartVoxelPoint.FieldValue - voxelPoint.FieldValue) > FieldRange)
                {
                    ContourEnded = true;
                }

                switch (VectorType)
                {
                    case 0:
                        pathVector = voxelPoint.ContourVector;
                        break;
                    case 1:
                        pathVector = voxelPoint.GradientVector;
                        break;
                    case 2:
                        pathVector = voxelPoint.ContourVector3d;
                        break;
                    default:
                        pathVector = voxelPoint.ContourVector;
                        break;
                }

                V2GPoint newPoint = lastPoint + pathVector * SegmentLength;

                lines.Add(new V2GLine(newPoint, Point));
                Points.Add(newPoint);
                lastPoint = newPoint;
                if (i > 50 && newPoint.DistanceTo(Point) < 0.001)
                {
                    ContourEnded = true;
                    Points.Add(Point);
                }
                ++i;
            }
            return Points;
        }
    }
}
