using System.Net.Http.Json;  // JSON verisi almak için kullanılan kütüphane // Library used to handle JSON data fetching

namespace radarApi // Namespace tanımlaması // Defines the namespace for the project
{
    public class DataService<T> : IDataService<T>  // Generic veri servisi // Generic data service
    {
        private readonly HttpClient httpClient;  // HTTP istemcisi // HTTP client for making requests
        private readonly string url;  // Veriyi alacağımız API URL'si // The API URL from which to fetch data

        public DataService(HttpClient httpClient, string url)  // Constructor: HttpClient ve URL parametrelerini alır // Constructor that accepts HttpClient and URL parameters
        {
            this.httpClient = httpClient;  // HttpClient'ı başlatır // Initializes the HttpClient
            this.url = url;  // URL'yi atar // Assigns the URL
        }

        public async Task<List<T>> GetDataAsync()  // API'den veri alır // Fetches data from the API asynchronously
        {
            try
            {
                var data = await httpClient.GetFromJsonAsync<List<T>>(url);  // Veriyi JSON formatında alır // Fetches data as JSON and deserializes it into a List<T>
                if (data == null || data.Count == 0)  // Eğer veri alınamazsa // Checks if no data is returned
                {
                    //Console.WriteLine("No flight data received from the API.");  // Hata mesajı verir // Logs an error message (currently commented out)
                    return new List<T>();  // Boş liste döndürür // Returns an empty list if no data is fetched
                }
                return data;  // Alınan veriyi döndürür // Returns the fetched data
            }
            catch (System.Text.Json.JsonException jsonEx)  // JSON deserialization hatası // Catches JSON deserialization errors
            {
                throw new DataServiceException($"JSON deserialization error: {jsonEx.Message}");  // JSON hatası fırlatır // Throws a custom exception for JSON errors
            }
            catch (Exception ex)  // Genel hata işleme // General exception handling
            {
                throw new DataServiceException($"An error occurred while fetching data: {ex.Message}");  // Diğer hataları fırlatır // Throws a custom exception for other errors
            }
        }
    }
}
