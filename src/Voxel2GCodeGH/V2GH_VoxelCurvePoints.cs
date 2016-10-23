using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeCore;
using Voxel2GCodeCore.Geometry;
using Voxel2GCodeCore.Utilities;
// Monolith
using MalachiteNet;
using MonolithLib;
using CGeometrica;
using CSerializer;

namespace Voxel2GCodeGH
{
    public class V2GH_VoxelCurvePoints : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the V2GVoxelCurvePoints class.
        /// </summary>
        public V2GH_VoxelCurvePoints()
          : base("Get Voxel Curve Points", "VoxelCurvePoints",
              "Get voxel gradient and contour curve points from a Monolith voxel field.",
              "V2G", "Voxel")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Voxel", "Voxel", "Monolith voxel field.", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "Point", "Point3d.", GH_ParamAccess.item, new Point3d(0,0,0));
            pManager.AddIntegerParameter("Vector Type", "Vector Type", "(0) Contour vector projected to z plane (1) Gradient vector projected to z plane (2) 3D Gradient vector", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Segment Length", "SegmentLength", "Length of each of the segments.", GH_ParamAccess.item, 0.0001);
            pManager.AddIntegerParameter("Iterations", "Iterations", "Maximum number of iterations to find an end point.", GH_ParamAccess.item, 10000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "Polyline", "Field polyline.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            VoxelImage VoxelImage = null;
            Point3d Point = new Point3d();
            int VectorType = 0;
            double SegmentLength = 0.0001;
            int Iterations = 25000;

            if(!DA.GetData(0, ref VoxelImage)) return;
            DA.GetData(1, ref Point);
            DA.GetData(2, ref VectorType);
            DA.GetData(3, ref SegmentLength);
            DA.GetData(4, ref Iterations);
            
            List<V2GPoint> Points = V2GVoxel.VoxelCurvePoints(VoxelImage, V2GH.V2GPoint(Point), SegmentLength, Iterations, VectorType);
            List<Point3d> RhinoPoints = new List<Point3d>();
            foreach(V2GPoint p in Points)
            {
                RhinoPoints.Add(new Point3d(p.X, p.Y, p.Z));
            }
            Polyline Polyline = new Polyline(RhinoPoints);

            DA.SetData(0, Polyline);
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
            get { return new Guid("{648cd942-f2e4-4085-9664-e662f83fedca}"); }
        }
    }
}