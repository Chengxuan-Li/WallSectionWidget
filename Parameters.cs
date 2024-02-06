using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallSectionWidget
{
    public class Parameters
    {
        public double InteriorTemperature;
        public double InteriorHumidity;
        public double ExteriorTemperature;
        public double ExteriorHumidity;
        public double InteriorContactDistance = 0.00625;
        public double ExteriorContactDistance = 0.001;
        public double MaxStepLength = 0.005;

        public GHIOParam<Parameters> GHIOParam => new GHIOParam<Parameters>(this);

        public static Parameters DefaultWinter => new Parameters
        {
            InteriorTemperature = 20.0,
            InteriorHumidity = 50.0,
            ExteriorTemperature = 5.0,
            ExteriorHumidity = 80.0,
        };

        public static Parameters DefaultSummer => new Parameters
        {
            InteriorTemperature = 25.0,
            InteriorHumidity = 50.0,
            ExteriorTemperature = 30.0,
            ExteriorHumidity = 30.0,
        };

    }
}
