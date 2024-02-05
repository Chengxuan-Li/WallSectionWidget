using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace WallSectionWidget
{
    public class WallSectionWidgetInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "WallSectionWidget";
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
                return "Wall section widget containing a wall-section modeler, material presets, and visualiser";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("5dfde172-f5b7-4943-a8b6-345478caa0c6");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Chengxuan Li";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return @"Chengxuan.Li@aaschool.ac.uk";
            }
        }
    }
}
