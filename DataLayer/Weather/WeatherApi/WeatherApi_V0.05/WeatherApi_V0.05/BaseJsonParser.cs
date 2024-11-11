using Newtonsoft.Json.Linq;
using System;




    public abstract class BaseJsonParser : IJsonParser
    {
    // Bu metot, JSON yanıtını ve şehir adını alır ve WeatherInfo nesnesini döndürür.
    public abstract WeatherInfo ParseWeather(string jsonResponse, string city);

    // Bu metot, JSON yanıtını ve şehir adını alır ve WeatherInfo nesnesini döndürür.
    protected string GetJsonValue(JObject json, string path)
        {

        // Verilen yol ile JSON'da bir token seçilir.
        var token = json.SelectToken(path);
        // Token mevcutsa, string olarak döner; aksi halde null döner.
        return token != null ? token.ToString() : null;
        }

    }
