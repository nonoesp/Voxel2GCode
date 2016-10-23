using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeCore;
using Voxel2GCodeCore.Utilities;
using Voxel2GCodeCore.Geometry;
using Voxel2GCodeRhino;

namespace Voxel2GCodeGH
{
    public class V2GH_SortCurves : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the V2GCurveSort class.
        /// </summary>
        public V2GH_SortCurves()
          : base("Sort Curves to Print", "CurveSort",
              "Sort curves on the best path to print.",
              "V2G", "Slicer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "List of curves to sort for printing.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "List of sorted curves for printing.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> curves = new List<Curve>();
            List<Curve> sortedCurves = new List<Curve>();
            if (!DA.GetDataList(0, curves)) return;

            List<V2GCurve> V2GRhinoCurves = new List<V2GCurve>();
            foreach (Curve c in curves)
            {
                V2GRhinoCurves.Add(new V2GRhinoCurve(c));
            }
            List<V2GCurve> vCurves = V2GGeometry.SortCurves(V2GRhinoCurves);
            foreach (V2GCurve c in vCurves)
            {
                sortedCurves.Add((c as V2GRhinoCurve).curve);
            }
            DA.SetDataList(0, sortedCurves);
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
            get { return new Guid("{687c293d-f136-4672-bcb4-9b032f9f958f}"); }
        }
    }
}