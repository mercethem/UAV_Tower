using Newtonsoft.Json.Linq;
using System;





    public class JsonParser : BaseJsonParser
    {

    // ParseWeather metodu, bir JSON yanıtını ve şehir adını alarak
    // bir WeatherInfo nesnesi döndürür. Metot, BaseJsonParser'dan
    // miras alınan soyut metodu uygular.
    public override WeatherInfo ParseWeather(string jsonResponse, string city)
    {   
        //  JSON yanıtını JObject türünde bir nesneye dönüştür.

        var json = JObject.Parse(jsonResponse);

            return new WeatherInfo(GetJsonValue(json, "weather[0].description"),
                 double.Parse(GetJsonValue(json, "main.temp")),
                 double.Parse(GetJsonValue(json, "main.feels_like")),
                 int.Parse(GetJsonValue(json, "main.humidity")),
                 double.Parse(GetJsonValue(json, "wind.speed")),
                 int.Parse(GetJsonValue(json, "main.pressure")),
                 int.Parse(GetJsonValue(json, "clouds.all")),
                 int.Parse(GetJsonValue(json, "visibility")),
                 int.Parse(GetJsonValue(json, "weather[0].id")),
                 DateTimeOffset.FromUnixTimeSeconds(long.Parse(GetJsonValue(json, "sys.sunrise"))).DateTime,
                 DateTimeOffset.FromUnixTimeSeconds(long.Parse(GetJsonValue(json, "sys.sunset"))).DateTime,
                 double.Parse(GetJsonValue(json, "coord.lat")),
                 double.Parse(GetJsonValue(json, "coord.lon")),
                 city,
                 DateTime.Now);

        }
    }


