using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxel2GCodeCore.Geometry
{
    public abstract class V2GPrintable
    {
        /// <summary>
        /// Determines wether the printable movement is relative or absolute.
        /// </summary>
        public bool IsRelative = false;
        public virtual void GenerateInstructions(V2GModel model)
        {
        }
    }

}
