using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallSectionWidget
{
    using System;
    using System.Collections.Generic;

    public class Construction
    {
        public List<Layer> Layers;
        public double Ti;
        public double Te;
        public double Hi;
        public double He;

        public Construction(List<Layer> layers)
        {
            Layers = layers;
            Ti = 20.0;
            Te = 5.0;
            Hi = 50.0;
            He = 80.0;

            UpdateLayers();
        }

        public Construction()
        {
        }

        public double Thickness
        {
            get
            {
                double s = 0;
                foreach (Layer layer in Layers)
                {
                    s += layer.Thickness;
                }
                return s;
            }
        }

        public double Resistance
        {
            get
            {
                double rr = 0;
                foreach (Layer layer in Layers)
                {
                    rr += layer.Resistivity;
                }
                return rr;
            }
        }

        public double VapourResistance
        {
            get
            {
                double rr = 0;
                foreach (Layer layer in Layers)
                {
                    rr += layer.VapourResistivity;
                }
                return rr;
            }
        }

        public double U
        {
            get { return 1 / Resistance; }
        }

        public double DT
        {
            get { return Te - Ti; }
        }

        public double Pi
        {
            get { return Psychrometrics.VapourPressure(Ti, Hi); }
        }

        public double Pe
        {
            get { return Psychrometrics.VapourPressure(Te, He); }
        }

        public double DP
        {
            get { return Pe - Pi; }
        }

        public void UpdateLayers()
        {
            double T = Ti;
            double P = Pi;
            double X = 0.0;
            foreach (Layer layer in Layers)
            {
                layer.Dt = layer.Resistivity / Resistance * DT;
                layer.Ti = T;
                T += layer.Dt;

                layer.Dp = layer.VapourResistivity / VapourResistance * DP;
                layer.Pi = P;
                P += layer.Dp;

                layer.Xi = X;
                X += layer.Thickness;
            }
        }
    }

}
