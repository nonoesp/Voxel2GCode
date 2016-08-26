using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Geometry;

using System.IO;

using Voxel2GCodeCore;
using Voxel2GCodeCore.Geometry;
using Voxel2GCodeCore.Utilities;
using Voxel2GCodeDesignScript.Utilities;

using MonolithLib;
using CSerializer;
using CGeometrica;
using MalachiteNet;
using MonolithCore;

///////////////////////////////////////////////////////////////////
/// NOTE: This project requires references to the DynamoServices
/// and ProtoGeometry DLLs. These can be found in the latest
/// ZeroTouch and DynamoServices Nuget packages.
///////////////////////////////////////////////////////////////////

namespace Voxel2GCodeDynamo
{
    public class Voxel2GCodeDynamo
    {
        private Voxel2GCodeDynamo()
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Printables"></param>
        /// <param name="Settings"></param>
        /// <param name="IsVerbose"></param>
        /// <returns name="GCode">Printable GCode</returns>

        public static string Printer(
            List<V2GPrintPolyline> Printables,
            [DefaultArgumentAttribute("Voxel2GCodeZeroTouch.GetUnsetValue()")] object Settings,
            [DefaultArgumentAttribute("Voxel2GCodeZeroTouch.GetUnsetValue()")] bool IsVerbose)
        {
            // Stuff
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Create State
            V2GState printer = new V2GState();
            if(IsSet(IsVerbose))    printer.settings.IsVerbose = (bool)IsVerbose;
            if(IsSet(Settings))     printer.SetSettings((V2GSettings)Settings);
            
            // Create Model
            V2GModel model = new V2GModel();
            printer.settings.StartPoint = new V2GPrintPosition(20, 20, 0);
            printer.settings.EndPoint = printer.settings.StartPoint;

            // Append PrintPolylines
            foreach (var printable in Printables)
            {
                if (printable is V2GPrintPolyline)
                {
                    model.AppendAsPath(printable as V2GPrintPolyline);
                }

                // if (printable is PrintPoint) { }
                // if (printable is PrintSurface) { }
            }

            model.AppendAsRelativeMovement(new V2GPrintPosition(0, 0, 10.0), 0);

            // Generate GCode
            model.GenerateGCode(sb, printer);

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GCode"></param>
        /// <param name="FilePath"></param>
        /// <param name="ShouldExport"></param>
        /// 
        public static void Export(
            string GCode,
            string FilePath,
            bool ShouldExport = true)
        {
            if(ShouldExport)
            {
                using (StreamWriter outputFile = new StreamWriter(FilePath))
                {
                    outputFile.Write(GCode);
                }
            }
        }

        /// <summary>
        /// Construct a printable polyline.
        /// </summary>
        /// <param name="PolyCurve">a</param>
        /// <param name="MaterialAmount">a</param>
        /// <param name="Speed">a</param>
        /// <param name="Toolhead">a</param>
        /// <param name="MixPercentage">a</param>
        /// <returns name="PrintPolyline">A newly-constructed ZeroTouchEssentials object.</returns>

        public static V2GPrintPolyline PrintPolyline(
            Autodesk.DesignScript.Geometry.PolyCurve PolyCurve,
            double MaterialAmount = 0.033,
            double Speed = 700.0,
            int Toolhead = 0,
            double MixPercentage = 0.0)
        {
            int i = 0;
            V2GPrintPolyline printPolyline = new V2GPrintPolyline();
            foreach (Curve c in PolyCurve.Curves())
            {
                if(i == 0)
                {
                    printPolyline.AddPrintPosition(V2GDesignScriptGeometry.V2GPoint(c.StartPoint));
                }
                printPolyline.AddPrintPosition(V2GDesignScriptGeometry.V2GPoint(c.EndPoint));
            }
            return printPolyline;
        }

        /*
         * // TODO: Output list of values
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrintPolyline"></param>
        /// <returns name="MaterialAmount">Amount of material extruder per linear mm.</returns>
        /// 
        [MultiReturn(new[] { "MaterialAmount", "Speed", "Toolhead", "MixPercentage" })]
        public static Dictionary<string, object> PrintPolylineData(V2GPrintPolyline PrintPolyline)
        {
            return new Dictionary<string, object>
            {
                { "MaterialAmount", PrintPolyline.MaterialAmount },
                { "Speed", PrintPolyline.Speed },
                { "Toolhead", PrintPolyline.Head },
                { "MixPercentage", PrintPolyline.MixPercentage }
            };
        }*/

        [IsVisibleInDynamoLibrary(false)]
        public static UnsetValue GetUnsetValue()
        {
            return new UnsetValue();
        }

        [IsVisibleInDynamoLibrary(false)]
        public class UnsetValue {
            public UnsetValue()
            {
            }
        }

        [IsVisibleInDynamoLibrary(false)]
        public static bool IsSet(object o)
        {
            return !(o is UnsetValue);
        }
        

        /// <summary>
        /// Customize the settings of the printer.
        /// </summary>
        /// <param name="StartPoint">(true by default)</param>
        /// <param name="EndPoint">(true by default)</param>
        /// <param name="ShouldHeatUpOnStart">(true by default)</param>
        /// <param name="ShouldCoolDownOnEnd">(true by default)</param>
        /// <param name="ZOffset">(true by default)</param>
        /// <param name="BedTemperature">(true by default)</param>
        /// <param name="T0Temperature">(true by default)</param>
        /// <param name="T1Temperature">(true by default)</param>
        /// <returns name="PrintSettings">Printing settings.</returns>
        [MultiReturn(new[] { "PrintSettings", "Log" })]
        public static Dictionary<string, object> PrintSettings(
            [DefaultArgumentAttribute("Voxel2GCodeZeroTouch.GetUnsetValue()")] object StartPoint,
            [DefaultArgumentAttribute("Voxel2GCodeZeroTouch.GetUnsetValue()")] object EndPoint,
            [DefaultArgumentAttribute("Voxel2GCodeZeroTouch.GetUnsetValue()")] bool ShouldHeatUpOnStart,
            [DefaultArgumentAttribute("Voxel2GCodeZeroTouch.GetUnsetValue()")] bool ShouldCoolDownOnEnd,
            double ZOffset = 0.2,
            double BedTemperature = 60.0,
            double T0Temperature = 200.0,
            double T1Temperature = 0
            )
        {
            V2GSettings settings = new V2GSettings();
            if (IsSet(StartPoint))
            {
                Autodesk.DesignScript.Geometry.Point p = (Autodesk.DesignScript.Geometry.Point)StartPoint;
                settings.StartPoint = new V2GPrintPosition(p.X, p.Y, p.Z);
            }
            if (IsSet(EndPoint))
            {
                Autodesk.DesignScript.Geometry.Point p = (Autodesk.DesignScript.Geometry.Point)EndPoint;
                settings.EndPoint = new V2GPrintPosition(p.X, p.Y, p.Z);
            }
            if (IsSet(ShouldHeatUpOnStart)) settings.ShouldHeatUpOnStart = ShouldHeatUpOnStart;
            if (IsSet(ShouldCoolDownOnEnd)) settings.ShouldCoolDownOnEnd = ShouldCoolDownOnEnd;
            if (IsSet(ZOffset)) settings.ZOffset = ZOffset;
            settings.BedTemperature = BedTemperature;
            settings.T0Temperature = T0Temperature;
            settings.T1Temperature = T1Temperature;

            settings.LayerHeight = 0.2;
            string log = "settings\n";
            log += "\nShouldHeatUpOnStart: " + settings.ShouldHeatUpOnStart;
            log += "\nShouldCoolDownOnEnd: " + settings.ShouldCoolDownOnEnd;
            log += "\nZOffset: " + settings.ZOffset;

            return new Dictionary<string, object>
            {
                { "PrintSettings", settings },
                { "Log",  log }
            };
        }
   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Curves"></param>
        /// <returns name="SortedCurves"></returns>

        public static List<Autodesk.DesignScript.Geometry.Curve> CurveSort(
            List<Autodesk.DesignScript.Geometry.Curve> Curves)
        {
            return SortPrintableCurves(Curves);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Curve"></param>
        /// <param name="WaveLength"></param>
        /// <param name="Amplitude"></param>
        /// <param name="Resolution"></param>
        /// <returns name="Points"></returns>
        /// <returns name="k"></returns>


        [MultiReturn(new[] { "Points", "k" })]
        public static Dictionary<string, object> CurveSinusoidalPoints(
            Autodesk.DesignScript.Geometry.Curve _Curve,
            double WaveLength = 5.0,
            double Amplitude = 1.0,
            double Resolution = 2.0)
        {

            double n = (_Curve.Length / WaveLength) * 4 * Resolution;
            double _Span = _Curve.Length / n;
            List<Autodesk.DesignScript.Geometry.Point> points = new List<Autodesk.DesignScript.Geometry.Point>();
            List<double> ks = new List<double>();

            for (int i = 0; i < n; i++)
            {
                double k = Amplitude * Math.Sin(i * _Span * Math.PI * 2 / WaveLength + Math.PI * 2);
                Autodesk.DesignScript.Geometry.Plane pl = _Curve.PlaneAtParameter(_Curve.ParameterAtSegmentLength(i * _Span));
                Autodesk.DesignScript.Geometry.Point p = _Curve.PointAtSegmentLength(i * _Span);
                points.Add((Autodesk.DesignScript.Geometry.Point)p.Translate(pl.YAxis, k));
                ks.Add(k);
            }

            return new Dictionary<string, object>
            {
                { "Points", points },
                { "k",  ks }
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Curve"></param>
        /// <param name="WaveLength"></param>
        /// <param name="Amplitude"></param>
        /// <param name="Resolution"></param>
        /// <returns name="Points"></returns>
        /// <returns name="k"></returns>


        [MultiReturn(new[] { "Points", "k" })]
        public static Dictionary<string, object> CurveSinusoidalPointsWithVoxel(
            Autodesk.DesignScript.Geometry.Curve _Curve,
            VoxelImage Voxel,
            double WaveLength = 5.0,
            double Amplitude = 1.0,
            double Resolution = 2.0)
        {
            VoxelChannel _VoxelChannel = Voxel.GetChannel(VoxelImageLayout.SHAPECHANNEL);

            double n = (_Curve.Length / WaveLength) * 4 * Resolution;
            double _Span = _Curve.Length / n;
            List<Autodesk.DesignScript.Geometry.Point> points = new List<Autodesk.DesignScript.Geometry.Point>();
            List<double> ks = new List<double>();

            for (int i = 0; i < n; i++)
            {
                Autodesk.DesignScript.Geometry.Plane pl = _Curve.PlaneAtParameter(_Curve.ParameterAtSegmentLength(i * _Span));
                Autodesk.DesignScript.Geometry.Point p = _Curve.PointAtSegmentLength(i * _Span);
                double FieldValue = V2GVoxel.GetVoxelFieldValue(_VoxelChannel, p.X, p.Y, p.Z);
                
                double k = FieldValue * Amplitude * Math.Sin(i * _Span * Math.PI * 2 / WaveLength + Math.PI * 2);

                points.Add((Autodesk.DesignScript.Geometry.Point)p.Translate(pl.YAxis, k));
                ks.Add(k);
            }

            return new Dictionary<string, object>
            {
                { "Points", points },
                { "k",  ks }
            };
        }

        /// <summary>
        /// Create a bounding box and display its edges.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="depth"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<Autodesk.DesignScript.Geometry.Curve> PrintingBoundaries(double width = 248.92, double depth = 233.68, double height = 162.56)
        {
            // ZMorph 2.0 dimensions
            // in: { 9.8,9.2,6.4}
            // mm: {248.92,487.68,162.56}

            List<Autodesk.DesignScript.Geometry.Curve> curves = new List<Autodesk.DesignScript.Geometry.Curve>();
            Autodesk.DesignScript.Geometry.BoundingBox bb = Autodesk.DesignScript.Geometry.BoundingBox.ByCorners(
                   Autodesk.DesignScript.Geometry.Point.ByCoordinates(0,0,0),
                   Autodesk.DesignScript.Geometry.Point.ByCoordinates(width, depth, height));
            Edge[] edges = bb.ToPolySurface().Edges;
            foreach(Edge edge in edges)
            {
                curves.Add(edge.CurveGeometry);
            }
            return curves;
        }

        /// <summary>
        /// Augment a point with voxel metadata.
        /// </summary>
        /// <param name="Voxel">A voxel model from Monolith.</param>
        /// <param name="Point">A point in space.</param>
        /// <returns name="VoxelPoint"></returns>
        /// 

        public static V2GVoxelPoint VoxelPoint(MonolithLib.VoxelImage Voxel, Autodesk.DesignScript.Geometry.Point Point)
        {
            return V2GVoxel.GetVoxelPoint(Voxel, new V2GPoint(Point.X, Point.Y, Point.Z));
        }

        /// <summary>
        /// Read a .vol file into a VoxelImage.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns name="Voxel"></returns>

        public static VoxelImage LoadVolFile(string Path)
        {
            VoxelImage vox = new VoxelImage(VoxelImageLayout.SHAPE_MATERIALRATIO);
            vox.LoadVolFile(Path);
            return vox;// deLib.GeometricUtilities.GetVoxelPoint(Voxel, Point);
        }

        /// <summary>
        /// Get the Shape and Material Ratio channels from a VoxelImage.
        /// </summary>
        /// <param name="Voxel"></param>
        /// <returns name="Shape"></returns>
        /// <returns name="Material Ratio"></returns>
        [MultiReturn(new[] { "Shape", "Material Ratio" })]
        public static Dictionary<string, object> VoxelChannels(VoxelImage v)
        {
            VoxelChannel ShapeChannel = v.GetChannel(VoxelImageLayout.SHAPECHANNEL);
            VoxelChannel MaterialRatioChannel = v.GetChannel(VoxelImageLayout.MATERIALRATIOCHANNEL);
            return new Dictionary<string, object>
            {
                { "Shape", ShapeChannel },
                { "Material Ratio",  MaterialRatioChannel }
            };
        }

        /// <summary>
        /// Get field value of a VoxelChannel at a given point.
        /// </summary>
        /// <param name="vc"></param>
        /// <param name="p"></param>
        /// <returns name="Value"></returns>
        public static double VoxelChannelValueAtPoint(VoxelChannel vc, Autodesk.DesignScript.Geometry.Point p)
        {
            return V2GVoxel.GetVoxelFieldValue(vc, p.X, p.Y, p.Z);
        }

        /// <summary>
        /// Sort a series of curves in the shortest printing path.
        /// </summary>
        /// <param name="_curves"></param>
        /// <returns name="Curves"></returns>
        public static List<Autodesk.DesignScript.Geometry.Curve> SortPrintableCurves(List<Autodesk.DesignScript.Geometry.Curve> _curves)
        {
            List<Autodesk.DesignScript.Geometry.Curve> sortedCurves = new List<Autodesk.DesignScript.Geometry.Curve>();
            List<Autodesk.DesignScript.Geometry.Curve> curves = new List<Autodesk.DesignScript.Geometry.Curve>();
            curves.AddRange(_curves); // Copying elements to curves to avoid modifying the original list of curves

            Autodesk.DesignScript.Geometry.Curve currentCurve = curves[curves.Count - 1];
            curves.RemoveAt(curves.Count - 1);
            sortedCurves.Add(currentCurve);

            // find ouf next curve either startpoint or endpoint is closest to the end point of the current curve

            while (curves.Count != 0)
            {
                int bestI = 0;
                double bestDistance = double.MaxValue;
                Autodesk.DesignScript.Geometry.Point currentPoint = currentCurve.EndPoint;
                for (int i = 0; i < curves.Count; ++i)
                {
                    Autodesk.DesignScript.Geometry.Curve c = curves[i];

                    Autodesk.DesignScript.Geometry.Line l1 = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(currentPoint, c.StartPoint);
                    Autodesk.DesignScript.Geometry.Line l2 = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(currentPoint, c.EndPoint);

                    double distanceToStartPoint = l1.Length;
                    double distanceToEndPoint = l2.Length;

                    double d = Math.Min(distanceToStartPoint, distanceToEndPoint);

                    if (bestDistance > d)
                    {
                        bestI = i;
                        bestDistance = d;
                    }
                }

                currentCurve = curves[bestI];
                curves.RemoveAt(bestI);

                double distanceToCurrentCurveEndPoint = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(currentPoint, currentCurve.EndPoint).Length;
                double distanceToCurrentCurveStartPoint = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(currentPoint, currentCurve.StartPoint).Length;

                if (distanceToCurrentCurveEndPoint < distanceToCurrentCurveStartPoint)
                {
                    currentCurve = currentCurve.Reverse();
                }

                sortedCurves.Add(currentCurve);
            }

            return sortedCurves;
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point3d">aa</param>
        /// <returns></returns>
        /// 
        [MultiReturn(new[] { "X", "Y", "Z" })]
        public static Dictionary<string, object> Point3dData(Point3d Point3d)
        {
            return new Dictionary<string, object>
            {
                { "X", Point3d.X },
                { "Y",  Point3d.Y },
                { "Z",  Point3d.Z }
            };
        }*/
    }
}
