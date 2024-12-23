using StackExchange.Redis;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace radarApi.FlightDataSaver
{
    public class FlightDataSaverRedis // Class for saving flight data to Redis database
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisDatabase;

        public FlightDataSaverRedis(string redisConnectionString)  // Constructor method that takes the Redis connection string
        {
            _redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
            _redisDatabase = _redisConnection.GetDatabase();
        }

        public async Task SaveFlightDataAsync(List<FlightData> flightDataList) // Asynchronous method to save flight data to Redis database
        {
            foreach (var flightData in flightDataList)
            {

                if (flightData.Seen >= 60)
                {
                    continue;
                }

                var flightInfo = new
                {
                    Flight = flightData.Flight.Trim(),
                    Hex_ICAO = flightData.Hex_ICAO,
                    Squawk = flightData.Squawk,
                    Latitude = flightData.Lat,
                    Longitude = flightData.Lon,
                    Altitude = flightData.Altitude,
                    Speed = flightData.Speed,
                    VertRate = flightData.VertRate,
                    Track = flightData.Track,
                    ValidPosition = flightData.ValidPosition,
                    ValidTrack = flightData.ValidTrack,
                    Messages = flightData.Messages,
                    Seen = flightData.Seen,
                    LastUpdated = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                var flightJson = System.Text.Json.JsonSerializer.Serialize(flightInfo, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true
                });

                var redisKey = "Flights:" + flightData.Hex_ICAO;

                try
                {
                    await _redisDatabase.StringSetAsync(redisKey, flightJson);
                    Console.WriteLine($"Flight data for {flightData.Hex_ICAO} saved to Redis successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving flight data to Redis: {ex.Message}");
                }
            }
        }
    }
}
