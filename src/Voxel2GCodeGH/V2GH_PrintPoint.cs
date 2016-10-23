using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeGH
{
    public class V2GH_PrintPoint : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the V2GPrintPoint class.
        /// </summary>
        public V2GH_PrintPoint()
          : base("Construct PrintPoint", "PrintPoint",
              "Construct a PrintPoint from a Point3d.",
              "V2G", "G-code")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point",
                "Point to define a printable segment.",
                GH_ParamAccess.list);
            pManager.AddNumberParameter("Material per mm", "Material per mm",
                "Amount of filament to extrude per linear mm, i.e. E value, spread between E and A if using dual extruder.",
                GH_ParamAccess.list, 0.033);
            pManager.AddNumberParameter("Speed", "Speed",
                "Feedrate at which extrude the material.",
                GH_ParamAccess.list, 700.0);
            pManager.AddIntegerParameter("Tool-head", "Tool-head",
                "Number of the corresponding tool-head to use, e.g. T0 (left extruder), T1 (right extruder), T3 (dual pro extruder mix), etc.",
                GH_ParamAccess.list, 0);
            pManager.AddNumberParameter("Mix Percetage", "Mix Percentage",
                "Amount of secondary material to mix with dual extruder, i.e. 0 for 100%-0%, 0.5 for 50%-50% 1.0 for 0%-100%).",
                GH_ParamAccess.list, 0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PrintPoint", "PrintPoint", "Printable point wrapper.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<V2GPrintPosition> PrintPoints = new List<V2GPrintPosition>();
            List<Point3d> InputPoints = new List<Point3d>();
            List<double> MaterialAmount = new List<double>();
            List<double> Speed = new List<double>();
            List<int> Toolhead = new List<int>();
            List<double> MixPercentage = new List<double>();

            if (!DA.GetDataList(0, InputPoints)) return;
            if (!DA.GetDataList(1, MaterialAmount)) return;
            if (!DA.GetDataList(2, Speed)) return;
            if (!DA.GetDataList(3, Toolhead)) return;
            if (!DA.GetDataList(4, MixPercentage)) return;

            int idx = 0;
            double _Speed = Speed[0];
            double _MaterialAmount = MaterialAmount[0];
            int _Head = Toolhead[0];
            double _MixPercentage = MixPercentage[0];
            foreach (Point3d p in InputPoints)
            {
                if (Speed.Count - 1 >= idx) _Speed = Speed[idx];
                if (MaterialAmount.Count - 1 >= idx) _MaterialAmount = MaterialAmount[idx];
                if (Toolhead.Count - 1 >= idx) _Head = Toolhead[idx];
                if (MixPercentage.Count - 1 >= idx) _MixPercentage = MixPercentage[idx];

                V2GPrintPosition position = new V2GPrintPosition(V2GH.V2GPoint(p), _Speed, _MaterialAmount, _Head, _MixPercentage);
                PrintPoints.Add(new V2GPrintPosition(p.X, p.Y, p.Z));
                idx++;
            }

            DA.SetDataList(0, PrintPoints);
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
            get { return new Guid("{ebdbafc2-c9a3-4a0c-aa74-dfb137eb007f}"); }
        }
    }
}