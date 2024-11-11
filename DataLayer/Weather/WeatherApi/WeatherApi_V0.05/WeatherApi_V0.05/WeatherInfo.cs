using System;

    public class WeatherInfo : BaseWeatherInfo
    {
        // WeatherInfo constructor'ı, tüm alanları BaseWeatherInfo constructor'ına iletir
        public WeatherInfo(
            string description, double temperature, double feelsLike, int humidity,
            double windSpeed, int pressure, int clouds, int visibility, int weatherId,
            DateTime sunrise, DateTime sunset, double lat, double lon, string city, DateTime recordDateTime)
            : base(description, temperature, feelsLike, humidity, windSpeed, pressure, clouds,
                   visibility, weatherId, sunrise, sunset, lat, lon, city, recordDateTime)
        {
        }
        public override string GetWeatherDetails()
        {
            return $"Weather in {city}: {description}\n" +
                   $"Temperature: {temperature}°C\n" +
                   $"Feels Like: {feelsLike}°C\n" +
                   $"Humidity: {humidity}%\n" +
                   $"Wind Speed: {windSpeed} m/s\n" +
                   $"Pressure: {pressure} hPa\n" +
                   $"Clouds: {clouds}%\n" +
                   $"Visibility: {visibility} meters\n" +
                   $"Weather ID: {weatherId}\n" +
                   $"Sunrise: {sunrise}\n" +
                   $"Sunset: {sunset}\n" +
                   $"Latitude: {lat}\n" +
                   $"Longitude: {lon}\n" +
                   $"Record Date Time: {recordDateTime}";
        }
    }

