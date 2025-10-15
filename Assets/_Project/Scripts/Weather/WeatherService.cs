using Cysharp.Threading.Tasks;
using SimpleJSON;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace WeatherSystem
{
    public class WeatherService
    {
        private readonly SignalBus _signalBus;

        //public url so I won`t hide it in .env or smth
        private const string API_URL =
            "https://api.open-meteo.com/v1/forecast?latitude=51.759&longitude=19.458&current_weather=true";

        [Inject]
        public WeatherService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public async UniTaskVoid FetchWeatherAsync(CancellationToken token = default)
        {
            using (var req = UnityWebRequest.Get(API_URL))
            {
                await req.SendWebRequest().ToUniTask(cancellationToken: token);

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[WeatherService] Weather fetch failed: {req.error}");
                    return;
                }

                var json = req.downloadHandler.text;
                var root = JSON.Parse(json);

                if (root == null || root["current_weather"] == null)
                {
                    Debug.LogError("[WeatherService] Weather JSON parse failed.");
                    return;
                }

                var current = root["current_weather"];

                var data = new WeatherData
                {
                    Temperature = current["temperature"].AsFloat,
                    WindSpeed = current["windspeed"].AsFloat,
                    WindDirection = current["winddirection"].AsFloat,
                    IsDay = current["is_day"].AsInt == 1,
                    WeatherCode = current["weathercode"].AsInt,
                    Time = current["time"]
                };

                _signalBus.Fire(new WeatherDataReadySignal(data));
            }
        }
    }
}
