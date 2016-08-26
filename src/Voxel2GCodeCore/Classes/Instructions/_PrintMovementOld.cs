using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeCore.Instructions
{

    /// <summary>
    ///  A PrintInstruction subclass to move the tool-head without printing.
    /// </summary>

    public class PrintMovementOld : V2GInstruction
    {
        public V2GPrintPosition p;
        public double speed = 700.0;
        public double thickness;
        public double zDelta = 0.0;
        public double FilamentRetract = 0.0; // mm
        public double FilamentFeed = 0.0;//mm
        public bool IsRelative = false;
        public bool ForceFilamentOperations = false;

        public void SetRelativePoint(V2GState printer)
        {
            this.p = printer.Position + this.p;
            if (this.p.Z <= 0.0) this.p.Z = 0.05;
        }

        public override void GenerateGCode(StringBuilder s, V2GState printer)
        {
            double MovementLength = printer.Position.DistanceTo(printer.PrintPointOnBase(this.p));

            if (MovementLength > 0.001)
            {
                if (this.IsRelative)
                {
                    this.SetRelativePoint(printer);
                }

                if (printer.settings.IsVerbose) s.Append("\n(A. PrintMovement) (Printer has to move)");

                printer.F = this.speed;

                // Filament retract
                if (this.FilamentRetract > 0.0 && (MovementLength > 2.0 || ForceFilamentOperations))
                {
                    printer.E += -this.FilamentRetract;
                    s.Append("\nG1 E" + Math.Round(printer.E, 4));
                }

                // Add zDelta
                if (zDelta != 0)
                {
                    printer.Position.Z += zDelta;
                    s.Append("\nG1 Z" + Math.Round(printer.Position.Z, 4)); if (printer.settings.IsVerbose) s.Append(" (B. PrintMovement) (move zDelta " + zDelta + ")");
                }

                // Move
                // todo: rollback and forth
                printer.SetPosition(this.p + new V2GPrintPosition(0, 0, zDelta));
                s.Append("\nG1");
                s.Append(" X" + Math.Round(printer.Position.X, 4));
                s.Append(" Y" + Math.Round(printer.Position.Y, 4));
                s.Append(" Z" + Math.Round(printer.Position.Z, 4));
                s.Append(" F" + Math.Round(printer.F, 4));

                if (printer.settings.IsVerbose) s.Append(" (C. PrintMovement) (move towards point with zDelta " + zDelta + ")");

                // Substract zDelta
                if (zDelta != 0)
                {
                    printer.Position.Z -= zDelta;
                    if (printer.Position.Z < 0) printer.Position.Z = 0.0;
                    s.Append("\nG1 Z" + Math.Round(printer.Position.Z, 4));
                    //if (printer.IsVerbose) s.Append(" (D. PrintMovement) (move zDelta " + zDelta + ")");
                }

                // Filament feed (while moving)

                if (this.FilamentFeed > 0.0 && (MovementLength > 2.0 || ForceFilamentOperations)) // todo: check if this makes PLA behavior worse (length restriction wasn't here, just testing for NinjaFlex not to go out of the extruder)
                {
                    if (zDelta == 0)
                    {
                        s.Append("\nG1");
                    }
                    printer.E += this.FilamentFeed;
                    s.Append(" E" + Math.Round(printer.E, 4));
                }
            }
            else
            {
                if (printer.settings.IsVerbose) s.Append("\n (A. PrintMovement) (Printer is already here)");
            }
        }
    }
}
