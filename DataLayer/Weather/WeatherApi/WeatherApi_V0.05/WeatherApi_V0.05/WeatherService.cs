using System;
using System.Net.Http;
using System.Threading.Tasks;

public class WeatherService : BaseWeatherService
{  
    // WeatherService sınıfının tekil örneğini tutmak için bir alan.
    private static WeatherService instance;

    // Özel yapıcı (constructor). HttpClient, API anahtarı ve JSON ayrıştırıcısı alır.
    private WeatherService(HttpClient client, string apiKey, JsonParser jsonParser)
    : base(client, apiKey, jsonParser) // Base sınıfının constructor'ını çağırır.
    {
    }

    // WeatherService sınıfının tekil örneğini almak için bir metot.
    public static WeatherService GetInstance(HttpClient client, string apikey, JsonParser jsonParser)
    {
        // Eğer instance null ise, yeni bir WeatherService nesnesi oluştur.
        if (instance == null)
        {
            instance = new WeatherService(client, apikey, jsonParser);
        }
        // Mevcut örneği döndür
        return instance;

    }
    
    // Belirli bir şehir için hava durumu bilgilerini almak için asenkron metot.
    public override async Task<WeatherInfo> GetWeatherAsync(string city)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={this.apiKey}&units=metric";
        string response = await this.client.GetStringAsync(url);
        return this.jsonParser.ParseWeather(response, city);

    }

   
}

