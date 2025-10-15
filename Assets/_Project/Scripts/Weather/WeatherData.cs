using System;

namespace WeatherSystem
{
    [Serializable]
    public class WeatherData
    {
        public float Temperature { get; set; }
        public float WindSpeed { get; set; }
        public float WindDirection { get; set; }

        public bool IsDay { get; set; }
        public int WeatherCode { get; set; }

        public string Time { get; set; }
    }
}
