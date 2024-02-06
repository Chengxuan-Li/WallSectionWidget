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
        public double InteriorTemperature = 20.0;
        public double ExteriorTemperature = 5.0;
        public double InteriorHumidity = 50.0;
        public double ExteriorHumidity = 80.0;

        public GHIOParam<Construction> GHIOParam => new GHIOParam<Construction>(this);

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

        public double TemperatureDifference
        {
            get { return ExteriorTemperature - InteriorTemperature; }
        }

        public double InteriorVapourPressure
        {
            get { return Psychrometrics.VapourPressure(InteriorTemperature, InteriorHumidity); }
        }

        public double ExteriorVapourPressure
        {
            get { return Psychrometrics.VapourPressure(ExteriorTemperature, ExteriorHumidity); }
        }

        public double VapourPressureDifference
        {
            get { return ExteriorVapourPressure - InteriorVapourPressure; }
        }

        public void UpdateLayers()
        {
            double ti = InteriorTemperature;
            double pi = InteriorVapourPressure;
            double xi = 0.0;
            foreach (Layer layer in Layers)
            {
                // assign the correct temperature difference for each layer
                layer.TemperatureDifference = layer.Resistivity / Resistance * TemperatureDifference;
                layer.InteriorTemperature = ti;
                
                // update the inner surface temperature for the next layer
                ti += layer.TemperatureDifference;

                // assign the correct VP difference for each layer
                layer.VapourPressureDifference = layer.VapourResistivity / VapourResistance * VapourPressureDifference;
                layer.InteriorVapourPressure = pi;

                // update the inner surface VP for the next layer
                pi += layer.VapourPressureDifference;

                // assign the correct innter surface depth for each layer
                layer.InteriorSideDepthFromSurface = xi;

                // update the inner surface 
                xi += layer.Thickness;
            }

        }

        public static Construction Default => new Construction
        {
            Layers = new List<Layer> {
                new Layer(Material.Lamination, 0.04),
                new Layer(Material.Cork, 0.16),
                new Layer(Material.Concrete, 0.3),
            },
        };

    }

}
