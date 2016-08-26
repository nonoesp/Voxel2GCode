using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeCore.Utilities
{
    public class V2GGeometry
    {
        public int value = 32;
        /// <summary>
        /// Sort curves on the shortest path.
        /// </summary>
        /// <param name="curvesToSort"></param>
        /// <returns></returns>
        public static List<V2GCurve> SortCurves(List<V2GCurve> curvesToSort)
        {
            List<V2GCurve> sortedCurves = new List<V2GCurve>();
            List<V2GCurve> curves = new List<V2GCurve>();
            curves.AddRange(curvesToSort);

            // todo: first curve could be the furthest from the center
            // as of now it's the last curve of the list
            V2GCurve currentCurve = curves[curves.Count - 1];
            curves.RemoveAt(curves.Count - 1);
            sortedCurves.Add(currentCurve);

            // find ouf next curve either startpoint or endpoint is closest to the end point of the current curve
            while (curves.Count != 0)
            {
                int bestI = 0;
                double bestDistance = double.MaxValue;
                V2GPoint currentPoint = currentCurve.EndPoint;
                for (int i = 0; i < curves.Count; ++i)
                {
                    V2GCurve c = curves[i];
                    double d = Math.Min(currentPoint.DistanceTo(c.StartPoint), currentPoint.DistanceTo(c.EndPoint));
                    if (bestDistance > d)
                    {
                        bestI = i;
                        bestDistance = d;
                    }
                }
                currentCurve = curves[bestI];
                curves.RemoveAt(bestI);
                if (currentPoint.DistanceTo(currentCurve.EndPoint) < currentPoint.DistanceTo(currentCurve.StartPoint))
                {
                    currentCurve.Reverse();
                }
                sortedCurves.Add(currentCurve);
            }
            return sortedCurves;
        }

        /*
        /// <summary>
        /// Sort curves on the shortest path for 3D printing.
        /// </summary>
        /// <param name="_curves"></param>
        /// <returns></returns>
        public static List<Rhino.Geometry.Curve> SortPrintableCurvesRhino(List<Rhino.Geometry.Curve> _curves)
        {
            List<Curve> sortedCurves = new List<Curve>();
            List<Curve> curves = new List<Curve>();
            curves.AddRange(_curves); // Copying elements to curves to avoid modifying the original list of curves

            // todo: first curve could be the furthest from the center
            // now the last curve of the list
            Curve currentCurve = curves[curves.Count - 1];
            curves.RemoveAt(curves.Count - 1);
            sortedCurves.Add(currentCurve);

            // find ouf next curve either startpoint or endpoint is closest to the end point of the current curve

            while (curves.Count != 0)
            {
                int bestI = 0;
                double bestDistance = double.MaxValue;
                Point3d currentPoint = currentCurve.PointAtEnd;
                for (int i = 0; i < curves.Count; ++i)
                {
                    Curve c = curves[i];
                    double d = Math.Min(currentPoint.DistanceTo(c.PointAtStart), currentPoint.DistanceTo(c.PointAtEnd));
                    if (bestDistance > d)
                    {
                        bestI = i;
                        bestDistance = d;
                    }
                }

                currentCurve = curves[bestI];
                curves.RemoveAt(bestI);
                if (currentPoint.DistanceTo(currentCurve.PointAtEnd) < currentPoint.DistanceTo(currentCurve.PointAtStart))
                {
                    currentCurve.Reverse();
                }

                sortedCurves.Add(currentCurve);
            }
            return sortedCurves;
        }*/
    }
}
