using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeCore;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeGH
{
  public class V2GH_PrintSettings : GH_Component
  {
    /// <summary>
    /// Initializes a new instance of the V2GH_PrintSettings class.
    /// </summary>
    public V2GH_PrintSettings()
      : base("Construct Printer Settings", "V2GSettings",
          "Create custom printing settings for a V2GPrinter",
          "V2G", "G-code")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
            /*
        /// <param name="StartPoint">(true by default)</param>
        /// <param name="EndPoint">(true by default)</param>
        /// <param name="ShouldHeatUpOnStart">(true by default)</param>
        /// <param name="ShouldCoolDownOnEnd">(true by default)</param>
        /// <param name="ZOffset">(true by default)</param>
        /// <param name="BedTemperature">(true by default)</param>
        /// <param name="T0Temperature">(true by default)</param>
        /// <param name="T1Temperature">(true by default)</param>
        /// <returns name="PrintSettings">Printing settings.</returns>*/

            pManager.AddPointParameter("StartPoint", "StartPoint",
                "Start point for the print job.",
                GH_ParamAccess.item, new Point3d(25,25,0));
            pManager.AddPointParameter("EndPoint", "EndPoint",
                "End point for the print job.", 
                GH_ParamAccess.item, new Point3d(25,25,0));
            pManager.AddBooleanParameter("ShouldHeatUpOnStart", "ShouldHeatUpOnStart",
                "Determine if the printer should heat up at the beginning of the print.",
                GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("ShouldCoolDownOnEnd", "ShouldCoolDownOnEnd",
                "Determine if the printer should cool down at the end of the print.",
                GH_ParamAccess.item, true);
            pManager.AddNumberParameter("ZOffset", "ZOffset",
                "Offset from the bed to the extrusion in mm.", 
                GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("BedTemperature", "BedTemperature",
                "Temperature of the heated bed (if your printer has one) in celsius degrees.",
                GH_ParamAccess.item, 60.0);
            pManager.AddNumberParameter("T0Temperature", "T0Temperature",
                "Temperature of the primary extruder head in celsius degrees.",
                GH_ParamAccess.item, 200.0);
            pManager.AddNumberParameter("T1Temperature", "T1Temperature",
                "Temperature of the secondary extruder head in celsius degrees.",
                GH_ParamAccess.item, 200.0);
            pManager.AddBooleanParameter("Verbose", "Verbose",
                "Generate G-code with inline comments.",
                GH_ParamAccess.item, false);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
            pManager.AddGenericParameter("Settings", "Settings",
                "Custom print settings for a V2GPrinter.",
                GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d StartPoint = new Point3d();
            Point3d EndPoint = new Point3d();
            bool ShouldHeatUpOnStart = false;
            bool ShouldCoolDownOnEnd = false;
            double ZOffset = 0.2;
            double BedTemperature = 60.0;
            double T0Temperature = 200.0;
            double T1Temperature = 200.0;
            bool IsVerbose = false;

            DA.GetData(0, ref StartPoint);
            DA.GetData(1, ref EndPoint);
            DA.GetData(2, ref ShouldHeatUpOnStart);
            DA.GetData(3, ref ShouldCoolDownOnEnd);
            DA.GetData(4, ref ZOffset);
            DA.GetData(5, ref BedTemperature);
            DA.GetData(6, ref T0Temperature);
            DA.GetData(7, ref T1Temperature);
            DA.GetData(8, ref IsVerbose);

            V2GSettings settings = new V2GSettings();
            settings.StartPoint = new V2GPrintPosition(V2GH.V2GPoint(StartPoint));
            settings.EndPoint = new V2GPrintPosition(V2GH.V2GPoint(EndPoint));
            settings.ShouldHeatUpOnStart = ShouldHeatUpOnStart;
            settings.ShouldCoolDownOnEnd = ShouldCoolDownOnEnd;
            settings.ZOffset = ZOffset;
            settings.BedTemperature = BedTemperature;
            settings.T0Temperature = T0Temperature;
            settings.T1Temperature = T1Temperature;
            settings.IsVerbose = IsVerbose;

            DA.SetData(0, settings);
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
      get { return new Guid("{b78c03ff-9442-4acf-9c34-30460cbe7ade}"); }
    }
  }
}