public class WeatherService : IWeatherService
{
    private static WeatherService _instance; // Singleton instance // Singleton örneği
    private static readonly object _lock = new object(); // Lock object for thread safety // Thread güvenliği için kilit nesnesi
    private readonly HttpClient _client; // HTTP client for making requests // İstek yapmak için HTTP istemcisi
    private readonly string _apiKey; // API key for authentication // Kimlik doğrulama için API anahtarı
    private readonly IWeatherJsonParser _jsonParser; // JSON parser for parsing weather data // Hava durumu verilerini ayrıştırmak için JSON ayrıştırıcısı

    // WeatherService constructor // WeatherService yapıcısı
    private WeatherService(HttpClient client, string apiKey, IWeatherJsonParser jsonParser)
    {
        _client = client;
        _apiKey = apiKey;
        _jsonParser = jsonParser;
    }

    // GetInstance method for singleton pattern // Singleton desenine göre GetInstance metodu
    public static WeatherService GetInstance(HttpClient client, string apiKey, IWeatherJsonParser jsonParser)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new WeatherService(client, apiKey, jsonParser);
                }
            }
        }
        return _instance;
    }

    // Asynchronous method to get weather information // Hava durumu bilgisi almak için asenkron metot
    public async Task<WeatherInfo> GetWeatherAsync(string city)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric"; // API URL
        string response = await _client.GetStringAsync(url); // API'den yanıtı al
        return _jsonParser.ParseWeather(response, city); // Yanıtı ayrıştır
    }
}
