using System.Collections.Concurrent;

namespace radarApi
{
    class RunFlightDataProcessingAsync
    {

        public async Task RunFlightDataProcessingAsyncMongoDB()
        {
            var httpClient = new HttpClient();
            var flightDataService = new DataService<FlightData>(httpClient, "http://127.0.0.1:30003/data.json");  // Initializes the flight data service

            var flightDataFetcher = new FlightDataFetcher(flightDataService);  // Initializes the flight data fetcher
            var flightDataHandler = new FlightDataHandler();  // Initializes the flight data handler
            var flightDataUpdater = new FlightDataUpdater();   // Initializes the flight data updater
            //var flightDataSaver = new FlightDataSaver.FlightDataSaverMongoDb("mongodb://host.docker.internal:27017", "FlightDataDB", "Flights");  // MongoDB veritabanına kaydetme sınıfını başlatır // Initializes the MongoDB saver
            var flightDataSaver = new FlightDataSaver.FlightDataSaverMongoDb("mongodb://127.0.0.1:27017", "FlightDataDB", "Flights");  // Initializes the MongoDB saver

            var lastMessageCounts = new ConcurrentDictionary<string, (int Count, DateTime LastUpdated)>(); // A dictionary for message counts and last update time

            // Main loop for continuous fetching and displaying
            while (true)
            {
                var flightData = await flightDataFetcher.FetchFlightDataAsync();
                flightDataUpdater.UpdateMessageCounts(flightData, lastMessageCounts);
                await flightDataSaver.SaveFlightDataAsync(flightData);
                await Task.Delay(1000);
            }
        }

        public async Task RunFlightDataProcessingAsyncPostgreSql()
        {
            var httpClient = new HttpClient();
            var flightDataService = new DataService<FlightData>(httpClient, "http://127.0.0.1:30003/data.json"); // Initializes the flight data service

            var flightDataFetcher = new FlightDataFetcher(flightDataService); // Initializes the flight data fetcher
            var flightDataHandler = new FlightDataHandler(); // Initializes the flight data handler
            var flightDataUpdater = new FlightDataUpdater();  // Initializes the flight data updater
            //var flightDataSaver = new FlightDataSaver.FlightDataSaverPostgreSql("Host=localhost;Port=5432;Username=postgres;Database=postgres");   // Initializes the PostgreSQL saver
            var flightDataSaver = new FlightDataSaver.FlightDataSaverPostgreSql("Host=127.0.0.1;Port=5432;Username=postgres;Database=postgres");   // Initializes the PostgreSQL saver

            var lastMessageCounts = new ConcurrentDictionary<string, (int Count, DateTime LastUpdated)>(); // A dictionary for message counts and last update time


            while (true)
            {
                var flightData = await flightDataFetcher.FetchFlightDataAsync();
                flightDataUpdater.UpdateMessageCounts(flightData, lastMessageCounts);
                await flightDataSaver.SaveFlightDataAsync(flightData);
                await Task.Delay(1000);
            }
        }


        public async Task RunFlightDataProcessingAsyncRedis()
        {
            var httpClient = new HttpClient();
            var flightDataService = new DataService<FlightData>(httpClient, "http://127.0.0.1:30003/data.json"); // Initializes the flight data service

            var flightDataFetcher = new FlightDataFetcher(flightDataService); // Initializes the flight data fetcher
            var flightDataHandler = new FlightDataHandler(); // Initializes the flight data handler
            var flightDataUpdater = new FlightDataUpdater(); // Initializes the flight data updater
            //var flightDataSaver = new FlightDataSaver.FlightDataSaverRedis("host.docker.internal:6379");  // Redis'e veri kaydetme sınıfını başlatır (Redis bağlantı adresi) // Initializes the Redis saver
            var flightDataSaver = new FlightDataSaver.FlightDataSaverRedis("127.0.0.1:6379");  // Initializes the Redis saver

            var lastMessageCounts = new ConcurrentDictionary<string, (int Count, DateTime LastUpdated)>(); // A dictionary for message counts and last update time


            while (true)
            {
                var flightData = await flightDataFetcher.FetchFlightDataAsync();
                flightDataUpdater.UpdateMessageCounts(flightData, lastMessageCounts);
                await flightDataSaver.SaveFlightDataAsync(flightData);  // Uçuş verilerini Redis'e kaydet // Save flight data to Redis
                await Task.Delay(1000);
            }
        }
    }
}
