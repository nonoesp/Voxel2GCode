using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Voxel2GCodeCore.Geometry;
using Voxel2GCodeCore.Instructions;

namespace Voxel2GCodeCore
{
    /// <summary>
    ///  A class to hold a printable model which contains printable paths and slices.
    /// </summary>
    public class V2GModel
    {
        /// <summary>
        /// A dictionary with slices of the model.
        /// </summary>
        public Dictionary<int, V2GSlice> Slices = new Dictionary<int, V2GSlice>();
        /// <summary>
        /// A list of paths of the model.
        /// </summary>
        public List<V2GPath> Paths = new List<V2GPath>();

        /*
        // TODO
        public void AppendAsPath(Printable printable)
        {
            this.Paths.Add(printable.GetPath());

            or

            this.Path.Add(Printable.GenerateInstructions())
        }
        */

        /// <summary>
        /// Append a PrintPolyline as a path.
        /// </summary>
        /// <param name="ppl"></param>
        public void AppendAsPath(V2GPrintPolyline ppl)
        {
            V2GPath path = new V2GPath();
            foreach(V2GPrintPosition PrintPosition in ppl.PrintPositions)
            {
                V2GInstruction seg;
                if (path.Segments.Count == 0)
                {
                    seg = new V2GMovement(PrintPosition, 7200.0); // First: PrintMovement
                }
                else
                {
                    seg = new V2GPrintSegment(PrintPosition); // Others: PrintSegment
                }
                path.Segments.Add(seg);
            }
            Paths.Add(path);
        }

        // TODO: Implement with V2GMovement
        public void AppendAsMovementPath(List<V2GPrintPosition> pl)
        {
            V2GPath path = new V2GPath();

            foreach (V2GPrintPosition p in pl)
            {
                V2GInstruction seg;
                seg = new PrintMovementOld();
                (seg as PrintMovementOld).p = p;
                path.Segments.Add(seg);
            }
            Paths.Add(path);
        }

        // TODO: Implement with V2GMovement
        public void AppendAsRelativeMovement(V2GPrintPosition p)
        {
            V2GPath path = new V2GPath();

            V2GInstruction seg;
            seg = new PrintMovementOld();
            (seg as PrintMovementOld).p = p;
            (seg as PrintMovementOld).IsRelative = true;
            (seg as PrintMovementOld).speed = 2400;
            path.Segments.Add(seg);

            Paths.Add(path);
        }

        public void AppendAsRelativeMovement(V2GPrintPosition p, double Retraction, double Speed = 2400)
        {
            V2GPath path = new V2GPath();

            V2GInstruction seg;
            seg = new PrintMovementOld();
            (seg as PrintMovementOld).p = p;
            (seg as PrintMovementOld).IsRelative = true;
            (seg as PrintMovementOld).ForceFilamentOperations = true;
            (seg as PrintMovementOld).FilamentRetract = Retraction; // mm
            (seg as PrintMovementOld).speed = Speed;
            path.Segments.Add(seg);

            Paths.Add(path);
        }

