using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeGH
{
    public class V2GH_PrintPolylineData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the V2GPrintPolylineData class.
        /// </summary>
        public V2GH_PrintPolylineData()
          : base("Extract PrintPolyline Data", "PrintPolylineData",
              "Extract the metadata of a PrintPolyline.",
              "V2G", "G-code")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PrintPolyline", "PrintPolyline", "PrintPolyine(s) to extract data from.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "Polyline",
                "Polyline contained inside the PrintPolyline.",
                GH_ParamAccess.list);
            pManager.AddPointParameter("Positions", "Positions",
                "List of print positions for each of the printable polylines.",
                GH_ParamAccess.list);
            pManager.AddNumberParameter("Material per mm", "Material per mm",
                "Amount of filament to extrude per linear mm, i.e. E value, spread between E and A if using dual extruder.",
                GH_ParamAccess.list);
            pManager.AddNumberParameter("Speed", "Speed",
                "Feedrate at which extrude the material.",
                GH_ParamAccess.list);
            pManager.AddIntegerParameter("Tool-head", "Tool-head",
                "Number of the corresponding tool-head to use, e.g. T0 (left extruder), T1 (right extruder), T3 (dual pro extruder mix), etc.",
                GH_ParamAccess.list);
            pManager.AddNumberParameter("Mix Percetage", "Mix Percentage",
                "Amount of secondary material to mix with dual extruder, i.e. 0 for 100%-0%, 0.5 for 50%-50% 1.0 for 0%-100%).",
                GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<V2GPrintPolyline> printPolylines = new List<V2GPrintPolyline>();
            List<List<V2GPrintPosition>> Points = new List<List<V2GPrintPosition>>();

            List<Polyline> RhinoPolyline = new List<Polyline>();
            List<Point3d> RhinoPositions = new List<Point3d>();
            List<double> MaterialAmount = new List<double>();
            List<double> Speed = new List<double>();
            List<int> Head = new List<int>();
            List<double> MixPercentage = new List<double>();

            if (!DA.GetDataList(0, printPolylines)) return;

            foreach(V2GPrintPolyline ppl in printPolylines)
            {
                Points.Add(ppl.PrintPositions);
                List<Point3d> RhinoPoints = new List<Point3d>();
                foreach (V2GPrintPosition PrintPosition in ppl.PrintPositions)
                {
                    Speed.Add(PrintPosition.Speed);
                    MaterialAmount.Add(PrintPosition.MaterialAmount);
                    Head.Add(PrintPosition.Head);
                    MixPercentage.Add(PrintPosition.MixPercentage);
                    RhinoPoints.Add(V2GH.Point3d(PrintPosition.Position));
                }
                RhinoPolyline.Add(new Rhino.Geometry.Polyline(RhinoPoints));
                // TODO: Create a GH_Tree with points
                RhinoPositions.AddRange(RhinoPoints);
            }

            DA.SetDataList(0, RhinoPolyline);
            DA.SetDataList(1, RhinoPositions);
            DA.SetDataList(2, MaterialAmount);
            DA.SetDataList(3, Speed);
            DA.SetDataList(4, Head);
            DA.SetDataList(5, MixPercentage);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{064f93d7-f1b5-453a-bb59-ecc89ad56613}"); }
        }
    }
}