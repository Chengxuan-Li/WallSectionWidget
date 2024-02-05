using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallSectionWidget
{
    public static class Psychrometrics
    {
        public static double SaturationPressure(double temperature)
        {
            // Using the buck equation
            return 0.61121 * Math.Exp((18.678 - temperature / 234.5) * (temperature / (257.14 + temperature))) * 1000;
        }

        public static double VapourPressure(double temperature, double relativeHumidity)
        {
            return SaturationPressure(temperature) * relativeHumidity / 100;
        }

        public static double DewPoint(double vapourPressure)
        {
            return Math.Log(vapourPressure / 610.78) * 237.3 / (17.27 - Math.Log(vapourPressure / 610.78));
        }

        public static double RelativeHumidity(double temperature, double vapourPressure)
        {
            return vapourPressure / SaturationPressure(temperature) * 100;
        }
    }
}
