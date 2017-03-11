using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeCore
{
    /// <summary>
    /// A class to hold 3D printing settings.
    /// </summary>
    public class V2GSettings
    {
        /// <summary>
        /// Layer height in mm.
        /// </summary>
        public double LayerHeight = 0.2;

        /// <summary>
        /// Default feed rate of the printer in mm/min.
        /// </summary>
        public double Speed = 700.0;

        /// <summary>
        /// Default retraction length for retraction operations of the filament(s) in mm.
        /// </summary>
        public double Retraction_Length = 0.2;

        /// <summary>
        /// Temperature of the heated bed (if your printer has one) in celsius degrees.
        /// </summary>
        public double BedTemperature = 60.0;

        /// <summary>
        /// Temperature of the primary extruder head in celsius degrees.
        /// </summary>
        public double T0Temperature = 200.0;

        /// <summary>
        /// Temperature of the secondary extruder head in celsius degrees.
        /// </summary>
        public double T1Temperature = 200.0;

        /// <summary>
        /// Diameter of the printing filament in mm.
        /// </summary>
        public double FilamentThickness = 1.75;

        /// <summary>
        /// Determine if the printer should heat up at the beginning of the print.
        /// </summary>
        public bool ShouldHeatUpOnStart = true;

        /// <summary>
        /// Determine if the printer should cool down at the end of the print.
        /// </summary>
        public bool ShouldCoolDownOnEnd = true;

        /// <summary>
        /// Offset from the bed to the extrusion in mm.
        /// </summary>
        public double ZOffset = 0.0;

        /// <summary>
        /// Printer start point.
        /// </summary>
        public V2GPrintPosition StartPoint = new V2GPrintPosition(25.0, 25.0, 0.0);

        /// <summary>
        /// Printer end point.
        /// </summary>
        public V2GPrintPosition EndPoint = new V2GPrintPosition(25.0, 25.0, 0.0);

        /// <summary>
        /// Determines wether the generated G-code should contain detailed comments or not.
        /// </summary>
        public bool IsVerbose = false;

        /// <summary>
        /// Constructor of PrintSettings.
        /// </summary>
        public V2GSettings()
        {
            ///
        }
    }
}
