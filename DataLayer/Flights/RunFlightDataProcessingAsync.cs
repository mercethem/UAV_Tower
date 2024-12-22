using System.Collections.Concurrent;

namespace radarApi
{
    class RunFlightDataProcessingAsync
    {
        // MongoDB'ye veri kaydedecek asenkron işlem
        public async Task RunFlightDataProcessingAsyncMongoDB()  // Artık statik değil // No longer static
        {
            var httpClient = new HttpClient();  // HTTP istemcisini başlatır // Initializes the HTTP client
            var flightDataService = new DataService<FlightData>(httpClient, "http://127.0.0.1:30003/data.json");  // Veri servisini başlatır // Initializes the flight data service

            var flightDataFetcher = new FlightDataFetcher(flightDataService);  // Uçuş verilerini çekmek için fetcher'ı başlatır // Initializes the flight data fetcher
            var flightDataHandler = new FlightDataHandler();  // Uçuş verilerini işlemek için handler'ı başlatır // Initializes the flight data handler
            var flightDataUpdater = new FlightDataUpdater();  // Uçuş verilerini güncellemek için updater'ı başlatır // Initializes the flight data updater
            //var flightDataSaver = new FlightDataSaver.FlightDataSaverMongoDb("mongodb://host.docker.internal:27017", "FlightDataDB", "Flights");  // MongoDB veritabanına kaydetme sınıfını başlatır // Initializes the MongoDB saver
            var flightDataSaver = new FlightDataSaver.FlightDataSaverMongoDb("mongodb://127.0.0.1:27017", "FlightDataDB", "Flights");  // MongoDB veritabanına kaydetme sınıfını başlatır // Initializes the MongoDB saver

            var lastMessageCounts = new ConcurrentDictionary<string, (int Count, DateTime LastUpdated)>();  // Mesaj sayıları ve son güncellenme zamanı için dictionary // A dictionary for message counts and last update time

            // Main loop for continuous fetching and displaying
            while (true)
            {
                // 1. Flight verilerini al // Fetch flight data
                var flightData = await flightDataFetcher.FetchFlightDataAsync();

                // 2. Mesaj sayısını güncelle // Update message counts
                flightDataUpdater.UpdateMessageCounts(flightData, lastMessageCounts);

                // 3. Güncellenmiş uçuşları görüntüle // Display updated flights
                //flightDataDisplayer.DisplayUpdatedFlights(flightData, lastMessageCounts);

                // 4. MongoDB'ye kaydet // Save to MongoDB
                await flightDataSaver.SaveFlightDataAsync(flightData);  // Uçuş verilerini MongoDB'ye kaydet // Save flight data to MongoDB

                // 5. 1 saniye bekle // Wait for 1 second
                await Task.Delay(1000);
            }
        }

        // PostgreSQL'ye veri kaydedecek asenkron işlem
        public async Task RunFlightDataProcessingAsyncPostgreSql()  // Artık statik değil // No longer static
        {
            var httpClient = new HttpClient();  // HTTP istemcisini başlatır // Initializes the HTTP client
            var flightDataService = new DataService<FlightData>(httpClient, "http://127.0.0.1:30003/data.json");  // Veri servisini başlatır // Initializes the flight data service

            var flightDataFetcher = new FlightDataFetcher(flightDataService);  // Uçuş verilerini çekmek için fetcher'ı başlatır // Initializes the flight data fetcher
            var flightDataHandler = new FlightDataHandler();  // Uçuş verilerini işlemek için handler'ı başlatır // Initializes the flight data handler
            var flightDataUpdater = new FlightDataUpdater();  // Uçuş verilerini güncellemek için updater'ı başlatır // Initializes the flight data updater
            var flightDataSaver = new FlightDataSaver.FlightDataSaverPostgreSql("Host=127.0.0.1;Port=5432;Username=postgres;Database=postgres");  // PostgreSQL veritabanına kaydetme sınıfını başlatır // Initializes the PostgreSQL saver

            var lastMessageCounts = new ConcurrentDictionary<string, (int Count, DateTime LastUpdated)>();  // Mesaj sayıları ve son güncellenme zamanı için dictionary // A dictionary for message counts and last update time

            // Main loop for continuous fetching and displaying
            while (true)
            {
                // 1. Flight verilerini al // Fetch flight data
                var flightData = await flightDataFetcher.FetchFlightDataAsync();

                // 2. Mesaj sayısını güncelle // Update message counts
                flightDataUpdater.UpdateMessageCounts(flightData, lastMessageCounts);

                // 3. Güncellenmiş uçuşları görüntüle // Display updated flights
                //flightDataDisplayer.DisplayUpdatedFlights(flightData, lastMessageCounts);

                // 4. PostgreSQL'e kaydet // Save to PostgreSQL
                await flightDataSaver.SaveFlightDataAsync(flightData);  // Uçuş verilerini PostgreSQL'e kaydet // Save flight data to PostgreSQL

                // 5. 1 saniye bekle // Wait for 1 second
                await Task.Delay(1000);
            }
        }

        // Redis'e veri kaydedecek asenkron işlem
        public async Task RunFlightDataProcessingAsyncRedis()  // Artık statik değil // No longer static
        {
            var httpClient = new HttpClient();  // HTTP istemcisini başlatır // Initializes the HTTP client
            var flightDataService = new DataService<FlightData>(httpClient, "http://127.0.0.1:30003/data.json");  // Veri servisini başlatır // Initializes the flight data service

            var flightDataFetcher = new FlightDataFetcher(flightDataService);  // Uçuş verilerini çekmek için fetcher'ı başlatır // Initializes the flight data fetcher
            var flightDataHandler = new FlightDataHandler();  // Uçuş verilerini işlemek için handler'ı başlatır // Initializes the flight data handler
            var flightDataUpdater = new FlightDataUpdater();  // Uçuş verilerini güncellemek için updater'ı başlatır // Initializes the flight data updater
            //var flightDataSaver = new FlightDataSaver.FlightDataSaverRedis("host.docker.internal:6379");  // Redis'e veri kaydetme sınıfını başlatır (Redis bağlantı adresi) // Initializes the Redis saver
            var flightDataSaver = new FlightDataSaver.FlightDataSaverRedis("127.0.0.1:6379");  // Redis'e veri kaydetme sınıfını başlatır (Redis bağlantı adresi) // Initializes the Redis saver

            var lastMessageCounts = new ConcurrentDictionary<string, (int Count, DateTime LastUpdated)>();  // Mesaj sayıları ve son güncellenme zamanı için dictionary // A dictionary for message counts and last update time

            // Main loop for continuous fetching and displaying
            while (true)
            {
                // 1. Flight verilerini al // Fetch flight data
                var flightData = await flightDataFetcher.FetchFlightDataAsync();

                // 2. Mesaj sayısını güncelle // Update message counts
                flightDataUpdater.UpdateMessageCounts(flightData, lastMessageCounts);

                // 3. Güncellenmiş uçuşları görüntüle // Display updated flights
                //flightDataDisplayer.DisplayUpdatedFlights(flightData, lastMessageCounts);

                // 4. Redis'e kaydet // Save to Redis
                await flightDataSaver.SaveFlightDataAsync(flightData);  // Uçuş verilerini Redis'e kaydet // Save flight data to Redis

                // 5. 1 saniye bekle // Wait for 1 second
                await Task.Delay(1000);
            }
        }
    }
}
