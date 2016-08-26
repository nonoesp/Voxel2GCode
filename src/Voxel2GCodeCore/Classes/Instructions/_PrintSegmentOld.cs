using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;
using Voxel2GCodeCore.Utilities;

namespace Voxel2GCodeCore.Instructions
{
    /// <summary>
    ///  A PrintInstruction subclass to print segments.
    /// </summary>
    public class PrintSegmentOld : V2GInstruction
    {
        public V2GPrintPosition p;
        public double speed;
        public double thickness;
        public bool AllowsGroundOverride = true;
        public double HeightFactor = 1.0;

        //public bool IsFirst = false;

        public double Height()
        {
            return V2GPrint.Height(this.thickness, this.HeightFactor);
        }

        public PrintSegmentOld()
        {
            speed = 700;
            thickness = 0.05;
        }

        public double EUnitIncrease()
        {
            return V2GPrint.EUnitIncrease(this.thickness);
        }

        public override void GenerateGCode(StringBuilder s, V2GState printer)
        {
            V2GPrintPosition prevP = printer.Position;

            printer.F = this.speed;
            printer.SetPosition(this.p);
            double length = printer.Position.DistanceTo(prevP);
            double extrusionHeight = this.Height();

            if (printer.Position.Z < 0) printer.Position.Z = 0; // todo: this safes inside the PrintState class

            if (printer.Position.Z < 0.7 && this.AllowsGroundOverride)
            {
                if (this.thickness < 700) this.speed = 400;
                extrusionHeight = 0.25;
            }

            // Locate Z in its place
            s.Append("\nG1");
            s.Append(" Z" + Math.Round((printer.Position.Z + extrusionHeight), 4));

            printer.EPerMM = this.EUnitIncrease();

            // Calculate E increase
            double EUnitIncrease = printer.EPerMM;
            double EIncrease = EUnitIncrease * length;
            printer.E += EIncrease;

            // Extrude
            // - todo: check if coordinates are equal than previous ones
            s.Append("\nG1");
            s.Append(" X" + Math.Round(printer.Position.X, 4));
            s.Append(" Y" + Math.Round(printer.Position.Y, 4));
            s.Append(" Z" + Math.Round((printer.Position.Z + extrusionHeight), 4));
            s.Append(" E" + Math.Round(printer.E, 4));
            s.Append(" F" + Math.Round(printer.F, 4));
            if (printer.settings.IsVerbose) s.Append(" (EUnitIncrease " + EUnitIncrease + ", thickness " + this.thickness + ", height " + extrusionHeight + ")");
            if (printer.settings.IsVerbose) s.Append(" (PrintSegment)");
        }
    }
}
