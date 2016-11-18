using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeCore
{
    /// <summary>
    /// A class to hold the state of a printer.
    /// </summary>
    public class V2GState
    {
        /// <summary>
        /// Contains a PrintSettings object with the printing settings of this printer.
        /// </summary>
        public V2GSettings settings = new V2GSettings();

        /// <summary>
        /// Current position of the tool-head (x, y, z).
        /// </summary>
        public V2GPrintPosition Position;

        /// <summary>
        /// Current amount of material feeded to the primary extruder head of the printer. (e.g. T0, T1, or first head on T3.)
        /// </summary>
        public double E = 0.0;

        /// <summary>
        ///Current amount of material feeded to the secondary extruder head of the printer. (e.g. second head on T3.)
        /// </summary>
        public double A = 0.0;

        /// <summary>
        /// Current feed rate of the printer, i.e. speed, in mm/min.
        /// </summary>
        public double F = 700.0;

        /// <summary>
        /// Current extruder-head being used to print. (e.g. T0 for left extruder, T1 for right extruder, T3 for dual extruder, etc.) This values vary from printer to printer.
        /// </summary>
        public int Head = 0;

        /// <summary>
        ///  Current amount of material .
        /// </summary>
        public double EPerMM = 0.033;

        /// <summary>
        /// Constructor of a printer.
        /// </summary>
        public V2GState() {
            ///
        }

        /// <summary>
        /// Set the 
        /// </summary>
        /// <param name="Settings"></param>
        public void SetSettings(V2GSettings Settings)
        {
            this.settings = Settings;
            this.F = Settings.Speed;
        }
        
        /// <summary>
        /// Set material settings.
        /// </summary>
        /// <param name="Material"></param>

        public void SetMaterial(string Material)
        {
            if(Material.Equals("PLA"))
            {
                this.settings.BedTemperature = 60.0;
                this.settings.T0Temperature = 200.0;
                this.settings.T1Temperature = 200.0;
            }
            else if (Material.Equals("ABS"))
            {
                this.settings.BedTemperature = 60.0;
                this.settings.T0Temperature = 240.0;
                this.settings.T1Temperature = 240.0;
            }
            else if (Material.Equals("NinjaFlex"))
            {
                this.settings.BedTemperature = 80.0;
                this.settings.T0Temperature = 220.0;
                this.settings.T1Temperature = 220.0;
            }
        }

        /// <summary>
        /// Set the active head.
        /// </summary>
        /// <param name="s">StringBuilder to store the generated G-code.</param>
        /// <param name="Head">Integer identifying the printing head. (e.g. 0 for T0, 1 for T1, etc.)</param>
        public void SetHead(StringBuilder s, int Head)
        {
            if (this.Head != Head)
            {
                this.Head = Head;
                this.ResetHead(s);
            }
        }

        /// <summary>
        /// Generate G-code instructions to set the current material feeding position as zero.
        /// </summary>
        /// <param name="s">StringBuilder to store the generated G-code.</param>
        public void ResetHead(StringBuilder s)
        {
            this.E = 0;
            this.A = 0;
            s.Append("\nT" + (this.Head));
            s.Append("\nG92 E0.0");
            if (this.Head == 3) s.Append("\nG92 A0.0"); // e.g. Dual PRO right filament
        }

        /// <summary>
        /// Generate G-code instructions to feed more material along a printing path.
        /// </summary>
        /// <param name="s">StringBuilder to store the generated G-code.</param>
        /// <param name="EPerMM">Amount of material to feed per mm.</param>
        /// <param name="PathLength">Length of the path to extrude along in mm.</param>
        /// <param name="MaterialPercentage">Material percentage of the secondary extruder. (e.g. 0.5 feeds 50% of each when using a dual head.)</param>
        public void IncreaseE(StringBuilder s, double EPerMM, double PathLength, double MaterialPercentage = 0.0)
        {
            double prevA = this.A;
            double prevE = this.E;

            if (this.Head == 3)
            {
                this.E += EPerMM * PathLength * (1.0 - MaterialPercentage);
                this.A += EPerMM * PathLength * MaterialPercentage;
                if (this.E != prevE) s.Append(" E" + Math.Round(this.E,4));
                if (this.A != prevA) s.Append(" A" + Math.Round(this.A,4));
            }
            else
            {
                this.E += EPerMM * PathLength;
                if (this.E != prevE) s.Append(" E" + Math.Round(this.E, 4));
            }
        }

        /// <summary>
        /// Set the position of the printer's extruder head.
        /// </summary>
        /// <param name="p">A PrintPoint.</param>
        public void SetPosition(V2GPrintPosition p, StringBuilder s = null)
        {
            if ( s != null && Math.Abs(this.Position.Z - p.Z + this.settings.ZOffset) > 0.01 )
            {
                if (this.settings.IsVerbose)
                {
                    s.Append("\n(Z Changed)");
                    s.Append(" (from " + this.Position.Z + " to " + p.Z +")");
                }
                this.ResetHead(s);
            }
            this.Position = new V2GPrintPosition(p.X, p.Y, p.Z + this.settings.ZOffset);
        }

        /// <summary>
        /// Transform a PrintPoint from absolute coordinates to printer space coordinates.
        /// </summary>
        /// <param name="p">A PrintPoint.</param>
        /// <returns></returns>
        public V2GPrintPosition PrintPointOnBase(V2GPrintPosition p)
        {
            return new V2GPrintPosition(p.X, p.Y, p.Z + this.settings.ZOffset);
        }
    }
}
