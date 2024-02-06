using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;

namespace WallSectionWidget
{
    public class MaterialVisualiserSetting
    {
        public double UnitScale => 1.0; // RhinoMath.UnitScale(RhinoDoc.ActiveDoc.ModelUnitSystem, UnitSystem.Meters);
        public int HatchPatternIndex = 1;
        public double HatchPatternScale = 0.1;
        public double HatchPatternRotation = 0.0;
        public double actualHatchPatternScale => HatchPatternScale * UnitScale;
        
        public Color Color = Color.Black;

        public static MaterialVisualiserSetting Default => new MaterialVisualiserSetting();
        public GHIOParam<MaterialVisualiserSetting> GHIOParam => new GHIOParam<MaterialVisualiserSetting>(this);
    }
}
