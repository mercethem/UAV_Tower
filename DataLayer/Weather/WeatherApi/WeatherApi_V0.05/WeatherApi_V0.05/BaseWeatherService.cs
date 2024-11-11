using System.Net.Http;


    public abstract class BaseWeatherService : IWeatherClass
    {
        protected readonly HttpClient client;
        protected readonly string apiKey;
        protected readonly IJsonParser jsonParser;

        // Yapıcı metot: BaseWeatherService sınıfının örneğini oluşturur
        protected BaseWeatherService(HttpClient client, string apiKey, IJsonParser jsonParser)
        {
            this.client = client;
            this.apiKey = apiKey;
            this.jsonParser = jsonParser;
        }

        // Hava durumunu almak için asenkron metot (Soyut)
        public abstract Task<WeatherInfo> GetWeatherAsync(string city);

       
    }

