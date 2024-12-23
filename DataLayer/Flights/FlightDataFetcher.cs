namespace radarApi
{
    public class FlightDataFetcher   // Class to fetch flight data
    {
        private readonly IDataService<FlightData> _flightDataService;

        public FlightDataFetcher(IDataService<FlightData> flightDataService)  // Constructor to initialize the flight data service
        {
            _flightDataService = flightDataService;
        }

        public async Task<List<FlightData>> FetchFlightDataAsync() // Fetch flight data asynchronously
        {
            try
            {
                var data = await _flightDataService.GetDataAsync();
                if (data == null || data.Count == 0)
                {
                    return new List<FlightData>();
                }

                foreach (var flight in data)
                {
                    flight.UpdateMissingFields();
                }

                return data;
            }
            catch (DataServiceException ex)
            {
                Console.WriteLine($"Error fetching flight data: {ex.Message}");
                return new List<FlightData>();
            }
        }
    }
}
