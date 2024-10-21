public interface IWeatherJsonParser
{
    // Method to parse weather data from a JSON response // JSON yanıtından hava durumu verilerini ayrıştırmak için metot
    WeatherInfo ParseWeather(string jsonResponse, string city);
}
