using System;

namespace Xas.LightTeams.Device
{
    public class Pixel
    {
        private decimal brightness = 0.0m;

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public decimal Brightness
        {
            get
            {
                return brightness;
            }

            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("brightness value must be between 0 and 1");
                }
                brightness = value;
            }
        }
    }
}
