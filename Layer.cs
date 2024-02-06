using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallSectionWidget
{
    public class Layer
    {
        public Material Material;
        public double Thickness { get; private set; }
        public double InteriorSideDepthFromSurface;
        public double InteriorTemperature;
        public double TemperatureDifference;
        public double InteriorVapourPressure;
        public double VapourPressureDifference;

        public GHIOParam<Layer> GHIOParam => new GHIOParam<Layer>(this);

        public Layer(Material material, double thickness)
        {
            Material = material;
            Thickness = thickness;
            InteriorSideDepthFromSurface = 0.0;
            InteriorTemperature = 15.0;
            TemperatureDifference = -5.0;
            InteriorVapourPressure = 2000;
            VapourPressureDifference = -300;
        }

        public double Conductivity
        {
            get { return Material.Conductivity; }
        }

        public double Resistivity
        {
            get { return Thickness / Material.Conductivity; }
        }

        public double VapourResistivity
        {
            get { return Material.VapourResistivity * Thickness; }
        }

        public double ExteriorTemperature
        {
            get { return InteriorTemperature + TemperatureDifference; }
        }

        public double ExteriorVapourPressure
        {
            get { return InteriorVapourPressure + VapourPressureDifference; }
        }

        public double InteriorHumidity
        {
            get { return Psychrometrics.RelativeHumidity(InteriorTemperature, InteriorVapourPressure); }
        }

        public double ExteriorHumidity
        {
            get { return Psychrometrics.RelativeHumidity(ExteriorTemperature, ExteriorVapourPressure); }
        }

        public IntermediateConditions IntermediateConditions
        {
            get { return new IntermediateConditions(this); }
        }

        public double ExteriorSideDepthFromSurface
        {
            get { return InteriorSideDepthFromSurface + Thickness; }
        }
    }

}
