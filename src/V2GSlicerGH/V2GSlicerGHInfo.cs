using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace V2GSlicerGH
{
    public class V2GSlicerGHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "NSlicerGH";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("64611e22-1089-4441-b1af-597ff0eb4392");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