        /*
        /// <summary>
        /// Append a polyline as a path.
        /// </summary>
        /// <param name="pl"></param>
        public void AppendAsPath(Polyline pl)
        {
            this.AppendAsPath(pl, 0.05, 700, true);
        }

        /// <summary>
        /// Append a polyline as a path with a given speed.
        /// </summary>
        /// <param name="pl"></param>
        /// <param name="speed"></param>
        public void AppendAsPath2(Polyline pl, double speed)
        {
            PrintPath path = new PrintPath();

            foreach (Point3d p in pl)
            {
                //seg.IsFirst = (path.Segments.Count == 0);
                PrintInstruction seg;
                if (path.Segments.Count == 0)
                {
                    // First: PrintMovement
                    seg = new PrintMovement2();
                    (seg as PrintMovement2).p = p;
                    (seg as PrintMovement2).speed = 7200;
                }
                else
                {
                    // Others: PrintSegment
                    seg = new PrintSegment2();
                    (seg as PrintSegment2).p = p;
                    (seg as PrintSegment2).speed = speed;
                }
                path.Segments.Add(seg);
            }
            Paths.Add(path);
        }

        public void AppendAsPath(Polyline pl, double thickness, double speed, bool AllowsGroundOverride = true, double HeightFactor = 1.0, double MovementZ = 0.3)
        {
            PrintPath path = new PrintPath();

            foreach (Point3d p in pl)
            {
                //seg.IsFirst = (path.Segments.Count == 0);
                PrintInstruction seg;
                if (path.Segments.Count == 0)
                {
                    // First: PrintMovement
                    seg = new PrintMovement();
                    (seg as PrintMovement).p = p;
                    (seg as PrintMovement).zDelta = MovementZ; // how much the printer moves when moving to the new path
                    (seg as PrintMovement).FilamentRetract = 1.0;
                    (seg as PrintMovement).FilamentFeed = 1.0; // 2.00; // mm
                    (seg as PrintMovement).speed = 4000;
                }
                else
                {
                    // Others: PrintSegment
                    seg = new PrintSegment();
                    (seg as PrintSegment).p = p;
                    (seg as PrintSegment).thickness = thickness;
                    (seg as PrintSegment).speed = speed;
                    (seg as PrintSegment).HeightFactor = HeightFactor;
                    (seg as PrintSegment).AllowsGroundOverride = AllowsGroundOverride;
                }
                path.Segments.Add(seg);
            }
            Paths.Add(path);
        }

        public void AppendAsPath(Polyline pl, double speed)
        {

        }

        public void AppendAsPath(Polyline pl, double speed0, double speed1)
        {

        }

        public delegate double SpeedDelegate(Point3d p, double len);

        public PrintPath AppendAsPath(Polyline pl, SpeedDelegate speedDel)
        {
            if (pl == null || pl.Count < 2) return null;

            PrintPath path = new PrintPath();

            double length = 0.0;
            Point3d p0 = pl[0];
            Point3d lastP = p0;

            foreach(Point3d p in pl)
            {
                PrintSegment seg = new PrintSegment();

                length += p.DistanceTo(lastP);

                seg.p = p;
                seg.speed = speedDel(p, length);

                lastP = p;


                path.Segments.Add(seg);
            }

            return path;
        }

        public delegate PrintInstruction SegDelegate(PrintPath path, Point3d prevP, Point3d p, double len);

        public PrintPath AppendAsPath(Polyline pl, SegDelegate segmentProvider)
        {
            if (pl == null || pl.Count < 2) return null;

            PrintPath path = new PrintPath();

            double length = 0.0;
            Point3d p0 = pl[0];
            Point3d lastP = p0;

            foreach (Point3d p in pl)
            {
        //        p.Z += zBase;
                length += p.DistanceTo(lastP);
                PrintInstruction seg = segmentProvider(path, lastP, p, length);

                if (seg != null)
                {
                    path.Segments.Add(seg);
                }

                lastP = p;
            }

            Paths.Add(path);
            return path;
        }

        public void AppendAsPath(List<Point3d> pt)
        {
            Polyline pl = new Polyline();
            AppendAsPath(pl, (p, len) => {
                return len;
            });

            AppendAsPath(pl, (path, lastP, p, len) => {
                PrintInstruction s = null;

               // path.Segments.Add(new PrintMovement());
                if (p.X<0.0)
                {
                    s = new PrintWaveSegment();
                    (s as PrintWaveSegment).speed = p.Y;
                }
                else
                {
                    s = new PrintSegment();
                    (s as PrintSegment).speed = p.Y*0.5;
                }

                return s;
            });
        }

        public void AppendAsPath(List<Line> pt)
        {

        }

        public void AppendAndSlice(object o)
        {
            if (o is Mesh)
            {
                Mesh m = o as Mesh;
                // Curve [] c=Mesh.CreateContourCurves(m, )
            }
            else if (o is Curve)
            {

            }
            else if (o is Line)
            {

            }
        }
        */

