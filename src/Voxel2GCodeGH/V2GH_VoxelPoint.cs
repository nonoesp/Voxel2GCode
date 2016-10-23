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
    public class V2GH_VoxelPoint : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the V2GVoxelPoint class.
        /// </summary>
        public V2GH_VoxelPoint()
          : base("Create VoxelPoint", "VoxelPoint",
              "VoxelPoint from a Monolith voxel field and a Point3d.",
              "V2G", "Voxel")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Voxel", "Voxel", "Monolith voxel field.", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "Point", "Point3d.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("VoxelPoint", "VoxelPoint", "VoxelPoint with metadata of the Voxel field.", GH_ParamAccess.item);
            pManager.AddPointParameter("Position", "Position", "Position of the point.", GH_ParamAccess.item);
            pManager.AddNumberParameter("FieldValue", "FieldValue", "Value of the field at this point.", GH_ParamAccess.item);
            pManager.AddVectorParameter("ContourVector", "ContourVector", "Vector along iso-curves (projected to the z coordinate of the point).", GH_ParamAccess.item);
            pManager.AddVectorParameter("GradientVector", "GradientVector", "Vector along gradient curves (projected to the z coordinate of the point)s.", GH_ParamAccess.item);
            pManager.AddVectorParameter("ContourVector3d", "ContourVector3", "Vector3d along the gradient of the field.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            VoxelImage VoxelImage = null;
            Point3d Point = new Point3d();

            DA.GetData(0, ref VoxelImage);
            DA.GetData(1, ref Point);

            V2GVoxelPoint VoxelPoint = new V2GVoxelPoint(VoxelImage, V2GH.V2GPoint(Point));

            DA.SetData(0, VoxelPoint);
            DA.SetData(1, V2GH.Point3d(VoxelPoint.Position));
            DA.SetData(2, VoxelPoint.FieldValue);
            DA.SetData(3, V2GH.Vector3d(VoxelPoint.ContourVector));
            DA.SetData(4, V2GH.Vector3d(VoxelPoint.GradientVector));
            DA.SetData(5, V2GH.Vector3d(VoxelPoint.ContourVector3d));
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
            get { return new Guid("{9fbd4d7e-be65-427b-aecb-d97df9666184}"); }
        }
    }
}