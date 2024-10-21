public interface IWeatherService
{
    // Asynchronous method to get weather information for a specific city // Belirli bir şehir için hava durumu bilgisi almak için asenkron metot
    Task<WeatherInfo> GetWeatherAsync(string city);
}
