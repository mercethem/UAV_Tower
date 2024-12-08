using StackExchange.Redis; // Redis kütüphanesini kullanmak için import eder // Imports the StackExchange.Redis library for Redis operations
using System.Text.Json.Serialization; // JSON serileştirme işlemleri için gerekli kütüphaneyi import eder // Imports the library for JSON serialization
using System.Text.Json; // JSON işlemleri için kütüphaneyi import eder // Imports the library for handling JSON

namespace radarApi.FlightDataSaver // Namespace tanımlaması // Defines the namespace for the project
{
    public class FlightDataSaverRedis // Redis veritabanına uçuş verilerini kaydeden sınıf // Class for saving flight data to Redis database
    {
        private readonly ConnectionMultiplexer _redisConnection; // Redis bağlantısını tutan değişken // Variable holding the Redis connection
        private readonly IDatabase _redisDatabase; // Redis veritabanını tutan değişken // Variable holding the Redis database instance

        public FlightDataSaverRedis(string redisConnectionString) // Yapıcı metod, Redis bağlantı dizesini alır // Constructor method that takes the Redis connection string
        {
            _redisConnection = ConnectionMultiplexer.Connect(redisConnectionString); // Redis'e bağlanır // Connects to Redis using the connection string
            _redisDatabase = _redisConnection.GetDatabase(); // Redis veritabanını alır // Gets the Redis database instance
        }

        public async Task SaveFlightDataAsync(List<FlightData> flightDataList) // Uçuş verilerini Redis veritabanına kaydetmek için asenkron metod // Asynchronous method to save flight data to Redis database
        {
            foreach (var flightData in flightDataList) // Uçuş verileri listesindeki her bir uçuş için işlem yapılır // Loops through each flight data in the list
            {
                // Eğer Seen değeri 60 veya daha büyükse, bu uçuşu kaydetme // If Seen value is 60 or greater, skip saving this flight
                if (flightData.Seen >= 60) // Seen değeri 60 veya daha büyükse atla // If Seen value is greater than or equal to 60, skip this flight
                {
                    continue;  // Bu uçuşu atla // Skip this flight
                }

                // Manuel sıralama ile veriyi hazırlayalım // Prepare the data manually for insertion
                var flightInfo = new
                {
                    Flight = flightData.Flight.Trim(), // Flight kodunu alır, boşlukları temizler // Gets the flight code and trims any whitespace
                    Hex_ICAO = flightData.Hex_ICAO, // Hex_ICAO kodunu alır // Gets the Hex_ICAO code
                    Squawk = flightData.Squawk, // Squawk kodunu alır // Gets the Squawk code
                    Latitude = flightData.Lat, // Enlem bilgisini alır // Gets the latitude
                    Longitude = flightData.Lon, // Boylam bilgisini alır // Gets the longitude
                    Altitude = flightData.Altitude, // Yükseklik bilgisini alır // Gets the altitude
                    Speed = flightData.Speed, // Hız bilgisini alır // Gets the speed
                    VertRate = flightData.VertRate, // Yükseklik değişim oranını alır // Gets the vertical rate
                    Track = flightData.Track, // İzleme bilgisini alır // Gets the track
                    ValidPosition = flightData.ValidPosition, // Geçerli pozisyon bilgisini alır // Gets the valid position information
                    ValidTrack = flightData.ValidTrack, // Geçerli izleme bilgisini alır // Gets the valid track information
                    Messages = flightData.Messages, // Mesaj bilgilerini alır // Gets the messages
                    Seen = flightData.Seen, // Uçuş verisinin görüldüğü süreyi alır // Gets the time the flight data has been seen
                    LastUpdated = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") // UTC Zamanı formatında son güncellenme zamanını alır // Gets the last updated time in UTC format
                };

                // JSON serileştirme // JSON serialization
                var flightJson = System.Text.Json.JsonSerializer.Serialize(flightInfo, new JsonSerializerOptions // flightInfo nesnesini JSON formatına çevirir // Serializes the flightInfo object to JSON format
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  // Null değerlerini atla // Ignores null values
                    WriteIndented = true  // JSON'u okunabilir hale getir // Makes the JSON readable (pretty print)
                });

                // Redis key'i için bir değer oluşturmak amacıyla flight ID (örneğin Flight veya Hex_ICAO) kullanılabilir // We use the flight ID (like Flight or Hex_ICAO) to create a unique Redis key
                var redisKey = "Flights:" + flightData.Hex_ICAO;  // Flight için benzersiz bir key oluşturuyoruz // Creates a unique Redis key for the flight

                try
                {
                    // Redis'e JSON verisini kaydet // Saves the JSON data to Redis
                    await _redisDatabase.StringSetAsync(redisKey, flightJson); // Saves the JSON string to Redis asynchronously
                    Console.WriteLine($"Flight data for {flightData.Hex_ICAO} saved to Redis successfully."); // Başarı mesajı // Prints a success message
                }
                catch (Exception ex) // Hata oluşursa // If an error occurs
                {
                    Console.WriteLine($"Error saving flight data to Redis: {ex.Message}"); // Hata mesajını yazdırır // Prints the error message
                }
            }
        }
    }
}
