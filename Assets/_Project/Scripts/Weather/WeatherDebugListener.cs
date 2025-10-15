using UnityEngine;
using Zenject;

namespace WeatherSystem
{
    public class WeatherDebugListener : MonoBehaviour
    {
        [Inject] private WeatherService _service;
        [Inject] private SignalBus _signalBus;

        private void OnEnable() => _signalBus.Subscribe<WeatherDataReadySignal>(On_WeatherReady);
        private void OnDisable() => _signalBus.Unsubscribe<WeatherDataReadySignal>(On_WeatherReady);

        private void Start() => _service.FetchWeatherAsync().Forget();

        private static void On_WeatherReady(WeatherDataReadySignal signal)
        {
            var data = signal.Data;

            Debug.Log($"WeatherData:\n" +
                      $"Temperature: {data.Temperature}°C\n" +
                      $"Wind Speed: {data.WindSpeed} m/s\n" +
                      $"Wind Direction: {data.WindDirection}°\n" +
                      $"Is Day: {data.IsDay}\n" +
                      $"Weather Code: {data.WeatherCode}\n" +
                      $"Time: {data.Time}");
        }
    }
}