        public void GenerateGCodeOpen(StringBuilder s, V2GState printer)
        {
            // GCode: Start
            // - Header.
            // - Model-specific initialization.

            // If left as "0.1.*" the version number will fill the Build and Revision parameters automatically.
            // Build will be the number is the number of days since the year 2000
            // Revision will be the number of seconds since midnight (divided by 2)

            Version versionInfo = Assembly.GetExecutingAssembly().GetName().Version;
            String versionStr = String.Format("{0}.{1}.{2}.{3}", versionInfo.Major.ToString(), versionInfo.Minor.ToString(), (versionInfo.Build - 6082).ToString(), versionInfo.Revision.ToString());

            // Header
            s.Append("\n; Voxel2GCodeLib " + versionStr);
            s.Append("\n; Material Gradients with Monolith");
            s.Append("\n; Autodesk Generative Design");
            s.Append("\n; Nono Martínez Alonso (www.nono.ma)");
            s.Append("\n; G-code generated on " + DateTime.Now.ToString("MMMM dd, yyyy (hh:mm:ss)"));
            s.Append("\n");

            // Printer settings.
            //s.Append("\n; Filament diameter: " + printer.settings.FilamentThickness + " mm");
            //s.Append("\n; Layer height: " + printer.settings.LayerHeight + " mm");

            // Warm head up
            if(printer.settings.ShouldHeatUpOnStart)
            {
                s.Append("\n\n; Heat Up On Start");
                s.Append("\nT0");
                s.Append("\nM104 S" + printer.settings.T0Temperature + "; set temp for extruder 0 do not wait");
                s.Append("\nT1");
                s.Append("\nM104 S" + printer.settings.T1Temperature + "; set temp for extruder 1 do not wait");
                s.Append("\nM140 S" + printer.settings.BedTemperature + "; set temp for bed do not wait");
                s.Append("\nM190 S" + printer.settings.BedTemperature + "; wait for bed to reach temp");
                s.Append("\nT0");
                s.Append("\nM109 S" + printer.settings.T0Temperature + "; wait for extruder 0 to reach temp");
                s.Append("\nT1");
                s.Append("\nM109 S" + printer.settings.T1Temperature + "; wait for extruder 1 to reach temp");
            }

            // Initialization.
            s.Append("\n\n; Initialization");
            s.Append("\nG91");
            s.Append("\nG1 Z1.000 F200.000");
            s.Append("\nG90");
            s.Append("\nG28 X0.000 Y0.000");
            printer.SetPosition(printer.settings.StartPoint);
            s.Append("\nG1 X" + printer.Position.X + " Y" + printer.Position.Y + " F8000.000"); // Start point
            if (printer.settings.IsVerbose) s.Append(" (Start Point) (Z" + printer.Position.Z + ")");
            s.Append("\nG28 Z0.000"); //  G28 Returns to machine's reference position, the "zero position"
            s.Append("\nT1");
            s.Append("\nG92 E0.00000");
            s.Append("\nT0");
            s.Append("\nG92 E0.00000");
            s.Append("\n");
        }

        public void GenerateGCodeClose(StringBuilder s, V2GState printer)
        {
            // GCode: End
            // - Move tool-head backwards
            // - Cool down

            // Move tool-head backwards.
            s.Append("\n\n; Move tool-head to end point and 15 mm up.");
            s.Append("\nG91");
            s.Append("\nG1 E-3.00000 F1800.000");
            s.Append("\nG90"); // absolute positioning
            printer.Position.Z += 0.5;
            s.Append("\nG1 Z" + Math.Round(printer.Position.Z, 4));
            s.Append("\nG1 X" + Math.Round(printer.settings.EndPoint.X, 4) +
                         " Y" + Math.Round(printer.settings.EndPoint.Y, 4) +
                         " Z" + Math.Round(printer.settings.EndPoint.Z, 4) +
                         " F3400.000");
            printer.Position = printer.settings.EndPoint;
            s.Append("\nG91"); // incremental positioning
            s.Append("\nG1 Z15");

            // Cool down.
            if (printer.settings.ShouldCoolDownOnEnd)
            {
                s.Append("\n\n;Cool down");
                s.Append("\nT0;");
                s.Append("\nM104 S0; extruder 0 heater off");
                s.Append("\nT1;");
                s.Append("\nM104 S0; extruder 1 heater off");
                s.Append("\nM140 S0; bed heater off");
                //s.Append("\nM106 S0; wait until extruder off");
            }

            // TODO: Add end instructions
            // The following is an example
            // from the Cura settings file
            // for the ZMorph 2.0
            /*
            s.Append("\n\nG91; relative positioning");
            s.Append("\nG1 E-1 F300; retract the filament a bit before lifting the nozzle, to release some of the pressure");
            s.Append("\nG1 Z+0.5 E-5 X-20 Y-20 F1200; move Z up a bit and retract filament even more");
            s.Append("\nG28 X0 Y0; move X/ Y to min endstops, so the head is out of the way");
            s.Append("\nM84; steppers off");
            s.Append("\nG90; absolute positioning");
            */
        }

        public void GenerateGCode(StringBuilder s, V2GState printer)
        {
            // GCode: Open
            this.GenerateGCodeOpen(s, printer);

            // GCode: Body (Slices & paths)
            s.Append("\n; PrintModel.");

            /*foreach (PrintSlice sl in Slices)
            {
                sl.GenerateGCode(s, printer);
            }*/

            foreach (V2GPath p in Paths)
            {
                p.GenerateGCode(s, printer);
            }

            // GCode: Close
            this.GenerateGCodeClose(s, printer);
        }

    }


}
