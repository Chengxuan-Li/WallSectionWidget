using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WallSectionWidget
{
    public class Material
    {
        public double Conductivity;
        public double VapourResistivity;
        public double Density;
        public double HeatCapacity;
        public string Name;
        public MaterialVisualiserSetting VisualiserSetting;

        public GHIOParam<Material> GHIOParam => new GHIOParam<Material>(this);

        public static Material Air
        {
            get
            {
                return new Material
                {
                    Conductivity = 0.025,
                    VapourResistivity = 1.0,
                    Density = 1.2,
                    HeatCapacity = 1000,
                    Name = "Ordinary air",
                    VisualiserSetting = new MaterialVisualiserSetting {
                        HatchPatternIndex = -1,
                        Color = Color.White,
                    },
                };
            }
        }

        public static Material Concrete
        {
            get
            {
                return new Material
                {
                    Conductivity = 2.0,
                    VapourResistivity = 80,
                    Density = 2400,
                    HeatCapacity = 900,
                    Name = "Concrete",
                    VisualiserSetting = new MaterialVisualiserSetting
                    {
                        HatchPatternIndex = 1,
                        HatchPatternRotation = Math.PI * 0.25,
                        Color = Color.DarkGray,
                    },
                };
            }
        }

        public static Material Cork
        {
            get
            {
                return new Material
                {
                    Conductivity = 0.05,
                    VapourResistivity = 5,
                    Density = 160,
                    HeatCapacity = 1800,
                    Name = "Cork",
                    VisualiserSetting = new MaterialVisualiserSetting
                    {
                        HatchPatternIndex = 5,
                        Color = Color.Bisque,
                    },
                };
            }
        }

        public static Material Lamination
        {
            get
            {
                return new Material
                {
                    Conductivity = 0.13,
                    VapourResistivity = 30,
                    Density = 500,
                    HeatCapacity = 1600,
                    Name = "Lamination",
                    VisualiserSetting = new MaterialVisualiserSetting
                    {
                        HatchPatternIndex = 2,
                        HatchPatternRotation = Math.PI * 0.5,
                        Color = Color.BurlyWood,
                    },
                };
            }
        }

        public static Material VapourRetarder
        {
            get
            {
                return new Material
                {
                    Conductivity = 0.25,
                    VapourResistivity = 70000,
                    Density = 1230,
                    HeatCapacity = 10000,
                    Name = "Vapour retarder",
                    VisualiserSetting = new MaterialVisualiserSetting
                    {
                        HatchPatternIndex = 0,
                        Color = Color.DodgerBlue,
                    },
                };
            }
        }

    }
}
