namespace radarApi
{
    public class DataService<T> : IDataService<T>  // Generic data service
    {
        private readonly HttpClient httpClient;  // HTTP client for making requests
        private readonly string url;  // The API URL from which to fetch data

        public DataService(HttpClient httpClient, string url)   // Constructor that accepts HttpClient and URL parameters
        {
            this.httpClient = httpClient;
            this.url = url;
        }

        public async Task<List<T>> GetDataAsync()  // API'den veri alır // Fetches data from the API asynchronously
        {
            try
            {
                var data = await httpClient.GetFromJsonAsync<List<T>>(url);
                if (data == null || data.Count == 0)
                {
                    return new List<T>();
                }
                return data;
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                throw new DataServiceException($"JSON deserialization error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                throw new DataServiceException($"An error occurred while fetching data: {ex.Message}");
            }
        }
    }
}
