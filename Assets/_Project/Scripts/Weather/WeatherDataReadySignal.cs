namespace WeatherSystem
{
    public class WeatherDataReadySignal
    {
        public readonly WeatherData Data;

        public WeatherDataReadySignal(WeatherData data)
        {
            this.Data = data;
        }
    }
}
