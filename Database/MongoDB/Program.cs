using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private const string apiKey = "3583b31f210c210344db810cd563fd6b"; // API key // API anahtarı
    private const string connectionString = "mongodb://localhost:27017"; // MongoDB connection string // MongoDB bağlantı dizesi
    public const string databaseName = "mongodbverileri"; // Database name // Veritabanı adı
    public const string collectionName = "mongocollect"; // Collection name // Koleksiyon adı

    static async Task Main(string[] args)
    {
        string city = "Elazığ"; // City name // Şehir adı
        IWeatherJsonParser jsonParser = new WeatherJsonParser();
        WeatherService weatherService = WeatherService.GetInstance(client, apiKey, jsonParser);
        DatabaseService dbService = DatabaseService.GetInstance(connectionString);

        while (true) // Infinite loop // Sonsuz döngü
        {
            try
            {
                WeatherInfo weatherInfo = await weatherService.GetWeatherAsync(city);
                Console.WriteLine(weatherInfo);
                dbService.SaveWeatherInfo(weatherInfo); // Save to database // Veritabanına kaydet
                Console.WriteLine("Hava durumu veritabanına kaydedildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            await Task.Delay(60000); // Wait for 60 seconds // 60 saniye bekle
        }
    }
}
