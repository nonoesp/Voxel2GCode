using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeRhino;
using Voxel2GCodeRhino.Utilities;

namespace Voxel2GCodeGH
{
  public class V2GH_SlicePlanes : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the V2GH_SlicePlanes class.
    /// </summary>
    public V2GH_SlicePlanes()
          : base("Get Slice Planes", "SlicePlanes",
              "Get z planes to slice a given geometry.",
              "V2G", "Slicer")
        {
    }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "Geometry", "Geometry to slice.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Layer Height", "Layer Height", "Distance between z planes.", GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("Global Offset", "Global Offset", "Offset distance applied to all layers.", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Bottom Layer Offset", "Bottom Layer Offset", "Bottom layer offset distance.", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Top Layer Offset", "Bottom Layer Offset", "Top layer offset distance.", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("Force Top Layer", "Force Top Layer", "Ensure there is a layer at the top. (0 or 1; True or False.)", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Planes", "Planes", "Z planes.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Plane> SlicePlanes = new List<Plane>();

            GeometryBase Geometry = null;
            double LayerHeight = 0.2;
            double OffsetBottomGlobal = 0;
            double OffsetBottom = 0;
            double OffsetTop = 0;

            if (!DA.GetData(0, ref Geometry)) return;
            DA.GetData(1, ref LayerHeight);
            DA.GetData(2, ref OffsetBottomGlobal);
            DA.GetData(3, ref OffsetBottom);
            DA.GetData(4, ref OffsetTop);

            SlicePlanes = V2GRhinoGeometry.SlicePlanes(Geometry, LayerHeight, OffsetBottomGlobal, OffsetBottom, OffsetTop);

            DA.SetDataList(0, SlicePlanes);
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
      get { return new Guid("{f3820b46-1ceb-4536-b1b4-5fbf4eae7bc8}"); }
    }
  }
}