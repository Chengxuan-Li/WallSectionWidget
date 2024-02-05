using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallSectionWidget
{
    public class Layer
    {
        private Material Material;
        public double Thickness { get; private set; }
        public double Xi;
        public double Ti;
        public double Dt;
        public double Pi;
        public double Dp;

        public Layer(Material material, double thickness)
        {
            Material = material;
            Thickness = thickness;
            Xi = 0.0;
            Ti = 15.0;
            Dt = -5.0;
            Pi = 2000;
            Dp = -300;
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

        public double Te
        {
            get { return Ti + Dt; }
        }

        public double Pe
        {
            get { return Pi + Dp; }
        }

        public double Hi
        {
            get { return Psychrometrics.RelativeHumidity(Ti, Pi); }
        }

        public double He
        {
            get { return Psychrometrics.RelativeHumidity(Te, Pe); }
        }

        public IntermediateConditions IntermediateConditions
        {
            get { return new IntermediateConditions(this); }
        }

        public double Xe
        {
            get { return Xi + Thickness; }
        }
    }

}
