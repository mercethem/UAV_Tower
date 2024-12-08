namespace radarApi  // Namespace tanımlaması // Defines the namespace for the project
{
    public class FlightDataFetcher  // Uçuş verilerini alır // Class to fetch flight data
    {
        private readonly IDataService<FlightData> _flightDataService;  // Uçuş verisini sağlayan servisi tanımlar // Declares the service to fetch flight data

        public FlightDataFetcher(IDataService<FlightData> flightDataService)  // Constructor, veri servisini başlatır // Constructor to initialize the flight data service
        {
            _flightDataService = flightDataService;  // Servisi başlatır // Initializes the flight data service
        }

        public async Task<List<FlightData>> FetchFlightDataAsync()  // Uçuş verilerini asenkron alır // Fetch flight data asynchronously
        {
            try
            {
                var data = await _flightDataService.GetDataAsync();  // API'den uçuş verisini alır // Fetches flight data from the API
                if (data == null || data.Count == 0)  // Eğer veri alınamazsa // If no data is received
                {
                    //Console.WriteLine("No flight data received.");  // Veri alınamazsa mesaj verir // Prints message if no data is received
                    return new List<FlightData>();  // Boş liste döndürür // Returns an empty list
                }

                foreach (var flight in data)  // Her uçuş için // Iterates through each flight data
                {
                    flight.UpdateMissingFields();  // Eksik alanları günceller // Updates missing fields for each flight
                }

                return data;  // Alınan veriyi döndürür // Returns the fetched data
            }
            catch (DataServiceException ex)  // Veri servisi hatası // Catches DataServiceException errors
            {
                Console.WriteLine($"Error fetching flight data: {ex.Message}");  // Hata mesajını yazdırır // Prints error message
                return new List<FlightData>();  // Boş liste döndürür // Returns an empty list
            }
        }
    }
}
