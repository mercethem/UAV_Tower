using Newtonsoft.Json.Linq;

public class WeatherJsonParser : IWeatherJsonParser
{
    public WeatherInfo ParseWeather(string jsonResponse, string city)
    {
        var json = JObject.Parse(jsonResponse);

        return new WeatherInfo(
            json["weather"][0]["description"].ToString(),
            double.Parse(json["main"]["temp"].ToString()),
            double.Parse(json["main"]["feels_like"].ToString()),
            int.Parse(json["main"]["humidity"].ToString()),
            double.Parse(json["wind"]["speed"].ToString()),
            int.Parse(json["main"]["pressure"].ToString()),
            int.Parse(json["clouds"]["all"].ToString()),
            int.Parse(json["visibility"].ToString()),
            int.Parse(json["weather"][0]["id"].ToString()),
            DateTimeOffset.FromUnixTimeSeconds(long.Parse(json["sys"]["sunrise"].ToString())).DateTime,
            DateTimeOffset.FromUnixTimeSeconds(long.Parse(json["sys"]["sunset"].ToString())).DateTime,
            double.Parse(json["coord"]["lat"].ToString()),
            double.Parse(json["coord"]["lon"].ToString()),
            city,
            DateTime.Now
        );
    }
}