
class Test
{
    private static readonly HttpClient client = new HttpClient();
    private const string apiKey = "3583b31f210c210344db810cd563fd6b"; // api anahtarı
    private const string connectionString = "Server=.\\SQLEXPRESS;Database=weatherDatasetOnlineElazig_V1.01;Integrated Security = True;"; // Bağlantı dizesi
    static async Task Main(string[] args)
    {
        string city = "Elazığ"; // Şehri burada belirtiliyor
        JsonParser jsonParser = new JsonParser(); // JSON ayrıştırıcısı oluşturuldu
        WeatherService weatherService = WeatherService.GetInstance(client, apiKey, jsonParser); // JSON ayrıştırıcısı servise geçirildi
        DatabaseService dbService = new DatabaseService(connectionString); // Veritabanı servisi alındı

        while (true)
        {
            try
            {
                WeatherInfo weatherInfo = await weatherService.GetWeatherAsync(city);
                Console.WriteLine(weatherInfo);
                dbService.SaveWeatherInfo(weatherInfo);
                Console.WriteLine("Hava durumu veritabanına kaydedildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
            await Task.Delay(60000);
        }

    }
}