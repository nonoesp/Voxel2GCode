using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using System.IO;

namespace Voxel2GCodeGH
{
    public class V2GH_Export : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the V2GExport class.
        /// </summary>
        public V2GH_Export()
          : base("Export G-code", "Export",
              "Write a string of text to file",
              "V2G", "G-code")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "Text", "String to write to file.", GH_ParamAccess.item, "Intentionally left blank.");
            pManager.AddTextParameter("FilePath", "FilePath", "Path to export the file to.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Wether should be exporting or not.", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Variables
            string Text = "";
            string FilePath = "";
            bool IsActive = false;

            // Get Data
            DA.GetData(0, ref Text);
            DA.GetData(1, ref FilePath);
            DA.GetData(2, ref IsActive);

            if (IsActive)
            {
                using (StreamWriter outputFile = new StreamWriter(FilePath))
                {
                    outputFile.Write(Text);
                }
            }
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
            get { return new Guid("{3a1ae457-ca4b-4888-b329-fed85d306455}"); }
        }
    }
}