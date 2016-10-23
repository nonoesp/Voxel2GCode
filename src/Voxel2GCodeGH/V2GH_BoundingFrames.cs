using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Voxel2GCodeCore.Utilities;
using Voxel2GCodeRhino.Utilities;
using Rhino.Geometry;

namespace Voxel2GCodeGH
{
    public class V2GH_BoundingFrames : GH_Component
    {
        // TODO: Fix glitches at some parameters

        /// <summary>
        /// Initializes a new instance of the V2GUtilBoundingFrames class.
        /// </summary>
        public V2GH_BoundingFrames()
          : base("Get Bounding Frames", "BoundingFrames",
              "Get a set of bounding frames for a given geometry.",
              "V2G", "Slicer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "Geometry", "Geometry to extract frames from.", GH_ParamAccess.item);
            pManager.AddNumberParameter("X Span", "X Span", "Span between x-axis frames.", GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("Y Span", "Y Span", "Span between y-axis frames.", GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("Z Span", "Z Span", "Span between z-axis frames.", GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("DX Span", "DX Span", "Span between diagonal-x-axis frames.", GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("DY Span", "DY Span", "Span between diagonal-y-axis frames.", GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("Diagonal Angle", "Angle", "Angle of rotation of diagonal frames.", GH_ParamAccess.item, 45.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("X Frames", "X Frames", "Frames on the x-axis.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Y Frames", "Y Frames", "Frames on the y-axis.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Z Frames", "Z Frames", "Frames on the z-axis.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("DX Frames", "DX Frames", "Frames on the diagonal-x-axis.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("DY Frames", "DY Frames", "Frames on the diagonal-y-axis.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Frames", "Frames", "Lists containing all frames.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryBase Geometry = null;
            double XSpan = 0.2;
            double YSpan = 0.2;
            double ZSpan = 0.2;
            double DXSpan = 0.2;
            double DYSpan = 0.2;
            double Angle = 45.0;
            double minSpan = 0.0001;
            if(!DA.GetData(0, ref Geometry)) return;
            DA.GetData(1, ref XSpan);
            DA.GetData(2, ref YSpan);
            DA.GetData(3, ref ZSpan);
            DA.GetData(4, ref DXSpan);
            DA.GetData(5, ref DYSpan);
            DA.GetData(6, ref Angle);
            
            if (XSpan < minSpan) XSpan = minSpan;
            if (YSpan < minSpan) YSpan = minSpan;
            if (ZSpan < minSpan) ZSpan = minSpan;
            if (DXSpan < minSpan) DXSpan = minSpan;
            if (DXSpan < minSpan) DXSpan = minSpan;

            List<List<Plane>> Frames = V2GRhinoGeometry.BoundingFrames(
              Geometry,
              XSpan,
              YSpan,
              ZSpan,
              DXSpan,
              DYSpan,
              Angle// Optional
              );

            DA.SetDataList(0, Frames[0]);
            DA.SetDataList(1, Frames[1]);
            DA.SetDataList(2, Frames[2]);
            DA.SetDataList(3, Frames[3]);
            DA.SetDataList(4, Frames[4]);
            DA.SetDataList(5, Frames);
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
            get { return new Guid("{0a39c99b-02ca-418b-b22b-f6a0c2f0d69e}"); }
        }
    }
}