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
    ///  A parent class for printing instructions.
    /// </summary>
    public abstract class V2GInstruction
    {
        public virtual void GenerateGCode(StringBuilder s, V2GState printer)
        {

        }
    }
}
