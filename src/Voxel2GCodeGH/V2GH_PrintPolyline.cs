using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeGH
{
  public class V2GH_PrintPolyline : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public V2GH_PrintPolyline()
      : base("Construct PrintPolyline", "PrintPolyline",
          "Construct a PrintPolyline from a Polyline with printing properties.",
          "V2G", "G-code")
    {
          
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
            pManager.AddCurveParameter("Polyline", "Polyline",
                "Polyline to print.",
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
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            
            pManager.AddGenericParameter("PrintPolyline", "PrintPolyline", "Printable polyline wrapper.", GH_ParamAccess.list);
            pManager.AddCurveParameter("Polyline", "Polyline", "Filtered list of Polylines.", GH_ParamAccess.list);
            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
    {
            List<Curve> curves = new List<Curve>();
            List<Polyline> polylines = new List<Polyline>();
            List<V2GPrintPolyline> printPolylines = new List<V2GPrintPolyline>();
            List<Point3d> points = new List<Point3d>();

            List<double> MaterialAmount = new List<double>();
            List<double> Speed = new List<double>();
            List<int> Toolhead = new List<int>();
            List<double> MixPercentage = new List<double>();

            if (!DA.GetDataList(0, curves)) return;
            if (!DA.GetDataList(1, MaterialAmount)) return;
            if (!DA.GetDataList(2, Speed)) return;
            if (!DA.GetDataList(3, Toolhead)) return;
            if (!DA.GetDataList(4, MixPercentage)) return;

            foreach (Curve c in curves)
            {
                if(c.IsPolyline())
                {
                    Polyline pl;
                    if (c.TryGetPolyline(out pl))
                    {
                        polylines.Add(pl);
                        /*foreach (Point3d p in pl)
                        {
                            points.Add(p);
                        }*/
                    }
                }
            }

            int idx = 0;
            double _Speed = Speed[0];
            double _MaterialAmount = MaterialAmount[0];
            int _Head = Toolhead[0];
            double _MixPercentage = MixPercentage[0];

            foreach (Polyline pl in polylines)
            {
                if (Speed.Count - 1 >= idx) _Speed = Speed[idx];
                if (MaterialAmount.Count - 1 >= idx) _MaterialAmount = MaterialAmount[idx];
                if (Toolhead.Count - 1 >= idx) _Head = Toolhead[idx];
                if (MixPercentage.Count - 1 >= idx) _MixPercentage = MixPercentage[idx];

                V2GPrintPolyline ppl = new V2GPrintPolyline();
                foreach(Point3d p in pl)
                {
                    ppl.AddPrintPosition(new V2GPoint(p.X, p.Y, p.Z), _Speed, _MaterialAmount, _Head, _MixPercentage);
                }
                printPolylines.Add(ppl);
                idx++;
            }
        
            DA.SetDataList(0, printPolylines);
            DA.SetDataList(1, polylines);
        }
        

    /// <summary>
    /// The Exposure property controls where in the panel a component icon 
    /// will appear. There are seven possible locations (primary to septenary), 
    /// each of which can be combined with the GH_Exposure.obscure flag, which 
    /// ensures the component will only be visible on panel dropdowns.
    /// </summary>
    public override GH_Exposure Exposure
    {
      get { return GH_Exposure.primary; }
    }

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// </summary>
    protected override System.Drawing.Bitmap Icon
    {
      get
      { 
        // You can add image files to your project resources and access them like this:
        //return Resources.IconForThisComponent;
        return null;
      }
    }

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("{b6739c27-3de6-4a1a-a4b5-2e6e2eecec12}"); }
    }
  }
}
