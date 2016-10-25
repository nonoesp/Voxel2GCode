using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeCore.Geometry;
using Voxel2GCodeCore;

namespace Voxel2GCodeGH
{
    public class V2GH_Printer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the V2GPrinter class.
        /// </summary>
        public V2GH_Printer()
          : base("Construct Printer", "V2GPrinter",
              "Generates printable G-code from printable geometry objects.",
              "V2G", "G-code")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Printables", "Printables", 
                "A set of printable geometries.", 
                GH_ParamAccess.list);
            pManager.AddGenericParameter("Settings", "Settings",
                "Custom printing settings", 
                GH_ParamAccess.item);
            //pManager.AddBooleanParameter("Verbose", "Verbose", 
            //   "Display G-code comments.", 
            //    GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("GCode", "GCode", "Generated printable GCode.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Variables
            List<V2GPrintable> printables = new List<V2GPrintable>();
            V2GSettings settings = new V2GSettings();

            // Get Data
            DA.GetDataList(0, printables);
            DA.GetData(1, ref settings);
            //if (!DA.GetData(1, ref settings))
            //{
            //   settings = new V2GSettings();
            // }

            // Stuff
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Create Printer and adjust settings
            V2GState printer = new V2GState();
            printer.SetSettings(settings);

            // Create Model
            V2GModel model = new V2GModel();

            // Append PrintPolylines
            foreach (V2GPrintable printable in printables)
            {
                if(printable is V2GPrintPolyline)
                {
                    model.AppendAsPath(printable as Voxel2GCodeCore.Geometry.V2GPrintPolyline);
                } else if (printable is V2GPrintPosition)
                {
                    /// TODO: implement model.AppendAsPath(V2GPrintPosition position) { }
                }
            }

            model.AppendAsRelativeMovement(new V2GPrintPosition(0, 0, 10.0), 0, 7200);

            // Generate GCode
            model.GenerateGCode(sb, printer);
            
            // Set Data
            DA.SetData(0, sb.ToString());
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
            get { return new Guid("{e12a17be-2812-4a4e-8d58-9dcbc2dce6f4}"); }
        }
    }
}